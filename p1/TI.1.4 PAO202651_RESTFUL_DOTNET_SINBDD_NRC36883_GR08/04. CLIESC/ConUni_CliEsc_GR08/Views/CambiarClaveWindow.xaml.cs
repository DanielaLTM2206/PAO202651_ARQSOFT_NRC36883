using System.Windows;
using System.Windows.Media;
using ConUni_CliEsc_GR08.Services;

namespace ConUni_CliEsc_GR08.Views
{
    public partial class CambiarClaveWindow : Window
    {
        public CambiarClaveWindow()
        {
            InitializeComponent();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string actual = txtActual.Password.Trim();
            string nueva = txtNueva.Password.Trim();
            string confirmar = txtConfirmar.Password.Trim();

            if (string.IsNullOrWhiteSpace(actual) || string.IsNullOrWhiteSpace(nueva))
            {
                MostrarMensaje("Debe completar todos los campos.", false);
                return;
            }

            if (nueva.Length < 4)
            {
                MostrarMensaje("La nueva contrasena debe tener al menos 4 caracteres.", false);
                return;
            }

            if (nueva != confirmar)
            {
                MostrarMensaje("La confirmacion no coincide con la nueva contrasena.", false);
                return;
            }

            var res = await RestClient.Instance.CambiarClaveAsync(actual, nueva);
            if (res.Exitoso)
            {
                MostrarMensaje(res.Mensaje, true);
                await System.Threading.Tasks.Task.Delay(1200);
                Close();
            }
            else
            {
                MostrarMensaje($"Error: {res.Mensaje}", false);
            }
        }

        private void MostrarMensaje(string mensaje, bool exito)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.Foreground = exito ? new SolidColorBrush(Color.FromRgb(52, 211, 153)) : new SolidColorBrush(Color.FromRgb(248, 113, 113));
            lblMensaje.Visibility = Visibility.Visible;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
