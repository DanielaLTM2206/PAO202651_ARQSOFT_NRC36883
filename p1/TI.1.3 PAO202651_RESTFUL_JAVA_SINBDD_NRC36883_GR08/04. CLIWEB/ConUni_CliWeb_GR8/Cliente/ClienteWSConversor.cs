using System.Net.Http.Json;
using System.Text.Json;
using ec.edu.monster.Cliente.Dto;
using ec.edu.monster.Util;
using Microsoft.Extensions.DependencyInjection;

namespace ec.edu.monster.Cliente;

/// <summary>
/// Cliente HTTP del servicio RESTful WSConversorUnidades. Envía y recibe JSON.
/// Lo crea ASP.NET Core en cada petición con el HttpClient y el manejador de token de ese usuario.
/// </summary>
public class ClienteWSConversor : IContratoWSConversor
{
    private readonly HttpClient clienteHttp;
    private readonly ManejadorTokenRESTFUL manejadorToken;

    public string BaseUrl { get; }

    // Se lee la dirección en cada petición: un cambio en "Configurar servidor" se aplica de inmediato
    [ActivatorUtilitiesConstructor]
    public ClienteWSConversor(HttpClient clienteHttp, ManejadorTokenRESTFUL manejadorToken, ConfiguracionServidor configuracion)
        : this(clienteHttp, manejadorToken, configuracion.UrlActual)
    {
    }

    public ClienteWSConversor(HttpClient clienteHttp, ManejadorTokenRESTFUL manejadorToken, string baseUrl)
    {
        this.clienteHttp = clienteHttp;
        this.manejadorToken = manejadorToken;
        BaseUrl = baseUrl.Trim().TrimEnd('/');
    }

    public string? TokenSesion => manejadorToken.ObtenerToken();

    public async Task<string> LoginAsync(string usuario, string contrasenia)
    {
        var solicitud = new LoginSolicitud { Usuario = usuario, Contrasena = contrasenia };
        LoginRespuesta respuesta = await EnviarAsync<LoginRespuesta>(CrearPost("/seguridad/login", solicitud));

        manejadorToken.GuardarToken(respuesta.Token);
        return respuesta.Token;
    }

    public async Task<string> CambiarContraseniaAsync(string contraseniaActual, string contraseniaNueva)
    {
        var solicitud = new CambioContrasenaSolicitud
        {
            ContrasenaActual = contraseniaActual,
            NuevaContrasena = contraseniaNueva
        };
        HttpRequestMessage peticion = CrearPost("/seguridad/cambiar-contrasena", solicitud);
        manejadorToken.AdjuntarToken(peticion);

        MensajeRespuesta respuesta = await EnviarAsync<MensajeRespuesta>(peticion);
        return respuesta.Mensaje ?? "Contraseña actualizada correctamente";
    }

    public Task<double> ConvertirLongitudAsync(double valor, string unidadInicial, string unidadFinal)
        => ConvertirAsync("longitud", valor, unidadInicial, unidadFinal);

    public Task<double> ConvertirMasaAsync(double valor, string unidadInicial, string unidadFinal)
        => ConvertirAsync("masa", valor, unidadInicial, unidadFinal);

    public Task<double> ConvertirTemperaturaAsync(double valor, string unidadInicial, string unidadFinal)
        => ConvertirAsync("temperatura", valor, unidadInicial, unidadFinal);

    public async Task<IReadOnlyList<string>> ConsultarUnidadesAsync(string categoria)
    {
        var peticion = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/unidades/{Uri.EscapeDataString(categoria)}");
        manejadorToken.AdjuntarToken(peticion);

        UnidadesCategoriaRespuesta respuesta = await EnviarAsync<UnidadesCategoriaRespuesta>(peticion);
        return respuesta.Unidades;
    }

    public async Task<IReadOnlyList<RegistroConversionRespuesta>> ConsultarHistorialAsync()
    {
        var peticion = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/conversiones/historial");
        manejadorToken.AdjuntarToken(peticion);

        return await EnviarAsync<List<RegistroConversionRespuesta>>(peticion);
    }

    public async Task<string> LimpiarHistorialAsync()
    {
        var peticion = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/conversiones/historial");
        manejadorToken.AdjuntarToken(peticion);

        MensajeRespuesta respuesta = await EnviarAsync<MensajeRespuesta>(peticion);
        return respuesta.Mensaje ?? "Historial de conversiones borrado";
    }

    public void CerrarSesion()
    {
        manejadorToken.LimpiarToken();
    }

    private async Task<double> ConvertirAsync(string tipo, double valor, string unidadInicial, string unidadFinal)
    {
        var solicitud = new ConversionSolicitud
        {
            Valor = valor,
            UnidadOrigen = unidadInicial,
            UnidadDestino = unidadFinal
        };
        HttpRequestMessage peticion = CrearPost($"/conversiones/{tipo}", solicitud);
        manejadorToken.AdjuntarToken(peticion);

        ConversionRespuesta respuesta = await EnviarAsync<ConversionRespuesta>(peticion);
        return respuesta.ValorConvertido;
    }

    private HttpRequestMessage CrearPost<T>(string ruta, T cuerpo)
    {
        return new HttpRequestMessage(HttpMethod.Post, BaseUrl + ruta)
        {
            Content = JsonContent.Create(cuerpo)
        };
    }

    private async Task<T> EnviarAsync<T>(HttpRequestMessage peticion)
    {
        HttpResponseMessage respuesta;
        try
        {
            respuesta = await clienteHttp.SendAsync(peticion);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new ExcepcionClienteWS(ExcepcionClienteWS.SinConexion,
                $"No fue posible conectar con el servidor REST en {BaseUrl}. " +
                "Verifique que Payara esté activo y el servicio desplegado.");
        }

        using (respuesta)
        {
            int codigo = (int)respuesta.StatusCode;

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new ExcepcionClienteWS(codigo, await ExtraerMensajeAsync(respuesta));
            }

            try
            {
                T? datos = await respuesta.Content.ReadFromJsonAsync<T>();
                return datos ?? throw new ExcepcionClienteWS(codigo, "El servidor devolvió una respuesta vacía.");
            }
            catch (JsonException)
            {
                throw new ExcepcionClienteWS(codigo, "El servidor devolvió una respuesta con formato inválido.");
            }
        }
    }

    // Los errores del servidor llegan como {"Mensaje": "..."}
    private static async Task<string> ExtraerMensajeAsync(HttpResponseMessage respuesta)
    {
        try
        {
            MensajeRespuesta? error = await respuesta.Content.ReadFromJsonAsync<MensajeRespuesta>();
            if (!string.IsNullOrWhiteSpace(error?.Mensaje))
            {
                return error.Mensaje;
            }
        }
        catch (Exception ex) when (ex is JsonException or NotSupportedException)
        {
            // El cuerpo no es JSON (por ejemplo, una página de error del servidor)
        }
        return $"El servidor respondió con el código HTTP {(int)respuesta.StatusCode}.";
    }
}
