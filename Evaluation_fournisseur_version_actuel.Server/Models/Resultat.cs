using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evaluation_fournisseur_version_actuel.Server.Models;

[Table("eval_resultat")]
public class Resultat
{
    [Key]
    [Column("id_resultat")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nom_resultat")]
    public string NomResultat { get; set; } = string.Empty;

    [MaxLength(500)]
    [Column("observation")]
    public string? Observation { get; set; }

    // Navigation
    public ICollection<Evaluation> Evaluations { get; set; } = [];
}
