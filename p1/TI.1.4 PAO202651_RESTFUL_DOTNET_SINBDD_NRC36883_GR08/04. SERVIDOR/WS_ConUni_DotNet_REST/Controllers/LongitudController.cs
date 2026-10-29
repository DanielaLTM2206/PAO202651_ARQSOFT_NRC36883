using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WS_ConUni_DotNet_REST.Models;
using WS_ConUni_DotNet_REST.Services;

namespace WS_ConUni_DotNet_REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LongitudController : ControllerBase
    {
        private readonly LongitudService _longitudService;
        private readonly IGestorHistorial _gestorHistorial;
        private readonly IServicioSeguridad _seguridad;

        public LongitudController(
            LongitudService longitudService,
            IGestorHistorial gestorHistorial,
            IServicioSeguridad seguridad)
        {
            _longitudService = longitudService;
            _gestorHistorial = gestorHistorial;
            _seguridad = seguridad;
        }

        /// <summary>
        /// Convierte valores entre unidades de Longitud (Milla, Metro, Pulgada, Kilómetro, Pie, Yarda)
        /// </summary>
        [HttpPost("convertir")]
        public ActionResult<ConversionResultModel> Convertir([FromBody] ConversionRequest request)
        {
            var res = _longitudService.Convertir(request.Valor, request.UnidadOrigen, request.UnidadDestino);

            if (res.Exitoso && res.Resultado != null)
            {
                string usuario = ObtenerUsuario();
                _gestorHistorial.RegistrarConversion(
                    res.Resultado.TipoConversion,
                    res.Resultado.ValorOriginal,
                    res.Resultado.UnidadOrigen,
                    res.Resultado.ValorConvertidoRedondeado,
                    res.Resultado.UnidadDestino,
                    usuario);

                return Ok(res);
            }

            return BadRequest(res);
        }

        private string ObtenerUsuario()
        {
            if (Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var val = authHeader.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(val) && _seguridad.ValidarToken(val, out string user))
                {
                    return user;
                }
            }
            return "monster";
        }
    }
}
