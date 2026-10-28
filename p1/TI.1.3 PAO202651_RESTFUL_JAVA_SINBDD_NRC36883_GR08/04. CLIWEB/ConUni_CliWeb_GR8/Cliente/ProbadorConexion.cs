using ec.edu.monster.Models;

namespace ec.edu.monster.Cliente;

/// <summary>
/// Comprueba que en una dirección responda el servicio RESTful WSConversorUnidades antes de guardarla.
/// Pide GET /unidades sin token: el servicio responde 401 con {"Mensaje": "..."}, lo que confirma que está
/// activo. Distingue un servidor apagado o bloqueado por el firewall de una ruta equivocada (404).
/// </summary>
public class ProbadorConexion
{
    private static readonly TimeSpan TiempoEspera = TimeSpan.FromSeconds(5);

    private readonly HttpMessageHandler? manejadorHttp;

    public ProbadorConexion()
    {
    }

    // El manejador HTTP se puede reemplazar en las pruebas para no depender del servidor
    public ProbadorConexion(HttpMessageHandler manejadorHttp)
    {
        this.manejadorHttp = manejadorHttp;
    }

    public async Task<ResultadoConexion> ProbarAsync(string baseUrl)
    {
        string url = baseUrl.Trim().TrimEnd('/');
        using var clienteHttp = manejadorHttp is null ? new HttpClient() : new HttpClient(manejadorHttp, false);
        clienteHttp.Timeout = TiempoEspera;

        try
        {
            using var peticion = new HttpRequestMessage(HttpMethod.Get, url + "/unidades");
            peticion.Headers.Accept.ParseAdd("application/json");
            using HttpResponseMessage respuesta = await clienteHttp.SendAsync(peticion);

            int codigo = (int)respuesta.StatusCode;
            string contenido = await respuesta.Content.ReadAsStringAsync();
            bool esElServicio = (codigo == 401 && contenido.Contains("\"Mensaje\"", StringComparison.Ordinal))
                    || (codigo == 200 && contenido.Contains("\"Unidades\"", StringComparison.Ordinal));

            if (esElServicio)
            {
                return new ResultadoConexion(true, $"Conexión correcta con el servidor REST en {url}.");
            }
            if (codigo == 404)
            {
                return new ResultadoConexion(false,
                    $"El equipo en {url} respondió con el código 404. " +
                    "Revise que la dirección termine en /WS_ConUni_Java_RESTFUL/api.");
            }
            return new ResultadoConexion(false,
                $"La dirección {url} responde (código {codigo}), pero no es el servicio REST WSConversorUnidades.");
        }
        catch (TaskCanceledException)
        {
            return new ResultadoConexion(false,
                $"El servidor en {url} no respondió en {TiempoEspera.TotalSeconds:0} segundos. " +
                "Verifique la IP, que ambas computadoras estén en la misma red y que el firewall permita el puerto 8080.");
        }
        catch (Exception ex) when (ex is HttpRequestException or InvalidOperationException or UriFormatException)
        {
            return new ResultadoConexion(false,
                $"No fue posible conectar con {url}. " +
                "Verifique que Payara esté encendido con el servicio desplegado, la IP y el puerto, y el firewall del servidor.");
        }
    }
}
