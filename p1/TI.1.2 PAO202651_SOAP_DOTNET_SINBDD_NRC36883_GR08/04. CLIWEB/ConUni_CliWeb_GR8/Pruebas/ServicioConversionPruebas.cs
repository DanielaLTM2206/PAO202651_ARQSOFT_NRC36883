using ec.edu.monster.Servicios;
using ec.edu.monster.Models;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ServicioConversionPruebas
{
    private readonly ContratoFalso clienteWs = new();
    private readonly ServicioConversion servicio;

    public ServicioConversionPruebas()
    {
        servicio = new ServicioConversion(clienteWs);
    }

    [Theory]
    [InlineData(TipoConversion.Longitud, 10, "METRO", "YARDA", "ConvertirLongitud")]
    [InlineData(TipoConversion.Masa, 2.5, "KILOGRAMO", "ONZA", "ConvertirMasa")]
    [InlineData(TipoConversion.Temperatura, 25, "CELSIUS", "RANKINE", "ConvertirTemperatura")]
    public async Task DeberiaLlamarLaOperacionDelTipoSeleccionado(TipoConversion tipo, double valor,
            string unidadInicial, string unidadFinal, string operacionEsperada)
    {
        double resultado = await servicio.ConvertirAsync(new ConversionModelo(tipo, valor, unidadInicial, unidadFinal));

        Assert.Equal(42.0, resultado, 6);
        Assert.Equal(operacionEsperada, clienteWs.OperacionLlamada);
        Assert.Equal(valor, clienteWs.ValorRecibido, 6);
        Assert.Equal(unidadInicial, clienteWs.UnidadInicialRecibida);
        Assert.Equal(unidadFinal, clienteWs.UnidadFinalRecibida);
    }

    [Theory]
    [InlineData(TipoConversion.Longitud, "longitud")]
    [InlineData(TipoConversion.Masa, "masa")]
    [InlineData(TipoConversion.Temperatura, "temperatura")]
    public async Task DeberiaConsultarUnidadesPorCategoria(TipoConversion tipo, string categoria)
    {
        await servicio.ObtenerUnidadesAsync(tipo);

        Assert.Equal("ConsultarUnidades:" + categoria, clienteWs.OperacionLlamada);
    }

    [Theory]
    [InlineData(TipoConversion.Longitud, -1)]
    [InlineData(TipoConversion.Longitud, -0.001)]
    [InlineData(TipoConversion.Masa, -5)]
    [InlineData(TipoConversion.Masa, -28.3495)]
    public async Task DeberiaRechazarLongitudYMasaNegativas(TipoConversion tipo, double valor)
    {
        Assert.Throws<ArgumentException>(() => servicio.ValidarValor(tipo, valor));
        await Assert.ThrowsAsync<ArgumentException>(
                () => servicio.ConvertirAsync(new ConversionModelo(tipo, valor, "A", "B")));

        Assert.Null(clienteWs.OperacionLlamada);
    }

    [Theory]
    [InlineData(TipoConversion.Longitud, 0)]
    [InlineData(TipoConversion.Masa, 0)]
    [InlineData(TipoConversion.Temperatura, -40)]
    [InlineData(TipoConversion.Temperatura, -273.15)]
    public void DeberiaAceptarCeroYTemperaturasNegativas(TipoConversion tipo, double valor)
    {
        var ex = Record.Exception(() => servicio.ValidarValor(tipo, valor));

        Assert.Null(ex);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void DeberiaRechazarValoresNoFinitos(double valor)
    {
        Assert.Throws<ArgumentException>(() => servicio.ValidarValor(TipoConversion.Temperatura, valor));
    }

    [Theory]
    [InlineData("METRO", "METRO")]
    [InlineData("metro", "METRO")]
    [InlineData("CELSIUS", "CELSIUS")]
    public void DeberiaRechazarUnidadesIguales(string unidadInicial, string unidadFinal)
    {
        var ex = Assert.Throws<ArgumentException>(() => servicio.ValidarUnidades(unidadInicial, unidadFinal));

        Assert.Equal("La unidad de destino debe ser diferente a la unidad de origen.", ex.Message);
    }

    [Theory]
    [InlineData(null, "YARDA", "La unidad de origen es obligatoria.")]
    [InlineData("   ", "YARDA", "La unidad de origen es obligatoria.")]
    [InlineData("METRO", null, "La unidad de destino es obligatoria.")]
    [InlineData("METRO", "   ", "La unidad de destino es obligatoria.")]
    public async Task DeberiaFallarCuandoFaltaUnaUnidad(string? unidadInicial, string? unidadFinal, string mensajeEsperado)
    {
        var modelo = new ConversionModelo(TipoConversion.Longitud, 10, unidadInicial, unidadFinal);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => servicio.ConvertirAsync(modelo));

        Assert.Equal(mensajeEsperado, ex.Message);
        Assert.Null(clienteWs.OperacionLlamada);
    }

    [Fact]
    public async Task DeberiaFallarCuandoTipoEsNulo()
    {
        var modelo = new ConversionModelo(null, 10, "METRO", "YARDA");

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => servicio.ConvertirAsync(modelo));

        Assert.Equal("Debe seleccionar el tipo de conversión.", ex.Message);
    }
}
