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
    }
}
