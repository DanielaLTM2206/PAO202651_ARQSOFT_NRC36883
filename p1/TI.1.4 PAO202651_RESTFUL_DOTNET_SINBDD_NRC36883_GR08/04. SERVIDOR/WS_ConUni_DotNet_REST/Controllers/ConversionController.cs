using Microsoft.AspNetCore.Mvc;
using WS_ConUni_DotNet_REST.Models;
using WS_ConUni_DotNet_REST.Services;

namespace WS_ConUni_DotNet_REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ConversionController : ControllerBase
    {
        private readonly LongitudController _longitudController;
        private readonly MasaController _masaController;
        private readonly TemperaturaController _temperaturaController;
        private readonly ICatalogoService _catalogoService;

        public ConversionController(
            LongitudController longitudController,
            MasaController masaController,
            TemperaturaController temperaturaController,
            ICatalogoService catalogoService)
        {
            _longitudController = longitudController;
            _masaController = masaController;
            _temperaturaController = temperaturaController;
            _catalogoService = catalogoService;
        }

        /// <summary>
        /// RF-05: Operación de conversión unificada que detecta automáticamente la categoría de la unidad
        /// </summary>
        [HttpPost("convertir")]
        public ActionResult<ConversionResultModel> Convertir([FromBody] ConversionRequest request)
        {
            string origen = request.UnidadOrigen.Trim().ToLowerInvariant();

            // Detectar categoría
            if (EsLongitud(origen))
            {
                return _longitudController.Convertir(request);
            }

            if (EsMasa(origen))
            {
                return _masaController.Convertir(request);
            }

            if (EsTemperatura(origen))
            {
                return _temperaturaController.Convertir(request);
            }

            // Si no coincide con ninguno, probar primero con Longitud
            return _longitudController.Convertir(request);
        }

        private static bool EsLongitud(string u)
        {
            return u.StartsWith("met") || u.StartsWith("mil") || u.StartsWith("pul") ||
                   u.StartsWith("kil") || u.StartsWith("pie") || u.StartsWith("yar") ||
                   u == "m" || u == "mi" || u == "in" || u == "km" || u == "ft" || u == "yd";
        }

        private static bool EsMasa(string u)
        {
            return u.StartsWith("kilo") || u.StartsWith("lib") || u.StartsWith("onz") || u.StartsWith("gra") ||
                   u == "kg" || u == "lb" || u == "oz" || u == "g";
        }

        private static bool EsTemperatura(string u)
        {
            return u.StartsWith("cel") || u.StartsWith("fah") || u.StartsWith("kel") ||
                   u == "c" || u == "f" || u == "k" || u == "°c" || u == "°f";
        }
    }
}
