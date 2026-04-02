using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evaluation_fournisseur_version_actuel.Server.Models;

[Table("eval_evaluation")]
public class Evaluation
{
    [Key]
    [Column("id_evaluation")]
    public int Id { get; set; }

    // FK
    [Column("id_fournisseur")]
    public int FournisseurId { get; set; }

    [Column("id_ponderation")]
    public int PonderationId { get; set; }

    [Column("id_campagne")]
    public int CampagneId { get; set; }

    [Column("id_resultat")]
    public int? ResultatId { get; set; }

    // Notes de 0 à 5 (null = critère non applicable)
    [Column("I_transparence")]
    [Range(0, 5)]
    public double? ITransparence { get; set; }

    [Column("II_facture")]
    [Range(0, 5)]
    public double? IIFacture { get; set; }

    [Column("III_notoriete")]
    [Range(0, 5)]
    public double? IIINotoriete { get; set; }

    [Column("IV_prevention_corruption")]
    [Range(0, 5)]
    public double? IVPreventionCorruption { get; set; }

    [Column("V_prevention_droit_homme")]
    [Range(0, 5)]
    public double? VPreventionDroitHomme { get; set; }

    [Column("VI_conformite_vs_exigence")]
    [Range(0, 5)]
    public double? VIConformiteVsExigence { get; set; }

    [Column("VII_delai_livraison")]
    [Range(0, 5)]
    public double? VIIDelaiLivraison { get; set; }

    [Column("VIII_communication")]
    [Range(0, 5)]
    public double? VIIICommunication { get; set; }

    [Column("IX_disponibilite_documents")]
    [Range(0, 5)]
    public double? IXDisponibiliteDocuments { get; set; }

    [Column("X_consigne_QHSE")]
    [Range(0, 5)]
    public double? XConsigneQhse { get; set; }

    [Column("XI_qualite_prix")]
    [Range(0, 5)]
    public double? XIQualitePrix { get; set; }

    [Column("XII_duree_paiement")]
    [Range(0, 5)]
    public double? XIIDureePaiement { get; set; }

    [Column("XIII_conformite_fiscale")]
    [Range(0, 5)]
    public double? XIIIConformiteFiscale { get; set; }

    /// <summary>Résultat pondéré en % — mis à jour par le trigger SQL ou recalculé ici.</summary>
    [Column("moyenne_pct")]
    public double? MoyennePct { get; set; }

    [Column("observation")]
    public string? Observation { get; set; }

    [Column("proposition_amelioration")]
    public string? PropositionAmelioration { get; set; }

    [Column("observations_fournisseur")]
    public string? ObservationsFournisseur { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("responsable")]
    public string Responsable { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("numero_action_pac")]
    public string? NumeroActionPac { get; set; }

    [Column("deadline")]
    public DateOnly? Deadline { get; set; }

    [Column("date_evaluation")]
    public DateTime DateEvaluation { get; set; } = DateTime.UtcNow;

    // Navigation
    public Fournisseur  Fournisseur  { get; set; } = null!;
    public Ponderation  Ponderation  { get; set; } = null!;
    public Campagne     Campagne     { get; set; } = null!;
    public Resultat?    Resultat     { get; set; }

    // -------------------------------------------------------
    // Calcul applicatif (miroir du trigger SQL)
    // -------------------------------------------------------

    /// <summary>
    /// Recalcule <see cref="MoyennePct"/> à partir des notes et de la pondération.
    /// À appeler avant SaveChanges() si le trigger SQL n'est pas disponible.
    /// </summary>
    public void RecalculerMoyenne(Ponderation p)
    {
        double score =
            (ITransparence            ?? 0) * p.ITransparence
          + (IIFacture                 ?? 0) * p.IIFacture
          + (IIINotoriete             ?? 0) * p.IIINotoriete
          + (IVPreventionCorruption   ?? 0) * p.IVPreventionCorruption
          + (VPreventionDroitHomme    ?? 0) * p.VPreventionDroitHomme
          + (VIConformiteVsExigence   ?? 0) * p.VIConformiteVsExigence
          + (VIIDelaiLivraison        ?? 0) * p.VIIDelaiLivraison
          + (VIIICommunication        ?? 0) * p.VIIICommunication
          + (IXDisponibiliteDocuments ?? 0) * p.IXDisponibiliteDocuments
          + (XConsigneQhse            ?? 0) * p.XConsigneQhse
          + (XIQualitePrix            ?? 0) * p.XIQualitePrix
          + (XIIDureePaiement         ?? 0) * p.XIIDureePaiement
          + (XIIIConformiteFiscale    ?? 0) * p.XIIIConformiteFiscale;

        MoyennePct = score / (5.0 * 100.0) * 100.0;
    }
}
