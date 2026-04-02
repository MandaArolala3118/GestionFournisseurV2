namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class FournisseurDto
    {
        public int Id { get; set; }
        public int NomFournisseurId { get; set; }
        public int? FabricantId { get; set; }
        public int CategorieId { get; set; }
        public int NatureId { get; set; }
        public int SiteId { get; set; }
        public string Email { get; set; }
        public bool EmailEnvoye { get; set; }
        public DateTime? DateEnvoiEmail { get; set; }
    }
}
