using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UniversalConverter.Client.Models;
using UniversalConverter.Client.Services;
using UniversalConverter.Client.Views;

namespace UniversalConverter.Client.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IConversionService _restService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        [ObservableProperty]
        private string _valorEntrada = "10";

        [ObservableProperty]
        private ConversionType _selectedConversionType = ConversionType.Longitud;

        [ObservableProperty]
        private string? _selectedUnitFrom;

        [ObservableProperty]
        private string? _selectedUnitTo;

        [ObservableProperty]
        private string _usuarioSesion = "monster";

        // Resultados
        [ObservableProperty]
        private bool _tieneResultado;

        [ObservableProperty]
        private bool _tieneError;

        [ObservableProperty]
        private string? _errorCodigo;

        [ObservableProperty]
        private string? _errorMensaje;

        [ObservableProperty]
        private string? _errorDetalles;

        [ObservableProperty]
        private string? _resultadoTitulo;

        [ObservableProperty]
        private string? _resultadoGrande;

        [ObservableProperty]
        private string? _resultadoExacto;

        [ObservableProperty]
        private string? _resultadoFactor;

        // Historial
        public ObservableCollection<RegistroHistorial> Historial { get; } = new();

        // Cambio de Clave
        [ObservableProperty]
        private bool _mostrarModalClave;

        [ObservableProperty]
        private string _claveActual = string.Empty;

        [ObservableProperty]
        private string _claveNueva = string.Empty;

        [ObservableProperty]
        private string _claveConfirmar = string.Empty;

        [ObservableProperty]
        private string? _mensajeClave;

        [ObservableProperty]
        private bool _mensajeClaveExito;

        // Colecciones
        public ObservableCollection<ConversionType> ConversionTypes { get; } = new()
        {
            ConversionType.Longitud,
            ConversionType.Masa,
            ConversionType.Temperatura
        };

        public ObservableCollection<string> UnitsFrom { get; } = new();
        public ObservableCollection<string> UnitsTo { get; } = new();

        public bool IsNotBusy => !IsBusy;

        public MainViewModel(IConversionService restService)
        {
            _restService = restService;
            UsuarioSesion = _restService.Usuario ?? "monster";
            UpdateUnitLists();
        }

        partial void OnSelectedConversionTypeChanged(ConversionType value)
        {
            UpdateUnitLists();
            TieneResultado = false;
            TieneError = false;
        }

        private void UpdateUnitLists()
        {
            UnitsFrom.Clear();
            UnitsTo.Clear();

            List<string> units = SelectedConversionType switch
            {
                ConversionType.Longitud => new List<string> { "Milla", "Metro", "Pulgada", "Kilometro", "Pie", "Yarda" },
                ConversionType.Masa => new List<string> { "Kilogramo", "Libra", "Onza", "Gramo" },
                ConversionType.Temperatura => new List<string> { "Celsius", "Fahrenheit", "Kelvin" },
                _ => new List<string>()
            };

            foreach (var unit in units)
            {
                UnitsFrom.Add(unit);
                UnitsTo.Add(unit);
            }

            SelectedUnitFrom = UnitsFrom.FirstOrDefault();
            SelectedUnitTo = UnitsTo.Count > 1 ? UnitsTo[1] : UnitsFrom.FirstOrDefault();
        }

        [RelayCommand(CanExecute = nameof(IsNotBusy))]
        private async Task Convert()
        {
            TieneResultado = false;
            TieneError = false;

            string valor = ValorEntrada?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(valor))
            {
                MostrarError("ERR_VACIO", "El valor a convertir no puede estar vacio.", "Ingrese un valor numerico.");
                return;
            }

            // Validacion VAL_007: Maximo 15 digitos / 1 billon
            string cleanDigits = valor.Replace("-", "").Replace(".", "").Replace(",", "");
            if (cleanDigits.Length > 15)
            {
                MostrarError("VAL_007", "El valor ingresado supera el limite maximo permitido (maximo 15 digitos).", "Ingrese un valor numerico razonable.");
                return;
            }

            if (string.IsNullOrEmpty(SelectedUnitFrom) || string.IsNullOrEmpty(SelectedUnitTo))
            {
                MostrarError("ERR_SEL", "Seleccione las unidades de origen y destino.", "");
                return;
            }

            IsBusy = true;

            try
            {
                var request = new ConversionRequest
                {
                    Valor = valor,
                    UnidadOrigen = SelectedUnitFrom,
                    UnidadDestino = SelectedUnitTo,
                    TipoConversion = SelectedConversionType
                };

                var res = await _restService.ConvertAsync(request);
                if (res.Exitoso && res.Resultado != null)
                {
                    ResultadoTitulo = $"{res.Resultado.ValorOriginal} {res.Resultado.UnidadOrigen} equivalen a:";
                    ResultadoGrande = $"{res.Resultado.ValorConvertidoRedondeado} {res.Resultado.UnidadDestino}";
                    ResultadoExacto = $"Valor Exacto: {res.Resultado.ValorConvertidoExacto}";
                    ResultadoFactor = $"Factor: {res.Resultado.FactorConversion} | {res.Resultado.FechaConversion.ToLocalTime():HH:mm:ss}";

                    TieneResultado = true;

                    // Refrescar historial tras conversion
                    await CargarHistorialInterno();
                }
                else
                {
                    string cod = res.Error?.CodigoError ?? "ERR";
                    string msg = res.Error?.Mensaje ?? "Error al procesar la conversion";
                    string det = res.Error?.Detalles ?? "";
                    MostrarError(cod, msg, det);
                }
            }
            catch (Exception ex)
            {
                MostrarError("CONN_ERR", "Error al comunicarse con el servidor REST.", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void MostrarError(string codigo, string mensaje, string detalles)
        {
            ErrorCodigo = codigo;
            ErrorMensaje = mensaje;
            ErrorDetalles = detalles;
            TieneError = true;
        }

        [RelayCommand]
        public async Task CargarHistorial()
        {
            await CargarHistorialInterno();
        }

        private async Task CargarHistorialInterno()
        {
            try
            {
                var res = await _restService.ObtenerHistorialAsync();
                if (res.Exitoso && res.Registros != null)
                {
                    Historial.Clear();
                    foreach (var reg in res.Registros)
                    {
                        Historial.Add(reg);
                    }
                }
            }
            catch
            {
                // Manejo silencioso en background
            }
        }

        [RelayCommand]
        public async Task LimpiarHistorial()
        {
            bool confirmar = await Shell.Current.DisplayAlert(
                "Confirmar Limpieza",
                "Esta seguro de limpiar el historial global en memoria?",
                "Si, Limpiar",
                "Cancelar");

            if (confirmar)
            {
                await _restService.LimpiarHistorialAsync();
                Historial.Clear();
            }
        }

        [RelayCommand]
        public void AbrirModalClave()
        {
            ClaveActual = string.Empty;
            ClaveNueva = string.Empty;
            ClaveConfirmar = string.Empty;
            MensajeClave = null;
            MostrarModalClave = true;
        }

        [RelayCommand]
        public void CerrarModalClave()
        {
            MostrarModalClave = false;
        }

        [RelayCommand]
        public async Task GuardarClave()
        {
            if (string.IsNullOrWhiteSpace(ClaveActual) || string.IsNullOrWhiteSpace(ClaveNueva))
            {
                MensajeClave = "Debe completar todos los campos.";
                MensajeClaveExito = false;
                return;
            }

            if (ClaveNueva.Length < 4)
            {
                MensajeClave = "La nueva contrasena debe tener al menos 4 caracteres.";
                MensajeClaveExito = false;
                return;
            }

            if (ClaveNueva != ClaveConfirmar)
            {
                MensajeClave = "La confirmacion no coincide con la nueva contrasena.";
                MensajeClaveExito = false;
                return;
            }

            var res = await _restService.CambiarClaveAsync(ClaveActual, ClaveNueva);
            if (res.Exitoso)
            {
                MensajeClave = res.Mensaje;
                MensajeClaveExito = true;
                await Task.Delay(1500);
                MostrarModalClave = false;
            }
            else
            {
                MensajeClave = $"Error: {res.Mensaje}";
                MensajeClaveExito = false;
            }
        }

        [RelayCommand]
        public async Task Logout()
        {
            _restService.CerrarSesion();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
