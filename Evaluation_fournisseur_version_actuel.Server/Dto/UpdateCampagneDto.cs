using System.ComponentModel.DataAnnotations;

namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class UpdateCampagneDto
    {
        [Required(ErrorMessage = "L'année est requise")]
        [Range(1900, 2100, ErrorMessage = "L'année doit être entre 1900 et 2100")]
        public short Annee { get; set; }

        [Required(ErrorMessage = "Le nom de la campagne est requis")]
        [MaxLength(200, ErrorMessage = "Le nom de la campagne ne peut pas dépasser 200 caractères")]
        public string NomCampagne { get; set; } = string.Empty;

        public bool Active { get; set; }
    }
}
