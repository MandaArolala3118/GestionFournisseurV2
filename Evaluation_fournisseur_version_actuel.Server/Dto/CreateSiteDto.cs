using System.ComponentModel.DataAnnotations;

namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class CreateSiteDto
    {
        [Required(ErrorMessage = "Le nom du site est requis")]
        [MaxLength(150, ErrorMessage = "Le nom du site ne peut pas dépasser 150 caractères")]
        public string NomSite { get; set; } = string.Empty;
    }
}
