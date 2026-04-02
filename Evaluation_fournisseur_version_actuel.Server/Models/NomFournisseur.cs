using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evaluation_fournisseur_version_actuel.Server.Models;

[Table("eval_nom_fournisseur")]
public class NomFournisseur
{
    [Key]
    [Column("id_nom_fournisseur")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("vendor_number")]
    public string VendorNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    [Column("vendor_name")]
    public string VendorName { get; set; } = string.Empty;

    [MaxLength(50)]
    [Column("company_code")]
    public string? CompanyCode { get; set; }

    [MaxLength(50)]
    [Column("ap_division")]
    public string? ApDivision { get; set; }

    [MaxLength(100)]
    [Column("str_class_status")]
    public string? StrClassStatus { get; set; }

    [MaxLength(200)]
    [Column("sys_code_description")]
    public string? SysCodeDescription { get; set; }

    [MaxLength(200)]
    [Column("signification_companycode")]
    public string? SignificationCompanyCode { get; set; }

    [MaxLength(200)]
    [Column("signification_apdivision")]
    public string? SignificationApDivision { get; set; }

    [MaxLength(100)]
    [Column("str_vendor_class")]
    public string? StrVendorClass { get; set; }

    [MaxLength(50)]
    [Column("credit_terms_code")]
    public string? CreditTermsCode { get; set; }

    // FK
    [Column("id_campagne")]
    public int CampagneId { get; set; }

    // Navigation
    public Campagne                 Campagne     { get; set; } = null!;
    public ICollection<Fournisseur> Fournisseurs { get; set; } = [];
}
