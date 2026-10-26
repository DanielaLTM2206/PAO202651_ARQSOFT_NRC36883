using ec.edu.monster.Modelo;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class TipoConversionPruebas
{
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
