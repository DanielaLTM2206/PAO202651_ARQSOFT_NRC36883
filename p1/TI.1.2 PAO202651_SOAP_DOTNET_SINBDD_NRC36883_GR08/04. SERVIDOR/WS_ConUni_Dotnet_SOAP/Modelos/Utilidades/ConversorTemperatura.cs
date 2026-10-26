using System.Globalization;
using ec.edu.monster.Excepciones;
using ec.edu.monster.Modelos.Utilidades.Enums;

namespace ec.edu.monster.Modelos.Utilidades;

/// <summary>
/// La temperatura no es proporcional (tiene desplazamientos), por eso cada par de unidades usa su fórmula.
/// </summary>
public class ConversorTemperatura : IConversor<UnidadTemperatura>
{
    private static readonly Dictionary<UnidadTemperatura, double> ceroAbsoluto = new()
    {
        [UnidadTemperatura.CELSIUS] = -273.15,
        [UnidadTemperatura.FAHRENHEIT] = -459.67,
        [UnidadTemperatura.KELVIN] = 0.0,
        [UnidadTemperatura.RANKINE] = 0.0
    };

    public double Convertir(double valor, UnidadTemperatura origen, UnidadTemperatura destino)
    {
        if (!ceroAbsoluto.TryGetValue(origen, out double minimo) || !ceroAbsoluto.ContainsKey(destino))
        {
            throw new ArgumentException("Unidad no soportada");
        }
        if (valor < minimo)
        {
            throw new ExcepcionValidacion("La temperatura no puede ser menor al cero absoluto ("
                    + minimo.ToString(CultureInfo.InvariantCulture) + " " + origen + ")");
        }
        if (origen == destino)
        {
            return valor;
        }

        return (origen, destino) switch
        {
            (UnidadTemperatura.CELSIUS, UnidadTemperatura.FAHRENHEIT) => (valor * 9 / 5) + 32,
            (UnidadTemperatura.CELSIUS, UnidadTemperatura.KELVIN) => valor + 273.15,
            (UnidadTemperatura.CELSIUS, UnidadTemperatura.RANKINE) => (valor + 273.15) * 9 / 5,

            (UnidadTemperatura.FAHRENHEIT, UnidadTemperatura.CELSIUS) => (valor - 32) * 5 / 9,
            (UnidadTemperatura.FAHRENHEIT, UnidadTemperatura.KELVIN) => (valor - 32) * 5 / 9 + 273.15,
            (UnidadTemperatura.FAHRENHEIT, UnidadTemperatura.RANKINE) => valor + 459.67,

            (UnidadTemperatura.KELVIN, UnidadTemperatura.CELSIUS) => valor - 273.15,
            (UnidadTemperatura.KELVIN, UnidadTemperatura.FAHRENHEIT) => (valor - 273.15) * 9 / 5 + 32,
            (UnidadTemperatura.KELVIN, UnidadTemperatura.RANKINE) => valor * 9 / 5,

            (UnidadTemperatura.RANKINE, UnidadTemperatura.CELSIUS) => (valor - 491.67) * 5 / 9,
            (UnidadTemperatura.RANKINE, UnidadTemperatura.FAHRENHEIT) => valor - 459.67,
            (UnidadTemperatura.RANKINE, UnidadTemperatura.KELVIN) => valor * 5 / 9,

            _ => throw new ArgumentException("Conversión no soportada")
        };
    }
}
