using System.Net;

namespace ec.edu.monster.Pruebas;

/// <summary>
/// Responde a la prueba de conexión sin red: por defecto imita al servicio RESTful sin token
/// (401 con {"Mensaje": "..."}), o lanza la excepción indicada.
/// </summary>
public class ManejadorHttpPrueba : HttpMessageHandler
{
    public HttpStatusCode Codigo { get; set; } = HttpStatusCode.Unauthorized;

    public string Contenido { get; set; } = "{\"Mensaje\":\"Token de sesión requerido\"}";

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
