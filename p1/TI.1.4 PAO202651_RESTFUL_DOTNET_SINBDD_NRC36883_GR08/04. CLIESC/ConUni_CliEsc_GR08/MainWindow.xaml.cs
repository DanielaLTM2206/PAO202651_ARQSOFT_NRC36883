using System.Windows;
using ConUni_CliEsc_GR08.Services;
using ConUni_CliEsc_GR08.Views;

namespace ConUni_CliEsc_GR08
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lblUsuarioSesion.Text = $"Sesion: {RestClient.Instance.Usuario ?? "MONSTER"}";

            // Conectar eventos de conversión para refrescar historial automáticamente
            viewLongitud.ConversionRealizada += async () => await CargarHistorialAsync();
            viewMasa.ConversionRealizada += async () => await CargarHistorialAsync();
            viewTemperatura.ConversionRealizada += async () => await CargarHistorialAsync();

            Loaded += async (s, e) => await CargarHistorialAsync();
        }

        private async System.Threading.Tasks.Task CargarHistorialAsync()
        {
            try
            {
                var res = await RestClient.Instance.ObtenerHistorialAsync();
                if (res.Exitoso && res.Registros != null)
                {
                    dgHistorial.ItemsSource = res.Registros;
                }
            }
            catch
            {
                // Manejo silencioso en carga inicial
            }
        }

        private async void BtnActualizarHistorial_Click(object sender, RoutedEventArgs e)
        {
            await CargarHistorialAsync();
        }

        private async void BtnLimpiarHistorial_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show(
                "Esta seguro de limpiar el historial global en memoria?",
                "Confirmar Limpieza",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (res == MessageBoxResult.Yes)
            {
                await RestClient.Instance.LimpiarHistorialAsync();
                await CargarHistorialAsync();
            }
        }

        private void BtnCambiarClave_Click(object sender, RoutedEventArgs e)
        {
            var win = new CambiarClaveWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            RestClient.Instance.CerrarSesion();
            var login = new LoginWindow();
            login.Show();
            Close();
        }
    }
}