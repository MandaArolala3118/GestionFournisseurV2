using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using evaluation_fournisseur_version_actuel.Server.Models;
using evaluation_fournisseur_version_actuel.Server.Data;
using evaluation_fournisseur_version_actuel.Server.Dto;

namespace Evaluation_fournisseur_version_actuel.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategorieController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategorieController> _logger;

        public CategorieController(AppDbContext context, ILogger<CategorieController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategorieDto>>> GetAllCategories()
        {
            try
            {
                var categories = await _context.Categories
                    .Include(c => c.Ponderation)
                    .Select(c => new CategorieDto
                    {
                        Id = c.Id,
                        NomCategorie = c.NomCategorie,
                        Description = c.Description,
                        PonderationId = c.Ponderation != null ? c.Ponderation.Id : (int?)null,
                        Ponderation = c.Ponderation != null ? new PonderationDto
                        {
                            Id = c.Ponderation.Id,
                            CategorieId = c.Ponderation.CategorieId,
                            ITransparence = c.Ponderation.ITransparence,
                            IIFacture = c.Ponderation.IIFacture,
                            IIINotoriete = c.Ponderation.IIINotoriete,
                            IVPreventionCorruption = c.Ponderation.IVPreventionCorruption,
                            VPreventionDroitHomme = c.Ponderation.VPreventionDroitHomme,
                            VIConformiteVsExigence = c.Ponderation.VIConformiteVsExigence,
                            VIIDelaiLivraison = c.Ponderation.VIIDelaiLivraison,
                            VIIICommunication = c.Ponderation.VIIICommunication,
                            IXDisponibiliteDocuments = c.Ponderation.IXDisponibiliteDocuments,
                            XConsigneQhse = c.Ponderation.XConsigneQhse,
                            XIQualitePrix = c.Ponderation.XIQualitePrix,
                            XIIDureePaiement = c.Ponderation.XIIDureePaiement,
                            XIIIConformiteFiscale = c.Ponderation.XIIIConformiteFiscale,
                            SommePonderations = c.Ponderation.SommePonderations
                        } : null
                    })
                    .ToListAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des catégories");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des catégories");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategorieDto>> GetCategorieById(int id)
        {
            try
            {
                var categorie = await _context.Categories
                    .Include(c => c.Ponderation)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (categorie == null)
                {
                    return NotFound($"Catégorie avec ID {id} non trouvée");
                }

                var categorieDto = new CategorieDto
                {
                    Id = categorie.Id,
                    NomCategorie = categorie.NomCategorie,
                    Description = categorie.Description,
                    PonderationId = categorie.Ponderation != null ? categorie.Ponderation.Id : (int?)null,
                    Ponderation = categorie.Ponderation != null ? new PonderationDto
                    {
                        Id = categorie.Ponderation.Id,
                        CategorieId = categorie.Ponderation.CategorieId,
                        ITransparence = categorie.Ponderation.ITransparence,
                        IIFacture = categorie.Ponderation.IIFacture,
                        IIINotoriete = categorie.Ponderation.IIINotoriete,
                        IVPreventionCorruption = categorie.Ponderation.IVPreventionCorruption,
                        VPreventionDroitHomme = categorie.Ponderation.VPreventionDroitHomme,
                        VIConformiteVsExigence = categorie.Ponderation.VIConformiteVsExigence,
                        VIIDelaiLivraison = categorie.Ponderation.VIIDelaiLivraison,
                        VIIICommunication = categorie.Ponderation.VIIICommunication,
                        IXDisponibiliteDocuments = categorie.Ponderation.IXDisponibiliteDocuments,
                        XConsigneQhse = categorie.Ponderation.XConsigneQhse,
                        XIQualitePrix = categorie.Ponderation.XIQualitePrix,
                        XIIDureePaiement = categorie.Ponderation.XIIDureePaiement,
                        XIIIConformiteFiscale = categorie.Ponderation.XIIIConformiteFiscale,
                        SommePonderations = categorie.Ponderation.SommePonderations
                    } : null
                };

                return Ok(categorieDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la catégorie avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la récupération de la catégorie");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategorieDto>> CreateCategorie([FromBody] CreateCategorieDto createCategorieDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var categorie = new Categorie
                {
                    NomCategorie = createCategorieDto.NomCategorie,
                    Description = createCategorieDto.Description
                };

                _context.Categories.Add(categorie);
                await _context.SaveChangesAsync();

                var categorieDto = new CategorieDto
                {
                    Id = categorie.Id,
                    NomCategorie = categorie.NomCategorie,
                    Description = categorie.Description,
                    PonderationId = categorie.Ponderation != null ? categorie.Ponderation.Id : null
                };

                return CreatedAtAction(nameof(GetCategorieById), new { id = categorie.Id }, categorieDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de la catégorie");
                return StatusCode(500, "Une erreur est survenue lors de la création de la catégorie");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategorie(int id, [FromBody] UpdateCategorieDto updateCategorieDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var categorie = await _context.Categories.FindAsync(id);
                if (categorie == null)
                {
                    return NotFound($"Catégorie avec ID {id} non trouvée");
                }

                categorie.NomCategorie = updateCategorieDto.NomCategorie;
                categorie.Description = updateCategorieDto.Description;

                _context.Categories.Update(categorie);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de la catégorie avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour de la catégorie");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategorie(int id)
        {
            try
            {
                var categorie = await _context.Categories.FindAsync(id);
                if (categorie == null)
                {
                    return NotFound($"Catégorie avec ID {id} non trouvée");
                }

                _context.Categories.Remove(categorie);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de la catégorie avec ID {Id}", id);
                return BadRequest("Impossible de supprimer cette catégorie car elle est utilisée par d'autres entités");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de la catégorie avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la suppression de la catégorie");
            }
        }
    }
}
