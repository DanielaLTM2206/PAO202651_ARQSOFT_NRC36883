namespace ec.edu.monster.Vista
{
    partial class FormConversor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle estiloEncabezadoHistorial = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloCeldasHistorial = new DataGridViewCellStyle();
            pnlEncabezado = new Panel();
            btnUsuario = new ec.edu.monster.Vista.Controles.BotonRedondeado();
            lblSubtitulo = new Label();
            lblTitulo = new Label();
            mnuUsuario = new ContextMenuStrip(components);
            mnuCambiarContrasenia = new ToolStripMenuItem();
            mnuSeparador = new ToolStripSeparator();
            mnuCerrarSesion = new ToolStripMenuItem();
            btnLongitud = new ec.edu.monster.Vista.Controles.BotonRedondeado();
            btnMasa = new ec.edu.monster.Vista.Controles.BotonRedondeado();
            btnTemperatura = new ec.edu.monster.Vista.Controles.BotonRedondeado();
            pnlFormulario = new Panel();
            lblValor = new Label();
            txtValor = new ec.edu.monster.Vista.Controles.CampoTexto();
            lblOrigen = new Label();
            cmbOrigen = new ComboBox();
            btnIntercambiar = new ec.edu.monster.Vista.Controles.BotonRedondeado();
            lblDestino = new Label();
            cmbDestino = new ComboBox();
            btnConvertir = new ec.edu.monster.Vista.Controles.BotonRedondeado();
            pnlResultado = new Panel();
            lblResultado = new Label();
            lblResultadoTitulo = new Label();
            pnlHistorial = new Panel();
            lblHistorial = new Label();
            lblContadorHistorial = new Label();
            btnLimpiarHistorial = new ec.edu.monster.Vista.Controles.BotonRedondeado();
            btnActualizarHistorial = new ec.edu.monster.Vista.Controles.BotonRedondeado();
            dgvHistorial = new DataGridView();
            colHora = new DataGridViewTextBoxColumn();
            colTipo = new DataGridViewTextBoxColumn();
            colConversion = new DataGridViewTextBoxColumn();
            barraEstado = new StatusStrip();
            lblEstado = new ToolStripStatusLabel();
            errorProvider = new ErrorProvider(components);
            toolTip = new ToolTip(components);
            pnlEncabezado.SuspendLayout();
            mnuUsuario.SuspendLayout();
            pnlFormulario.SuspendLayout();
            pnlResultado.SuspendLayout();
            pnlHistorial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).BeginInit();
            barraEstado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // pnlEncabezado
            //
            pnlEncabezado.BackColor = Color.FromArgb(0, 84, 147);
            pnlEncabezado.Controls.Add(btnUsuario);
            pnlEncabezado.Controls.Add(lblSubtitulo);
            pnlEncabezado.Controls.Add(lblTitulo);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Location = new Point(0, 0);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(1060, 92);
            pnlEncabezado.TabIndex = 0;
            //
            // btnUsuario
            //
            btnUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUsuario.BackColor = Color.FromArgb(16, 105, 170);
            btnUsuario.ColorHover = Color.FromArgb(34, 124, 192);
            btnUsuario.ColorPresionado = Color.FromArgb(0, 70, 122);
            btnUsuario.Font = new Font("Segoe UI", 10.5F);
            btnUsuario.Location = new Point(868, 26);
            btnUsuario.Name = "btnUsuario";
            btnUsuario.RadioBorde = 20;
            btnUsuario.Size = new Size(160, 40);
            btnUsuario.TabIndex = 2;
            btnUsuario.Text = "Usuario  ▼";
            toolTip.SetToolTip(btnUsuario, "Opciones de la cuenta");
            btnUsuario.Click += btnUsuario_Click;
            //
            // lblSubtitulo
            //
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.ForeColor = Color.FromArgb(214, 228, 240);
            lblSubtitulo.Location = new Point(34, 54);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(318, 23);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Elija la magnitud, el valor y las unidades";
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(30, 10);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(327, 45);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Conversor de Unidades";
            //
            // mnuUsuario
            //
            mnuUsuario.Font = new Font("Segoe UI", 10.5F);
            mnuUsuario.ImageScalingSize = new Size(20, 20);
            mnuUsuario.Items.AddRange(new ToolStripItem[] { mnuCambiarContrasenia, mnuSeparador, mnuCerrarSesion });
            mnuUsuario.Name = "mnuUsuario";
            mnuUsuario.Size = new Size(225, 70);
            //
            // mnuCambiarContrasenia
            //
            mnuCambiarContrasenia.Name = "mnuCambiarContrasenia";
            mnuCambiarContrasenia.Padding = new Padding(0, 5, 0, 5);
            mnuCambiarContrasenia.Size = new Size(224, 34);
            mnuCambiarContrasenia.Text = "Cambiar contraseña";
            mnuCambiarContrasenia.Click += mnuCambiarContrasenia_Click;
            //
            // mnuSeparador
            //
            mnuSeparador.Name = "mnuSeparador";
            mnuSeparador.Size = new Size(221, 6);
            //
            // mnuCerrarSesion
            //
            mnuCerrarSesion.ForeColor = Color.FromArgb(192, 0, 0);
            mnuCerrarSesion.Name = "mnuCerrarSesion";
            mnuCerrarSesion.Padding = new Padding(0, 5, 0, 5);
            mnuCerrarSesion.Size = new Size(224, 34);
            mnuCerrarSesion.Text = "Cerrar sesión";
            mnuCerrarSesion.Click += mnuCerrarSesion_Click;
            //
            // btnLongitud
            //
            btnLongitud.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLongitud.Location = new Point(32, 114);
            btnLongitud.Name = "btnLongitud";
            btnLongitud.RadioBorde = 12;
            btnLongitud.Size = new Size(180, 48);
            btnLongitud.TabIndex = 1;
            btnLongitud.Text = "Longitud";
            btnLongitud.Click += btnTipo_Click;
            //
            // btnMasa
            //
            btnMasa.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnMasa.Location = new Point(222, 114);
            btnMasa.Name = "btnMasa";
            btnMasa.RadioBorde = 12;
            btnMasa.Size = new Size(180, 48);
            btnMasa.TabIndex = 2;
            btnMasa.Text = "Masa";
            btnMasa.Click += btnTipo_Click;
            //
            // btnTemperatura
            //
            btnTemperatura.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnTemperatura.Location = new Point(412, 114);
            btnTemperatura.Name = "btnTemperatura";
            btnTemperatura.RadioBorde = 12;
            btnTemperatura.Size = new Size(180, 48);
            btnTemperatura.TabIndex = 3;
            btnTemperatura.Text = "Temperatura";
            btnTemperatura.Click += btnTipo_Click;
            //
            // pnlFormulario
            //
            pnlFormulario.BackColor = Color.White;
            pnlFormulario.Controls.Add(lblValor);
            pnlFormulario.Controls.Add(txtValor);
            pnlFormulario.Controls.Add(lblOrigen);
            pnlFormulario.Controls.Add(cmbOrigen);
            pnlFormulario.Controls.Add(btnIntercambiar);
            pnlFormulario.Controls.Add(lblDestino);
            pnlFormulario.Controls.Add(cmbDestino);
            pnlFormulario.Controls.Add(btnConvertir);
            pnlFormulario.Location = new Point(32, 178);
            pnlFormulario.Name = "pnlFormulario";
            pnlFormulario.Size = new Size(560, 270);
            pnlFormulario.TabIndex = 4;
            //
            // lblValor
            //
            lblValor.AutoSize = true;
            lblValor.Font = new Font("Segoe UI", 10.5F);
            lblValor.ForeColor = Color.FromArgb(73, 80, 87);
            lblValor.Location = new Point(24, 18);
            lblValor.Name = "lblValor";
            lblValor.Size = new Size(144, 25);
            lblValor.TabIndex = 0;
            lblValor.Text = "Valor a convertir";
            //
            // txtValor
            //
            txtValor.Font = new Font("Segoe UI", 13F);
            txtValor.Location = new Point(24, 44);
            txtValor.MaxLength = 27;
            txtValor.Name = "txtValor";
            txtValor.PlaceholderText = "Ejemplo: 1000 o 12.5";
            txtValor.Size = new Size(512, 48);
            txtValor.TabIndex = 1;
            txtValor.TextChanged += txtValor_TextChanged;
            txtValor.KeyPress += txtValor_KeyPress;
            //
            // lblOrigen
            //
            lblOrigen.AutoSize = true;
            lblOrigen.Font = new Font("Segoe UI", 10.5F);
            lblOrigen.ForeColor = Color.FromArgb(73, 80, 87);
            lblOrigen.Location = new Point(24, 106);
            lblOrigen.Name = "lblOrigen";
            lblOrigen.Size = new Size(30, 25);
            lblOrigen.TabIndex = 2;
            lblOrigen.Text = "De";
            //
            // cmbOrigen
            //
            cmbOrigen.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrigen.FlatStyle = FlatStyle.Flat;
            cmbOrigen.Font = new Font("Segoe UI", 12F);
            cmbOrigen.FormattingEnabled = true;
            cmbOrigen.Location = new Point(24, 132);
            cmbOrigen.Name = "cmbOrigen";
            cmbOrigen.Size = new Size(217, 36);
            cmbOrigen.TabIndex = 3;
            cmbOrigen.SelectedIndexChanged += cmbUnidad_SelectedIndexChanged;
            //
            // btnIntercambiar
            //
            btnIntercambiar.BackColor = Color.FromArgb(225, 234, 244);
            btnIntercambiar.ColorHover = Color.FromArgb(205, 222, 240);
            btnIntercambiar.ColorPresionado = Color.FromArgb(185, 208, 232);
            btnIntercambiar.Font = new Font("Segoe UI Symbol", 14F);
            btnIntercambiar.ForeColor = Color.FromArgb(0, 84, 147);
            btnIntercambiar.Location = new Point(253, 128);
            btnIntercambiar.Name = "btnIntercambiar";
            btnIntercambiar.RadioBorde = 21;
            btnIntercambiar.Size = new Size(54, 42);
            btnIntercambiar.TabIndex = 4;
            btnIntercambiar.Text = "⇄";
            toolTip.SetToolTip(btnIntercambiar, "Intercambiar unidades");
            btnIntercambiar.Click += btnIntercambiar_Click;
            //
            // lblDestino
            //
            lblDestino.AutoSize = true;
            lblDestino.Font = new Font("Segoe UI", 10.5F);
            lblDestino.ForeColor = Color.FromArgb(73, 80, 87);
            lblDestino.Location = new Point(319, 106);
            lblDestino.Name = "lblDestino";
            lblDestino.Size = new Size(20, 25);
            lblDestino.TabIndex = 5;
            lblDestino.Text = "A";
            //
            // cmbDestino
            //
            cmbDestino.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDestino.FlatStyle = FlatStyle.Flat;
            cmbDestino.Font = new Font("Segoe UI", 12F);
            cmbDestino.FormattingEnabled = true;
            cmbDestino.Location = new Point(319, 132);
            cmbDestino.Name = "cmbDestino";
            cmbDestino.Size = new Size(217, 36);
            cmbDestino.TabIndex = 6;
            cmbDestino.SelectedIndexChanged += cmbUnidad_SelectedIndexChanged;
            //
            // btnConvertir
            //
            btnConvertir.Font = new Font("Segoe UI", 12.5F, FontStyle.Bold);
            btnConvertir.Location = new Point(24, 194);
            btnConvertir.Name = "btnConvertir";
            btnConvertir.RadioBorde = 12;
            btnConvertir.Size = new Size(512, 54);
            btnConvertir.TabIndex = 7;
            btnConvertir.Text = "Convertir";
            btnConvertir.Click += btnConvertir_Click;
            //
            // pnlResultado
            //
            pnlResultado.BackColor = Color.FromArgb(225, 234, 244);
            pnlResultado.Controls.Add(lblResultado);
            pnlResultado.Controls.Add(lblResultadoTitulo);
            pnlResultado.Location = new Point(32, 464);
            pnlResultado.Name = "pnlResultado";
            pnlResultado.Size = new Size(560, 120);
            pnlResultado.TabIndex = 5;
            //
            // lblResultado
            //
            lblResultado.AutoEllipsis = true;
            lblResultado.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblResultado.ForeColor = Color.FromArgb(0, 84, 147);
            lblResultado.Location = new Point(22, 46);
            lblResultado.Name = "lblResultado";
            lblResultado.Size = new Size(516, 60);
            lblResultado.TabIndex = 1;
            lblResultado.Text = "—";
            //
            // lblResultadoTitulo
            //
            lblResultadoTitulo.AutoEllipsis = true;
            lblResultadoTitulo.Font = new Font("Segoe UI", 10.5F);
            lblResultadoTitulo.ForeColor = Color.FromArgb(73, 80, 87);
            lblResultadoTitulo.Location = new Point(22, 18);
            lblResultadoTitulo.Name = "lblResultadoTitulo";
            lblResultadoTitulo.Size = new Size(516, 28);
            lblResultadoTitulo.TabIndex = 0;
            lblResultadoTitulo.Text = "Resultado";
            //
            // pnlHistorial
            //
            pnlHistorial.BackColor = Color.White;
            pnlHistorial.Controls.Add(lblHistorial);
            pnlHistorial.Controls.Add(lblContadorHistorial);
            pnlHistorial.Controls.Add(btnLimpiarHistorial);
            pnlHistorial.Controls.Add(btnActualizarHistorial);
            pnlHistorial.Controls.Add(dgvHistorial);
            pnlHistorial.Location = new Point(612, 114);
            pnlHistorial.Name = "pnlHistorial";
            pnlHistorial.Size = new Size(416, 470);
            pnlHistorial.TabIndex = 6;
            //
            // lblHistorial
            //
            lblHistorial.AutoSize = true;
            lblHistorial.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblHistorial.ForeColor = Color.FromArgb(33, 37, 41);
            lblHistorial.Location = new Point(18, 14);
            lblHistorial.Name = "lblHistorial";
            lblHistorial.Size = new Size(243, 30);
            lblHistorial.TabIndex = 0;
            lblHistorial.Text = "Historial de conversiones";
            //
            // lblContadorHistorial
            //
            lblContadorHistorial.AutoSize = true;
            lblContadorHistorial.Font = new Font("Segoe UI", 9.5F);
            lblContadorHistorial.ForeColor = Color.FromArgb(108, 117, 125);
            lblContadorHistorial.Location = new Point(20, 46);
            lblContadorHistorial.Name = "lblContadorHistorial";
            lblContadorHistorial.Size = new Size(186, 21);
            lblContadorHistorial.TabIndex = 1;
            lblContadorHistorial.Text = "Aún no hay conversiones";
            //
            // btnLimpiarHistorial
            //
            btnLimpiarHistorial.BackColor = Color.FromArgb(233, 236, 239);
            btnLimpiarHistorial.ColorHover = Color.FromArgb(222, 226, 230);
            btnLimpiarHistorial.ColorPresionado = Color.FromArgb(206, 212, 218);
            btnLimpiarHistorial.Enabled = false;
            btnLimpiarHistorial.Font = new Font("Segoe UI", 9.5F);
            btnLimpiarHistorial.ForeColor = Color.FromArgb(73, 80, 87);
            btnLimpiarHistorial.Location = new Point(304, 76);
            btnLimpiarHistorial.Name = "btnLimpiarHistorial";
            btnLimpiarHistorial.RadioBorde = 16;
            btnLimpiarHistorial.Size = new Size(94, 32);
            btnLimpiarHistorial.TabIndex = 2;
            btnLimpiarHistorial.Text = "Limpiar";
            toolTip.SetToolTip(btnLimpiarHistorial, "Borrar el historial compartido por todos los clientes");
            btnLimpiarHistorial.Click += btnLimpiarHistorial_Click;
            //
            // btnActualizarHistorial
            //
            btnActualizarHistorial.BackColor = Color.FromArgb(225, 234, 244);
            btnActualizarHistorial.ColorHover = Color.FromArgb(205, 222, 240);
            btnActualizarHistorial.ColorPresionado = Color.FromArgb(185, 208, 232);
            btnActualizarHistorial.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnActualizarHistorial.ForeColor = Color.FromArgb(0, 84, 147);
            btnActualizarHistorial.Location = new Point(192, 76);
            btnActualizarHistorial.Name = "btnActualizarHistorial";
            btnActualizarHistorial.RadioBorde = 16;
            btnActualizarHistorial.Size = new Size(104, 32);
            btnActualizarHistorial.TabIndex = 4;
            btnActualizarHistorial.Text = "Actualizar";
            toolTip.SetToolTip(btnActualizarHistorial, "Ver las conversiones hechas desde los otros clientes");
            btnActualizarHistorial.Click += btnActualizarHistorial_Click;
            //
            // dgvHistorial
            //
            dgvHistorial.AllowUserToAddRows = false;
            dgvHistorial.AllowUserToDeleteRows = false;
            dgvHistorial.AllowUserToResizeColumns = false;
            dgvHistorial.AllowUserToResizeRows = false;
            dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistorial.BackgroundColor = Color.White;
            dgvHistorial.BorderStyle = BorderStyle.None;
            dgvHistorial.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHistorial.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            estiloEncabezadoHistorial.Alignment = DataGridViewContentAlignment.MiddleLeft;
            estiloEncabezadoHistorial.BackColor = Color.FromArgb(244, 246, 249);
            estiloEncabezadoHistorial.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            estiloEncabezadoHistorial.ForeColor = Color.FromArgb(73, 80, 87);
            estiloEncabezadoHistorial.Padding = new Padding(6, 0, 0, 0);
            estiloEncabezadoHistorial.SelectionBackColor = Color.FromArgb(244, 246, 249);
            estiloEncabezadoHistorial.SelectionForeColor = Color.FromArgb(73, 80, 87);
            estiloEncabezadoHistorial.WrapMode = DataGridViewTriState.False;
            dgvHistorial.ColumnHeadersDefaultCellStyle = estiloEncabezadoHistorial;
            dgvHistorial.ColumnHeadersHeight = 38;
            dgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvHistorial.Columns.AddRange(new DataGridViewColumn[] { colHora, colTipo, colConversion });
            estiloCeldasHistorial.Alignment = DataGridViewContentAlignment.MiddleLeft;
            estiloCeldasHistorial.BackColor = Color.White;
            estiloCeldasHistorial.Font = new Font("Segoe UI", 9.5F);
            estiloCeldasHistorial.ForeColor = Color.FromArgb(33, 37, 41);
            estiloCeldasHistorial.Padding = new Padding(6, 2, 4, 2);
            estiloCeldasHistorial.SelectionBackColor = Color.FromArgb(225, 234, 244);
            estiloCeldasHistorial.SelectionForeColor = Color.FromArgb(0, 84, 147);
            estiloCeldasHistorial.WrapMode = DataGridViewTriState.True;
            dgvHistorial.DefaultCellStyle = estiloCeldasHistorial;
            dgvHistorial.EnableHeadersVisualStyles = false;
            dgvHistorial.GridColor = Color.FromArgb(233, 236, 239);
            dgvHistorial.Location = new Point(18, 120);
            dgvHistorial.MultiSelect = false;
            dgvHistorial.Name = "dgvHistorial";
            dgvHistorial.ReadOnly = true;
            dgvHistorial.RowHeadersVisible = false;
            dgvHistorial.RowTemplate.Height = 54;
            dgvHistorial.ScrollBars = ScrollBars.Vertical;
            dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorial.Size = new Size(380, 332);
            dgvHistorial.TabIndex = 3;
            toolTip.SetToolTip(dgvHistorial, "Doble clic en una fila para volver a cargar esa conversión");
            dgvHistorial.CellDoubleClick += dgvHistorial_CellDoubleClick;
            //
            // colHora
            //
            colHora.FillWeight = 24F;
            colHora.HeaderText = "Hora";
            colHora.Name = "colHora";
            colHora.ReadOnly = true;
            colHora.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // colTipo
            //
            colTipo.FillWeight = 30F;
            colTipo.HeaderText = "Tipo";
            colTipo.Name = "colTipo";
            colTipo.ReadOnly = true;
            colTipo.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // colConversion
            //
            colConversion.FillWeight = 70F;
            colConversion.HeaderText = "Conversión";
            colConversion.Name = "colConversion";
            colConversion.ReadOnly = true;
            colConversion.SortMode = DataGridViewColumnSortMode.NotSortable;
            //
            // barraEstado
            //
            barraEstado.BackColor = Color.White;
            barraEstado.ImageScalingSize = new Size(20, 20);
            barraEstado.Items.AddRange(new ToolStripItem[] { lblEstado });
            barraEstado.Location = new Point(0, 614);
            barraEstado.Name = "barraEstado";
            barraEstado.Padding = new Padding(12, 0, 16, 0);
            barraEstado.Size = new Size(1060, 26);
            barraEstado.SizingGrip = false;
            barraEstado.TabIndex = 7;
            //
            // lblEstado
            //
            lblEstado.ForeColor = Color.DimGray;
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(41, 20);
            lblEstado.Text = "Listo";
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // FormConversor
            //
            AcceptButton = btnConvertir;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(244, 246, 249);
            ClientSize = new Size(1060, 640);
            Controls.Add(barraEstado);
            Controls.Add(pnlHistorial);
            Controls.Add(pnlResultado);
            Controls.Add(pnlFormulario);
            Controls.Add(btnTemperatura);
            Controls.Add(btnMasa);
            Controls.Add(btnLongitud);
            Controls.Add(pnlEncabezado);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormConversor";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ConUni GR08 - Conversor de Unidades";
            Shown += FormConversor_Shown;
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            mnuUsuario.ResumeLayout(false);
            pnlFormulario.ResumeLayout(false);
            pnlFormulario.PerformLayout();
            pnlResultado.ResumeLayout(false);
            pnlHistorial.ResumeLayout(false);
            pnlHistorial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).EndInit();
            barraEstado.ResumeLayout(false);
            barraEstado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlEncabezado;
        private ec.edu.monster.Vista.Controles.BotonRedondeado btnUsuario;
        private Label lblSubtitulo;
        private Label lblTitulo;
        private ContextMenuStrip mnuUsuario;
        private ToolStripMenuItem mnuCambiarContrasenia;
        private ToolStripSeparator mnuSeparador;
        private ToolStripMenuItem mnuCerrarSesion;
        private ec.edu.monster.Vista.Controles.BotonRedondeado btnLongitud;
        private ec.edu.monster.Vista.Controles.BotonRedondeado btnMasa;
        private ec.edu.monster.Vista.Controles.BotonRedondeado btnTemperatura;
        private Panel pnlFormulario;
        private Label lblValor;
        private ec.edu.monster.Vista.Controles.CampoTexto txtValor;
        private Label lblOrigen;
        private ComboBox cmbOrigen;
        private ec.edu.monster.Vista.Controles.BotonRedondeado btnIntercambiar;
        private Label lblDestino;
        private ComboBox cmbDestino;
        private ec.edu.monster.Vista.Controles.BotonRedondeado btnConvertir;
        private Panel pnlResultado;
        private Label lblResultado;
        private Label lblResultadoTitulo;
        private Panel pnlHistorial;
        private Label lblHistorial;
        private Label lblContadorHistorial;
        private ec.edu.monster.Vista.Controles.BotonRedondeado btnLimpiarHistorial;
        private ec.edu.monster.Vista.Controles.BotonRedondeado btnActualizarHistorial;
        private DataGridView dgvHistorial;
        private DataGridViewTextBoxColumn colHora;
        private DataGridViewTextBoxColumn colTipo;
        private DataGridViewTextBoxColumn colConversion;
        private StatusStrip barraEstado;
        private ToolStripStatusLabel lblEstado;
        private ErrorProvider errorProvider;
        private ToolTip toolTip;
    }
}
