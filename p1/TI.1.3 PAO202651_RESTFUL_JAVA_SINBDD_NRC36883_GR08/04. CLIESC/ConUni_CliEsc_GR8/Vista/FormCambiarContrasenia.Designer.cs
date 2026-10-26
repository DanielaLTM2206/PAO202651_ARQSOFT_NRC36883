namespace ec.edu.monster.Vista
{
    partial class FormCambiarContrasenia
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
            lblActual = new Label();
            txtActual = new ec.edu.monster.Vista.Controles.CampoTexto();
            lblNueva = new Label();
            txtNueva = new ec.edu.monster.Vista.Controles.CampoTexto();
            lblConfirmacion = new Label();
            txtConfirmacion = new ec.edu.monster.Vista.Controles.CampoTexto();
            lblError = new Label();
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
            lblIndicacion.Size = new Size(392, 25);
            lblIndicacion.TabIndex = 1;
            lblIndicacion.Text = "La nueva contraseña debe ser diferente a la actual";
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
            lblTitulo.Text = "Cambiar contraseña";
            //
            // lblActual
            //
            lblActual.AutoSize = true;
            lblActual.Font = new Font("Segoe UI", 10.5F);
            lblActual.ForeColor = Color.FromArgb(73, 80, 87);
            lblActual.Location = new Point(40, 124);
            lblActual.Name = "lblActual";
            lblActual.Size = new Size(155, 25);
            lblActual.TabIndex = 1;
            lblActual.Text = "Contraseña actual";
            //
            // txtActual
            //
            txtActual.EsContrasena = true;
            txtActual.Font = new Font("Segoe UI", 12F);
            txtActual.Location = new Point(40, 152);
            txtActual.MaxLength = 50;
            txtActual.Name = "txtActual";
            txtActual.PlaceholderText = "Escriba su contraseña actual";
            txtActual.Size = new Size(480, 48);
            txtActual.TabIndex = 2;
            //
            // lblNueva
            //
            lblNueva.AutoSize = true;
            lblNueva.Font = new Font("Segoe UI", 10.5F);
            lblNueva.ForeColor = Color.FromArgb(73, 80, 87);
            lblNueva.Location = new Point(40, 216);
            lblNueva.Name = "lblNueva";
            lblNueva.Size = new Size(152, 25);
            lblNueva.TabIndex = 3;
            lblNueva.Text = "Nueva contraseña";
            //
            // txtNueva
            //
            txtNueva.EsContrasena = true;
            txtNueva.Font = new Font("Segoe UI", 12F);
            txtNueva.Location = new Point(40, 244);
            txtNueva.MaxLength = 50;
            txtNueva.Name = "txtNueva";
            txtNueva.PlaceholderText = "Escriba la nueva contraseña";
            txtNueva.Size = new Size(480, 48);
            txtNueva.TabIndex = 4;
            //
            // lblConfirmacion
            //
            lblConfirmacion.AutoSize = true;
            lblConfirmacion.Font = new Font("Segoe UI", 10.5F);
            lblConfirmacion.ForeColor = Color.FromArgb(73, 80, 87);
            lblConfirmacion.Location = new Point(40, 308);
            lblConfirmacion.Name = "lblConfirmacion";
            lblConfirmacion.Size = new Size(234, 25);
            lblConfirmacion.TabIndex = 5;
            lblConfirmacion.Text = "Confirmar nueva contraseña";
            //
            // txtConfirmacion
            //
            txtConfirmacion.EsContrasena = true;
            txtConfirmacion.Font = new Font("Segoe UI", 12F);
            txtConfirmacion.Location = new Point(40, 336);
            txtConfirmacion.MaxLength = 50;
            txtConfirmacion.Name = "txtConfirmacion";
            txtConfirmacion.PlaceholderText = "Repita la nueva contraseña";
            txtConfirmacion.Size = new Size(480, 48);
            txtConfirmacion.TabIndex = 6;
            //
            // lblError
            //
            lblError.Font = new Font("Segoe UI", 10F);
            lblError.ForeColor = Color.FromArgb(192, 0, 0);
            lblError.Location = new Point(40, 394);
            lblError.Name = "lblError";
            lblError.Size = new Size(480, 44);
            lblError.TabIndex = 7;
            //
            // btnCancelar
            //
            btnCancelar.BackColor = Color.FromArgb(233, 236, 239);
            btnCancelar.ColorHover = Color.FromArgb(222, 226, 230);
            btnCancelar.ColorPresionado = Color.FromArgb(206, 212, 218);
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Font = new Font("Segoe UI", 11F);
            btnCancelar.ForeColor = Color.FromArgb(73, 80, 87);
            btnCancelar.Location = new Point(40, 452);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.RadioBorde = 12;
            btnCancelar.Size = new Size(232, 52);
            btnCancelar.TabIndex = 9;
            btnCancelar.Text = "Cancelar";
            //
            // btnGuardar
            //
            btnGuardar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnGuardar.Location = new Point(288, 452);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.RadioBorde = 12;
            btnGuardar.Size = new Size(232, 52);
            btnGuardar.TabIndex = 8;
            btnGuardar.Text = "Guardar cambios";
            btnGuardar.Click += btnGuardar_Click;
            //
            // FormCambiarContrasenia
            //
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            CancelButton = btnCancelar;
            ClientSize = new Size(560, 540);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            Controls.Add(lblError);
            Controls.Add(txtConfirmacion);
            Controls.Add(lblConfirmacion);
            Controls.Add(txtNueva);
            Controls.Add(lblNueva);
            Controls.Add(txtActual);
            Controls.Add(lblActual);
            Controls.Add(pnlEncabezado);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCambiarContrasenia";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ConUni GR08 - Cambiar contraseña";
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlEncabezado;
        private Label lblIndicacion;
        private Label lblTitulo;
        private Label lblActual;
        private ec.edu.monster.Vista.Controles.CampoTexto txtActual;
        private Label lblNueva;
        private ec.edu.monster.Vista.Controles.CampoTexto txtNueva;
        private Label lblConfirmacion;
        private ec.edu.monster.Vista.Controles.CampoTexto txtConfirmacion;
        private Label lblError;
        private ec.edu.monster.Vista.Controles.BotonRedondeado btnCancelar;
        private ec.edu.monster.Vista.Controles.BotonRedondeado btnGuardar;
    }
}
