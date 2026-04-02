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
    public class CampagneController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CampagneController> _logger;

        public CampagneController(AppDbContext context, ILogger<CampagneController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampagneDto>>> GetAllCampagnes()
        {
            try
            {
                var campagnes = await _context.Campagnes
                    .Select(c => new CampagneDto
                    {
                        Id = c.Id,
                        Annee = c.Annee,
                        NomCampagne = c.NomCampagne,
                        Active = c.Active
                    })
                    .ToListAsync();

                return Ok(campagnes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des campagnes");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des campagnes");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CampagneDto>> GetCampagneById(int id)
        {
            try
            {
                var campagne = await _context.Campagnes.FindAsync(id);

                if (campagne == null)
                {
                    return NotFound($"Campagne avec ID {id} non trouvée");
                }

                var campagneDto = new CampagneDto
                {
                    Id = campagne.Id,
                    Annee = campagne.Annee,
                    NomCampagne = campagne.NomCampagne,
                    Active = campagne.Active
                };

                return Ok(campagneDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la campagne avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la récupération de la campagne");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CampagneDto>> CreateCampagne([FromBody] CreateCampagneDto createCampagneDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var campagne = new Campagne
                {
                    Annee = createCampagneDto.Annee,
                    NomCampagne = createCampagneDto.NomCampagne,
                    Active = createCampagneDto.Active
                };

                _context.Campagnes.Add(campagne);
                await _context.SaveChangesAsync();

                var campagneDto = new CampagneDto
                {
                    Id = campagne.Id,
                    Annee = campagne.Annee,
                    NomCampagne = campagne.NomCampagne,
                    Active = campagne.Active
                };

                return CreatedAtAction(nameof(GetCampagneById), new { id = campagne.Id }, campagneDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de la campagne");
                return StatusCode(500, "Une erreur est survenue lors de la création de la campagne");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampagne(int id, [FromBody] UpdateCampagneDto updateCampagneDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var campagne = await _context.Campagnes.FindAsync(id);
                if (campagne == null)
                {
                    return NotFound($"Campagne avec ID {id} non trouvée");
                }

                campagne.Annee = updateCampagneDto.Annee;
                campagne.NomCampagne = updateCampagneDto.NomCampagne;
                campagne.Active = updateCampagneDto.Active;

                _context.Campagnes.Update(campagne);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de la campagne avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour de la campagne");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampagne(int id)
        {
            try
            {
                var campagne = await _context.Campagnes.FindAsync(id);
                if (campagne == null)
                {
                    return NotFound($"Campagne avec ID {id} non trouvée");
                }

                _context.Campagnes.Remove(campagne);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de la campagne avec ID {Id}", id);
                return BadRequest("Impossible de supprimer cette campagne car elle est utilisée par d'autres entités");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de la campagne avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la suppression de la campagne");
            }
        }
    }
}
