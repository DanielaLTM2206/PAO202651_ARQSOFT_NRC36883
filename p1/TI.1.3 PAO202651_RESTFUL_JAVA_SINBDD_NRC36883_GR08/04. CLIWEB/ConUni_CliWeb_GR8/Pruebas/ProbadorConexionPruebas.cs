using System.Net;
using ec.edu.monster.Cliente;
using ec.edu.monster.Models;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ProbadorConexionPruebas
{
    private const string Url = "http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api";

    private readonly ManejadorHttpPrueba manejador = new();
    private readonly ProbadorConexion probador;

    public ProbadorConexionPruebas()
    {
        probador = new ProbadorConexion(manejador);
    }

    [Fact]
    public async Task DeberiaAceptarElServicioAunqueRespondaSinToken()
    {
        ResultadoConexion resultado = await probador.ProbarAsync(Url);

        Assert.True(resultado.Exitosa);
        Assert.Equal(Url + "/unidades", manejador.UltimaUrl!.ToString());
    }

    [Fact]
    public async Task DeberiaAceptarUnaRespuestaCorrectaDelCatalogo()
    {
        manejador.Codigo = HttpStatusCode.OK;
        manejador.Contenido = "[{\"Categoria\":\"masa\",\"Unidades\":[\"KILOGRAMO\"]}]";

        Assert.True((await probador.ProbarAsync(Url + "/")).Exitosa);
    }

    [Fact]
    public async Task DeberiaIndicarRutaEquivocadaConUn404()
    {
        manejador.Codigo = HttpStatusCode.NotFound;
        manejador.Contenido = "<html>Not Found</html>";

        ResultadoConexion resultado = await probador.ProbarAsync(Url);

        Assert.False(resultado.Exitosa);
        Assert.Contains("/WS_ConUni_Java_RESTFUL/api", resultado.Mensaje);
    }

    [Fact]
    public async Task DeberiaRechazarOtroServicio()
    {
        manejador.Codigo = HttpStatusCode.OK;
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
