using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.DirectoryServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using evaluation_fournisseur_version_actuel.Server.Data;
using evaluation_fournisseur_version_actuel.Server.Models;

namespace Evaluation_fournisseur_version_actuel.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _ldapConnectionString;
        private readonly string _ldapUsername;
        private readonly string _ldapPassword;

        public LoginController(AppDbContext context, ILogger<LoginController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;

            // Charger les paramètres LDAP depuis appsettings.json
            var serverAddress = _configuration["LDAPSettings:ServerAddress"] ?? "";
            var port = _configuration["LDAPSettings:Port"] ?? "";
            _ldapConnectionString = $"LDAP://{serverAddress}:{port}";
            _ldapUsername = _configuration["LDAPSettings:Username"] ?? "";
            _ldapPassword = _configuration["LDAPSettings:Password"] ?? "";
        }

        [HttpGet("windowsIdentity")]
        [Authorize]
        public IActionResult GetWindowsIdentity()
        {
            try
            {
                string? username = User.Identity?.Name;

                if (string.IsNullOrEmpty(username))
                {
                    return BadRequest("Nom d'utilisateur non trouvé.");
                }

                string userName = username.Contains("\\")
                    ? username.Split('\\')[1]
                    : username;

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(userName, options);

                return new ContentResult
                {
                    Content = json,
                    ContentType = "application/json",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération de l'identité: {ex.Message}");
            }
        }

        [HttpGet("getDetailsLogin")]
        [Authorize]
        public IActionResult GetDetailsLogin()
        {
            try
            {
                string? connectedUsername = HttpContext.User.Identity?.Name;

                if (string.IsNullOrEmpty(connectedUsername))
                {
                    return BadRequest("Nom d'utilisateur connecté non trouvé.");
                }

                string username = connectedUsername.Contains("\\")
                    ? connectedUsername.Split('\\')[1]
                    : connectedUsername;

                // Utiliser la méthode réutilisable
                var userDetails = GetLdapUserDetails(username);

                if (userDetails == null)
                {
                    return NotFound("Utilisateur non trouvé dans LDAP.");
                }

                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur de connexion LDAP: {ex.Message}");
            }
        }

        [HttpGet("getAllUsers")]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            try
            {
                // Connexion LDAP en mode LECTURE SEULE pour plus de sécurité
                using (DirectoryEntry ldap = new DirectoryEntry(
                    _ldapConnectionString,
                    _ldapUsername,
                    _ldapPassword,
                    AuthenticationTypes.ReadonlyServer | AuthenticationTypes.Secure))
                {
                    // Vérifier la connexion
                    object nativeObject = ldap.NativeObject;

                    using (DirectorySearcher searcher = new DirectorySearcher(ldap))
                    {
                        // Filtre pour obtenir uniquement les utilisateurs actifs avec email
                        searcher.Filter = "(&(displayname=*)(mail=*.*@castel-afrique.com)(!(userAccountControl=514)))";

                        searcher.PropertiesToLoad.Add("displayName");
                        searcher.PropertiesToLoad.Add("mail");
                        searcher.PropertiesToLoad.Add("title");
                        searcher.PropertiesToLoad.Add("employeeNumber");
                        searcher.PropertiesToLoad.Add("sAMAccountName");

                        SearchResultCollection results = searcher.FindAll();

                        if (results.Count == 0)
                        {
                            return NotFound("Aucun utilisateur trouvé dans LDAP.");
                        }

                        var users = new List<UserDetails>();

                        foreach (SearchResult result in results)
                        {
                            // Vérifier que toutes les propriétés requises existent
                            if (!HasRequiredProperties(result))
                            {
                                continue;
                            }

                            var user = new UserDetails
                            {
                                FullName = GetPropertyValue(result, "displayName"),
                                Email = GetPropertyValue(result, "mail"),
                                Poste = GetPropertyValue(result, "title"),
                                Matricule = GetPropertyValue(result, "employeeNumber"),
                                UserName = GetPropertyValue(result, "sAMAccountName")
                            };

                            users.Add(user);
                        }

                        return Ok(users);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur de connexion LDAP: {ex.Message}");
            }
        }

        [HttpGet("check-admin")]
        [Authorize]
        public async Task<ActionResult<LoginResponseDto>> CheckAdmin()
        {
            try
            {
                // Récupérer le nom d'utilisateur depuis l'authentification Windows
                var userName = User.Identity?.Name;
                
                if (string.IsNullOrEmpty(userName))
                {
                    _logger.LogWarning("Nom d'utilisateur non trouvé dans l'authentification Windows");
                    return Ok(new LoginResponseDto 
                    { 
                        Admin = null,
                        UserName = null,
                        Message = "Utilisateur non authentifié"
                    });
                }

                // Extraire juste le nom d'utilisateur (sans le domaine)
                var shortUserName = userName.Contains('\\') 
                    ? userName.Split('\\')[1] 
                    : userName;

                // Vérifier d'abord si l'utilisateur existe dans LDAP
                var ldapUser = GetLdapUserDetails(shortUserName);
                if (ldapUser == null)
                {
                    _logger.LogInformation("Utilisateur {UserName} non trouvé dans LDAP", shortUserName);
                    return Ok(new LoginResponseDto 
                    { 
                        Admin = null,
                        UserName = shortUserName,
                        Message = "Utilisateur non trouvé dans LDAP"
                    });
                }

                // Si l'utilisateur existe dans LDAP, vérifier s'il est administrateur
                var shortUserNameLower = shortUserName.ToLower();

                var administrateur = await _context.Administrateurs
                    .FirstOrDefaultAsync(a => a.IdentiteUtilisateur.ToLower() == shortUserNameLower);
                var isAdmin = administrateur != null;

                _logger.LogInformation("Vérification admin pour utilisateur {UserName}: {IsAdmin}", shortUserName, isAdmin);

                return Ok(new LoginResponseDto 
                { 
                    Admin = isAdmin, 
                    UserName = shortUserName,
                    Message = isAdmin ? "Utilisateur administrateur" : "Utilisateur non administrateur"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification administrateur");
                return StatusCode(500, new LoginResponseDto 
                { 
                    Admin = null,
                    UserName = null,
                    Message = "Erreur lors de la vérification"
                });
            }
        }

        #region Méthodes privées réutilisables

        /// <summary>
        /// Récupère les détails d'un utilisateur depuis LDAP
        /// </summary>
        private UserDetails? GetLdapUserDetails(string username)
        {
            using (DirectoryEntry ldap = new DirectoryEntry(
                _ldapConnectionString,
                _ldapUsername,
                _ldapPassword,
                AuthenticationTypes.ReadonlyServer | AuthenticationTypes.Secure))
            {
                object nativeObject = ldap.NativeObject;

                using (DirectorySearcher searcher = new DirectorySearcher(ldap))
                {
                    searcher.Filter = $"(&(objectClass=user)(sAMAccountName={username}))";

                    searcher.PropertiesToLoad.Add("displayName");
                    searcher.PropertiesToLoad.Add("mail");
                    searcher.PropertiesToLoad.Add("employeeNumber");
                    searcher.PropertiesToLoad.Add("sAMAccountName");

                    SearchResult result = searcher.FindOne();

                    if (result == null || !HasRequiredProperties(result, requireTitle: false))
                    {
                        return null;
                    }

                    return new UserDetails
                    {
                        FullName = GetPropertyValue(result, "displayName"),
                        Email = GetPropertyValue(result, "mail"),
                        UserName = GetPropertyValue(result, "sAMAccountName"),
                        Matricule = GetPropertyValue(result, "employeeNumber")
                    };
                }
            }
        }

        /// <summary>
        /// Vérifie que les propriétés requises existent dans le résultat LDAP
        /// </summary>
        private bool HasRequiredProperties(SearchResult result, bool requireTitle = true)
        {
            if (result.Properties["displayName"].Count == 0 ||
                result.Properties["mail"].Count == 0 ||
                result.Properties["sAMAccountName"].Count == 0)
            {
                return false;
            }

            if (requireTitle && result.Properties["title"].Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Récupère la valeur d'une propriété LDAP de manière sécurisée
        /// </summary>
        private string GetPropertyValue(SearchResult result, string propertyName)
        {
            if (result.Properties[propertyName].Count > 0)
            {
                return result.Properties[propertyName][0]?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        #endregion
    }

    public class LoginResponseDto
    {
        public bool? Admin { get; set; }
        public string? UserName { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class UserDetails
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Poste { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Matricule { get; set; }
        public int? Id { get; set; }
    }
}
