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
}
