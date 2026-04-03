namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class FournisseurDetailDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public bool EmailEnvoye { get; set; }
        public DateTime? DateEnvoiEmail { get; set; }
        public NomFournisseurDto NomFournisseur { get; set; } = null!;
        public FabricantDto? Fabricant { get; set; }
        public CategorieDto Categorie { get; set; } = null!;
        public NatureDto Nature { get; set; } = null!;
        public SiteDto Site { get; set; } = null!;
        public List<EvaluationDto> Evaluations { get; set; } = [];
    }
}
