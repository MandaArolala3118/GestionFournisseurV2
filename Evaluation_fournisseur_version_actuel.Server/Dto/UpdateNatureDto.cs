using System.ComponentModel.DataAnnotations;

namespace evaluation_fournisseur_version_actuel.Server.Dto
{
    public class UpdateNatureDto
    {
        [Required(ErrorMessage = "Le libellé de la nature est requis")]
        [MaxLength(500, ErrorMessage = "Le libellé de la nature ne peut pas dépasser 500 caractères")]
        public string LibelNature { get; set; } = string.Empty;
    }
}
