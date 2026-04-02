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
    public class FabricantController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FabricantController> _logger;

        public FabricantController(AppDbContext context, ILogger<FabricantController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FabricantDto>>> GetAllFabricants()
        {
            try
            {
                var fabricants = await _context.Fabricants
                    .Select(f => new FabricantDto
                    {
                        Id = f.Id,
                        NomFabricant = f.NomFabricant
                    })
                    .ToListAsync();

                return Ok(fabricants);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des fabricants");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des fabricants");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FabricantDto>> GetFabricantById(int id)
        {
            try
            {
                var fabricant = await _context.Fabricants.FindAsync(id);

                if (fabricant == null)
                {
                    return NotFound($"Fabricant avec ID {id} non trouvé");
                }

                var fabricantDto = new FabricantDto
                {
                    Id = fabricant.Id,
                    NomFabricant = fabricant.NomFabricant
                };

                return Ok(fabricantDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du fabricant avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la récupération du fabricant");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FabricantDto>> CreateFabricant([FromBody] CreateFabricantDto createFabricantDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var fabricant = new Fabricant
                {
                    NomFabricant = createFabricantDto.NomFabricant
                };

                _context.Fabricants.Add(fabricant);
                await _context.SaveChangesAsync();

                var fabricantDto = new FabricantDto
                {
                    Id = fabricant.Id,
                    NomFabricant = fabricant.NomFabricant
                };

                return CreatedAtAction(nameof(GetFabricantById), new { id = fabricant.Id }, fabricantDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du fabricant");
                return StatusCode(500, "Une erreur est survenue lors de la création du fabricant");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFabricant(int id, [FromBody] UpdateFabricantDto updateFabricantDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var fabricant = await _context.Fabricants.FindAsync(id);
                if (fabricant == null)
                {
                    return NotFound($"Fabricant avec ID {id} non trouvé");
                }

                fabricant.NomFabricant = updateFabricantDto.NomFabricant;

                _context.Fabricants.Update(fabricant);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du fabricant avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour du fabricant");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFabricant(int id)
        {
            try
            {
                var fabricant = await _context.Fabricants.FindAsync(id);
                if (fabricant == null)
                {
                    return NotFound($"Fabricant avec ID {id} non trouvé");
                }

                _context.Fabricants.Remove(fabricant);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du fabricant avec ID {Id}", id);
                return BadRequest("Impossible de supprimer ce fabricant car il est utilisé par d'autres entités");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du fabricant avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la suppression du fabricant");
            }
        }
    }
}
