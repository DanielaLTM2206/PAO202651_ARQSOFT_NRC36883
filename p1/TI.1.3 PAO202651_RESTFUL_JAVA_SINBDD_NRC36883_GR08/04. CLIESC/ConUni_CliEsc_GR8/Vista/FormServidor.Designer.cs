namespace ec.edu.monster.Vista
{
    partial class FormServidor
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
            pnlEncabezado = new Panel();
            lblIndicacion = new Label();
            lblTitulo = new Label();
            lblServidor = new Label();
            txtServidor = new ec.edu.monster.Vista.Controles.CampoTexto();
            lblEstado = new Label();
            btnProbar = new ec.edu.monster.Vista.Controles.BotonRedondeado();
            btnCancelar = new ec.edu.monster.Vista.Controles.BotonRedondeado();
            btnGuardar = new ec.edu.monster.Vista.Controles.BotonRedondeado();
            pnlEncabezado.SuspendLayout();
            SuspendLayout();
            //
            // pnlEncabezado
            //
            pnlEncabezado.BackColor = Color.FromArgb(0, 84, 147);
            pnlEncabezado.Controls.Add(lblIndicacion);
            pnlEncabezado.Controls.Add(lblTitulo);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Location = new Point(0, 0);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(560, 100);
            pnlEncabezado.TabIndex = 0;
            //
            // lblIndicacion
            //
            lblIndicacion.AutoSize = true;
            lblIndicacion.Font = new Font("Segoe UI", 10.5F);
            lblIndicacion.ForeColor = Color.FromArgb(214, 228, 240);
            lblIndicacion.Location = new Point(38, 62);
            lblIndicacion.Name = "lblIndicacion";
            lblIndicacion.Size = new Size(420, 25);
            lblIndicacion.TabIndex = 1;
            lblIndicacion.Text = "Computadora donde corre Payara con el servicio REST";
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(34, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(296, 45);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Configurar servidor";
            //
            // lblServidor
            //
            lblServidor.AutoSize = true;
            lblServidor.Font = new Font("Segoe UI", 10.5F);
            lblServidor.ForeColor = Color.FromArgb(73, 80, 87);
            lblServidor.Location = new Point(40, 124);
            lblServidor.Name = "lblServidor";
            lblServidor.Size = new Size(420, 25);
            lblServidor.TabIndex = 1;
            lblServidor.Text = "IP del servidor (ej.: 192.168.1.25) o localhost";
            //
            // txtServidor
            //
            txtServidor.Font = new Font("Segoe UI", 12F);
            txtServidor.Location = new Point(40, 152);
            txtServidor.MaxLength = 200;
            txtServidor.Name = "txtServidor";
            txtServidor.PlaceholderText = "Ej.: 192.168.1.25";
            txtServidor.Size = new Size(480, 48);
            txtServidor.TabIndex = 2;
            //
            // lblEstado
            //
            lblEstado.Font = new Font("Segoe UI", 10F);
            lblEstado.ForeColor = Color.FromArgb(108, 117, 125);
            lblEstado.Location = new Point(40, 210);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(480, 62);
            lblEstado.TabIndex = 6;
            //
            // btnProbar
            //
            btnProbar.BackColor = Color.FromArgb(233, 236, 239);
            btnProbar.ColorHover = Color.FromArgb(222, 226, 230);
            btnProbar.ColorPresionado = Color.FromArgb(206, 212, 218);
            btnProbar.Font = new Font("Segoe UI", 11F);
            btnProbar.ForeColor = Color.FromArgb(0, 84, 147);
            btnProbar.Location = new Point(40, 284);
            btnProbar.Name = "btnProbar";
            btnProbar.RadioBorde = 12;
            btnProbar.Size = new Size(156, 52);
            btnProbar.TabIndex = 3;
            btnProbar.Text = "Probar conexión";
            btnProbar.Click += btnProbar_Click;
            //
            // btnCancelar
            //
            btnCancelar.BackColor = Color.FromArgb(233, 236, 239);
            btnCancelar.ColorHover = Color.FromArgb(222, 226, 230);
            btnCancelar.ColorPresionado = Color.FromArgb(206, 212, 218);
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Font = new Font("Segoe UI", 11F);
            btnCancelar.ForeColor = Color.FromArgb(73, 80, 87);
            btnCancelar.Location = new Point(208, 284);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.RadioBorde = 12;
            btnCancelar.Size = new Size(150, 52);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            //
            // btnGuardar
            //
            btnGuardar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnGuardar.Location = new Point(370, 284);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.RadioBorde = 12;
            btnGuardar.Size = new Size(150, 52);
            btnGuardar.TabIndex = 4;
            btnGuardar.Text = "Guardar";
            btnGuardar.Click += btnGuardar_Click;
            //
            // FormServidor
            //
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            CancelButton = btnCancelar;
            ClientSize = new Size(560, 372);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            Controls.Add(btnProbar);
            Controls.Add(lblEstado);
            Controls.Add(txtServidor);
            Controls.Add(lblServidor);
            Controls.Add(pnlEncabezado);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormServidor";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ConUni GR08 - Configurar servidor";
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlEncabezado;
        private Label lblIndicacion;
        private Label lblTitulo;
        private Label lblServidor;
        private Controles.CampoTexto txtServidor;
        private Label lblEstado;
        private Controles.BotonRedondeado btnProbar;
        private Controles.BotonRedondeado btnCancelar;
        private Controles.BotonRedondeado btnGuardar;
    }
}
