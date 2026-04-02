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
    }
}
