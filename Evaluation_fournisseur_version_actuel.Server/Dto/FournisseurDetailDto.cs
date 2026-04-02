namespace evaluation_fournisseur_version_actuel.Server.Dto;

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

public class NomFournisseurDto
{
    public int Id { get; set; }
    public string VendorNumber { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public string? CompanyCode { get; set; }
    public string? ApDivision { get; set; }
    public string? StrClassStatus { get; set; }
    public string? SysCodeDescription { get; set; }
    public string? SignificationCompanyCode { get; set; }
    public string? SignificationApDivision { get; set; }
    public string? StrVendorClass { get; set; }
    public string? CreditTermsCode { get; set; }
    public CampagneDto Campagne { get; set; } = null!;
}

public class FabricantDto
{
    public int Id { get; set; }
    public string NomFabricant { get; set; } = string.Empty;
}

public class CategorieDto
{
    public int Id { get; set; }
    public string NomCategorie { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? PonderationId { get; set; }
    public PonderationDto? Ponderation { get; set; }
}

public class NatureDto
{
    public int Id { get; set; }
    public string LibelNature { get; set; } = string.Empty;
}

public class SiteDto
{
    public int Id { get; set; }
    public string NomSite { get; set; } = string.Empty;
}

public class CampagneDto
{
    public int Id { get; set; }
    public short Annee { get; set; }
    public string NomCampagne { get; set; } = string.Empty;
    public bool Active { get; set; }
}

public class EvaluationDto
{
    public int Id { get; set; }
    public int FournisseurId { get; set; }
    public int PonderationId { get; set; }
    public int CampagneId { get; set; }
    public int? ResultatId { get; set; }
    public double? ITransparence { get; set; }
    public double? IIFacture { get; set; }
    public double? IIINotoriete { get; set; }
    public double? IVPreventionCorruption { get; set; }
    public double? VPreventionDroitHomme { get; set; }
    public double? VIConformiteVsExigence { get; set; }
    public double? VIIDelaiLivraison { get; set; }
    public double? VIIICommunication { get; set; }
    public double? IXDisponibiliteDocuments { get; set; }
    public double? XConsigneQhse { get; set; }
    public double? XIQualitePrix { get; set; }
    public double? XIIDureePaiement { get; set; }
    public double? XIIIConformiteFiscale { get; set; }
    public double? MoyennePct { get; set; }
    public string? Observation { get; set; }
    public string? PropositionAmelioration { get; set; }
    public string? ObservationsFournisseur { get; set; }
    public string Responsable { get; set; } = string.Empty;
    public string? NumeroActionPac { get; set; }
    public DateOnly? Deadline { get; set; }
    public DateTime DateEvaluation { get; set; }
    public CampagneDto Campagne { get; set; } = null!;
    public PonderationDto Ponderation { get; set; } = null!;
    public ResultatDto? Resultat { get; set; }
}

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

public class ResultatDto
{
    public int Id { get; set; }
    public string NomResultat { get; set; } = string.Empty;
    public string? Observation { get; set; }
}
