using System.Threading.Tasks;
using UniversalConverter.Client.Models;

namespace UniversalConverter.Client.Services
{
    public interface IConversionService
    {
        Task<ConversionResponse> ConvertAsync(ConversionRequest request);
        Task<LoginResponse> LoginAsync(string usuario, string clave);
        Task<GenericApiResponse> CambiarClaveAsync(string claveActual, string claveNueva);
        Task<HistorialResponse> ObtenerHistorialAsync();
        Task<GenericApiResponse> LimpiarHistorialAsync();
        string? Token { get; }
        string? Usuario { get; }
        bool EstaAutenticado { get; }
        void CerrarSesion();
        void SetBaseUrl(string host, int port = 5000);
    }
}
