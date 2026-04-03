namespace evaluation_fournisseur_version_actuel.Server.Dto.Statistiques
{
    public class FournisseurDetailStatistiqueDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public double? MoyennePct { get; set; }
        public ResultatSimpleDto? Resultat { get; set; }
        public string? Fabricant { get; set; }
        public string Categorie { get; set; } = string.Empty;
        public int NombreEvaluations { get; set; }
        public DateTime? DateDerniereEvaluation { get; set; }
    }

    public class ResultatSimpleDto
    {
        public int Id { get; set; }
        public string NomResultat { get; set; } = string.Empty;
    }
}
