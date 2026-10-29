using ec.edu.monster.Modelo;
using ec.edu.monster.Util;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class FormatoUtilPruebas
{
    [Theory]
    [InlineData(1.0, "1")]
    [InlineData(1000.0, "1000")]
    [InlineData(0.0, "0")]
    [InlineData(-40.0, "-40")]
    [InlineData(1093.6132983377078, "1093.613298")]
    [InlineData(35273.990723115046, "35273.99072")]
    [InlineData(0.000000001, "0.000000001")]
    [InlineData(273.15, "273.15")]
    public void DeberiaMostrarNumerosSinNotacionCientificaNiCerosSobrantes(double numero, string esperado)
    {
        Assert.Equal(esperado, FormatoUtil.FormatearNumero(numero));
    }

    [Fact]
    public void DeberiaFormatearLaFechaDelHistorial()
    {
        Assert.Equal("29/10/2026 08:05:09", FormatoUtil.FormatearFechaHora(new DateTime(2026, 10, 29, 8, 5, 9)));
    }

    [Theory]
    [InlineData(1000, "METRO", 1, "KILOMETRO", "1000 METRO = 1 KILOMETRO")]
    [InlineData(100, "CELSIUS", 212, "FAHRENHEIT", "100 CELSIUS = 212 FAHRENHEIT")]
    [InlineData(2.5, "KILOGRAMO", 2500, "GRAMO", "2.5 KILOGRAMO = 2500 GRAMO")]
    public void DeberiaFormatearUnaConversionDelHistorial(double valor, string origen, double resultado,
            string destino, string esperado)
    {
        var registro = new RegistroConversion(DateTime.Now, TipoConversion.Longitud, valor, origen, resultado, destino);

        Assert.Equal(esperado, FormatoUtil.FormatearConversion(registro));
    }
}
