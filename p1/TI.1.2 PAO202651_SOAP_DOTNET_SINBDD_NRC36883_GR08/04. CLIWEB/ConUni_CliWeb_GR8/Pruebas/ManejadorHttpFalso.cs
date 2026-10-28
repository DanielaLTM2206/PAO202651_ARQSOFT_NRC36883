using System.Net;

namespace ec.edu.monster.Pruebas;

/// <summary>
/// Responde a las peticiones HTTP sin red: devuelve la respuesta indicada o lanza la excepción indicada.
/// </summary>
public class ManejadorHttpFalso : HttpMessageHandler
{
    public HttpStatusCode Codigo { get; set; } = HttpStatusCode.OK;

    public string Contenido { get; set; } = "<wsdl:definitions name=\"WSConversorUnidades\"></wsdl:definitions>";

    public Exception? Excepcion { get; set; }

    public Uri? UltimaUrl { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage peticion, CancellationToken cancelacion)
    {
        UltimaUrl = peticion.RequestUri;
        if (Excepcion is not null)
        {
            throw Excepcion;
        }
        return Task.FromResult(new HttpResponseMessage(Codigo) { Content = new StringContent(Contenido) });
    }
}
