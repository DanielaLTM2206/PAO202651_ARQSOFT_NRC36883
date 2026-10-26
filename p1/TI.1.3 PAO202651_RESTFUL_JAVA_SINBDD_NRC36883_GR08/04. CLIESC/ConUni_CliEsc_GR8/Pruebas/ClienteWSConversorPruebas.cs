using ec.edu.monster.Cliente;
using ec.edu.monster.Cliente.Dto;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ClienteWSConversorPruebas
{
    private readonly ManejadorHttpFalso servidor = new();
    private readonly ClienteWSConversor cliente;

    public ClienteWSConversorPruebas()
    {
        cliente = new ClienteWSConversor("http://servidor-de-prueba/api/", servidor);
    }

    [Fact]
    public async Task DeberiaGuardarElTokenAlIniciarSesion()
    {
        string token = await cliente.LoginAsync("Monster", "Monster9");

        Assert.Equal("abc-123", token);
        Assert.Equal("abc-123", cliente.TokenSesion);
    }

    [Fact]
    public async Task DeberiaEnviarJsonYTokenBearerAlConvertir()
    {
        await cliente.LoginAsync("Monster", "Monster9");

        double resultado = await cliente.ConvertirLongitudAsync(1000, "METRO", "KILOMETRO");

        Assert.Equal(1.0, resultado, 6);
        Assert.Equal("Bearer abc-123", servidor.UltimaAutorizacion);
        Assert.Equal("{\"Valor\":1000,\"UnidadOrigen\":\"METRO\",\"UnidadDestino\":\"KILOMETRO\"}", servidor.UltimoCuerpo);
    }

    [Fact]
    public async Task DeberiaLeerElCatalogoDeUnidades()
    {
        await cliente.LoginAsync("Monster", "Monster9");

        IReadOnlyList<string> unidades = await cliente.ConsultarUnidadesAsync("temperatura");

        Assert.Equal(["CELSIUS", "FAHRENHEIT", "KELVIN", "RANKINE"], unidades);
    }

    [Fact]
    public async Task DeberiaTraducirCredencialesIncorrectasA401()
    {
        var ex = await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.LoginAsync("Monster", "otra"));

        Assert.True(ex.EsErrorAutenticacion);
        Assert.Equal("Credenciales incorrectas", ex.Message);
    }

    [Fact]
    public async Task DeberiaPropagarElMensajeDeErrorDelServidor()
    {
        await cliente.LoginAsync("Monster", "Monster9");

        var ex = await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.ConvertirMasaAsync(1, "PIE", "GRAMO"));

        Assert.Equal(400, ex.CodigoEstado);
        Assert.Equal("Unidad de masa inválida: PIE", ex.Message);
    }

    [Fact]
    public async Task DeberiaUsarUnMensajeGenericoSiLaRespuestaNoEsJson()
    {
        await cliente.LoginAsync("Monster", "Monster9");

        var ex = await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.ConsultarUnidadesAsync("masa"));

        Assert.Equal(500, ex.CodigoEstado);
        Assert.Contains("500", ex.Message);
    }

    [Fact]
    public async Task DeberiaExigirSesionAntesDeConvertir()
    {
        var ex = await Assert.ThrowsAsync<ExcepcionClienteWS>(
                () => cliente.ConvertirLongitudAsync(1, "METRO", "KILOMETRO"));

        Assert.True(ex.EsErrorAutenticacion);
        Assert.Null(servidor.UltimoCuerpo);
    }

    [Fact]
    public async Task DeberiaInformarCuandoNoHayServidor()
    {
        servidor.SimularSinConexion = true;

        var ex = await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.LoginAsync("Monster", "Monster9"));

        Assert.True(ex.EsErrorConexion);
    }

    [Fact]
    public async Task DeberiaLeerElHistorialCompartidoDelServidor()
    {
        await cliente.LoginAsync("Monster", "Monster9");

        IReadOnlyList<RegistroConversionRespuesta> historial = await cliente.ConsultarHistorialAsync();

        RegistroConversionRespuesta registro = Assert.Single(historial);
        Assert.Equal(HttpMethod.Get, servidor.UltimoMetodo);
        Assert.Equal("Bearer abc-123", servidor.UltimaAutorizacion);
        Assert.Equal("2026-10-18T19:28:36", registro.FechaHora);
        Assert.Equal("Longitud", registro.Tipo);
        Assert.Equal(1000, registro.Valor);
        Assert.Equal("METRO", registro.UnidadOrigen);
        Assert.Equal(1.0, registro.Resultado);
        Assert.Equal("KILOMETRO", registro.UnidadDestino);
    }

    [Fact]
    public async Task DeberiaBorrarElHistorialConDelete()
    {
        await cliente.LoginAsync("Monster", "Monster9");

        string mensaje = await cliente.LimpiarHistorialAsync();

        Assert.Equal(HttpMethod.Delete, servidor.UltimoMetodo);
        Assert.Equal("Historial de conversiones borrado", mensaje);
    }

    [Fact]
    public async Task DeberiaExigirSesionParaConsultarElHistorial()
    {
        var ex = await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.ConsultarHistorialAsync());

        Assert.True(ex.EsErrorAutenticacion);
    }

    [Fact]
    public async Task DeberiaOlvidarElTokenAlCerrarSesion()
    {
        await cliente.LoginAsync("Monster", "Monster9");

        cliente.CerrarSesion();

        Assert.Null(cliente.TokenSesion);
        await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.ConvertirLongitudAsync(1, "METRO", "KILOMETRO"));
    }
}
