using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evaluation_fournisseur_version_actuel.Server.Models;

[Table("eval_campagne")]
public class Campagne
{
    [Key]
    [Column("id_campagne")]
    public int Id { get; set; }

    [Column("annee")]
    public short Annee { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("nom_campagne")]
    public string NomCampagne { get; set; } = string.Empty;

    [Column("active")]
    public bool Active { get; set; } = false;

    // Navigation
    public ICollection<NomFournisseur> NomFournisseurs { get; set; } = [];
    public ICollection<Evaluation>    Evaluations      { get; set; } = [];
}
