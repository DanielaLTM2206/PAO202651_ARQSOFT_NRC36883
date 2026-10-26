using ec.edu.monster.Cliente;
using ec.edu.monster.Controlador;
using ec.edu.monster.Vista.Controles;

namespace ec.edu.monster.Vista
{
    public partial class FormCambiarContrasenia : Form
    {
        private readonly LoginControlador loginControlador;

        // Si el servidor responde 401, la ventana principal debe volver al inicio de sesión
        public bool SesionExpirada { get; private set; }

        public FormCambiarContrasenia(LoginControlador loginControlador)
        {
            InitializeComponent();
            this.loginControlador = loginControlador;
        }

        private async void btnGuardar_Click(object? sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(txtActual.Text) || string.IsNullOrWhiteSpace(txtNueva.Text)
                    || string.IsNullOrWhiteSpace(txtConfirmacion.Text))
            {
                lblError.Text = "Complete los tres campos.";
                PrimerCampoVacio().Focus();
                return;
            }
            if (txtNueva.Text != txtConfirmacion.Text)
            {
                lblError.Text = "La confirmación no coincide con la nueva contraseña.";
                txtConfirmacion.SelectAll();
                txtConfirmacion.Focus();
                return;
            }

            CambiarEstadoOcupado(true);
            try
            {
                string mensaje = await loginControlador.CambiarContraseniaAsync(txtActual.Text, txtNueva.Text);
                MessageBox.Show(this, mensaje, "Contraseña actualizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (ExcepcionClienteWS ex) when (ex.EsErrorAutenticacion)
            {
                MessageBox.Show(this, ex.Message, "Sesión no válida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SesionExpirada = true;
                DialogResult = DialogResult.Abort;
            }
            catch (ExcepcionClienteWS ex)
            {
                lblError.Text = ex.Message;
            }
            catch (ArgumentException ex)
            {
                lblError.Text = ex.Message;
            }
            finally
            {
                CambiarEstadoOcupado(false);
            }
        }

        private CampoTexto PrimerCampoVacio()
        {
            if (string.IsNullOrWhiteSpace(txtActual.Text))
            {
                return txtActual;
            }
            return string.IsNullOrWhiteSpace(txtNueva.Text) ? txtNueva : txtConfirmacion;
        }

        private void CambiarEstadoOcupado(bool ocupado)
        {
            txtActual.Enabled = !ocupado;
            txtNueva.Enabled = !ocupado;
            txtConfirmacion.Enabled = !ocupado;
            btnGuardar.Enabled = !ocupado;
            btnCancelar.Enabled = !ocupado;
            UseWaitCursor = ocupado;
        }
    }
}
