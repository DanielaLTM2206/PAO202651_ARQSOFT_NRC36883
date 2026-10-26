using ec.edu.monster.Excepciones;
using ec.edu.monster.Modelos.Utilidades;
using ec.edu.monster.Modelos.Utilidades.Enums;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ConversorTemperaturaPruebas
{
    private readonly ConversorTemperatura conversor = new();

    [Theory]
    [InlineData(0, UnidadTemperatura.CELSIUS, UnidadTemperatura.FAHRENHEIT, 32.0)]
    [InlineData(32, UnidadTemperatura.FAHRENHEIT, UnidadTemperatura.CELSIUS, 0.0)]
    [InlineData(0, UnidadTemperatura.CELSIUS, UnidadTemperatura.KELVIN, 273.15)]
    [InlineData(273.15, UnidadTemperatura.KELVIN, UnidadTemperatura.CELSIUS, 0.0)]
    [InlineData(0, UnidadTemperatura.CELSIUS, UnidadTemperatura.RANKINE, 491.67)]
    [InlineData(491.67, UnidadTemperatura.RANKINE, UnidadTemperatura.CELSIUS, 0.0)]
    [InlineData(32, UnidadTemperatura.FAHRENHEIT, UnidadTemperatura.RANKINE, 491.67)]
    [InlineData(491.67, UnidadTemperatura.RANKINE, UnidadTemperatura.FAHRENHEIT, 32.0)]
    [InlineData(273.15, UnidadTemperatura.KELVIN, UnidadTemperatura.RANKINE, 491.67)]
    [InlineData(491.67, UnidadTemperatura.RANKINE, UnidadTemperatura.KELVIN, 273.15)]
    [InlineData(100, UnidadTemperatura.CELSIUS, UnidadTemperatura.FAHRENHEIT, 212.0)]
    [InlineData(-40, UnidadTemperatura.CELSIUS, UnidadTemperatura.FAHRENHEIT, -40.0)]
    [InlineData(212, UnidadTemperatura.FAHRENHEIT, UnidadTemperatura.KELVIN, 373.15)]
    [InlineData(0, UnidadTemperatura.KELVIN, UnidadTemperatura.FAHRENHEIT, -459.67)]
    [InlineData(25, UnidadTemperatura.CELSIUS, UnidadTemperatura.CELSIUS, 25.0)]
    [InlineData(-273.15, UnidadTemperatura.CELSIUS, UnidadTemperatura.KELVIN, 0.0)]
    public void DeberiaConvertirTemperatura(double valor, UnidadTemperatura origen, UnidadTemperatura destino, double esperado)
    {
        double resultado = conversor.Convertir(valor, origen, destino);

        Assert.Equal(esperado, resultado, 4);
    }

    [Theory]
    [InlineData(-1, UnidadTemperatura.KELVIN, UnidadTemperatura.CELSIUS)]
    [InlineData(-0.5, UnidadTemperatura.RANKINE, UnidadTemperatura.KELVIN)]
    [InlineData(-300, UnidadTemperatura.CELSIUS, UnidadTemperatura.FAHRENHEIT)]
    [InlineData(-500, UnidadTemperatura.FAHRENHEIT, UnidadTemperatura.CELSIUS)]
    [InlineData(-10, UnidadTemperatura.KELVIN, UnidadTemperatura.KELVIN)]
    public void DeberiaRechazarTemperaturaBajoCeroAbsoluto(double valor, UnidadTemperatura origen, UnidadTemperatura destino)
    {
        Assert.Throws<ExcepcionValidacion>(() => conversor.Convertir(valor, origen, destino));
    }
}
