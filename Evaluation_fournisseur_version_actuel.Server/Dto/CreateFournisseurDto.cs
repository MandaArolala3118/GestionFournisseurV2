using System.ComponentModel.DataAnnotations;

namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class CreateFournisseurDto
    {
        [Required(ErrorMessage = "Le nom du fournisseur est requis")]
        public int NomFournisseurId { get; set; }

        // Propriétés pour créer un nouveau NomFournisseur si NomFournisseurId = 0
        [MaxLength(50, ErrorMessage = "Le vendor number ne peut pas dépasser 50 caractères")]
        public string? VendorNumber { get; set; }

        [MaxLength(300, ErrorMessage = "Le vendor name ne peut pas dépasser 300 caractères")]
        public string? VendorName { get; set; }

        [MaxLength(50, ErrorMessage = "Le company code ne peut pas dépasser 50 caractères")]
        public string? CompanyCode { get; set; }

        [MaxLength(50, ErrorMessage = "L'AP division ne peut pas dépasser 50 caractères")]
        public string? ApDivision { get; set; }

        [MaxLength(100, ErrorMessage = "Le str class status ne peut pas dépasser 100 caractères")]
        public string? StrClassStatus { get; set; }

        [MaxLength(200, ErrorMessage = "Le sys code description ne peut pas dépasser 200 caractères")]
        public string? SysCodeDescription { get; set; }

        [MaxLength(200, ErrorMessage = "La signification company code ne peut pas dépasser 200 caractères")]
        public string? SignificationCompanyCode { get; set; }

        [MaxLength(200, ErrorMessage = "La signification AP division ne peut pas dépasser 200 caractères")]
        public string? SignificationApDivision { get; set; }

        [MaxLength(100, ErrorMessage = "Le str vendor class ne peut pas dépasser 100 caractères")]
        public string? StrVendorClass { get; set; }

        [MaxLength(50, ErrorMessage = "Le credit terms code ne peut pas dépasser 50 caractères")]
        public string? CreditTermsCode { get; set; }

        [Required(ErrorMessage = "La campagne est requise")]
        public int CampagneId { get; set; }

        [Required(ErrorMessage = "Le fabricant est requis")]
        public int FabricantId { get; set; }

        [Required(ErrorMessage = "La catégorie est requise")]
        public int CategorieId { get; set; }

        [Required(ErrorMessage = "La nature est requise")]
        public int NatureId { get; set; }

        [Required(ErrorMessage = "Le site est requis")]
        public int SiteId { get; set; }

        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        [MaxLength(254, ErrorMessage = "L'email ne peut pas dépasser 254 caractères")]
        public string? Email { get; set; }
    }
}
