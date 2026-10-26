using ec.edu.monster.Models;
using ec.edu.monster.Util;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class FormatoUtilPruebas
{
    [Theory]
    [InlineData(1.0, "1")]
    [InlineData(212.0, "212")]
    [InlineData(28.3495, "28.3495")]
    [InlineData(0.000000001, "0.000000001")]
    [InlineData(1093.6132983377078, "1093.613298")]
    [InlineData(0.30000000000000004, "0.3")]
    [InlineData(-459.67, "-459.67")]
    [InlineData(0.0, "0")]
    [InlineData(1000000000000.0, "1000000000000")]
    [InlineData(1e-19, "1E-19")]
    public void DeberiaMostrarNumerosSinCerosSobrantes(double numero, string esperado)
    {
        Assert.Equal(esperado, FormatoUtil.FormatearNumero(numero));
    }

    [Theory]
    [InlineData(TipoConversion.Longitud, "Longitud", "longitud")]
    [InlineData(TipoConversion.Masa, "Masa", "masa")]
    [InlineData(TipoConversion.Temperatura, "Temperatura", "temperatura")]
    public void DeberiaExponerNombreVisibleYCategoria(TipoConversion tipo, string nombreVisible, string categoria)
    {
        Assert.Equal(nombreVisible, tipo.NombreVisible());
        Assert.Equal(categoria, tipo.Categoria());
    }
}
