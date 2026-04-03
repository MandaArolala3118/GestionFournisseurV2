using Microsoft.EntityFrameworkCore;

namespace evaluation_fournisseur_version_actuel.Server.Models
{
    /// <summary>
    /// Vue SQL <c>v_ResultatGlobal_Fournisseur</c>.
    /// Se met à jour automatiquement à chaque insertion/modification d'évaluation.
    /// </summary>
    public class ResultatGlobalFournisseur
    {
        public int FournisseurId { get; set; }
        public short Annee { get; set; }
        public string VendorNumber { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public string NomCampagne { get; set; } = string.Empty;
        public int NbreEvaluations { get; set; }
        public double? MoyenneGlobalePct { get; set; }
        public string? ResultatGlobal { get; set; }
        public string? ResultatGlobalCalcule { get; set; }
    }
}
