using System.ServiceModel;
using ec.edu.monster.Cliente;
using ReferenciaServicio;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ClienteWSConversorPruebas
{
    private const string Url = "http://10.0.2.2:5080/WSConversorUnidades.svc";

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
        Assert.Equal(["Monster", "Monster9"], servicio.Argumentos);
    }

    [Fact]
    public async Task DeberiaEnviarValorYUnidadesAlConvertir()
    {
        servicio.ValorConvertido = 212.0;
        await cliente.LoginAsync("Monster", "Monster9");

        double resultado = await cliente.ConvertirTemperaturaAsync(100, "CELSIUS", "FAHRENHEIT");

        Assert.Equal(212.0, resultado);
        Assert.Equal("ConvertirTemperatura", servicio.OperacionLlamada);
        Assert.Equal([100.0, "CELSIUS", "FAHRENHEIT"], servicio.Argumentos);
    }

    [Fact]
    public async Task DeberiaLeerElCatalogoDeUnidades()
    {
        servicio.Unidades = ["GRAMO", "ONZA"];
        await cliente.LoginAsync("Monster", "Monster9");

        IReadOnlyList<string> unidades = await cliente.ConsultarUnidadesAsync("masa");

        Assert.Equal(["GRAMO", "ONZA"], unidades);
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

    [Theory]
    [InlineData(400, "La temperatura no puede ser menor al cero absoluto (-273.15 CELSIUS)")]
    [InlineData(401, "Acceso denegado: La sesión ha expirado. Inicie sesión nuevamente.")]
    public async Task DeberiaConvertirElFaultDelServidorEnExcepcionConSuCodigo(int codigo, string mensaje)
    {
        await cliente.LoginAsync("Monster", "Monster9");
        servicio.Error = new FaultException<ErrorServicio>(
                new ErrorServicio { Codigo = codigo, Mensaje = mensaje }, new FaultReason(mensaje));

        var error = await Assert.ThrowsAsync<ExcepcionClienteWS>(
                () => cliente.ConvertirTemperaturaAsync(-500, "CELSIUS", "KELVIN"));

        Assert.Equal(codigo, error.CodigoEstado);
        Assert.Equal(mensaje, error.Message);
    }

    [Fact]
    public async Task DeberiaExigirSesionAntesDeConsultarElHistorial()
    {
        var error = await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.ConsultarHistorialAsync());

        Assert.True(error.EsErrorAutenticacion);
        Assert.Null(servicio.OperacionLlamada);
    }

    [Fact]
    public async Task CerrarSesionDeberiaOlvidarElToken()
    {
        await cliente.LoginAsync("Monster", "Monster9");

        cliente.CerrarSesion();

        Assert.Null(cliente.TokenSesion);
        await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.LimpiarHistorialAsync());
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
}
