using ec.edu.monster.Cliente;
using ec.edu.monster.Controlador;
using ec.edu.monster.Modelo;
using ec.edu.monster.Util;
using ec.edu.monster.Vista.Controles;

namespace ec.edu.monster.Vista
{
    public partial class FormConversor : Form
    {
        private static readonly Color ColorPrimario = Color.FromArgb(0, 84, 147);
        private static readonly Color ColorPrimarioHover = Color.FromArgb(0, 99, 177);
        private static readonly Color ColorSuave = Color.FromArgb(225, 234, 244);
        private static readonly Color ColorSuaveHover = Color.FromArgb(205, 222, 240);
        private static readonly Color ColorSuavePresionado = Color.FromArgb(185, 208, 232);

        private readonly LoginControlador loginControlador;
        private readonly ConversionControlador conversionControlador;
        private readonly string urlServidor;
        private readonly Dictionary<BotonRedondeado, TipoConversion> pestanias;
        private TipoConversion? tipoSeleccionado;
        private bool enPeticion;

        /// <summary>
        /// true si el usuario cerró la ventana (salir del programa);
        /// false si cerró sesión o la sesión expiró (volver al inicio de sesión).
        /// </summary>
        public bool SalirDeLaAplicacion { get; private set; } = true;

        public FormConversor(LoginControlador loginControlador, string usuario)
        {
            InitializeComponent();
            this.loginControlador = loginControlador;
            conversionControlador = loginControlador.CrearControladorConversion();
            urlServidor = loginControlador.UrlServidor;

            btnUsuario.Text = $"{usuario}  ▼";
            lblEstado.Text = $"Conectado a {urlServidor}";

            pestanias = new Dictionary<BotonRedondeado, TipoConversion>
            {
                [btnLongitud] = TipoConversion.Longitud,
                [btnMasa] = TipoConversion.Masa,
                [btnTemperatura] = TipoConversion.Temperatura
            };
            PintarPestanias();
        }

        private async void FormConversor_Shown(object? sender, EventArgs e)
        {
            await SeleccionarTipoAsync(TipoConversion.Longitud);
            await CargarHistorialAsync();
        }

        private async void btnTipo_Click(object? sender, EventArgs e)
        {
            if (!enPeticion && sender is BotonRedondeado boton && pestanias.TryGetValue(boton, out TipoConversion tipo)
                    && tipo != tipoSeleccionado)
            {
                await SeleccionarTipoAsync(tipo);
            }
        }

        private async Task SeleccionarTipoAsync(TipoConversion tipo)
        {
            tipoSeleccionado = tipo;
            PintarPestanias();
            errorProvider.Clear();

            CambiarEstadoOcupado(true, $"Consultando unidades de {tipo.NombreVisible().ToLowerInvariant()}...");
            try
            {
                IReadOnlyList<string> unidades = await conversionControlador.ObtenerUnidadesAsync(tipo);

                cmbOrigen.Items.Clear();
                cmbDestino.Items.Clear();
                foreach (string unidad in unidades)
                {
                    cmbOrigen.Items.Add(unidad);
                    cmbDestino.Items.Add(unidad);
                }
                if (unidades.Count > 1)
                {
                    cmbOrigen.SelectedIndex = 0;
                    cmbDestino.SelectedIndex = 1;
                }
                LimpiarResultado();
            }
            catch (ExcepcionClienteWS ex)
            {
                ManejarErrorServicio(ex);
            }
            finally
            {
                CambiarEstadoOcupado(false);
            }
        }

        // La pestaña activa se pinta en azul; las demás en tono suave
        private void PintarPestanias()
        {
            foreach ((BotonRedondeado boton, TipoConversion tipo) in pestanias)
            {
                bool activa = tipo == tipoSeleccionado;
                boton.BackColor = activa ? ColorPrimario : ColorSuave;
                boton.ForeColor = activa ? Color.White : ColorPrimario;
                boton.ColorHover = activa ? ColorPrimarioHover : ColorSuaveHover;
                boton.ColorPresionado = activa ? ColorPrimario : ColorSuavePresionado;
            }
        }

        private async void btnConvertir_Click(object? sender, EventArgs e)
        {
            errorProvider.Clear();

            if (tipoSeleccionado is not TipoConversion tipo)
            {
                return;
            }

            double valor;
            try
            {
                valor = ValidacionUtil.InterpretarDecimal(txtValor.Text);
                conversionControlador.ValidarValor(tipo, valor);
            }
            catch (ArgumentException ex)
            {
                MostrarErrorDeCampo(txtValor, ex.Message);
                return;
            }

            string? unidadOrigen = cmbOrigen.SelectedItem as string;
            string? unidadDestino = cmbDestino.SelectedItem as string;
            try
            {
                conversionControlador.ValidarUnidades(unidadOrigen, unidadDestino);
            }
            catch (ArgumentException ex)
            {
                MostrarErrorDeCampo(cmbDestino, ex.Message);
                return;
            }

            CambiarEstadoOcupado(true, "Convirtiendo...");
            try
            {
                var modelo = new ConversionModelo(tipo, valor, unidadOrigen, unidadDestino);
                double resultado = await conversionControlador.ConvertirAsync(modelo);
                MostrarResultado(valor, unidadOrigen!, resultado, unidadDestino!);

                // El servidor ya registró la conversión: se recarga el historial compartido
                await CargarHistorialAsync();
            }
            catch (ExcepcionClienteWS ex)
            {
                ManejarErrorServicio(ex);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                CambiarEstadoOcupado(false);
            }
        }

        // Bloquea letras y símbolos mientras se escribe; el texto pegado se valida al convertir
        private void txtValor_KeyPress(object? sender, KeyPressEventArgs e)
        {
            string textoSinSeleccion = txtValor.Text.Remove(txtValor.SelectionStart, txtValor.SelectionLength);
            if (!ValidacionUtil.EsCaracterNumericoPermitido(e.KeyChar, textoSinSeleccion, txtValor.SelectionStart))
            {
                e.Handled = true;
            }
        }

        private void txtValor_TextChanged(object? sender, EventArgs e)
        {
            errorProvider.SetError(txtValor, string.Empty);
        }

        private void cmbUnidad_SelectedIndexChanged(object? sender, EventArgs e)
        {
            errorProvider.SetError(cmbDestino, string.Empty);
        }

        private void btnIntercambiar_Click(object? sender, EventArgs e)
        {
            int origen = cmbOrigen.SelectedIndex;
            cmbOrigen.SelectedIndex = cmbDestino.SelectedIndex;
            cmbDestino.SelectedIndex = origen;
        }

        // Las opciones de la cuenta se agrupan en un menú para no ocupar espacio en la ventana
        private void btnUsuario_Click(object? sender, EventArgs e)
        {
            mnuUsuario.Show(btnUsuario, new Point(btnUsuario.Width - mnuUsuario.Width, btnUsuario.Height + 4));
        }

        // Cambiar contraseña se abre como otra pantalla: mismo tamaño y misma posición que el conversor,
        // así lo cubre por completo en lugar de verse como una ventana flotante
        private void mnuCambiarContrasenia_Click(object? sender, EventArgs e)
        {
            using var pantalla = new FormCambiarContrasenia(loginControlador)
            {
                StartPosition = FormStartPosition.Manual,
                Bounds = Bounds
            };
            pantalla.ShowDialog(this);

            // Si el usuario movió la pantalla, el conversor reaparece en ese mismo lugar
            Location = pantalla.Location;

            if (pantalla.SesionExpirada)
            {
                VolverAlInicioDeSesion();
            }
        }

        private void mnuCerrarSesion_Click(object? sender, EventArgs e)
        {
            bool confirmar = DialogoConfirmacion.Confirmar(this, "¿Cerrar sesión?",
                    "Tendrá que volver a ingresar su usuario y contraseña para seguir usando el conversor.",
                    "Sí, cerrar sesión");

            if (confirmar)
            {
                VolverAlInicioDeSesion();
            }
        }

        private async void btnActualizarHistorial_Click(object? sender, EventArgs e)
        {
            await CargarHistorialAsync();
        }

        private async void btnLimpiarHistorial_Click(object? sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(this,
                    "Se borrarán las conversiones de todos los clientes (escritorio, web y móvil). "
                    + "Esta acción no se puede deshacer.\n\n¿Desea continuar?",
                    "Borrar historial", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                await conversionControlador.LimpiarHistorialAsync();
            }
            catch (ExcepcionClienteWS ex)
            {
                ManejarErrorServicio(ex);
                return;
            }
            await CargarHistorialAsync();
        }

        // Doble clic en una fila: vuelve a cargar esa conversión en el formulario para repetirla o ajustarla
        private async void dgvHistorial_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (enPeticion || e.RowIndex < 0 || dgvHistorial.Rows[e.RowIndex].Tag is not RegistroConversion registro)
            {
                return;
            }

            if (registro.Tipo != tipoSeleccionado)
            {
                await SeleccionarTipoAsync(registro.Tipo);
            }
            txtValor.Text = FormatoUtil.FormatearNumero(registro.Valor);
            cmbOrigen.SelectedItem = registro.UnidadOrigen;
            cmbDestino.SelectedItem = registro.UnidadDestino;
            MostrarResultado(registro.Valor, registro.UnidadOrigen, registro.Resultado, registro.UnidadDestino);
            txtValor.Focus();
            txtValor.SelectAll();
        }

        // El historial lo guarda el servidor: incluye lo convertido desde la web, el móvil y la consola
        private async Task CargarHistorialAsync()
        {
            IReadOnlyList<RegistroConversion> historial;
            btnActualizarHistorial.Enabled = false;
            try
            {
                historial = await conversionControlador.ObtenerHistorialAsync();
            }
            catch (ExcepcionClienteWS ex)
            {
                ManejarErrorServicio(ex);
                return;
            }
            finally
            {
                btnActualizarHistorial.Enabled = true;
            }

            dgvHistorial.SuspendLayout();
            dgvHistorial.Rows.Clear();
            foreach (RegistroConversion registro in historial)
            {
                string origen = $"{FormatoUtil.FormatearNumero(registro.Valor)} {registro.UnidadOrigen}";
                string destino = $"{FormatoUtil.FormatearNumero(registro.Resultado)} {registro.UnidadDestino}";

                // Dos líneas (origen arriba, resultado abajo) para que no se corten los valores largos
                int indice = dgvHistorial.Rows.Add(registro.FechaHora.ToString("HH:mm:ss"),
                        registro.Tipo.NombreVisible(), $"{origen}\n→ {destino}");
                DataGridViewRow fila = dgvHistorial.Rows[indice];
                fila.Tag = registro;
                fila.Cells[colConversion.Index].ToolTipText = $"{origen} = {destino}";
            }
            dgvHistorial.ResumeLayout();
            dgvHistorial.ClearSelection();
            dgvHistorial.CurrentCell = null;

            lblContadorHistorial.Text = historial.Count switch
            {
                0 => "Aún no hay conversiones",
                1 => "1 conversión en todos los clientes",
                int cantidad => $"{cantidad} conversiones en todos los clientes"
            };
            btnLimpiarHistorial.Enabled = historial.Count > 0;
        }

        private void MostrarResultado(double valor, string unidadOrigen, double resultado, string unidadDestino)
        {
            lblResultadoTitulo.Text = $"{FormatoUtil.FormatearNumero(valor)} {unidadOrigen} equivale a:";
            lblResultado.Text = $"{FormatoUtil.FormatearNumero(resultado)} {unidadDestino}";
            toolTip.SetToolTip(lblResultado, lblResultado.Text);
        }

        private void LimpiarResultado()
        {
            lblResultadoTitulo.Text = "Resultado";
            lblResultado.Text = "—";
            toolTip.SetToolTip(lblResultado, string.Empty);
        }

        private void MostrarErrorDeCampo(Control campo, string mensaje)
        {
            errorProvider.SetError(campo, mensaje);
            MessageBox.Show(this, mensaje, "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            campo.Focus();
        }

        // Un 401 significa token inválido o sesión expirada: se vuelve al inicio de sesión
        private void ManejarErrorServicio(ExcepcionClienteWS ex)
        {
            if (ex.EsErrorAutenticacion)
            {
                MessageBox.Show(this, ex.Message + "\n\nDebe iniciar sesión nuevamente.", "Sesión no válida",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                VolverAlInicioDeSesion();
                return;
            }

            string titulo = ex.EsErrorConexion ? "Sin conexión con el servidor" : "No se pudo completar la operación";
            MessageBox.Show(this, ex.Message, titulo, MessageBoxButtons.OK,
                    ex.EsErrorConexion ? MessageBoxIcon.Error : MessageBoxIcon.Warning);
        }

        private void VolverAlInicioDeSesion()
        {
            SalirDeLaAplicacion = false;
            Close();
        }

        private void CambiarEstadoOcupado(bool ocupado, string? mensaje = null)
        {
            // Las pestañas no se deshabilitan (se verían grises); se ignoran sus clics mientras hay una petición
            enPeticion = ocupado;
            pnlFormulario.Enabled = !ocupado;
            btnUsuario.Enabled = !ocupado;
            UseWaitCursor = ocupado;
            lblEstado.Text = ocupado && mensaje is not null ? mensaje : $"Conectado a {urlServidor}";

            if (!ocupado)
            {
                txtValor.Focus();
            }
        }
    }
}
