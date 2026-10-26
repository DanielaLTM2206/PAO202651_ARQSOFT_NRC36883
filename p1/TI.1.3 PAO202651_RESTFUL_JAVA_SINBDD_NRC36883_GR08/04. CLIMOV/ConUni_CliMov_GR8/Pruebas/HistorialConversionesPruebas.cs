using ec.edu.monster.Cliente.Dto;
using ec.edu.monster.Controlador;
using ec.edu.monster.Modelo;
using Xunit;

namespace ec.edu.monster.Pruebas;

/// <summary>
/// El historial es compartido: lo guarda el servidor y el controlador solo lo consulta o lo borra.
/// </summary>
public class HistorialConversionesPruebas
{
    private readonly ContratoFalso clienteWs = new();
    private readonly ConversionControlador controlador;

    public HistorialConversionesPruebas()
    {
        controlador = new ConversionControlador(clienteWs);
    }

    private static RegistroConversionRespuesta Registro(string fecha, string tipo, double valor,
            string origen, double resultado, string destino) => new()
    {
        FechaHora = fecha,
        Tipo = tipo,
        Valor = valor,
        UnidadOrigen = origen,
        Resultado = resultado,
        UnidadDestino = destino
    };

    [Fact]
    public async Task DeberiaIniciarVacioSiElServidorNoTieneConversiones()
    {
        IReadOnlyList<RegistroConversion> historial = await controlador.ObtenerHistorialAsync();

        Assert.Empty(historial);
        Assert.Equal("ConsultarHistorial", clienteWs.OperacionLlamada);
    }

    [Fact]
    public async Task DeberiaConvertirLosRegistrosDelServidorAlModelo()
    {
        clienteWs.HistorialServidor.Add(Registro("2026-10-18T19:28:36", "Longitud", 1000, "METRO", 1, "KILOMETRO"));

        RegistroConversion registro = Assert.Single(await controlador.ObtenerHistorialAsync());

        Assert.Equal(new RegistroConversion(new DateTime(2026, 10, 18, 19, 28, 36),
                TipoConversion.Longitud, 1000, "METRO", 1, "KILOMETRO"), registro);
    }

    [Fact]
    public async Task DeberiaConservarElOrdenDelServidor()
    {
        clienteWs.HistorialServidor.Add(Registro("2026-10-18T19:30:00", "Temperatura", 3, "CELSIUS", 276.15, "KELVIN"));
        clienteWs.HistorialServidor.Add(Registro("2026-10-18T19:29:00", "Masa", 2, "GRAMO", 0.07, "ONZA"));
        clienteWs.HistorialServidor.Add(Registro("2026-10-18T19:28:00", "Longitud", 1, "METRO", 1.09, "YARDA"));

        IReadOnlyList<RegistroConversion> historial = await controlador.ObtenerHistorialAsync();

        Assert.Equal(
            [TipoConversion.Temperatura, TipoConversion.Masa, TipoConversion.Longitud],
            historial.Select(r => r.Tipo));
    }

    [Theory]
    [InlineData("no-es-fecha", "Longitud", "METRO", "KILOMETRO")]
    [InlineData("2026-10-18T19:28:36", "Volumen", "METRO", "KILOMETRO")]
    [InlineData("2026-10-18T19:28:36", "Longitud", "", "KILOMETRO")]
    [InlineData("2026-10-18T19:28:36", "Longitud", "METRO", "")]
    public async Task DeberiaOmitirRegistrosIncompletos(string fecha, string tipo, string origen, string destino)
    {
        clienteWs.HistorialServidor.Add(Registro(fecha, tipo, 1, origen, 1, destino));
        clienteWs.HistorialServidor.Add(Registro("2026-10-18T19:28:36", "Masa", 1, "GRAMO", 0.001, "KILOGRAMO"));

        RegistroConversion registro = Assert.Single(await controlador.ObtenerHistorialAsync());

        Assert.Equal(TipoConversion.Masa, registro.Tipo);
    }

    [Fact]
    public async Task DeberiaBorrarElHistorialEnElServidor()
    {
        clienteWs.HistorialServidor.Add(Registro("2026-10-18T19:28:36", "Longitud", 1, "METRO", 100, "CENTIMETRO"));

        string mensaje = await controlador.LimpiarHistorialAsync();

        Assert.Equal("LimpiarHistorial", clienteWs.OperacionLlamada);
        Assert.Equal("Historial de conversiones borrado", mensaje);
        Assert.Empty(await controlador.ObtenerHistorialAsync());
    }
}
