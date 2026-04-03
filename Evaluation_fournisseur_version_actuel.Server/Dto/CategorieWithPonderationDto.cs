namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class CategorieWithPonderationDto
    {
        public int Id { get; set; }
        public string NomCategorie { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? PonderationId { get; set; }
        public PonderationDto? Ponderation { get; set; }
    }
}
