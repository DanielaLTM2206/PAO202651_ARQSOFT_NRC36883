using System.Net;
using System.Text;

namespace ec.edu.monster.Pruebas;

/// <summary>
/// Imita las respuestas del servicio RESTful sin abrir conexiones de red.
/// Guarda la última petición para verificar el JSON y el encabezado Authorization.
/// </summary>
internal class ManejadorHttpFalso : HttpMessageHandler
{
    public string? UltimoCuerpo { get; private set; }
    public string? UltimaAutorizacion { get; private set; }
    public HttpMethod? UltimoMetodo { get; private set; }
    public bool SimularSinConexion { get; set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage peticion, CancellationToken cancelacion)
    {
        if (SimularSinConexion)
        {
            throw new HttpRequestException("Conexión rechazada");
        }

        UltimaAutorizacion = peticion.Headers.Authorization?.ToString();
        UltimoMetodo = peticion.Method;
        UltimoCuerpo = peticion.Content is null ? null : await peticion.Content.ReadAsStringAsync(cancelacion);

        string ruta = peticion.RequestUri!.AbsolutePath;
        return ruta switch
        {
            "/api/seguridad/login" when UltimoCuerpo!.Contains("\"Contrasena\":\"Monster9\"")
                => Responder(HttpStatusCode.OK, "{\"Token\":\"abc-123\"}"),
            "/api/seguridad/login"
                => Responder(HttpStatusCode.Unauthorized, "{\"Mensaje\":\"Credenciales incorrectas\"}"),
            "/api/conversiones/longitud"
                => Responder(HttpStatusCode.OK, "{\"ValorConvertido\":1.0}"),
            "/api/conversiones/masa"
                => Responder(HttpStatusCode.BadRequest, "{\"Mensaje\":\"Unidad de masa inválida: PIE\"}"),
            "/api/conversiones/historial" when peticion.Method == HttpMethod.Delete
                => Responder(HttpStatusCode.OK, "{\"Mensaje\":\"Historial de conversiones borrado\"}"),
            "/api/conversiones/historial"
                => Responder(HttpStatusCode.OK,
                        "[{\"FechaHora\":\"2026-10-18T19:28:36\",\"Tipo\":\"Longitud\",\"Valor\":1000.0," +
                        "\"UnidadOrigen\":\"METRO\",\"Resultado\":1.0,\"UnidadDestino\":\"KILOMETRO\"}]"),
            "/api/unidades/temperatura"
                => Responder(HttpStatusCode.OK,
                        "{\"Categoria\":\"temperatura\",\"Unidades\":[\"CELSIUS\",\"FAHRENHEIT\",\"KELVIN\",\"RANKINE\"]}"),
            _ => Responder(HttpStatusCode.InternalServerError, "<html>Error interno</html>", "text/html")
        };
    }

    private static HttpResponseMessage Responder(HttpStatusCode codigo, string cuerpo, string tipo = "application/json")
    {
        return new HttpResponseMessage(codigo)
        {
            Content = new StringContent(cuerpo, Encoding.UTF8, tipo)
        };
    }
}
