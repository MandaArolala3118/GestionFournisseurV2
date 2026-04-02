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
    }
}
