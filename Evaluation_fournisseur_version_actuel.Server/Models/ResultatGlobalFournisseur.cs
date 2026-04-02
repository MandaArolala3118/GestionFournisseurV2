namespace evaluation_fournisseur_version_actuel.Server.Models;

/// <summary>
/// Projection de la vue SQL <c>v_ResultatGlobal_Fournisseur</c>.
/// Utilisée en lecture seule — ne pas persister directement.
/// </summary>
public class ResultatGlobalFournisseur
{
    public int     FournisseurId          { get; set; }
    public string  VendorNumber           { get; set; } = string.Empty;
    public string  VendorName             { get; set; } = string.Empty;
    public short   Annee                  { get; set; }
    public string  NomCampagne            { get; set; } = string.Empty;
    public int     NbreEvaluations        { get; set; }
    public double? MoyenneGlobalePct      { get; set; }
    public string? ResultatGlobal         { get; set; }
    public string? ResultatGlobalCalcule  { get; set; }
}
