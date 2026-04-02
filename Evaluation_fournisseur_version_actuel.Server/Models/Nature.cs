using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evaluation_fournisseur_version_actuel.Server.Models;

[Table("eval_nature")]
public class Nature
{
    [Key]
    [Column("id_nature")]
    public int Id { get; set; }

    [Required]
    [MaxLength(500)]
    [Column("libel_nature")]
    public string LibelNature { get; set; } = string.Empty;

    // Navigation
    public ICollection<Fournisseur> Fournisseurs { get; set; } = [];
}
