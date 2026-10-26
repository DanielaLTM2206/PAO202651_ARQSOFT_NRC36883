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

    [Theory]
    [InlineData('5', "", 0, true)]
    [InlineData('-', "", 0, true)]
    [InlineData('-', "5", 1, false)]
    [InlineData('-', "-5", 0, false)]
    [InlineData('.', "12", 2, true)]
    [InlineData(',', "12", 2, true)]
    [InlineData('.', "1.2", 3, false)]
    [InlineData(',', "1.2", 3, false)]
    [InlineData('a', "", 0, false)]
    [InlineData('e', "1", 1, false)]
    [InlineData(' ', "1", 1, false)]
    [InlineData('\b', "12", 2, true)]
    public void DeberiaFiltrarCaracteresMientrasSeEscribe(char caracter, string textoActual, int posicion, bool permitido)
    {
        Assert.Equal(permitido, ValidacionUtil.EsCaracterNumericoPermitido(caracter, textoActual, posicion));
    }
}
