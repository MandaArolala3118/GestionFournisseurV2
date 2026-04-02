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
    public class SiteController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SiteController> _logger;

        public SiteController(AppDbContext context, ILogger<SiteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SiteDto>>> GetAllSites()
        {
            try
            {
                var sites = await _context.Sites
                    .Select(s => new SiteDto
                    {
                        Id = s.Id,
                        NomSite = s.NomSite
                    })
                    .ToListAsync();

                return Ok(sites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des sites");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des sites");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SiteDto>> GetSiteById(int id)
        {
            try
            {
                var site = await _context.Sites.FindAsync(id);

                if (site == null)
                {
                    return NotFound($"Site avec ID {id} non trouvé");
                }

                var siteDto = new SiteDto
                {
                    Id = site.Id,
                    NomSite = site.NomSite
                };

                return Ok(siteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du site avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la récupération du site");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SiteDto>> CreateSite([FromBody] CreateSiteDto createSiteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var site = new Site
                {
                    NomSite = createSiteDto.NomSite
                };

                _context.Sites.Add(site);
                await _context.SaveChangesAsync();

                var siteDto = new SiteDto
                {
                    Id = site.Id,
                    NomSite = site.NomSite
                };

                return CreatedAtAction(nameof(GetSiteById), new { id = site.Id }, siteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du site");
                return StatusCode(500, "Une erreur est survenue lors de la création du site");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSite(int id, [FromBody] UpdateSiteDto updateSiteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var site = await _context.Sites.FindAsync(id);
                if (site == null)
                {
                    return NotFound($"Site avec ID {id} non trouvé");
                }

                site.NomSite = updateSiteDto.NomSite;

                _context.Sites.Update(site);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du site avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour du site");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSite(int id)
        {
            try
            {
                var site = await _context.Sites.FindAsync(id);
                if (site == null)
                {
                    return NotFound($"Site avec ID {id} non trouvé");
                }

                _context.Sites.Remove(site);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du site avec ID {Id}", id);
                return BadRequest("Impossible de supprimer ce site car il est utilisé par d'autres entités");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du site avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la suppression du site");
            }
        }
    }
}
