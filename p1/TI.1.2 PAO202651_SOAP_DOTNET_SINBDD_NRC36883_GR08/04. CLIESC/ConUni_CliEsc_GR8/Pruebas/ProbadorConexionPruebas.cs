using System.Net;
using ec.edu.monster.Cliente;
using ec.edu.monster.Modelo;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ProbadorConexionPruebas
{
    private const string Url = "http://192.168.1.25:5080/WSConversorUnidades.svc";

    private readonly ManejadorHttpFalso manejador = new();
    private readonly ProbadorConexion probador;

    public ProbadorConexionPruebas()
    {
        probador = new ProbadorConexion(manejador);
    }

    [Fact]
    public async Task DeberiaPedirElWsdlYAceptarElServicio()
    {
        ResultadoConexion resultado = await probador.ProbarAsync(Url);

        Assert.True(resultado.Exitosa);
        Assert.Equal(Url + "?wsdl", manejador.UltimaUrl!.ToString());
    }

    [Fact]
    public async Task DeberiaRechazarUnCodigoDeError()
    {
        manejador.Codigo = HttpStatusCode.NotFound;

        ResultadoConexion resultado = await probador.ProbarAsync(Url);

        Assert.False(resultado.Exitosa);
        Assert.Contains("404", resultado.Mensaje);
    }

    [Fact]
    public async Task DeberiaRechazarOtroServicio()
    {
        manejador.Contenido = "<html>Otra aplicación</html>";

        ResultadoConexion resultado = await probador.ProbarAsync(Url);

        Assert.False(resultado.Exitosa);
        Assert.Contains("no es el servicio", resultado.Mensaje);
    }

    [Fact]
    public async Task DeberiaInformarSiNoHayConexion()
    {
        manejador.Excepcion = new HttpRequestException("Conexión rechazada");

        ResultadoConexion resultado = await probador.ProbarAsync(Url);

        Assert.False(resultado.Exitosa);
        Assert.Contains("No fue posible conectar", resultado.Mensaje);
    }

    [Fact]
    public async Task DeberiaInformarSiElServidorNoResponde()
    {
        manejador.Excepcion = new TaskCanceledException();

        ResultadoConexion resultado = await probador.ProbarAsync(Url);

        Assert.False(resultado.Exitosa);
        Assert.Contains("no respondió", resultado.Mensaje);
    }
}
