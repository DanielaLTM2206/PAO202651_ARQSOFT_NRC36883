using System.Collections.Generic;
using System.Linq;
using WS_ConUni_DotNet_REST.Constants;
using WS_ConUni_DotNet_REST.Models;

namespace WS_ConUni_DotNet_REST.Services
{
    public interface ICatalogoService
    {
        List<CategoriaDTO> ObtenerCatalogoCompleto();
        CategoriaDTO? ObtenerPorCategoria(string categoria);
    }

    public class CatalogoService : ICatalogoService
    {
        private static readonly List<CategoriaDTO> _catalogo = new()
        {
            new CategoriaDTO
            {
                Categoria = "Longitud",
                UnidadBase = LongitudConstants.METRO,
                Unidades = new List<UnidadDTO>
                {
                    new() { Codigo = "m", Nombre = LongitudConstants.METRO, Simbolo = "m", FactorBase = 1.0 },
                    new() { Codigo = "mi", Nombre = LongitudConstants.MILLA, Simbolo = "mi", FactorBase = 1609.344 },
                    new() { Codigo = "in", Nombre = LongitudConstants.PULGADA, Simbolo = "in", FactorBase = 0.0254 },
                    new() { Codigo = "km", Nombre = LongitudConstants.KILOMETRO, Simbolo = "km", FactorBase = 1000.0 },
                    new() { Codigo = "ft", Nombre = LongitudConstants.PIE, Simbolo = "ft", FactorBase = 0.3048 },
                    new() { Codigo = "yd", Nombre = LongitudConstants.YARDA, Simbolo = "yd", FactorBase = 0.9144 }
                }
            },
            new CategoriaDTO
            {
                Categoria = "Masa",
                UnidadBase = MasaConstants.KILOGRAMO,
                Unidades = new List<UnidadDTO>
                {
                    new() { Codigo = "kg", Nombre = MasaConstants.KILOGRAMO, Simbolo = "kg", FactorBase = 1.0 },
                    new() { Codigo = "lb", Nombre = MasaConstants.LIBRA, Simbolo = "lb", FactorBase = 0.45359237 },
                    new() { Codigo = "oz", Nombre = MasaConstants.ONZA, Simbolo = "oz", FactorBase = 0.0283495231 },
                    new() { Codigo = "g", Nombre = MasaConstants.GRAMO, Simbolo = "g", FactorBase = 0.001 }
                }
            },
            new CategoriaDTO
            {
                Categoria = "Temperatura",
                UnidadBase = TemperaturaConstants.CELSIUS,
                Unidades = new List<UnidadDTO>
                {
                    new() { Codigo = "C", Nombre = TemperaturaConstants.CELSIUS, Simbolo = "°C", FactorBase = 1.0 },
                    new() { Codigo = "F", Nombre = TemperaturaConstants.FAHRENHEIT, Simbolo = "°F", FactorBase = 1.0 },
                    new() { Codigo = "K", Nombre = TemperaturaConstants.KELVIN, Simbolo = "K", FactorBase = 1.0 }
                }
            }
        };

        public List<CategoriaDTO> ObtenerCatalogoCompleto()
        {
            return _catalogo;
        }

        public CategoriaDTO? ObtenerPorCategoria(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria)) return null;
            return _catalogo.FirstOrDefault(c => c.Categoria.Equals(categoria.Trim(), System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
