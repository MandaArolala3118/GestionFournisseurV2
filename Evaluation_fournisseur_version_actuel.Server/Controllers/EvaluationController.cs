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
    }
}
