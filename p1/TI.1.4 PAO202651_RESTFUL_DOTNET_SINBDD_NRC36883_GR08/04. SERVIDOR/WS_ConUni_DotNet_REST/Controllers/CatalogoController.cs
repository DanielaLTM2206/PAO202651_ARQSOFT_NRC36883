using Microsoft.AspNetCore.Mvc;
using WS_ConUni_DotNet_REST.Models;
using WS_ConUni_DotNet_REST.Services;

namespace WS_ConUni_DotNet_REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CatalogoController : ControllerBase
    {
        private readonly ICatalogoService _catalogoService;

        public CatalogoController(ICatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        /// <summary>
        /// RF-02: Consulta del Catálogo completo de categorías y unidades de conversión
        /// </summary>
        [HttpGet]
        public ActionResult<RespuestaCatalogo> ObtenerCatalogo()
        {
            var catalogo = _catalogoService.ObtenerCatalogoCompleto();
            return Ok(RespuestaCatalogo.Ok(catalogo));
        }

        /// <summary>
        /// RF-02: Consulta de unidades pertenecientes a una categoría específica
        /// </summary>
        [HttpGet("{categoria}")]
        public ActionResult<RespuestaCatalogo> ObtenerPorCategoria(string categoria)
        {
            var cat = _catalogoService.ObtenerPorCategoria(categoria);
            if (cat == null)
            {
                return NotFound(RespuestaCatalogo.Error("CAT_001", $"La categoría '{categoria}' no existe."));
            }

            return Ok(RespuestaCatalogo.Ok(new() { cat }));
        }
    }
}
