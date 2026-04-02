using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evaluation_fournisseur_version_actuel.Server.Models;

[Table("eval_fabricant")]
public class Fabricant
{
    [Key]
    [Column("id_fabricant")]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("nom_fabricant")]
    public string NomFabricant { get; set; } = string.Empty;

    // Navigation
    public ICollection<Fournisseur> Fournisseurs { get; set; } = [];
}
