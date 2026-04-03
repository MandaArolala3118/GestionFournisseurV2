using Microsoft.EntityFrameworkCore;
using evaluation_fournisseur_version_actuel.Server.Data;
using evaluation_fournisseur_version_actuel.Server.Models;

namespace evaluation_fournisseur_version_actuel.Server.Services
{
    public class ResultatGlobalService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ResultatGlobalService> _logger;

        public ResultatGlobalService(AppDbContext context, ILogger<ResultatGlobalService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task UpdateResultatGlobalAsync(int fournisseurId, short annee)
        {
            try
            {
                // Récupérer toutes les évaluations du fournisseur pour l'année donnée
                var evaluations = await _context.Evaluations
                    .Include(e => e.Fournisseur)
                        .ThenInclude(f => f.NomFournisseur)
                    .Include(e => e.Campagne)
                    .Include(e => e.Resultat)
                    .Where(e => e.FournisseurId == fournisseurId && e.Campagne.Annee == annee)
                    .ToListAsync();

                if (!evaluations.Any())
                {
                    _logger.LogWarning("Aucune évaluation trouvée pour le fournisseur {FournisseurId} et l'année {Annee}", fournisseurId, annee);
                    return;
                }

                // Calculer la moyenne globale
                double moyenneGlobale = evaluations.Average(e => e.MoyennePct ?? 0);

                // Déterminer le résultat global
                string resultatGlobal = moyenneGlobale >= 75 ? "SATISFAISANT" : 
                                   moyenneGlobale >= 50 ? "SOUS RESERVE" : "NON SATISFAISANT";

                // Récupérer le fournisseur et la campagne
                var fournisseur = evaluations.First().Fournisseur;
                var campagne = evaluations.First().Campagne;

                _logger.LogInformation("Résultat global calculé pour le fournisseur {FournisseurId}, année {Annee}: {Resultat} (Moyenne: {Moyenne:F2}%)", 
                    fournisseurId, annee, resultatGlobal, moyenneGlobale);

                // Mettre à jour la vue via SQL direct
                var sql = @"
                    MERGE v_ResultatGlobal_Fournisseur AS target
                    USING (SELECT {0} AS FournisseurId, {1} AS Annee) AS source
                    ON target.FournisseurId = source.FournisseurId AND target.Annee = source.Annee
                    WHEN MATCHED THEN
                        UPDATE SET 
                            VendorNumber = {2},
                            VendorName = {3},
                            NomCampagne = {4},
                            NbreEvaluations = {5},
                            MoyenneGlobalePct = {6},
                            ResultatGlobal = {7},
                            ResultatGlobalCalcule = {8}
                    WHEN NOT MATCHED THEN
                        INSERT (FournisseurId, Annee, VendorNumber, VendorName, NomCampagne, NbreEvaluations, MoyenneGlobalePct, ResultatGlobal, ResultatGlobalCalcule)
                        VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8});";

                var vendorNumber = fournisseur.NomFournisseur?.VendorNumber ?? "";
                var vendorName = fournisseur.NomFournisseur?.VendorName ?? "";
                var nomCampagne = campagne.NomCampagne;
                var nbreEvaluations = evaluations.Count;
                var moyenneValue = moyenneGlobale;
                var resultatValue = resultatGlobal;
                var resultatGlobalCalcule = $"Moyenne: {moyenneGlobale:F2}% - {resultatGlobal}";

                await _context.Database.ExecuteSqlRawAsync(sql, 
                    fournisseurId, annee, vendorNumber, vendorName, nomCampagne, 
                    nbreEvaluations, moyenneValue, resultatValue, resultatGlobalCalcule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du résultat global pour le fournisseur {FournisseurId}, année {Annee}", 
                    fournisseurId, annee);
                throw;
            }
        }
    }
}
