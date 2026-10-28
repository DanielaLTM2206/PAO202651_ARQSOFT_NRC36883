using ec.edu.monster.Modelo;

namespace ec.edu.monster.Cliente;

/// <summary>
/// Comprueba que en una dirección responda el servicio SOAP WSConversorUnidades pidiendo su WSDL,
/// antes de guardarla. Distingue un servidor apagado o bloqueado por el firewall de una ruta equivocada.
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

    public async Task<ResultadoConexion> ProbarAsync(string urlServicio)
    {
        string url = urlServicio.Trim().TrimEnd('/');
        using var clienteHttp = manejadorHttp is null ? new HttpClient() : new HttpClient(manejadorHttp, false);
        clienteHttp.Timeout = TiempoEspera;

        try
        {
            using HttpResponseMessage respuesta = await clienteHttp.GetAsync(url + "?wsdl");
            if (!respuesta.IsSuccessStatusCode)
            {
                return new ResultadoConexion(false,
                    $"El equipo en {url} respondió con el código {(int)respuesta.StatusCode}. " +
                    "Revise que la dirección termine en /WSConversorUnidades.svc.");
            }

            string contenido = await respuesta.Content.ReadAsStringAsync();
            if (!contenido.Contains("WSConversorUnidades", StringComparison.Ordinal))
            {
                return new ResultadoConexion(false,
                    $"La dirección {url} responde, pero no es el servicio SOAP WSConversorUnidades.");
            }
            return new ResultadoConexion(true, $"Conexión correcta con el servidor SOAP en {url}.");
        }
        catch (TaskCanceledException)
        {
            return new ResultadoConexion(false,
                $"El servidor en {url} no respondió en {TiempoEspera.TotalSeconds:0} segundos. " +
                "Verifique la IP, que ambas computadoras estén en la misma red y que el firewall permita el puerto 5080.");
        }
        catch (HttpRequestException)
        {
            return new ResultadoConexion(false,
                $"No fue posible conectar con {url}. " +
                "Verifique que WS_ConUni_Dotnet_SOAP esté encendido, la IP y el puerto, y el firewall del servidor.");
        }
    }
}
