namespace evaluation_fournisseur_version_actuel.Server.Dto;

public class FournisseurCampagneDto
{
    public int IdFournisseur { get; set; }
    public string VendorNameFournisseur { get; set; } = string.Empty;
    public string? Fabricant { get; set; }
    public string Categorie { get; set; } = string.Empty;
    public string Nature { get; set; } = string.Empty;
    public bool Evaluation { get; set; }
}
