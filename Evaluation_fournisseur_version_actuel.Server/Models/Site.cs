using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evaluation_fournisseur_version_actuel.Server.Models;

[Table("eval_site")]
public class Site
{
    [Key]
    [Column("id_site")]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("nom_site")]
    public string NomSite { get; set; } = string.Empty;

    // Navigation
    public ICollection<Fournisseur> Fournisseurs { get; set; } = [];
}
