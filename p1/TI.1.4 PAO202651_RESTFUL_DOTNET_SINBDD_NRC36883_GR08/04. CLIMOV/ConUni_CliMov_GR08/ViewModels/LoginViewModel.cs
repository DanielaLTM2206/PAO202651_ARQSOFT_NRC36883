using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UniversalConverter.Client.Services;

namespace UniversalConverter.Client.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IConversionService _restService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        [ObservableProperty]
        private string _username = "MONSTER";

        [ObservableProperty]
        private string _password = "MONSTER9";

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private string _serverHost = "10.0.2.2";

        public bool IsNotBusy => !IsBusy;

        public LoginViewModel(IConversionService restService)
        {
            _restService = restService;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Debe ingresar usuario y contrasena.";
                return;
            }

            IsBusy = true;
            ErrorMessage = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(ServerHost))
                {
                    _restService.SetBaseUrl(ServerHost.Trim(), 5000);
                }

                var res = await _restService.LoginAsync(Username, Password);
                if (res.Exitoso)
                {
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }
                else
                {
                    ErrorMessage = $"[{res.Codigo ?? "ERR_AUTH"}] {res.Mensaje ?? "Credenciales invalidas."}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error inesperado: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
