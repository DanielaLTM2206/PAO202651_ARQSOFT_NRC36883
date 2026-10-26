using ec.edu.monster.Cliente;
using ec.edu.monster.Controlador;
using ec.edu.monster.Modelo;
using ec.edu.monster.Util;

namespace ec.edu.monster.Vista;

public partial class LoginPagina : ContentPage
{
    private readonly LoginControlador loginControlador;

    public LoginPagina(LoginControlador loginControlador, string? aviso = null)
    {
        InitializeComponent();
        this.loginControlador = loginControlador;

        lblServidor.Text = "Servidor SOAP: " + loginControlador.UrlServidor;
        ResaltadoCampo.Aplicar(txtUsuario, bordeUsuario);
        ResaltadoCampo.Aplicar(txtContrasenia, bordeContrasenia);

        if (!string.IsNullOrEmpty(aviso))
        {
            lblAviso.Text = aviso;
            pnlAviso.IsVisible = true;
        }
    }

    private void TxtUsuario_Completed(object? sender, EventArgs e)
    {
        txtContrasenia.Focus();
    }

    private void BtnVerContrasenia_Clicked(object? sender, EventArgs e)
    {
        txtContrasenia.IsPassword = !txtContrasenia.IsPassword;
        btnVerContrasenia.Text = txtContrasenia.IsPassword ? "Ver" : "Ocultar";
    }

    private async void BtnIngresar_Clicked(object? sender, EventArgs e)
    {
        MostrarError(null);
        pnlAviso.IsVisible = false;

        if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtContrasenia.Text))
        {
            MostrarError("Ingrese el usuario y la contraseña.");
            return;
        }

        string usuario = txtUsuario.Text.Trim();
        CambiarEstadoOcupado(true);
        try
        {
            await loginControlador.AutenticarAsync(new LoginModelo(usuario, txtContrasenia.Text));
        }
        catch (Exception ex) when (ex is ExcepcionClienteWS or ArgumentException)
        {
            CambiarEstadoOcupado(false);
            MostrarError(ex.Message);
            txtContrasenia.Text = string.Empty;
            return;
        }

        CambiarEstadoOcupado(false);
        App.Actual.MostrarConversor(loginControlador, usuario);
    }

    // Permite usar la app desde un teléfono real: allí se escribe la IP de la PC donde corre el servicio SOAP
    private async void BtnCambiarServidor_Clicked(object? sender, EventArgs e)
    {
        string? url = await DisplayPromptAsync(
                "Dirección del servidor",
                "Escriba la URL del servicio SOAP. En el emulador de Android use 10.0.2.2; "
                + "en un teléfono real, la IP de la PC conectada a la misma red Wi-Fi.",
                accept: "Guardar",
                cancel: "Cancelar",
                initialValue: ConfiguracionServidor.UrlActual,
                keyboard: Keyboard.Url);

        if (url is null)
        {
            return;
        }
        if (!ConfiguracionServidor.EsUrlValida(url))
        {
            await DisplayAlertAsync("Dirección no válida",
                    "Escriba una dirección completa, por ejemplo http://192.168.1.10:5080/WSConversorUnidades.svc",
                    "Entendido");
            return;
        }

        ConfiguracionServidor.UrlActual = url;
        App.Actual.MostrarInicioSesion("Servidor actualizado. Ya puede iniciar sesión.");
    }

    private void MostrarError(string? mensaje)
    {
        lblError.Text = mensaje;
        pnlError.IsVisible = !string.IsNullOrEmpty(mensaje);
    }

    private void CambiarEstadoOcupado(bool ocupado)
    {
        btnIngresar.IsEnabled = !ocupado;
        txtUsuario.IsEnabled = !ocupado;
        txtContrasenia.IsEnabled = !ocupado;
        btnIngresar.Text = ocupado ? "Conectando..." : "Iniciar sesión";
        indicadorCarga.IsRunning = ocupado;
        indicadorCarga.IsVisible = ocupado;
    }
}
