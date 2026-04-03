namespace evaluation_fournisseur_version_actuel.Server.Dto
{
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
}
