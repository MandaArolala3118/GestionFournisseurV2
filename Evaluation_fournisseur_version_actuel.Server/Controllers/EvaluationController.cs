using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using evaluation_fournisseur_version_actuel.Server.Models;
using evaluation_fournisseur_version_actuel.Server.Data;
using evaluation_fournisseur_version_actuel.Server.Dto;
using evaluation_fournisseur_version_actuel.Server.Dto.Statistiques;

namespace Evaluation_fournisseur_version_actuel.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EvaluationController> _logger;

        public EvaluationController(AppDbContext context, ILogger<EvaluationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("statistiques-campagne-active")]
        public async Task<ActionResult<StatistiquesCampagneDto>> GetStatistiquesCampagneActive()
        {
            try
            {
                // Récupérer la campagne active
                var campagneActive = await _context.Campagnes
                    .FirstOrDefaultAsync(c => c.Active);

                if (campagneActive == null)
                {
                    return NotFound("Aucune campagne active trouvée");
                }

                var campagneId = campagneActive.Id;

                // Nombre total de fournisseurs pour la campagne active
                var nombreTotalFournisseurs = await _context.Fournisseurs
                    .CountAsync(f => f.NomFournisseur.CampagneId == campagneId);

                // Liste des fournisseurs de la campagne active
                var fournisseursCampagne = await _context.Fournisseurs
                    .Where(f => f.NomFournisseur.CampagneId == campagneId)
                    .Select(f => f.Id)
                    .ToListAsync();

                // Nombre de fournisseurs évalués (ayant au moins une évaluation)
                var nombreFournisseursEvalues = await _context.Evaluations
                    .Where(e => fournisseursCampagne.Contains(e.FournisseurId))
                    .Select(e => e.FournisseurId)
                    .Distinct()
                    .CountAsync();

                // Nombre de fournisseurs non évalués
                var nombreFournisseursNonEvalues = nombreTotalFournisseurs - nombreFournisseursEvalues;

                // Nombre total d'évaluations pour la campagne active
                var nombreTotalEvaluations = await _context.Evaluations
                    .CountAsync(e => fournisseursCampagne.Contains(e.FournisseurId));

                var statistiques = new StatistiquesCampagneDto
                {
                    CampagneId = campagneId,
                    NomCampagne = campagneActive.NomCampagne,
                    Annee = campagneActive.Annee,
                    NombreTotalFournisseurs = nombreTotalFournisseurs,
                    NombreFournisseursEvalues = nombreFournisseursEvalues,
                    NombreFournisseursNonEvalues = nombreFournisseursNonEvalues,
                    NombreTotalEvaluations = nombreTotalEvaluations,
                    PourcentageEvalues = nombreTotalFournisseurs > 0 
                        ? Math.Round((double)nombreFournisseursEvalues / nombreTotalFournisseurs * 100, 2)
                        : 0
                };

                return Ok(statistiques);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des statistiques de la campagne active");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des statistiques");
            }
        }

        [HttpGet("statistiques-campagne/{campagneId}")]
        public async Task<ActionResult<StatistiquesCampagneDto>> GetStatistiquesCampagne(int campagneId)
        {
            try
            {
                // Vérifier que la campagne existe
                var campagne = await _context.Campagnes.FindAsync(campagneId);
                if (campagne == null)
                {
                    return NotFound($"Campagne avec ID {campagneId} non trouvée");
                }

                // Nombre total de fournisseurs pour la campagne
                var nombreTotalFournisseurs = await _context.Fournisseurs
                    .CountAsync(f => f.NomFournisseur.CampagneId == campagneId);

                // Liste des fournisseurs de la campagne
                var fournisseursCampagne = await _context.Fournisseurs
                    .Where(f => f.NomFournisseur.CampagneId == campagneId)
                    .Select(f => f.Id)
                    .ToListAsync();

                // Nombre de fournisseurs évalués (ayant au moins une évaluation)
                var nombreFournisseursEvalues = await _context.Evaluations
                    .Where(e => fournisseursCampagne.Contains(e.FournisseurId))
                    .Select(e => e.FournisseurId)
                    .Distinct()
                    .CountAsync();

                // Nombre de fournisseurs non évalués
                var nombreFournisseursNonEvalues = nombreTotalFournisseurs - nombreFournisseursEvalues;

                // Nombre total d'évaluations pour la campagne
                var nombreTotalEvaluations = await _context.Evaluations
                    .CountAsync(e => fournisseursCampagne.Contains(e.FournisseurId));

                var statistiques = new StatistiquesCampagneDto
                {
                    CampagneId = campagneId,
                    NomCampagne = campagne.NomCampagne,
                    Annee = campagne.Annee,
                    NombreTotalFournisseurs = nombreTotalFournisseurs,
                    NombreFournisseursEvalues = nombreFournisseursEvalues,
                    NombreFournisseursNonEvalues = nombreFournisseursNonEvalues,
                    NombreTotalEvaluations = nombreTotalEvaluations,
                    PourcentageEvalues = nombreTotalFournisseurs > 0 
                        ? Math.Round((double)nombreFournisseursEvalues / nombreTotalFournisseurs * 100, 2)
                        : 0
                };

                return Ok(statistiques);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des statistiques de la campagne {CampagneId}", campagneId);
                return StatusCode(500, "Une erreur est survenue lors de la récupération des statistiques");
            }
        }

        [HttpGet("fournisseurs-detailles")]
        public async Task<ActionResult<IEnumerable<FournisseurDetailStatistiqueDto>>> GetFournisseurDetailles()
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
                        .ThenInclude(e => e.Resultat)
                    .Select(f => new FournisseurDetailStatistiqueDto
                    {
                        Id = f.Id,
                        Nom = f.NomFournisseur.VendorName,
                        MoyennePct = f.Evaluations.Any() 
                            ? f.Evaluations.Average(e => e.MoyennePct) 
                            : (double?)null,
                        Resultat = f.Evaluations.OrderByDescending(e => e.DateEvaluation).FirstOrDefault() != null
                            ? new ResultatSimpleDto
                            {
                                Id = f.Evaluations.OrderByDescending(e => e.DateEvaluation).FirstOrDefault().Resultat.Id,
                                NomResultat = f.Evaluations.OrderByDescending(e => e.DateEvaluation).FirstOrDefault().Resultat.NomResultat
                            }
                            : null,
                        Fabricant = f.Fabricant != null ? f.Fabricant.NomFabricant : null,
                        Categorie = f.Categorie.NomCategorie,
                        NombreEvaluations = f.Evaluations.Count,
                        DateDerniereEvaluation = f.Evaluations.Any()
                            ? f.Evaluations.Max(e => e.DateEvaluation)
                            : (DateTime?)null
                    })
                    .ToListAsync();

                return Ok(fournisseurs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des fournisseurs détaillés");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des fournisseurs détaillés");
            }
        }

        [HttpPost("create-evaluation")]
        public async Task<ActionResult<EvaluationDto>> CreateEvaluation([FromBody] CreateEvaluationDto createEvaluationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Vérifier que le fournisseur existe
                var fournisseur = await _context.Fournisseurs
                    .Include(f => f.Categorie)
                    .ThenInclude(c => c.Ponderation)
                    .FirstOrDefaultAsync(f => f.Id == createEvaluationDto.FournisseurId);

                if (fournisseur == null)
                {
                    return NotFound($"Fournisseur avec ID {createEvaluationDto.FournisseurId} non trouvé");
                }

                // Vérifier que la catégorie a une pondération
                if (fournisseur.Categorie?.Ponderation == null)
                {
                    return BadRequest($"La catégorie du fournisseur n'a pas de pondération associée");
                }

                // Vérifier que la campagne existe
                var campagne = await _context.Campagnes.FindAsync(createEvaluationDto.CampagneId);
                if (campagne == null)
                {
                    return NotFound($"Campagne avec ID {createEvaluationDto.CampagneId} non trouvée");
                }

                var ponderation = fournisseur.Categorie.Ponderation;

                // Créer l'évaluation
                var evaluation = new Evaluation
                {
                    FournisseurId = createEvaluationDto.FournisseurId,
                    PonderationId = ponderation.Id,
                    CampagneId = campagne.Id,
                    ITransparence = createEvaluationDto.ITransparence,
                    IIFacture = createEvaluationDto.IIFacture,
                    IIINotoriete = createEvaluationDto.IIINotoriete,
                    IVPreventionCorruption = createEvaluationDto.IVPreventionCorruption,
                    VPreventionDroitHomme = createEvaluationDto.VPreventionDroitHomme,
                    VIConformiteVsExigence = createEvaluationDto.VIConformiteVsExigence,
                    VIIDelaiLivraison = createEvaluationDto.VIIDelaiLivraison,
                    VIIICommunication = createEvaluationDto.VIIICommunication,
                    IXDisponibiliteDocuments = createEvaluationDto.IXDisponibiliteDocuments,
                    XConsigneQhse = createEvaluationDto.XConsigneQhse,
                    XIQualitePrix = createEvaluationDto.XIQualitePrix,
                    XIIDureePaiement = createEvaluationDto.XIIDureePaiement,
                    XIIIConformiteFiscale = createEvaluationDto.XIIIConformiteFiscale,
                    Observation = createEvaluationDto.Observation,
                    PropositionAmelioration = createEvaluationDto.PropositionAmelioration,
                    ObservationsFournisseur = createEvaluationDto.ObservationsFournisseur,
                    Responsable = createEvaluationDto.Responsable,
                    NumeroActionPac = createEvaluationDto.NumeroActionPac,
                    Deadline = createEvaluationDto.Deadline,
                    DateEvaluation = DateTime.UtcNow
                };

                // Calculer la moyenne
                evaluation.RecalculerMoyenne(ponderation);

                // Calculer automatiquement le résultat
                evaluation.ResultatId = evaluation.CalculerResultatId();

                _context.Evaluations.Add(evaluation);
                await _context.SaveChangesAsync();

                // Créer le DTO de retour
                var evaluationDto = new EvaluationDto
                {
                    Id = evaluation.Id,
                    FournisseurId = evaluation.FournisseurId,
                    PonderationId = evaluation.PonderationId,
                    CampagneId = evaluation.CampagneId,
                    ResultatId = evaluation.ResultatId,
                    ITransparence = evaluation.ITransparence,
                    IIFacture = evaluation.IIFacture,
                    IIINotoriete = evaluation.IIINotoriete,
                    IVPreventionCorruption = evaluation.IVPreventionCorruption,
                    VPreventionDroitHomme = evaluation.VPreventionDroitHomme,
                    VIConformiteVsExigence = evaluation.VIConformiteVsExigence,
                    VIIDelaiLivraison = evaluation.VIIDelaiLivraison,
                    VIIICommunication = evaluation.VIIICommunication,
                    IXDisponibiliteDocuments = evaluation.IXDisponibiliteDocuments,
                    XConsigneQhse = evaluation.XConsigneQhse,
                    XIQualitePrix = evaluation.XIQualitePrix,
                    XIIDureePaiement = evaluation.XIIDureePaiement,
                    XIIIConformiteFiscale = evaluation.XIIIConformiteFiscale,
                    MoyennePct = evaluation.MoyennePct,
                    Observation = evaluation.Observation,
                    PropositionAmelioration = evaluation.PropositionAmelioration,
                    ObservationsFournisseur = evaluation.ObservationsFournisseur,
                    Responsable = evaluation.Responsable,
                    NumeroActionPac = evaluation.NumeroActionPac,
                    Deadline = evaluation.Deadline,
                    DateEvaluation = evaluation.DateEvaluation,
                    Campagne = new CampagneDto
                    {
                        Id = campagne.Id,
                        Annee = campagne.Annee,
                        NomCampagne = campagne.NomCampagne,
                        Active = campagne.Active
                    },
                    Ponderation = new PonderationDto
                    {
                        Id = ponderation.Id,
                        CategorieId = ponderation.CategorieId,
                        ITransparence = ponderation.ITransparence,
                        IIFacture = ponderation.IIFacture,
                        IIINotoriete = ponderation.IIINotoriete,
                        IVPreventionCorruption = ponderation.IVPreventionCorruption,
                        VPreventionDroitHomme = ponderation.VPreventionDroitHomme,
                        VIConformiteVsExigence = ponderation.VIConformiteVsExigence,
                        VIIDelaiLivraison = ponderation.VIIDelaiLivraison,
                        VIIICommunication = ponderation.VIIICommunication,
                        IXDisponibiliteDocuments = ponderation.IXDisponibiliteDocuments,
                        XConsigneQhse = ponderation.XConsigneQhse,
                        XIQualitePrix = ponderation.XIQualitePrix,
                        XIIDureePaiement = ponderation.XIIDureePaiement,
                        XIIIConformiteFiscale = ponderation.XIIIConformiteFiscale,
                        SommePonderations = ponderation.SommePonderations
                    }
                };

                return CreatedAtAction(nameof(GetEvaluationById), new { id = evaluation.Id }, evaluationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de l'évaluation");
                return StatusCode(500, "Une erreur est survenue lors de la création de l'évaluation");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluationDto>> GetEvaluationById(int id)
        {
            try
            {
                var evaluation = await _context.Evaluations
                    .Include(e => e.Fournisseur)
                        .ThenInclude(f => f.NomFournisseur)
                    .Include(e => e.Ponderation)
                    .Include(e => e.Campagne)
                    .Include(e => e.Resultat)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (evaluation == null)
                {
                    return NotFound($"Évaluation avec ID {id} non trouvée");
                }

                var evaluationDto = new EvaluationDto
                {
                    Id = evaluation.Id,
                    FournisseurId = evaluation.FournisseurId,
                    PonderationId = evaluation.PonderationId,
                    CampagneId = evaluation.CampagneId,
                    ResultatId = evaluation.ResultatId,
                    ITransparence = evaluation.ITransparence,
                    IIFacture = evaluation.IIFacture,
                    IIINotoriete = evaluation.IIINotoriete,
                    IVPreventionCorruption = evaluation.IVPreventionCorruption,
                    VPreventionDroitHomme = evaluation.VPreventionDroitHomme,
                    VIConformiteVsExigence = evaluation.VIConformiteVsExigence,
                    VIIDelaiLivraison = evaluation.VIIDelaiLivraison,
                    VIIICommunication = evaluation.VIIICommunication,
                    IXDisponibiliteDocuments = evaluation.IXDisponibiliteDocuments,
                    XConsigneQhse = evaluation.XConsigneQhse,
                    XIQualitePrix = evaluation.XIQualitePrix,
                    XIIDureePaiement = evaluation.XIIDureePaiement,
                    XIIIConformiteFiscale = evaluation.XIIIConformiteFiscale,
                    MoyennePct = evaluation.MoyennePct,
                    Observation = evaluation.Observation,
                    PropositionAmelioration = evaluation.PropositionAmelioration,
                    ObservationsFournisseur = evaluation.ObservationsFournisseur,
                    Responsable = evaluation.Responsable,
                    NumeroActionPac = evaluation.NumeroActionPac,
                    Deadline = evaluation.Deadline,
                    DateEvaluation = evaluation.DateEvaluation,
                    Campagne = new CampagneDto
                    {
                        Id = evaluation.Campagne.Id,
                        Annee = evaluation.Campagne.Annee,
                        NomCampagne = evaluation.Campagne.NomCampagne,
                        Active = evaluation.Campagne.Active
                    },
                    Ponderation = new PonderationDto
                    {
                        Id = evaluation.Ponderation.Id,
                        CategorieId = evaluation.Ponderation.CategorieId,
                        ITransparence = evaluation.Ponderation.ITransparence,
                        IIFacture = evaluation.Ponderation.IIFacture,
                        IIINotoriete = evaluation.Ponderation.IIINotoriete,
                        IVPreventionCorruption = evaluation.Ponderation.IVPreventionCorruption,
                        VPreventionDroitHomme = evaluation.Ponderation.VPreventionDroitHomme,
                        VIConformiteVsExigence = evaluation.Ponderation.VIConformiteVsExigence,
                        VIIDelaiLivraison = evaluation.Ponderation.VIIDelaiLivraison,
                        VIIICommunication = evaluation.Ponderation.VIIICommunication,
                        IXDisponibiliteDocuments = evaluation.Ponderation.IXDisponibiliteDocuments,
                        XConsigneQhse = evaluation.Ponderation.XConsigneQhse,
                        XIQualitePrix = evaluation.Ponderation.XIQualitePrix,
                        XIIDureePaiement = evaluation.Ponderation.XIIDureePaiement,
                        XIIIConformiteFiscale = evaluation.Ponderation.XIIIConformiteFiscale,
                        SommePonderations = evaluation.Ponderation.SommePonderations
                    }
                };

                return Ok(evaluationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de l'évaluation avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la récupération de l'évaluation");
            }
        }
    }
}
