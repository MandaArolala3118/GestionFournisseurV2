namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class PonderationDto
    {
        public int Id { get; set; }
        public int CategorieId { get; set; }
        public double ITransparence { get; set; }
        public double IIFacture { get; set; }
        public double IIINotoriete { get; set; }
        public double IVPreventionCorruption { get; set; }
        public double VPreventionDroitHomme { get; set; }
        public double VIConformiteVsExigence { get; set; }
        public double VIIDelaiLivraison { get; set; }
        public double VIIICommunication { get; set; }
        public double IXDisponibiliteDocuments { get; set; }
        public double XConsigneQhse { get; set; }
        public double XIQualitePrix { get; set; }
        public double XIIDureePaiement { get; set; }
        public double XIIIConformiteFiscale { get; set; }
        public double SommePonderations { get; set; }
        public CategorieDto? Categorie { get; set; }
    }
}
