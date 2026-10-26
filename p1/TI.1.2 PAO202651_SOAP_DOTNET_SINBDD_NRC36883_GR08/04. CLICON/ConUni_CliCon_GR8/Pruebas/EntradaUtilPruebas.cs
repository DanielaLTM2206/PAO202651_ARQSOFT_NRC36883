using ec.edu.monster.Util;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class EntradaUtilPruebas
{
    [Theory]
    [InlineData("25", 25.0)]
    [InlineData("12.5", 12.5)]
    [InlineData("12,5", 12.5)]
    [InlineData("-40", -40.0)]
    [InlineData("  +0.001 ", 0.001)]
    [InlineData("123456789012345.1234567891", 123456789012345.1234567891)]
    public void DeberiaAceptarNumerosDecimalesValidos(string texto, double esperado)
    {
        Assert.Equal(esperado, EntradaUtil.InterpretarDecimal(texto), 10);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("1e5")]
    [InlineData("NaN")]
    [InlineData("Infinity")]
    [InlineData("1,000.5")]
    [InlineData("5d")]
    [InlineData("1234567890123456")]
    [InlineData("1.12345678901")]
    [InlineData("٣")]
    public void DeberiaRechazarTextoYFormatosNoNumericos(string? texto)
    {
        Assert.Throws<ArgumentException>(() => EntradaUtil.InterpretarDecimal(texto));
    }
}
