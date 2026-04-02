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
    public class NatureController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<NatureController> _logger;

        public NatureController(AppDbContext context, ILogger<NatureController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NatureDto>>> GetAllNatures()
        {
            try
            {
                var natures = await _context.Natures
                    .Select(n => new NatureDto
                    {
                        Id = n.Id,
                        LibelNature = n.LibelNature
                    })
                    .ToListAsync();

                return Ok(natures);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des natures");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des natures");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NatureDto>> GetNatureById(int id)
        {
            try
            {
                var nature = await _context.Natures.FindAsync(id);

                if (nature == null)
                {
                    return NotFound($"Nature avec ID {id} non trouvée");
                }

                var natureDto = new NatureDto
                {
                    Id = nature.Id,
                    LibelNature = nature.LibelNature
                };

                return Ok(natureDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la nature avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la récupération de la nature");
            }
        }
    }
}
