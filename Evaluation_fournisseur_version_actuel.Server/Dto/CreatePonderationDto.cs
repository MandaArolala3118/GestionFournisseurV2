using System.ComponentModel.DataAnnotations;

namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class CreatePonderationDto
    {
        [Required(ErrorMessage = "La catégorie est requise")]
        public int CategorieId { get; set; }

        [Range(0, 100, ErrorMessage = "La pondération doit être entre 0 et 100")]
        public double ITransparence { get; set; }

        [Range(0, 100, ErrorMessage = "La pondération doit être entre 0 et 100")]
        public double IIFacture { get; set; }

        [Range(0, 100, ErrorMessage = "La pondération doit être entre 0 et 100")]
        public double IIINotoriete { get; set; }

        [Range(0, 100, ErrorMessage = "La pondération doit être entre 0 et 100")]
        public double IVPreventionCorruption { get; set; }

        [Range(0, 100, ErrorMessage = "La pondération doit être entre 0 et 100")]
        public double VPreventionDroitHomme { get; set; }

        [Range(0, 100, ErrorMessage = "La pondération doit être entre 0 et 100")]
        public double VIConformiteVsExigence { get; set; }

        [Range(0, 100, ErrorMessage = "La pondération doit être entre 0 et 100")]
        public double VIIDelaiLivraison { get; set; }

        [Range(0, 100, ErrorMessage = "La pondération doit être entre 0 et 100")]
        public double VIIICommunication { get; set; }

        [Range(0, 100, ErrorMessage = "La pondération doit être entre 0 et 100")]
        public double IXDisponibiliteDocuments { get; set; }

        [Range(0, 100, ErrorMessage = "La pondération doit être entre 0 et 100")]
        public double XConsigneQhse { get; set; }

        [Range(0, 100, ErrorMessage = "La pondération doit être entre 0 et 100")]
        public double XIQualitePrix { get; set; }

        [Range(0, 100, ErrorMessage = "La pondération doit être entre 0 et 100")]
        public double XIIDureePaiement { get; set; }

        [Range(0, 100, ErrorMessage = "La pondération doit être entre 0 et 100")]
        public double XIIIConformiteFiscale { get; set; }
    }
}
