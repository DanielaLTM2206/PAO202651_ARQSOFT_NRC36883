using Microsoft.AspNetCore.Mvc;
using WS_ConUni_DotNet_REST.Models;
using WS_ConUni_DotNet_REST.Services;

namespace WS_ConUni_DotNet_REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class HistorialController : ControllerBase
    {
        private readonly IGestorHistorial _gestorHistorial;

        public HistorialController(IGestorHistorial gestorHistorial)
        {
            _gestorHistorial = gestorHistorial;
        }

        /// <summary>
        /// RF-06: Consulta el historial de conversiones sincronizado para todos los clientes (Web, Móvil, Escritorio, Consola)
        /// </summary>
        [HttpGet]
        public ActionResult<RespuestaHistorial> ObtenerHistorial()
        {
            var registros = _gestorHistorial.ObtenerHistorial();
            return Ok(RespuestaHistorial.Ok(registros));
        }

        /// <summary>
        /// RF-07: Limpia el historial de conversiones en memoria
        /// </summary>
        [HttpDelete]
        public ActionResult<RespuestaBase> LimpiarHistorial()
        {
            _gestorHistorial.LimpiarHistorial();
            return Ok(RespuestaBase.Ok("Historial de conversiones limpiado exitosamente."));
        }
    }
}
