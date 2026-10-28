using System.Windows;
using System.Windows.Input;
using ConUni_CliEsc_GR08.Services;

namespace ConUni_CliEsc_GR08
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            Loaded += (s, e) => 
            {
                if (!string.IsNullOrWhiteSpace(RestClient.CustomHost))
                {
                    txtServidor.Text = RestClient.CustomHost;
                }
                txtUsuario.Focus();
            };
        }

        private async void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            await ValidarCredencialesAsync();
        }

        private async void TxtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ValidarCredencialesAsync();
            }
        }

        private async System.Threading.Tasks.Task ValidarCredencialesAsync()
        {
            pnlError.Visibility = Visibility.Collapsed;

            if (!string.IsNullOrWhiteSpace(txtServidor.Text))
            {
                RestClient.SetServerHost(txtServidor.Text.Trim());
            }

            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Password.Trim();

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
            {
                MostrarError("Por favor ingrese usuario y contrasena.");
                return;
            }

            btnIngresar.IsEnabled = false;
            btnIngresar.Content = "AUTENTICANDO...";

            try
            {
                var res = await RestClient.Instance.IniciarSesionAsync(usuario, clave);
                if (res.Exitoso)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    MostrarError($"[{res.Codigo}] {res.Mensaje}");
                }
            }
            catch (System.Exception ex)
            {
                MostrarError($"Error de conexion: {ex.Message}");
            }
            finally
            {
                btnIngresar.IsEnabled = true;
                btnIngresar.Content = "INGRESAR AL SISTEMA";
            }
        }

        private void MostrarError(string mensaje)
        {
            lblMensajeError.Text = mensaje;
            pnlError.Visibility = Visibility.Visible;
        }
    }
}
