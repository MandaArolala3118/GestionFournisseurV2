using System.ComponentModel.DataAnnotations;

namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class UpdateFabricantDto
    {
        [Required(ErrorMessage = "Le nom du fabricant est requis")]
        [MaxLength(500, ErrorMessage = "Le nom du fabricant ne peut pas dépasser 500 caractères")]
        public string NomFabricant { get; set; } = string.Empty;
    }
}
