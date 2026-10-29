using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WS_ConUni_DotNet_REST.Models;

namespace WS_ConUni_DotNet_REST.Services
{
    public interface IGestorHistorial
    {
        void RegistrarConversion(string tipo, double valorOriginal, string unidadOrigen, double valorResultado, string unidadDestino, string usuario);
        List<RegistroHistorialDTO> ObtenerHistorial();
        void LimpiarHistorial();
    }

    public class GestorHistorial : IGestorHistorial
    {
        private static readonly ConcurrentBag<RegistroHistorialDTO> _historial = new();

        public void RegistrarConversion(
            string tipo,
            double valorOriginal,
            string unidadOrigen,
            double valorResultado,
            string unidadDestino,
            string usuario)
        {
            var registro = new RegistroHistorialDTO
            {
                TipoConversion = tipo,
                ValorOriginal = valorOriginal,
                UnidadOrigen = unidadOrigen,
                ValorResultado = valorResultado,
                UnidadDestino = unidadDestino,
                Usuario = string.IsNullOrWhiteSpace(usuario) ? "monster" : usuario.ToLowerInvariant()
            };

            _historial.Add(registro);
        }

        public List<RegistroHistorialDTO> ObtenerHistorial()
        {
            return _historial.OrderByDescending(r => r.Fecha).ToList();
        }

        public void LimpiarHistorial()
        {
            _historial.Clear();
        }
    }
}
