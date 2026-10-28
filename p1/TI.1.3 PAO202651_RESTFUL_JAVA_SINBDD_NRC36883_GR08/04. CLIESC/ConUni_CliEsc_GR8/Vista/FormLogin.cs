using System.Diagnostics.CodeAnalysis;
using ec.edu.monster.Cliente;
using ec.edu.monster.Controlador;
using ec.edu.monster.Modelo;

namespace ec.edu.monster.Vista
{
    public partial class FormLogin : Form
    {
        private readonly ServidorControlador servidorControlador;
        private ClienteWSConversor clienteWs;
        private LoginControlador loginControlador;

        public FormLogin(ServidorControlador servidorControlador)
        {
            InitializeComponent();
            this.servidorControlador = servidorControlador;
            Conectar();
            FormClosed += (_, _) => clienteWs.Dispose();
        }

        // Crea el cliente del servicio con la dirección configurada (se repite al cambiar de servidor)
        [MemberNotNull(nameof(clienteWs), nameof(loginControlador))]
        private void Conectar()
        {
            clienteWs?.Dispose();
            clienteWs = new ClienteWSConversor(servidorControlador.UrlActual);
            loginControlador = new LoginControlador(clienteWs);
            lblServidor.Text = "Servidor REST: " + loginControlador.UrlServidor;
        }

        private void lnkServidor_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            using var dialogo = new FormServidor(servidorControlador);
            if (dialogo.ShowDialog(this) == DialogResult.OK)
            {
                Conectar();
                lblError.Text = string.Empty;
            }
        }

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtContrasenia.Text))
            {
                lblError.Text = "Ingrese el usuario y la contraseña.";
                (string.IsNullOrWhiteSpace(txtUsuario.Text) ? txtUsuario : txtContrasenia).Focus();
                return;
            }

            string usuario = txtUsuario.Text.Trim();
            CambiarEstadoOcupado(true);
            try
            {
                await loginControlador.AutenticarAsync(new LoginModelo(usuario, txtContrasenia.Text));
            }
            catch (ExcepcionClienteWS ex)
            {
                CambiarEstadoOcupado(false);
                lblError.Text = ex.Message;
                txtContrasenia.SelectAll();
                txtContrasenia.Focus();
                return;
            }
            catch (ArgumentException ex)
            {
                CambiarEstadoOcupado(false);
                lblError.Text = ex.Message;
                return;
            }

            CambiarEstadoOcupado(false);
            AbrirConversor(usuario);
        }

        private void AbrirConversor(string usuario)
        {
            Hide();

            bool salirDeLaAplicacion;
            using (var conversor = new FormConversor(loginControlador, usuario))
            {
                conversor.ShowDialog();
                salirDeLaAplicacion = conversor.SalirDeLaAplicacion;
            }

            loginControlador.CerrarSesion();

            if (salirDeLaAplicacion)
            {
                Close();
                return;
            }

            // Cerró sesión o la sesión expiró: vuelve al inicio de sesión
            txtContrasenia.Clear();
            txtContrasenia.EsContrasena = true;
            Show();
            txtContrasenia.Focus();
        }

        private void CambiarEstadoOcupado(bool ocupado)
        {
            btnIngresar.Enabled = !ocupado;
            txtUsuario.Enabled = !ocupado;
            txtContrasenia.Enabled = !ocupado;
            btnIngresar.Text = ocupado ? "Conectando..." : "Iniciar sesión";
            UseWaitCursor = ocupado;
        }
    }
}
