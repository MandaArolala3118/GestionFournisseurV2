using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using evaluation_fournisseur_version_actuel.Server.Models;
using evaluation_fournisseur_version_actuel.Server.Data;
using evaluation_fournisseur_version_actuel.Server.Dto;

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

        [HttpGet("fournisseur/{id}")]
        public async Task<ActionResult<FournisseurDetailDto>> GetFournisseurById(int id)
        {
            try
            {
                var fournisseur = await _context.Fournisseurs
                    .Include(f => f.NomFournisseur)
                        .ThenInclude(nf => nf.Campagne)
                    .Include(f => f.Fabricant)
                    .Include(f => f.Categorie)
                    .Include(f => f.Nature)
                    .Include(f => f.Site)
                    .Include(f => f.Evaluations)
                        .ThenInclude(e => e.Campagne)
                    .Include(f => f.Evaluations)
                        .ThenInclude(e => e.Ponderation)
                    .Include(f => f.Evaluations)
                        .ThenInclude(e => e.Resultat)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (fournisseur == null)
                {
                    return NotFound($"Fournisseur avec ID {id} non trouvé");
                }

                var fournisseurDetail = new FournisseurDetailDto
                {
                    Id = fournisseur.Id,
                    Email = fournisseur.Email,
                    EmailEnvoye = fournisseur.EmailEnvoye,
                    DateEnvoiEmail = fournisseur.DateEnvoiEmail,
                    NomFournisseur = new NomFournisseurDto
                    {
                        Id = fournisseur.NomFournisseur.Id,
                        VendorNumber = fournisseur.NomFournisseur.VendorNumber,
                        VendorName = fournisseur.NomFournisseur.VendorName,
                        CompanyCode = fournisseur.NomFournisseur.CompanyCode,
                        ApDivision = fournisseur.NomFournisseur.ApDivision,
                        StrClassStatus = fournisseur.NomFournisseur.StrClassStatus,
                        SysCodeDescription = fournisseur.NomFournisseur.SysCodeDescription,
                        SignificationCompanyCode = fournisseur.NomFournisseur.SignificationCompanyCode,
                        SignificationApDivision = fournisseur.NomFournisseur.SignificationApDivision,
                        StrVendorClass = fournisseur.NomFournisseur.StrVendorClass,
                        CreditTermsCode = fournisseur.NomFournisseur.CreditTermsCode,
                        Campagne = new CampagneDto
                        {
                            Id = fournisseur.NomFournisseur.Campagne.Id,
                            Annee = fournisseur.NomFournisseur.Campagne.Annee,
                            NomCampagne = fournisseur.NomFournisseur.Campagne.NomCampagne,
                            Active = fournisseur.NomFournisseur.Campagne.Active
                        }
                    },
                    Fabricant = fournisseur.Fabricant != null ? new FabricantDto
                    {
                        Id = fournisseur.Fabricant.Id,
                        NomFabricant = fournisseur.Fabricant.NomFabricant
                    } : null,
                    Categorie = new CategorieDto
                    {
                        Id = fournisseur.Categorie.Id,
                        NomCategorie = fournisseur.Categorie.NomCategorie,
                        Description = fournisseur.Categorie.Description,
                        PonderationId = fournisseur.Categorie.Ponderation?.Id
                    },
                    Nature = new NatureDto
                    {
                        Id = fournisseur.Nature.Id,
                        LibelNature = fournisseur.Nature.LibelNature
                    },
                    Site = new SiteDto
                    {
                        Id = fournisseur.Site.Id,
                        NomSite = fournisseur.Site.NomSite
                    },
                    Evaluations = fournisseur.Evaluations.Select(e => new EvaluationDto
                    {
                        Id = e.Id,
                        FournisseurId = e.FournisseurId,
                        PonderationId = e.PonderationId,
                        CampagneId = e.CampagneId,
                        ResultatId = e.ResultatId,
                        ITransparence = e.ITransparence,
                        IIFacture = e.IIFacture,
                        IIINotoriete = e.IIINotoriete,
                        IVPreventionCorruption = e.IVPreventionCorruption,
                        VPreventionDroitHomme = e.VPreventionDroitHomme,
                        VIConformiteVsExigence = e.VIConformiteVsExigence,
                        VIIDelaiLivraison = e.VIIDelaiLivraison,
                        VIIICommunication = e.VIIICommunication,
                        IXDisponibiliteDocuments = e.IXDisponibiliteDocuments,
                        XConsigneQhse = e.XConsigneQhse,
                        XIQualitePrix = e.XIQualitePrix,
                        XIIDureePaiement = e.XIIDureePaiement,
                        XIIIConformiteFiscale = e.XIIIConformiteFiscale,
                        MoyennePct = e.MoyennePct,
                        Observation = e.Observation,
                        PropositionAmelioration = e.PropositionAmelioration,
                        ObservationsFournisseur = e.ObservationsFournisseur,
                        Responsable = e.Responsable,
                        NumeroActionPac = e.NumeroActionPac,
                        Deadline = e.Deadline,
                        DateEvaluation = e.DateEvaluation,
                        Campagne = new CampagneDto
                        {
                            Id = e.Campagne.Id,
                            Annee = e.Campagne.Annee,
                            NomCampagne = e.Campagne.NomCampagne,
                            Active = e.Campagne.Active
                        },
                        Ponderation = new PonderationDto
                        {
                            Id = e.Ponderation.Id,
                            CategorieId = e.Ponderation.CategorieId,
                            ITransparence = e.Ponderation.ITransparence,
                            IIFacture = e.Ponderation.IIFacture,
                            IIINotoriete = e.Ponderation.IIINotoriete,
                            IVPreventionCorruption = e.Ponderation.IVPreventionCorruption,
                            VPreventionDroitHomme = e.Ponderation.VPreventionDroitHomme,
                            VIConformiteVsExigence = e.Ponderation.VIConformiteVsExigence,
                            VIIDelaiLivraison = e.Ponderation.VIIDelaiLivraison,
                            VIIICommunication = e.Ponderation.VIIICommunication,
                            IXDisponibiliteDocuments = e.Ponderation.IXDisponibiliteDocuments,
                            XConsigneQhse = e.Ponderation.XConsigneQhse,
                            XIQualitePrix = e.Ponderation.XIQualitePrix,
                            XIIDureePaiement = e.Ponderation.XIIDureePaiement,
                            XIIIConformiteFiscale = e.Ponderation.XIIIConformiteFiscale,
                            SommePonderations = e.Ponderation.SommePonderations
                        },
                        Resultat = e.Resultat != null ? new ResultatDto
                        {
                            Id = e.Resultat.Id,
                            NomResultat = e.Resultat.NomResultat,
                            Observation = e.Resultat.Observation
                        } : null
                    }).ToList()
                };

                return Ok(fournisseurDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du fournisseur avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la récupération des données");
            }
        }

        [HttpPost("fournisseur")]
        public async Task<ActionResult<FournisseurDto>> CreateFournisseur([FromBody] CreateFournisseurDto createFournisseurDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Vérifier que les entités de référence existent
                var categorie = await _context.Categories.FindAsync(createFournisseurDto.CategorieId);
                if (categorie == null)
                {
                    return BadRequest($"Catégorie avec ID {createFournisseurDto.CategorieId} non trouvée");
                }

                var nature = await _context.Natures.FindAsync(createFournisseurDto.NatureId);
                if (nature == null)
                {
                    return BadRequest($"Nature avec ID {createFournisseurDto.NatureId} non trouvée");
                }

                var site = await _context.Sites.FindAsync(createFournisseurDto.SiteId);
                if (site == null)
                {
                    return BadRequest($"Site avec ID {createFournisseurDto.SiteId} non trouvé");
                }

                Fabricant? fabricant = null;
                if (createFournisseurDto.FabricantId > 0)
                {
                    fabricant = await _context.Fabricants.FindAsync(createFournisseurDto.FabricantId);
                    if (fabricant == null)
                    {
                        return BadRequest($"Fabricant avec ID {createFournisseurDto.FabricantId} non trouvé");
                    }
                }

                // Créer le NomFournisseur si non fourni
                NomFournisseur nomFournisseur;
                if (createFournisseurDto.NomFournisseurId > 0)
                {
                    nomFournisseur = await _context.NomFournisseurs.FindAsync(createFournisseurDto.NomFournisseurId);
                    if (nomFournisseur == null)
                    {
                        return BadRequest($"NomFournisseur avec ID {createFournisseurDto.NomFournisseurId} non trouvé");
                    }
                }
                else
                {
                    // Créer un nouveau NomFournisseur
                    nomFournisseur = new NomFournisseur
                    {
                        VendorNumber = createFournisseurDto.VendorNumber ?? "",
                        VendorName = createFournisseurDto.VendorName ?? "",
                        CompanyCode = createFournisseurDto.CompanyCode,
                        ApDivision = createFournisseurDto.ApDivision,
                        StrClassStatus = createFournisseurDto.StrClassStatus,
                        SysCodeDescription = createFournisseurDto.SysCodeDescription,
                        SignificationCompanyCode = createFournisseurDto.SignificationCompanyCode,
                        SignificationApDivision = createFournisseurDto.SignificationApDivision,
                        StrVendorClass = createFournisseurDto.StrVendorClass,
                        CreditTermsCode = createFournisseurDto.CreditTermsCode,
                        CampagneId = createFournisseurDto.CampagneId
                    };

                    _context.NomFournisseurs.Add(nomFournisseur);
                    await _context.SaveChangesAsync();
                }

                var fournisseur = new Fournisseur
                {
                    NomFournisseurId = nomFournisseur.Id,
                    FabricantId = createFournisseurDto.FabricantId,
                    CategorieId = createFournisseurDto.CategorieId,
                    NatureId = createFournisseurDto.NatureId,
                    SiteId = createFournisseurDto.SiteId,
                    Email = createFournisseurDto.Email,
                    EmailEnvoye = false,
                    DateEnvoiEmail = null
                };

                _context.Fournisseurs.Add(fournisseur);
                await _context.SaveChangesAsync();

                var fournisseurDto = new FournisseurDto
                {
                    Id = fournisseur.Id,
                    NomFournisseurId = fournisseur.NomFournisseurId,
                    FabricantId = fournisseur.FabricantId,
                    CategorieId = fournisseur.CategorieId,
                    NatureId = fournisseur.NatureId,
                    SiteId = fournisseur.SiteId,
                    Email = fournisseur.Email,
                    EmailEnvoye = fournisseur.EmailEnvoye,
                    DateEnvoiEmail = fournisseur.DateEnvoiEmail
                };

                return CreatedAtAction(nameof(GetFournisseurById), new { id = fournisseur.Id }, fournisseurDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du fournisseur");
                return StatusCode(500, "Une erreur est survenue lors de la création du fournisseur");
            }
        }

        [HttpPut("fournisseur/{id}")]
        public async Task<IActionResult> UpdateFournisseur(int id, [FromBody] UpdateFournisseurDto updateFournisseurDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Vérifier que l'utilisateur est administrateur
                var userName = User.Identity?.Name;
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("Utilisateur non authentifié");
                }

                var shortUserName = userName.Contains('\\') 
                    ? userName.Split('\\')[1] 
                    : userName;

                var administrateur = await _context.Administrateurs
                    .FirstOrDefaultAsync(a => a.IdentiteUtilisateur.ToLower() == shortUserName.ToLower());

                if (administrateur == null)
                {
                    return Forbid("Seul un administrateur peut modifier un fournisseur");
                }

                var fournisseur = await _context.Fournisseurs
                    .Include(f => f.NomFournisseur)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (fournisseur == null)
                {
                    return NotFound($"Fournisseur avec ID {id} non trouvé");
                }

                // Vérifier que les entités de référence existent si fournies
                if (updateFournisseurDto.CategorieId.HasValue)
                {
                    var categorie = await _context.Categories.FindAsync(updateFournisseurDto.CategorieId.Value);
                    if (categorie == null)
                    {
                        return BadRequest($"Catégorie avec ID {updateFournisseurDto.CategorieId.Value} non trouvée");
                    }
                }

                if (updateFournisseurDto.NatureId.HasValue)
                {
                    var nature = await _context.Natures.FindAsync(updateFournisseurDto.NatureId.Value);
                    if (nature == null)
                    {
                        return BadRequest($"Nature avec ID {updateFournisseurDto.NatureId.Value} non trouvée");
                    }
                }

                if (updateFournisseurDto.SiteId.HasValue)
                {
                    var site = await _context.Sites.FindAsync(updateFournisseurDto.SiteId.Value);
                    if (site == null)
                    {
                        return BadRequest($"Site avec ID {updateFournisseurDto.SiteId.Value} non trouvé");
                    }
                }

                if (updateFournisseurDto.FabricantId.HasValue)
                {
                    var fabricant = await _context.Fabricants.FindAsync(updateFournisseurDto.FabricantId.Value);
                    if (fabricant == null)
                    {
                        return BadRequest($"Fabricant avec ID {updateFournisseurDto.FabricantId.Value} non trouvé");
                    }
                }

                // Mettre à jour le NomFournisseur si fourni
                if (updateFournisseurDto.NomFournisseur != null)
                {
                    var nomFournisseur = fournisseur.NomFournisseur;
                    
                    // Mettre à jour les propriétés du NomFournisseur
                    if (updateFournisseurDto.NomFournisseur.VendorNumber != null)
                        nomFournisseur.VendorNumber = updateFournisseurDto.NomFournisseur.VendorNumber;
                    
                    if (updateFournisseurDto.NomFournisseur.VendorName != null)
                        nomFournisseur.VendorName = updateFournisseurDto.NomFournisseur.VendorName;
                    
                    if (updateFournisseurDto.NomFournisseur.CompanyCode != null)
                        nomFournisseur.CompanyCode = updateFournisseurDto.NomFournisseur.CompanyCode;
                    
                    if (updateFournisseurDto.NomFournisseur.ApDivision != null)
                        nomFournisseur.ApDivision = updateFournisseurDto.NomFournisseur.ApDivision;
                    
                    if (updateFournisseurDto.NomFournisseur.StrClassStatus != null)
                        nomFournisseur.StrClassStatus = updateFournisseurDto.NomFournisseur.StrClassStatus;
                    
                    if (updateFournisseurDto.NomFournisseur.SysCodeDescription != null)
                        nomFournisseur.SysCodeDescription = updateFournisseurDto.NomFournisseur.SysCodeDescription;
                    
                    if (updateFournisseurDto.NomFournisseur.SignificationCompanyCode != null)
                        nomFournisseur.SignificationCompanyCode = updateFournisseurDto.NomFournisseur.SignificationCompanyCode;
                    
                    if (updateFournisseurDto.NomFournisseur.SignificationApDivision != null)
                        nomFournisseur.SignificationApDivision = updateFournisseurDto.NomFournisseur.SignificationApDivision;
                    
                    if (updateFournisseurDto.NomFournisseur.StrVendorClass != null)
                        nomFournisseur.StrVendorClass = updateFournisseurDto.NomFournisseur.StrVendorClass;
                    
                    if (updateFournisseurDto.NomFournisseur.CreditTermsCode != null)
                        nomFournisseur.CreditTermsCode = updateFournisseurDto.NomFournisseur.CreditTermsCode;
                    
                    if (updateFournisseurDto.NomFournisseur.CampagneId.HasValue)
                        nomFournisseur.CampagneId = updateFournisseurDto.NomFournisseur.CampagneId.Value;

                    _context.NomFournisseurs.Update(nomFournisseur);
                }

                // Mettre à jour le fournisseur
                if (updateFournisseurDto.FabricantId.HasValue)
                    fournisseur.FabricantId = updateFournisseurDto.FabricantId.Value;
                
                if (updateFournisseurDto.CategorieId.HasValue)
                    fournisseur.CategorieId = updateFournisseurDto.CategorieId.Value;
                
                if (updateFournisseurDto.NatureId.HasValue)
                    fournisseur.NatureId = updateFournisseurDto.NatureId.Value;
                
                if (updateFournisseurDto.SiteId.HasValue)
                    fournisseur.SiteId = updateFournisseurDto.SiteId.Value;
                
                if (updateFournisseurDto.Email != null)
                    fournisseur.Email = updateFournisseurDto.Email;

                _context.Fournisseurs.Update(fournisseur);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du fournisseur avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour du fournisseur");
            }
        }

        [HttpDelete("fournisseur/{id}")]
        public async Task<IActionResult> DeleteFournisseur(int id)
        {
            try
            {
                // Vérifier que l'utilisateur est administrateur
                var userName = User.Identity?.Name;
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("Utilisateur non authentifié");
                }

                var shortUserName = userName.Contains('\\') 
                    ? userName.Split('\\')[1] 
                    : userName;

                var administrateur = await _context.Administrateurs
                    .FirstOrDefaultAsync(a => a.IdentiteUtilisateur.ToLower() == shortUserName.ToLower());

                if (administrateur == null)
                {
                    return Forbid("Seul un administrateur peut supprimer un fournisseur");
                }

                var fournisseur = await _context.Fournisseurs
                    .Include(f => f.Evaluations)
                    .Include(f => f.NomFournisseur)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (fournisseur == null)
                {
                    return NotFound($"Fournisseur avec ID {id} non trouvé");
                }

                // Vérifier si le fournisseur a des évaluations
                if (fournisseur.Evaluations.Any())
                {
                    return BadRequest("Impossible de supprimer ce fournisseur car il a des évaluations associées");
                }

                var nomFournisseurId = fournisseur.NomFournisseurId;

                // Supprimer le fournisseur
                _context.Fournisseurs.Remove(fournisseur);
                await _context.SaveChangesAsync();

                // Vérifier si le NomFournisseur est utilisé par d'autres fournisseurs
                var autresFournisseurs = await _context.Fournisseurs
                    .AnyAsync(f => f.NomFournisseurId == nomFournisseurId);

                if (!autresFournisseurs)
                {
                    // Supprimer le NomFournisseur s'il n'est plus utilisé
                    var nomFournisseur = await _context.NomFournisseurs.FindAsync(nomFournisseurId);
                    if (nomFournisseur != null)
                    {
                        _context.NomFournisseurs.Remove(nomFournisseur);
                        await _context.SaveChangesAsync();
                    }
                }

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du fournisseur avec ID {Id}", id);
                return BadRequest("Impossible de supprimer ce fournisseur car il est utilisé par d'autres entités");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du fournisseur avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la suppression du fournisseur");
            }
        }
    }
}
