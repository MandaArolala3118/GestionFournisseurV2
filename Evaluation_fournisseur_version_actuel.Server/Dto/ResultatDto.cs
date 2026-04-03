namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class ResultatDto
    {
        public int Id { get; set; }
        public string NomResultat { get; set; } = string.Empty;
        public string? Observation { get; set; }
    }
}
