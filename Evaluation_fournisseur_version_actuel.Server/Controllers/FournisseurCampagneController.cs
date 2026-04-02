using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using evaluation_fournisseur_version_actuel.Server.Models;
using evaluation_fournisseur_version_actuel.Server.Data;

namespace Evaluation_fournisseur_version_actuel.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FournisseurCampagneController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FournisseurCampagneController> _logger;

        public FournisseurCampagneController(AppDbContext context, ILogger<FournisseurCampagneController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("fournisseurs-campagne-active")]
        public async Task<ActionResult<IEnumerable<FournisseurCampagneDto>>> GetFournisseursCampagneActive()
        {
            try
            {
                var fournisseurs = await _context.Fournisseurs
                    .Include(f => f.NomFournisseur)
                        .ThenInclude(nf => nf.Campagne)
                    .Include(f => f.Fabricant)
                    .Include(f => f.Categorie)
                    .Include(f => f.Nature)
                    .Include(f => f.Evaluations)
                        .ThenInclude(e => e.Campagne)
                    .Where(f => f.NomFournisseur.Campagne.Active == true)
                    .Select(f => new FournisseurCampagneDto
                    {
                        IdFournisseur = f.Id,
                        VendorNameFournisseur = f.NomFournisseur.VendorName,
                        Fabricant = f.Fabricant != null ? f.Fabricant.NomFabricant : null,
                        Categorie = f.Categorie.NomCategorie,
                        Nature = f.Nature.LibelNature,
                        Evaluation = f.Evaluations.Any(e => e.Campagne.Active == true)
                    })
                    .ToListAsync();

                return Ok(fournisseurs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des fournisseurs avec campagne active");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des données");
            }
        }
    }

    public class FournisseurCampagneDto
    {
        public int IdFournisseur { get; set; }
        public string VendorNameFournisseur { get; set; } = string.Empty;
        public string? Fabricant { get; set; }
        public string Categorie { get; set; } = string.Empty;
        public string Nature { get; set; } = string.Empty;
        public bool Evaluation { get; set; }
    }
}
