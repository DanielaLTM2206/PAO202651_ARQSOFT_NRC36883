using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using UniversalConverter.Client.Models;

namespace UniversalConverter.Client.Services
{
    public class RestConversionService : IConversionService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions _jsonOptions;

        private string _baseUrl;
        private string? _token;
        private string? _usuario;

        public string? Token => _token;
        public string? Usuario => _usuario;
        public bool EstaAutenticado => !string.IsNullOrWhiteSpace(_token);
        public string BaseUrl => _baseUrl;

        public RestConversionService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(25)
            };

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Detección automática de host para Android Emulator vs Windows/Localhost
            string host = _configuration["Hosts"] ?? "localhost";
            string defaultBaseUrl;

#if ANDROID
            // En el emulador Android de Google, 10.0.2.2 redirige al localhost de la máquina host Windows
            defaultBaseUrl = "http://10.0.2.2:5000/api";
#else
            defaultBaseUrl = $"http://{host}:5000/api";
#endif
            _baseUrl = defaultBaseUrl;
        }

        public void SetBaseUrl(string hostOrUrl, int port = 5000)
        {
            if (hostOrUrl.StartsWith("http://") || hostOrUrl.StartsWith("https://"))
            {
                _baseUrl = hostOrUrl.TrimEnd('/');
                if (!_baseUrl.EndsWith("/api", StringComparison.OrdinalIgnoreCase))
                {
                    _baseUrl += "/api";
                }
            }
            else
            {
                _baseUrl = $"http://{hostOrUrl.Trim()}:{port}/api";
            }
        }

        public async Task<LoginResponse> LoginAsync(string usuario, string clave)
        {
            try
            {
                var payload = new LoginRequest { Usuario = usuario.Trim(), Clave = clave.Trim() };
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/auth/login", payload, _jsonOptions);
                string raw = await response.Content.ReadAsStringAsync();

                var res = JsonSerializer.Deserialize<LoginResponse>(raw, _jsonOptions)
                    ?? new LoginResponse { Exitoso = false, Mensaje = "Respuesta vacia del servidor" };

                if (response.IsSuccessStatusCode && res.Exitoso)
                {
                    _token = res.Token;
                    _usuario = res.Usuario ?? usuario;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }

                return res;
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Exitoso = false,
                    Codigo = "CONN_ERR",
                    Mensaje = $"Error de comunicacion con el servidor REST: {ex.Message}"
                };
            }
        }

        public async Task<GenericApiResponse> CambiarClaveAsync(string claveActual, string claveNueva)
        {
            try
            {
                var payload = new CambiarClaveRequest
                {
                    ClaveActual = claveActual.Trim(),
                    ClaveNueva = claveNueva.Trim()
                };

                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/auth/cambiar-clave", payload, _jsonOptions);
                string raw = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<GenericApiResponse>(raw, _jsonOptions)
                    ?? new GenericApiResponse { Exitoso = false, Mensaje = "Respuesta nula del servidor" };
            }
            catch (Exception ex)
            {
                return new GenericApiResponse { Exitoso = false, Mensaje = $"Error de comunicacion: {ex.Message}" };
            }
        }

        public async Task<ConversionResponse> ConvertAsync(ConversionRequest request)
        {
            string categoria = request.TipoConversion switch
            {
                ConversionType.Longitud => "Longitud",
                ConversionType.Masa => "Masa",
                ConversionType.Temperatura => "Temperatura",
                _ => "Longitud"
            };

            string endpoint = $"{_baseUrl}/{categoria}/convertir";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, request, _jsonOptions);
                string raw = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ConversionResponse>(raw, _jsonOptions);
                return result ?? new ConversionResponse
                {
                    Exitoso = false,
                    Error = new ErrorData { Mensaje = "No se recibio respuesta valida del servidor" }
                };
            }
            catch (Exception ex)
            {
                return new ConversionResponse
                {
                    Exitoso = false,
                    Error = new ErrorData
                    {
                        CodigoError = "CONN_ERR",
                        Mensaje = $"Error de conexion: {ex.Message}"
                    }
                };
            }
        }

        public async Task<HistorialResponse> ObtenerHistorialAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/historial");
                string raw = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<HistorialResponse>(raw, _jsonOptions)
                    ?? new HistorialResponse { Exitoso = false, Mensaje = "Respuesta nula del servidor" };
            }
            catch (Exception ex)
            {
                return new HistorialResponse { Exitoso = false, Mensaje = ex.Message };
            }
        }

        public async Task<GenericApiResponse> LimpiarHistorialAsync()
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/historial");
                string raw = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<GenericApiResponse>(raw, _jsonOptions)
                    ?? new GenericApiResponse { Exitoso = false, Mensaje = "Respuesta nula del servidor" };
            }
            catch (Exception ex)
            {
                return new GenericApiResponse { Exitoso = false, Mensaje = ex.Message };
            }
        }

        public void CerrarSesion()
        {
            _token = null;
            _usuario = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
