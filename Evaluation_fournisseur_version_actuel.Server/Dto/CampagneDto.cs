namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class CampagneDto
    {
        public int Id { get; set; }
        public short Annee { get; set; }
        public string NomCampagne { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
}
