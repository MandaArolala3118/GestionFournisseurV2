namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class EvaluationDto
    {
        public int Id { get; set; }
        public int FournisseurId { get; set; }
        public int PonderationId { get; set; }
        public int CampagneId { get; set; }
        public int? ResultatId { get; set; }
        public double? ITransparence { get; set; }
        public double? IIFacture { get; set; }
        public double? IIINotoriete { get; set; }
        public double? IVPreventionCorruption { get; set; }
        public double? VPreventionDroitHomme { get; set; }
        public double? VIConformiteVsExigence { get; set; }
        public double? VIIDelaiLivraison { get; set; }
        public double? VIIICommunication { get; set; }
        public double? IXDisponibiliteDocuments { get; set; }
        public double? XConsigneQhse { get; set; }
        public double? XIQualitePrix { get; set; }
        public double? XIIDureePaiement { get; set; }
        public double? XIIIConformiteFiscale { get; set; }
        public double? MoyennePct { get; set; }
        public string? Observation { get; set; }
        public string? PropositionAmelioration { get; set; }
        public string? ObservationsFournisseur { get; set; }
        public string Responsable { get; set; } = string.Empty;
        public string? NumeroActionPac { get; set; }
        public DateOnly? Deadline { get; set; }
        public DateTime DateEvaluation { get; set; }
        public CampagneDto Campagne { get; set; } = null!;
        public PonderationDto Ponderation { get; set; } = null!;
        public ResultatDto? Resultat { get; set; }
    }
}
