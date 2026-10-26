using ec.edu.monster.Controlador;
using ec.edu.monster.Modelo;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ConversionControladorPruebas
{
    private readonly ContratoFalso contrato = new();
    private readonly ConversionControlador controlador;

    public ConversionControladorPruebas()
    {
        controlador = new ConversionControlador(contrato);
    }

    [Theory]
    [InlineData(TipoConversion.Longitud, "ConvertirLongitud")]
    [InlineData(TipoConversion.Masa, "ConvertirMasa")]
    [InlineData(TipoConversion.Temperatura, "ConvertirTemperatura")]
    public async Task DeberiaLlamarLaOperacionDelTipoSeleccionado(TipoConversion tipo, string operacionEsperada)
    {
        double resultado = await controlador.ConvertirAsync(new ConversionModelo(tipo, 10, "A", "B"));

        Assert.Equal(42.0, resultado);
        Assert.Equal(operacionEsperada, contrato.OperacionLlamada);
        Assert.Equal(10, contrato.ValorRecibido);
        Assert.Equal("A", contrato.UnidadInicialRecibida);
        Assert.Equal("B", contrato.UnidadFinalRecibida);
    }

    [Theory]
    [InlineData(TipoConversion.Longitud, "longitud")]
    [InlineData(TipoConversion.Masa, "masa")]
    [InlineData(TipoConversion.Temperatura, "temperatura")]
    public async Task DeberiaConsultarUnidadesPorCategoria(TipoConversion tipo, string categoria)
    {
        IReadOnlyList<string> unidades = await controlador.ObtenerUnidadesAsync(tipo);

        Assert.Equal(["A", "B"], unidades);
        Assert.Equal("ConsultarUnidades:" + categoria, contrato.OperacionLlamada);
    }

    [Theory]
    [InlineData(TipoConversion.Longitud, -1)]
    [InlineData(TipoConversion.Masa, -0.5)]
    public void DeberiaRechazarLongitudYMasaNegativas(TipoConversion tipo, double valor)
    {
        Assert.Throws<ArgumentException>(() => controlador.ValidarValor(tipo, valor));
    }

    [Theory]
    [InlineData(TipoConversion.Longitud, 0)]
    [InlineData(TipoConversion.Masa, 0)]
    [InlineData(TipoConversion.Temperatura, -40)]
    public void DeberiaAceptarCeroYTemperaturasNegativas(TipoConversion tipo, double valor)
    {
        controlador.ValidarValor(tipo, valor);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void DeberiaRechazarValoresNoFinitos(double valor)
    {
        Assert.Throws<ArgumentException>(() => controlador.ValidarValor(TipoConversion.Masa, valor));
    }

    [Theory]
    [InlineData("METRO", "METRO")]
    [InlineData("metro", "METRO")]
    public void DeberiaRechazarUnidadesIguales(string unidadInicial, string unidadFinal)
    {
        Assert.Throws<ArgumentException>(() => controlador.ValidarUnidades(unidadInicial, unidadFinal));
    }

    [Theory]
    [InlineData(null, "METRO", "La unidad inicial es obligatoria.")]
    [InlineData("METRO", " ", "La unidad final es obligatoria.")]
    public void DeberiaFallarCuandoFaltaUnaUnidad(string? unidadInicial, string? unidadFinal, string mensajeEsperado)
    {
        var error = Assert.Throws<ArgumentException>(() => controlador.ValidarUnidades(unidadInicial, unidadFinal));

        Assert.Equal(mensajeEsperado, error.Message);
    }

    [Fact]
    public async Task DeberiaFallarCuandoSolicitudEsNula()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => controlador.ConvertirAsync(null!));
        Assert.Null(contrato.OperacionLlamada);
    }

    [Fact]
    public async Task DeberiaFallarCuandoTipoEsNulo()
    {
        await Assert.ThrowsAsync<ArgumentException>(
                () => controlador.ConvertirAsync(new ConversionModelo(null, 1, "A", "B")));
        await Assert.ThrowsAsync<ArgumentException>(() => controlador.ObtenerUnidadesAsync(null));
    }
}
