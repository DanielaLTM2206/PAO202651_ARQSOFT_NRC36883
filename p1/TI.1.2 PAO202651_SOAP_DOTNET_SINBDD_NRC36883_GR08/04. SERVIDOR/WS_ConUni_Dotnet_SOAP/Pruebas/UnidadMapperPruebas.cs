using ec.edu.monster.Excepciones;
using ec.edu.monster.Modelos.Utilidades.Enums;
using ec.edu.monster.Modelos.Utilidades.Mapeadores;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class UnidadMapperPruebas
{
    [Theory]
    [InlineData("METRO", UnidadLongitud.METRO)]
    [InlineData("kilometro", UnidadLongitud.KILOMETRO)]
    [InlineData("  Yarda ", UnidadLongitud.YARDA)]
    public void DeberiaAceptarUnidadSinImportarMayusculasNiEspacios(string texto, UnidadLongitud esperada)
    {
        Assert.Equal(esperada, UnidadMapper.ALongitud(texto));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void DeberiaExigirLaUnidad(string? texto)
    {
        var error = Assert.Throws<ExcepcionValidacion>(() => UnidadMapper.AMasa(texto));
        Assert.Equal("La unidad de masa es obligatoria", error.Message);
    }

    [Theory]
    [InlineData("LIBRA")]
    [InlineData("METRO")]
    [InlineData("2")]
    public void DeberiaRechazarUnidadDeOtraCategoriaOInexistente(string texto)
    {
        Assert.Throws<ExcepcionValidacion>(() => UnidadMapper.AMasa(texto));
    }

    [Fact]
    public void DeberiaConvertirUnidadDeTemperatura()
    {
        Assert.Equal(UnidadTemperatura.RANKINE, UnidadMapper.ATemperatura("rankine"));
    }
}
