using System.ServiceModel;
using ReferenciaServicio;

namespace ec.edu.monster.Cliente;

/// <summary>
/// Cliente del servicio SOAP WSConversorUnidades. Usa el proxy generado desde el WSDL
/// (Connected Services\ReferenciaServicio) y traduce los SOAP Faults a ExcepcionClienteWS.
/// </summary>
public class ClienteWSConversor : IContratoWSConversor, IDisposable
{
    private static readonly TimeSpan TiempoEspera = TimeSpan.FromSeconds(10);

    private readonly ChannelFactory<IWSConversorUnidades>? fabrica;
    private readonly Func<IWSConversorUnidades> crearCanal;
    private readonly ManejadorTokenSOAP manejadorToken = new();

    public string UrlServicio { get; }

    public ClienteWSConversor(string urlServicio)
    {
        UrlServicio = urlServicio.Trim();

        fabrica = new ChannelFactory<IWSConversorUnidades>(CrearEnlace(), new EndpointAddress(UrlServicio));
        fabrica.Endpoint.EndpointBehaviors.Add(manejadorToken);
        crearCanal = fabrica.CreateChannel;
    }

    // El servicio se puede reemplazar en las pruebas para no depender del servidor
    public ClienteWSConversor(IWSConversorUnidades servicio, string urlServicio)
    {
        UrlServicio = urlServicio;
        crearCanal = () => servicio;
    }

    public string? TokenSesion => manejadorToken.ObtenerToken();

    public async Task<string> LoginAsync(string usuario, string contrasenia)
    {
        LoginRespuesta respuesta = await EjecutarAsync(canal => canal.LoginAsync(usuario, contrasenia));

        if (string.IsNullOrWhiteSpace(respuesta?.Token))
        {
            throw new ExcepcionClienteWS(500, "El servidor no devolvió un token de sesión.");
        }
        manejadorToken.GuardarToken(respuesta.Token);
        return respuesta.Token;
    }

    public async Task<string> CambiarContraseniaAsync(string contraseniaActual, string contraseniaNueva)
    {
        manejadorToken.ExigirSesion();
        MensajeRespuesta respuesta = await EjecutarAsync(
                canal => canal.CambiarContraseniaAsync(contraseniaActual, contraseniaNueva));
        return respuesta?.Mensaje ?? "Contraseña actualizada correctamente";
    }

    public async Task<double> ConvertirLongitudAsync(double valor, string unidadInicial, string unidadFinal)
    {
        manejadorToken.ExigirSesion();
        ConversionRespuesta respuesta = await EjecutarAsync(
                canal => canal.ConvertirLongitudAsync(valor, unidadInicial, unidadFinal));
        return respuesta.ValorConvertido;
    }

    public async Task<double> ConvertirMasaAsync(double valor, string unidadInicial, string unidadFinal)
    {
        manejadorToken.ExigirSesion();
        ConversionRespuesta respuesta = await EjecutarAsync(
                canal => canal.ConvertirMasaAsync(valor, unidadInicial, unidadFinal));
        return respuesta.ValorConvertido;
    }

    public async Task<double> ConvertirTemperaturaAsync(double valor, string unidadInicial, string unidadFinal)
    {
        manejadorToken.ExigirSesion();
        ConversionRespuesta respuesta = await EjecutarAsync(
                canal => canal.ConvertirTemperaturaAsync(valor, unidadInicial, unidadFinal));
        return respuesta.ValorConvertido;
    }

    public async Task<IReadOnlyList<string>> ConsultarUnidadesAsync(string categoria)
    {
        manejadorToken.ExigirSesion();
        UnidadesCategoriaRespuesta respuesta = await EjecutarAsync(
                canal => canal.ConsultarUnidadesPorCategoriaAsync(categoria));
        return respuesta?.Unidades ?? [];
    }

    public async Task<IReadOnlyList<RegistroConversionRespuesta>> ConsultarHistorialAsync()
    {
        manejadorToken.ExigirSesion();
        RegistroConversionRespuesta[] registros = await EjecutarAsync(canal => canal.ConsultarHistorialAsync());
        return registros ?? [];
    }

    public async Task<string> LimpiarHistorialAsync()
    {
        manejadorToken.ExigirSesion();
        MensajeRespuesta respuesta = await EjecutarAsync(canal => canal.LimpiarHistorialAsync());
        return respuesta?.Mensaje ?? "Historial de conversiones borrado";
    }

    public void CerrarSesion()
    {
        manejadorToken.LimpiarToken();
    }

    public void Dispose()
    {
        if (fabrica is null)
        {
            return;
        }
        try
        {
            fabrica.Close();
        }
        catch (Exception ex) when (ex is CommunicationException or TimeoutException)
        {
            fabrica.Abort();
        }
    }

    /// <summary>
    /// Abre un canal por operación y lo cierra al terminar. Si una llamada falla por red, el canal
    /// queda inservible (Faulted); al usar uno nuevo cada vez, el siguiente intento funciona.
    /// </summary>
    private async Task<T> EjecutarAsync<T>(Func<IWSConversorUnidades, Task<T>> operacion)
    {
        IWSConversorUnidades canal = crearCanal();
        bool exito = false;
        try
        {
            T resultado = await operacion(canal);
            exito = true;
            return resultado;
        }
        catch (FaultException<ErrorServicio> ex)
        {
            // Error controlado por el servidor: trae el código (400/401/500) y el mensaje
            string mensaje = string.IsNullOrWhiteSpace(ex.Detail?.Mensaje) ? ex.Message : ex.Detail.Mensaje;
            throw new ExcepcionClienteWS(ex.Detail?.Codigo ?? 500, mensaje);
        }
        catch (FaultException ex)
        {
            throw new ExcepcionClienteWS(500, "El servidor respondió con un error: " + ex.Message);
        }
        catch (TimeoutException)
        {
            // El servidor recibió la petición pero no respondió (por ejemplo, detenido en el depurador)
            throw new ExcepcionClienteWS(ExcepcionClienteWS.SinConexion,
                $"El servidor SOAP en {UrlServicio} no respondió a tiempo. " +
                "Verifique que WS_ConUni_Dotnet_SOAP no esté detenido en el depurador de Visual Studio.");
        }
        catch (CommunicationException)
        {
            throw new ExcepcionClienteWS(ExcepcionClienteWS.SinConexion,
                $"No fue posible conectar con el servidor SOAP en {UrlServicio}. " +
                "Verifique que el servicio WS_ConUni_Dotnet_SOAP esté en ejecución.");
        }
        finally
        {
            Liberar(canal, exito);
        }
    }

    private static void Liberar(IWSConversorUnidades canal, bool exito)
    {
        if (canal is not ICommunicationObject comunicacion)
        {
            return;
        }
        if (!exito || comunicacion.State == CommunicationState.Faulted)
        {
            comunicacion.Abort();
            return;
        }
        try
        {
            comunicacion.Close();
        }
        catch (Exception ex) when (ex is CommunicationException or TimeoutException)
        {
            comunicacion.Abort();
        }
    }

    private static BasicHttpBinding CrearEnlace()
    {
        return new BasicHttpBinding
        {
            MaxReceivedMessageSize = int.MaxValue,
            OpenTimeout = TiempoEspera,
            SendTimeout = TiempoEspera,
            ReceiveTimeout = TiempoEspera,
            CloseTimeout = TiempoEspera
        };
    }
}
