using System.ComponentModel.DataAnnotations;

namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class UpdateEvaluationDto
    {
        public int? CampagneId { get; set; }

        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5")]
        public double? ITransparence { get; set; }

        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5")]
        public double? IIFacture { get; set; }

        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5")]
        public double? IIINotoriete { get; set; }

        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5")]
        public double? IVPreventionCorruption { get; set; }

        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5")]
        public double? VPreventionDroitHomme { get; set; }

        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5")]
        public double? VIConformiteVsExigence { get; set; }

        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5")]
        public double? VIIDelaiLivraison { get; set; }

        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5")]
        public double? VIIICommunication { get; set; }

        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5")]
        public double? IXDisponibiliteDocuments { get; set; }

        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5")]
        public double? XConsigneQhse { get; set; }

        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5")]
        public double? XIQualitePrix { get; set; }

        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5")]
        public double? XIIDureePaiement { get; set; }

        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5")]
        public double? XIIIConformiteFiscale { get; set; }

        [MaxLength(1000, ErrorMessage = "L'observation ne peut pas dépasser 1000 caractères")]
        public string? Observation { get; set; }

        [MaxLength(1000, ErrorMessage = "La proposition d'amélioration ne peut pas dépasser 1000 caractères")]
        public string? PropositionAmelioration { get; set; }

        [MaxLength(1000, ErrorMessage = "Les observations fournisseur ne peuvent pas dépasser 1000 caractères")]
        public string? ObservationsFournisseur { get; set; }

        [MaxLength(200, ErrorMessage = "Le responsable ne peut pas dépasser 200 caractères")]
        public string? Responsable { get; set; }

        [MaxLength(100, ErrorMessage = "Le numéro d'action PAC ne peut pas dépasser 100 caractères")]
        public string? NumeroActionPac { get; set; }

        public DateOnly? Deadline { get; set; }
    }
}
