namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class StatistiquesCampagneDto
    {
        public int CampagneId { get; set; }
        public string NomCampagne { get; set; } = string.Empty;
        public short Annee { get; set; }
        public int NombreTotalFournisseurs { get; set; }
        public int NombreFournisseursEvalues { get; set; }
        public int NombreFournisseursNonEvalues { get; set; }
        public int NombreTotalEvaluations { get; set; }
        public double PourcentageEvalues { get; set; }
    }
}
