using System.ServiceModel;
using ec.edu.monster.Cliente;
using ReferenciaServicio;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ClienteWSConversorPruebas
{
    private const string Url = "http://localhost:5080/WSConversorUnidades.svc";

    private readonly ServicioSoapFalso servicio = new();
    private readonly AlmacenTokenMemoria almacen = new();
    private readonly ClienteWSConversor cliente;

    public ClienteWSConversorPruebas()
    {
        cliente = new ClienteWSConversor(servicio, new ManejadorTokenSOAP(almacen), Url);
    }

    [Fact]
    public async Task DeberiaGuardarElTokenEnLaSesionAlIniciarSesion()
    {
        string token = await cliente.LoginAsync("Monster", "Monster9");

        Assert.Equal("token-de-prueba", token);
        Assert.Equal("token-de-prueba", almacen.Token);
        Assert.Equal(["Monster", "Monster9"], servicio.Argumentos);
    }

    [Fact]
    public async Task DeberiaEnviarValorYUnidadesAlConvertir()
    {
        servicio.ValorConvertido = 0.9144;
        await cliente.LoginAsync("Monster", "Monster9");

        double resultado = await cliente.ConvertirLongitudAsync(1, "YARDA", "METRO");

        Assert.Equal(0.9144, resultado);
        Assert.Equal("ConvertirLongitud", servicio.OperacionLlamada);
        Assert.Equal([1.0, "YARDA", "METRO"], servicio.Argumentos);
    }

    [Fact]
    public async Task DeberiaLeerElCatalogoDeUnidades()
    {
        servicio.Unidades = ["CELSIUS", "KELVIN"];
        await cliente.LoginAsync("Monster", "Monster9");

        IReadOnlyList<string> unidades = await cliente.ConsultarUnidadesAsync("temperatura");

        Assert.Equal(["CELSIUS", "KELVIN"], unidades);
        Assert.Equal(["temperatura"], servicio.Argumentos);
    }

    [Fact]
    public async Task DeberiaLeerYLimpiarElHistorialCompartido()
    {
        servicio.Historial =
        [
            new RegistroConversionRespuesta
            {
                FechaHora = "2026-10-23T10:15:00", Tipo = "Masa", Valor = 1,
                UnidadOrigen = "TONELADA", Resultado = 1000, UnidadDestino = "KILOGRAMO"
            }
        ];
        await cliente.LoginAsync("Monster", "Monster9");

        var historial = await cliente.ConsultarHistorialAsync();
        string mensaje = await cliente.LimpiarHistorialAsync();

        Assert.Single(historial);
        Assert.Equal("TONELADA", historial[0].UnidadOrigen);
        Assert.Equal("Historial de conversiones borrado", mensaje);
        Assert.Empty(await cliente.ConsultarHistorialAsync());
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
    public async Task DeberiaExigirSesionAntesDeConvertir()
    {
        var error = await Assert.ThrowsAsync<ExcepcionClienteWS>(
                () => cliente.ConvertirTemperaturaAsync(10, "CELSIUS", "KELVIN"));

        Assert.True(error.EsErrorAutenticacion);
        Assert.Null(servicio.OperacionLlamada);
    }

    [Fact]
    public async Task CerrarSesionDeberiaQuitarElTokenDeLaSesion()
    {
        await cliente.LoginAsync("Monster", "Monster9");

        cliente.CerrarSesion();

        Assert.Null(almacen.Token);
        await Assert.ThrowsAsync<ExcepcionClienteWS>(() => cliente.ConsultarHistorialAsync());
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
        using var clienteReal = new ClienteWSConversor(new ManejadorTokenSOAP(new AlmacenTokenMemoria()),
                "http://localhost:9/WSConversorUnidades.svc");

        var error = await Assert.ThrowsAsync<ExcepcionClienteWS>(() => clienteReal.LoginAsync("Monster", "Monster9"));

        Assert.True(error.EsErrorConexion);
    }
}
