using ec.edu.monster.Controlador;
using ec.edu.monster.Modelo;

namespace ec.edu.monster.Vista
{
    /// <summary>
    /// Ventana para escribir la dirección del servidor REST cuando Payara corre en otra computadora de la red.
    /// Tiene el mismo estilo que FormCambiarContrasenia. Devuelve OK cuando se guardó una dirección nueva.
    /// </summary>
    public partial class FormServidor : Form
    {
        private static readonly Color ColorExito = Color.FromArgb(25, 135, 84);
        private static readonly Color ColorError = Color.FromArgb(192, 0, 0);
        private static readonly Color ColorTenue = Color.FromArgb(108, 117, 125);

        private readonly ServidorControlador servidorControlador;

        public FormServidor(ServidorControlador servidorControlador)
        {
            InitializeComponent();
            this.servidorControlador = servidorControlador;
            txtServidor.Text = servidorControlador.UrlActual;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            txtServidor.Focus();
            txtServidor.SelectAll();
        }

        private async void btnProbar_Click(object? sender, EventArgs e)
        {
            await ProbarAsync();
        }

        private async void btnGuardar_Click(object? sender, EventArgs e)
        {
            ResultadoConexion? resultado = await ProbarAsync();
            if (resultado is null)
            {
                return;
            }
            if (!resultado.Exitosa && MessageBox.Show(this,
                    "No se pudo conectar con esa dirección. ¿Desea guardarla de todas formas?", "Sin conexión",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            servidorControlador.Guardar(txtServidor.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <returns>El resultado de la prueba, o null si la dirección escrita no es válida.</returns>
        private async Task<ResultadoConexion?> ProbarAsync()
        {
            string url;
            try
            {
                url = servidorControlador.CompletarUrl(txtServidor.Text);
            }
            catch (ArgumentException ex)
            {
                MostrarEstado(ex.Message, ColorError);
                txtServidor.Focus();
                return null;
            }

            txtServidor.Text = url;
            CambiarEstadoOcupado(true);
            MostrarEstado("Probando la conexión con " + url + " ...", ColorTenue);
            ResultadoConexion resultado = await servidorControlador.ProbarAsync(url);
            CambiarEstadoOcupado(false);

            MostrarEstado(resultado.Mensaje, resultado.Exitosa ? ColorExito : ColorError);
            return resultado;
        }

        private void MostrarEstado(string mensaje, Color color)
        {
            lblEstado.ForeColor = color;
            lblEstado.Text = mensaje;
        }

        private void CambiarEstadoOcupado(bool ocupado)
        {
            txtServidor.Enabled = !ocupado;
            btnProbar.Enabled = !ocupado;
            btnGuardar.Enabled = !ocupado;
            btnCancelar.Enabled = !ocupado;
            btnProbar.Text = ocupado ? "Probando..." : "Probar conexión";
            UseWaitCursor = ocupado;
        }
    }
}
