using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConUni_CliEsc_GR08.Models;

namespace ConUni_CliEsc_GR08.Services
{
    public class RestClient : IDisposable
    {
        private static RestClient? _instance;
        private static string _customHost = "localhost";

        public static void SetServerHost(string host)
        {
            if (!string.IsNullOrWhiteSpace(host))
            {
                _customHost = host.Trim();
                _instance = null;
            }
        }

        public static RestClient Instance => _instance ??= new RestClient(
            _customHost.StartsWith("http") 
                ? (_customHost.EndsWith("/api") ? _customHost : $"{_customHost.TrimEnd('/')}/api")
                : $"http://{_customHost}:5000/api");

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;
        private string? _token;
        private string? _usuario;

        public string? Token => _token;
        public string? Usuario => _usuario;
        public bool EstaAutenticado => !string.IsNullOrWhiteSpace(_token);

        public RestClient(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<LoginResponse> IniciarSesionAsync(string usuario, string clave)
        {
            try
            {
                var payload = new LoginRequest { Usuario = usuario, Clave = clave };
                string json = JsonSerializer.Serialize(payload, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/auth/login", content);
                string responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<LoginResponse>(responseBody, _jsonOptions)
                    ?? new LoginResponse { Exitoso = false, Mensaje = "Respuesta vacia del servidor" };

                if (response.IsSuccessStatusCode && result.Exitoso)
                {
                    _token = result.Token;
                    _usuario = result.Usuario;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }

                return result;
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Exitoso = false,
                    Codigo = "CONN_ERR",
                    Mensaje = $"Error de conexion con el servidor REST: {ex.Message}"
                };
            }
        }

        public async Task<RespuestaBase> CambiarClaveAsync(string claveActual, string claveNueva)
        {
            try
            {
                var payload = new CambiarClaveRequest { ClaveActual = claveActual, ClaveNueva = claveNueva };
                string json = JsonSerializer.Serialize(payload, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/auth/cambiar-clave", content);
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<RespuestaBase>(responseBody, _jsonOptions)
                    ?? new RespuestaBase { Exitoso = false, Mensaje = "Respuesta nula" };
            }
            catch (Exception ex)
            {
                return new RespuestaBase { Exitoso = false, Mensaje = ex.Message };
            }
        }

        public async Task<ConversionResult> ConvertirLongitudAsync(string valor, string origen, string destino)
        {
            return await EjecutarConversionAsync("Longitud", valor, origen, destino);
        }

        public async Task<ConversionResult> ConvertirMasaAsync(string valor, string origen, string destino)
        {
            return await EjecutarConversionAsync("Masa", valor, origen, destino);
        }

        public async Task<ConversionResult> ConvertirTemperaturaAsync(string valor, string origen, string destino)
        {
            return await EjecutarConversionAsync("Temperatura", valor, origen, destino);
        }

        private async Task<ConversionResult> EjecutarConversionAsync(string categoria, string valor, string origen, string destino)
        {
            try
            {
                var payload = new ConversionRequest(valor, origen, destino, _token);
                string json = JsonSerializer.Serialize(payload, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/{categoria}/convertir", content);
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<ConversionResult>(responseBody, _jsonOptions)
                    ?? new ConversionResult
                    {
                        Exitoso = false,
                        Error = new ConversionError { Mensaje = "No se recibio respuesta valida del servidor" }
                    };
            }
            catch (Exception ex)
            {
                return new ConversionResult
                {
                    Exitoso = false,
                    Error = new ConversionError
                    {
                        CodigoError = "CONN_ERR",
                        Mensaje = $"Error de comunicacion con el servicio REST: {ex.Message}",
                        TipoError = "Cliente"
                    }
                };
            }
        }

        public async Task<RespuestaHistorial> ObtenerHistorialAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/historial");
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<RespuestaHistorial>(responseBody, _jsonOptions)
                    ?? new RespuestaHistorial { Exitoso = false, Mensaje = "Respuesta nula" };
            }
            catch (Exception ex)
            {
                return new RespuestaHistorial { Exitoso = false, Mensaje = ex.Message };
            }
        }

        public async Task<RespuestaBase> LimpiarHistorialAsync()
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/historial");
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<RespuestaBase>(responseBody, _jsonOptions)
                    ?? new RespuestaBase { Exitoso = false, Mensaje = "Respuesta nula" };
            }
            catch (Exception ex)
            {
                return new RespuestaBase { Exitoso = false, Mensaje = ex.Message };
            }
        }

        public void CerrarSesion()
        {
            _token = null;
            _usuario = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
