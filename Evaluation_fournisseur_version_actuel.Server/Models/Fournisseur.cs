using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evaluation_fournisseur_version_actuel.Server.Models;

[Table("eval_fournisseur")]
public class Fournisseur
{
    [Key]
    [Column("id_fournisseur")]
    public int Id { get; set; }

    // FK
    [Column("id_nom_fournisseur")]
    public int NomFournisseurId { get; set; }

    [Column("id_fabricant")]
    public int? FabricantId { get; set; }

    [Column("id_categorie")]
    public int CategorieId { get; set; }

    [Column("id_nature")]
    public int NatureId { get; set; }

    [Column("id_site")]
    public int SiteId { get; set; }

    [MaxLength(254)]
    [EmailAddress]
    [Column("email")]
    public string Email { get; set; }

    [Column("emailEnvoye")]
    public bool EmailEnvoye { get; set; } = false;

    [Column("dateEnvoiEmail")]
    public DateTime? DateEnvoiEmail { get; set; }

    // Navigation
    public NomFournisseur           NomFournisseur { get; set; } = null!;
    public Fabricant?               Fabricant      { get; set; }
    public Categorie                Categorie      { get; set; } = null!;
    public Nature                   Nature         { get; set; } = null!;
    public Site                     Site           { get; set; } = null!;
    public ICollection<Evaluation>  Evaluations    { get; set; } = [];
}
