using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evaluation_fournisseur_version_actuel.Server.Models;

[Table("eval_administrateur")]
public class Administrateur
{
    [Key]
    [Column("id_administrateur")]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("identite_utilisateur")]
    public string IdentiteUtilisateur { get; set; } = string.Empty;
}
