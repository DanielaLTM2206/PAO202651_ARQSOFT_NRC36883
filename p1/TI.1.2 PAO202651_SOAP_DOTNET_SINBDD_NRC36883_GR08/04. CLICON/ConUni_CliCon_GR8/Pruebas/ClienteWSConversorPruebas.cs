using System.ServiceModel;
using ec.edu.monster.Cliente;
using ReferenciaServicio;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ClienteWSConversorPruebas
{
    private const string Url = "http://localhost:5080/WSConversorUnidades.svc";

    private readonly ServicioSoapFalso servicio = new();
    private readonly ClienteWSConversor cliente;

    public ClienteWSConversorPruebas()
    {
        cliente = new ClienteWSConversor(servicio, Url);
    }

    [Fact]
    public async Task DeberiaGuardarElTokenAlIniciarSesion()
    {
        string token = await cliente.LoginAsync("Monster", "Monster9");

        Assert.Equal("token-de-prueba", token);
        Assert.Equal("token-de-prueba", cliente.TokenSesion);
        Assert.Equal("Login", servicio.OperacionLlamada);
        Assert.Equal(["Monster", "Monster9"], servicio.Argumentos);
    }

    [Fact]
    public async Task DeberiaEnviarValorYUnidadesAlConvertir()
    {
        servicio.ValorConvertido = 1.0;
        await cliente.LoginAsync("Monster", "Monster9");

        double resultado = await cliente.ConvertirLongitudAsync(1000, "METRO", "KILOMETRO");

        Assert.Equal(1.0, resultado);
        Assert.Equal("ConvertirLongitud", servicio.OperacionLlamada);
        Assert.Equal([1000.0, "METRO", "KILOMETRO"], servicio.Argumentos);
    }

    [Fact]
    public async Task DeberiaLeerElCatalogoDeUnidades()
    {
        servicio.Unidades = ["GRAMO", "ONZA"];
        await cliente.LoginAsync("Monster", "Monster9");

        IReadOnlyList<string> unidades = await cliente.ConsultarUnidadesAsync("masa");

        Assert.Equal(["GRAMO", "ONZA"], unidades);
        Assert.Equal(["masa"], servicio.Argumentos);
    }

    [Theory]
    [InlineData(400, "La masa no puede ser negativa")]
    [InlineData(401, "Acceso denegado: Token inválido.")]
    public async Task DeberiaConvertirElFaultDelServidorEnExcepcionConSuCodigo(int codigo, string mensaje)
    {
        await cliente.LoginAsync("Monster", "Monster9");
        servicio.Error = new FaultException<ErrorServicio>(
                new ErrorServicio { Codigo = codigo, Mensaje = mensaje }, new FaultReason(mensaje));

        var error = await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.ConvertirMasaAsync(-1, "GRAMO", "ONZA"));

        Assert.Equal(codigo, error.CodigoEstado);
        Assert.Equal(mensaje, error.Message);
    }

    [Fact]
    public async Task CredencialesIncorrectasDeberianSerErrorDeAutenticacion()
    {
        servicio.Error = new FaultException<ErrorServicio>(
                new ErrorServicio { Codigo = 401, Mensaje = "Credenciales incorrectas" },
                new FaultReason("Credenciales incorrectas"));

        var error = await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.LoginAsync("Monster", "mala"));

        Assert.True(error.EsErrorAutenticacion);
        Assert.Null(cliente.TokenSesion);
    }

    [Fact]
    public async Task DeberiaExigirSesionAntesDeConvertir()
    {
        var error = await Assert.ThrowsAsync<ExcepcionClienteWS>(
                () => cliente.ConvertirTemperaturaAsync(10, "CELSIUS", "KELVIN"));

        Assert.True(error.EsErrorAutenticacion);
        Assert.Null(servicio.OperacionLlamada);
    }

    [Fact]
    public async Task CerrarSesionDeberiaOlvidarElToken()
    {
        await cliente.LoginAsync("Monster", "Monster9");

        cliente.CerrarSesion();

        Assert.Null(cliente.TokenSesion);
        await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.ConsultarUnidadesAsync("masa"));
    }

    [Fact]
    public async Task ErrorDeComunicacionDeberiaSerErrorDeConexion()
    {
        servicio.Error = new EndpointNotFoundException("No hay ningún extremo escuchando");

        var error = await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.LoginAsync("Monster", "Monster9"));

        Assert.True(error.EsErrorConexion);
        Assert.Contains(Url, error.Message);
    }

    [Fact]
    public async Task DeberiaInformarCuandoNoHayServidor()
    {
        // Puerto sin ningún servicio escuchando: se usa el cliente real con WCF
        using var clienteReal = new ClienteWSConversor("http://localhost:9/WSConversorUnidades.svc");

        var error = await Assert.ThrowsAsync<ExcepcionClienteWS>(() => clienteReal.LoginAsync("Monster", "Monster9"));

        Assert.True(error.EsErrorConexion);
    }

    [Fact]
    public async Task DeberiaLeerYLimpiarElHistorialCompartido()
    {
        servicio.Historial =
        [
            new RegistroConversionRespuesta
            {
                FechaHora = "2026-10-19T15:30:00", Tipo = "Longitud", Valor = 1000,
                UnidadOrigen = "METRO", Resultado = 1, UnidadDestino = "KILOMETRO"
            }
        ];
        await cliente.LoginAsync("Monster", "Monster9");

        var historial = await cliente.ConsultarHistorialAsync();
        string mensaje = await cliente.LimpiarHistorialAsync();

        Assert.Single(historial);
        Assert.Equal("KILOMETRO", historial[0].UnidadDestino);
        Assert.Equal("Historial de conversiones borrado", mensaje);
        Assert.Empty(await cliente.ConsultarHistorialAsync());
    }

    [Fact]
    public async Task DeberiaExigirSesionAntesDeConsultarElHistorial()
    {
        var error = await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.ConsultarHistorialAsync());

        Assert.True(error.EsErrorAutenticacion);
        Assert.Null(servicio.OperacionLlamada);
    }
}
