using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evaluation_fournisseur_version_actuel.Server.Models;

[Table("eval_ponderation")]
public class Ponderation
{
    [Key]
    [Column("id_ponderation")]
    public int Id { get; set; }

    // FK
    [Column("id_categorie")]
    public int CategorieId { get; set; }

    // Critères — valeur en % (0 = critère non retenu pour cette catégorie)
    [Column("I_transparence")]
    public double ITransparence { get; set; }

    [Column("II_facture")]
    public double IIFacture { get; set; }

    [Column("III_notoriete")]
    public double IIINotoriete { get; set; }

    [Column("IV_prevention_corruption")]
    public double IVPreventionCorruption { get; set; }

    [Column("V_prevention_droit_homme")]
    public double VPreventionDroitHomme { get; set; }

    [Column("VI_conformite_vs_exigence")]
    public double VIConformiteVsExigence { get; set; }

    [Column("VII_delai_livraison")]
    public double VIIDelaiLivraison { get; set; }

    [Column("VIII_communication")]
    public double VIIICommunication { get; set; }

    [Column("IX_disponibilite_documents")]
    public double IXDisponibiliteDocuments { get; set; }

    [Column("X_consigne_QHSE")]
    public double XConsigneQhse { get; set; }

    [Column("XI_qualite_prix")]
    public double XIQualitePrix { get; set; }

    [Column("XII_duree_paiement")]
    public double XIIDureePaiement { get; set; }

    [Column("XIII_conformite_fiscale")]
    public double XIIIConformiteFiscale { get; set; }

    /// <summary>
    /// Somme de toutes les pondérations. Doit être égale à 100.
    /// Calculée côté applicatif avant persistance (la contrainte SQL valide en base).
    /// </summary>
    [NotMapped]
    public double SommePonderations =>
        ITransparence + IIFacture + IIINotoriete + IVPreventionCorruption
        + VPreventionDroitHomme + VIConformiteVsExigence + VIIDelaiLivraison
        + VIIICommunication + IXDisponibiliteDocuments + XConsigneQhse
        + XIQualitePrix + XIIDureePaiement + XIIIConformiteFiscale;

    // Navigation
    public Categorie               Categorie   { get; set; } = null!;
    public ICollection<Evaluation> Evaluations { get; set; } = [];
}
