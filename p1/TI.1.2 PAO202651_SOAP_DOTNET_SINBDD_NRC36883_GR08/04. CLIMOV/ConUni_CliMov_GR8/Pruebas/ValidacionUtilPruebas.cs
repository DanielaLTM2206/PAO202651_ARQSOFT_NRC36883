using ec.edu.monster.Util;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ValidacionUtilPruebas
{
    [Theory]
    [InlineData("25", 25.0)]
    [InlineData("12.5", 12.5)]
    [InlineData("12,5", 12.5)]
    [InlineData("-40", -40.0)]
    [InlineData("+3", 3.0)]
    [InlineData("  7.25  ", 7.25)]
    [InlineData("0", 0.0)]
    [InlineData("0.0000000001", 0.0000000001)]
    [InlineData("999999999999999", 999999999999999.0)]
    public void DeberiaAceptarNumerosDecimalesValidos(string texto, double esperado)
    {
        Assert.Equal(esperado, ValidacionUtil.InterpretarDecimal(texto));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("abc")]
    [InlineData("12abc")]
    [InlineData("1e5")]
    [InlineData("1E5")]
    [InlineData("NaN")]
    [InlineData("Infinity")]
    [InlineData("-Infinity")]
    [InlineData("5d")]
    [InlineData("0x10")]
    [InlineData("1.2.3")]
    [InlineData("1,000.5")]
    [InlineData(".5")]
    [InlineData("5.")]
    [InlineData("--5")]
    [InlineData("1 000")]
    [InlineData("1234567890123456")]
    [InlineData("1.12345678901")]
    [InlineData("٣")]
    public void DeberiaRechazarTextoYFormatosNoNumericos(string? texto)
    {
        Assert.Throws<ArgumentException>(() => ValidacionUtil.InterpretarDecimal(texto));
    }

    // Mientras se escribe se aceptan números incompletos; el texto que no es número se rechaza
    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("-", true)]
    [InlineData("+", true)]
    [InlineData("12", true)]
    [InlineData("12.", true)]
    [InlineData("12,", true)]
    [InlineData("-0.5", true)]
    [InlineData("999999999999999.1234567891", true)]
    [InlineData("a", false)]
    [InlineData("1e", false)]
    [InlineData("1 2", false)]
    [InlineData("1.2.3", false)]
    [InlineData("1,2.3", false)]
    [InlineData("--5", false)]
    [InlineData("5-", false)]
    [InlineData("1234567890123456", false)]
    [InlineData("1.12345678901", false)]
    [InlineData("٣", false)]
    public void DeberiaFiltrarElTextoMientrasSeEscribe(string? texto, bool permitido)
    {
        Assert.Equal(permitido, ValidacionUtil.EsTextoNumericoParcial(texto));
    }
}
