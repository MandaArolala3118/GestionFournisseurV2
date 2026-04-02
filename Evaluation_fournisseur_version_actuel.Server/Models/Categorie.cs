using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evaluation_fournisseur_version_actuel.Server.Models;

[Table("eval_categorie")]
public class Categorie
{
    [Key]
    [Column("id_categorie")]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("nom_categorie")]
    public string NomCategorie { get; set; } = string.Empty;

    [MaxLength(500)]
    [Column("description")]
    public string? Description { get; set; }

    // Navigation
    public ICollection<Fournisseur> Fournisseurs { get; set; } = [];
    public Ponderation?             Ponderation  { get; set; }
}
