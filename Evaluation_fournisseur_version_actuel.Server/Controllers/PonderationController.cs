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
    public class PonderationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PonderationController> _logger;

        public PonderationController(AppDbContext context, ILogger<PonderationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PonderationDto>>> GetAllPonderations()
        {
            try
            {
                var ponderations = await _context.Ponderations
                    .Include(p => p.Categorie)
                    .Select(p => new PonderationDto
                    {
                        Id = p.Id,
                        CategorieId = p.CategorieId,
                        ITransparence = p.ITransparence,
                        IIFacture = p.IIFacture,
                        IIINotoriete = p.IIINotoriete,
                        IVPreventionCorruption = p.IVPreventionCorruption,
                        VPreventionDroitHomme = p.VPreventionDroitHomme,
                        VIConformiteVsExigence = p.VIConformiteVsExigence,
                        VIIDelaiLivraison = p.VIIDelaiLivraison,
                        VIIICommunication = p.VIIICommunication,
                        IXDisponibiliteDocuments = p.IXDisponibiliteDocuments,
                        XConsigneQhse = p.XConsigneQhse,
                        XIQualitePrix = p.XIQualitePrix,
                        XIIDureePaiement = p.XIIDureePaiement,
                        XIIIConformiteFiscale = p.XIIIConformiteFiscale,
                        SommePonderations = p.SommePonderations,
                        Categorie = new CategorieDto
                        {
                            Id = p.Categorie.Id,
                            NomCategorie = p.Categorie.NomCategorie,
                            Description = p.Categorie.Description
                        }
                    })
                    .ToListAsync();

                return Ok(ponderations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des pondérations");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des pondérations");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PonderationDto>> GetPonderationById(int id)
        {
            try
            {
                var ponderation = await _context.Ponderations
                    .Include(p => p.Categorie)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (ponderation == null)
                {
                    return NotFound($"Pondération avec ID {id} non trouvée");
                }

                var ponderationDto = new PonderationDto
                {
                    Id = ponderation.Id,
                    CategorieId = ponderation.CategorieId,
                    ITransparence = ponderation.ITransparence,
                    IIFacture = ponderation.IIFacture,
                    IIINotoriete = ponderation.IIINotoriete,
                    IVPreventionCorruption = ponderation.IVPreventionCorruption,
                    VPreventionDroitHomme = ponderation.VPreventionDroitHomme,
                    VIConformiteVsExigence = ponderation.VIConformiteVsExigence,
                    VIIDelaiLivraison = ponderation.VIIDelaiLivraison,
                    VIIICommunication = ponderation.VIIICommunication,
                    IXDisponibiliteDocuments = ponderation.IXDisponibiliteDocuments,
                    XConsigneQhse = ponderation.XConsigneQhse,
                    XIQualitePrix = ponderation.XIQualitePrix,
                    XIIDureePaiement = ponderation.XIIDureePaiement,
                    XIIIConformiteFiscale = ponderation.XIIIConformiteFiscale,
                    SommePonderations = ponderation.SommePonderations,
                    Categorie = ponderation.Categorie != null ? new CategorieDto
                    {
                        Id = ponderation.Categorie.Id,
                        NomCategorie = ponderation.Categorie.NomCategorie,
                        Description = ponderation.Categorie.Description
                    } : null
                };

                return Ok(ponderationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la pondération avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la récupération de la pondération");
            }
        }

        /// <summary>
        /// Vérifie que la somme des pondérations est égale à 100
        /// </summary>
        /// <param name="ponderation">La pondération à vérifier</param>
        /// <returns>True si la somme est valide, False sinon</returns>
        private bool ValiderSommePonderations(Ponderation ponderation)
        {
            double somme = ponderation.ITransparence + ponderation.IIFacture + ponderation.IIINotoriete
                + ponderation.IVPreventionCorruption + ponderation.VPreventionDroitHomme + ponderation.VIConformiteVsExigence
                + ponderation.VIIDelaiLivraison + ponderation.VIIICommunication + ponderation.IXDisponibiliteDocuments
                + ponderation.XConsigneQhse + ponderation.XIQualitePrix + ponderation.XIIDureePaiement
                + ponderation.XIIIConformiteFiscale;

            return Math.Abs(somme - 100.0) < 0.001; // Tolérance de 0.001 pour les erreurs d'arrondi
        }

        [HttpPost]
        public async Task<ActionResult<PonderationDto>> CreatePonderation([FromBody] CreatePonderationDto createPonderationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Vérifier que la catégorie existe
                var categorie = await _context.Categories.FindAsync(createPonderationDto.CategorieId);
                if (categorie == null)
                {
                    return BadRequest($"Catégorie avec ID {createPonderationDto.CategorieId} non trouvée");
                }

                var ponderation = new Ponderation
                {
                    CategorieId = createPonderationDto.CategorieId,
                    ITransparence = createPonderationDto.ITransparence,
                    IIFacture = createPonderationDto.IIFacture,
                    IIINotoriete = createPonderationDto.IIINotoriete,
                    IVPreventionCorruption = createPonderationDto.IVPreventionCorruption,
                    VPreventionDroitHomme = createPonderationDto.VPreventionDroitHomme,
                    VIConformiteVsExigence = createPonderationDto.VIConformiteVsExigence,
                    VIIDelaiLivraison = createPonderationDto.VIIDelaiLivraison,
                    VIIICommunication = createPonderationDto.VIIICommunication,
                    IXDisponibiliteDocuments = createPonderationDto.IXDisponibiliteDocuments,
                    XConsigneQhse = createPonderationDto.XConsigneQhse,
                    XIQualitePrix = createPonderationDto.XIQualitePrix,
                    XIIDureePaiement = createPonderationDto.XIIDureePaiement,
                    XIIIConformiteFiscale = createPonderationDto.XIIIConformiteFiscale
                    // SommePonderations est calculée automatiquement par la propriété [NotMapped]
                };

                // Valider que la somme des pondérations est égale à 100
                if (!ValiderSommePonderations(ponderation))
                {
                    return BadRequest("La somme des pondérations doit être égale à 100%");
                }

                _context.Ponderations.Add(ponderation);
                await _context.SaveChangesAsync();

                var ponderationDto = new PonderationDto
                {
                    Id = ponderation.Id,
                    CategorieId = ponderation.CategorieId,
                    ITransparence = ponderation.ITransparence,
                    IIFacture = ponderation.IIFacture,
                    IIINotoriete = ponderation.IIINotoriete,
                    IVPreventionCorruption = ponderation.IVPreventionCorruption,
                    VPreventionDroitHomme = ponderation.VPreventionDroitHomme,
                    VIConformiteVsExigence = ponderation.VIConformiteVsExigence,
                    VIIDelaiLivraison = ponderation.VIIDelaiLivraison,
                    VIIICommunication = ponderation.VIIICommunication,
                    IXDisponibiliteDocuments = ponderation.IXDisponibiliteDocuments,
                    XConsigneQhse = ponderation.XConsigneQhse,
                    XIQualitePrix = ponderation.XIQualitePrix,
                    XIIDureePaiement = ponderation.XIIDureePaiement,
                    XIIIConformiteFiscale = ponderation.XIIIConformiteFiscale,
                    SommePonderations = ponderation.SommePonderations,
                    Categorie = new CategorieDto
                    {
                        Id = categorie.Id,
                        NomCategorie = categorie.NomCategorie,
                        Description = categorie.Description
                    }
                };

                return CreatedAtAction(nameof(GetPonderationById), new { id = ponderation.Id }, ponderationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de la pondération");
                return StatusCode(500, "Une erreur est survenue lors de la création de la pondération");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePonderation(int id, [FromBody] UpdatePonderationDto updatePonderationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var ponderation = await _context.Ponderations.FindAsync(id);
                if (ponderation == null)
                {
                    return NotFound($"Pondération avec ID {id} non trouvée");
                }

                // Vérifier que la catégorie existe si fournie
                if (updatePonderationDto.CategorieId.HasValue)
                {
                    var categorie = await _context.Categories.FindAsync(updatePonderationDto.CategorieId.Value);
                    if (categorie == null)
                    {
                        return BadRequest($"Catégorie avec ID {updatePonderationDto.CategorieId.Value} non trouvée");
                    }
                    ponderation.CategorieId = updatePonderationDto.CategorieId.Value;
                }

                // Mettre à jour les propriétés si fournies
                if (updatePonderationDto.ITransparence.HasValue)
                    ponderation.ITransparence = updatePonderationDto.ITransparence.Value;
                
                if (updatePonderationDto.IIFacture.HasValue)
                    ponderation.IIFacture = updatePonderationDto.IIFacture.Value;
                
                if (updatePonderationDto.IIINotoriete.HasValue)
                    ponderation.IIINotoriete = updatePonderationDto.IIINotoriete.Value;
                
                if (updatePonderationDto.IVPreventionCorruption.HasValue)
                    ponderation.IVPreventionCorruption = updatePonderationDto.IVPreventionCorruption.Value;
                
                if (updatePonderationDto.VPreventionDroitHomme.HasValue)
                    ponderation.VPreventionDroitHomme = updatePonderationDto.VPreventionDroitHomme.Value;
                
                if (updatePonderationDto.VIConformiteVsExigence.HasValue)
                    ponderation.VIConformiteVsExigence = updatePonderationDto.VIConformiteVsExigence.Value;
                
                if (updatePonderationDto.VIIDelaiLivraison.HasValue)
                    ponderation.VIIDelaiLivraison = updatePonderationDto.VIIDelaiLivraison.Value;
                
                if (updatePonderationDto.VIIICommunication.HasValue)
                    ponderation.VIIICommunication = updatePonderationDto.VIIICommunication.Value;
                
                if (updatePonderationDto.IXDisponibiliteDocuments.HasValue)
                    ponderation.IXDisponibiliteDocuments = updatePonderationDto.IXDisponibiliteDocuments.Value;
                
                if (updatePonderationDto.XConsigneQhse.HasValue)
                    ponderation.XConsigneQhse = updatePonderationDto.XConsigneQhse.Value;
                
                if (updatePonderationDto.XIQualitePrix.HasValue)
                    ponderation.XIQualitePrix = updatePonderationDto.XIQualitePrix.Value;
                
                if (updatePonderationDto.XIIDureePaiement.HasValue)
                    ponderation.XIIDureePaiement = updatePonderationDto.XIIDureePaiement.Value;
                
                if (updatePonderationDto.XIIIConformiteFiscale.HasValue)
                    ponderation.XIIIConformiteFiscale = updatePonderationDto.XIIIConformiteFiscale.Value;

                // Recalculer la somme des pondérations
                // La propriété SommePonderations est calculée automatiquement par [NotMapped]
                // Pas besoin de l'assigner manuellement
                
                // Valider que la somme est correcte
                if (!ValiderSommePonderations(ponderation))
                {
                    return BadRequest("La somme des pondérations doit être égale à 100%");
                }

                _context.Ponderations.Update(ponderation);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de la pondération avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour de la pondération");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePonderation(int id)
        {
            try
            {
                var ponderation = await _context.Ponderations.FindAsync(id);
                if (ponderation == null)
                {
                    return NotFound($"Pondération avec ID {id} non trouvée");
                }

                // Vérifier si la pondération est utilisée par des évaluations
                var evaluationsCount = await _context.Evaluations
                    .CountAsync(e => e.PonderationId == id);

                if (evaluationsCount > 0)
                {
                    return BadRequest("Impossible de supprimer cette pondération car elle est utilisée par des évaluations");
                }

                _context.Ponderations.Remove(ponderation);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de la pondération avec ID {Id}", id);
                return BadRequest("Impossible de supprimer cette pondération car elle est utilisée par d'autres entités");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de la pondération avec ID {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la suppression de la pondération");
            }
        }
    }
}
