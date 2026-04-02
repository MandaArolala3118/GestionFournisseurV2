using System.ComponentModel.DataAnnotations;

namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class UpdateCategorieDto
    {
        [Required(ErrorMessage = "Le nom de la catégorie est requis")]
        [MaxLength(500, ErrorMessage = "Le nom de la catégorie ne peut pas dépasser 500 caractères")]
        public string NomCategorie { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères")]
        public string? Description { get; set; }
    }
}
