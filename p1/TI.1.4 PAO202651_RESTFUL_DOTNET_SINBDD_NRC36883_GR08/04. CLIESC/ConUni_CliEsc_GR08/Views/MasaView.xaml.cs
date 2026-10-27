using System;
using System.Windows;
using System.Windows.Controls;
using ConUni_CliEsc_GR08.Models;
using ConUni_CliEsc_GR08.Services;

namespace ConUni_CliEsc_GR08.Views
{
    public partial class MasaView : UserControl
    {
        public event Action? ConversionRealizada;

        public MasaView()
        {
            InitializeComponent();
        }

        private async void BtnConvertir_Click(object sender, RoutedEventArgs e)
        {
            pnlResultado.Visibility = Visibility.Collapsed;
            pnlError.Visibility = Visibility.Collapsed;

            string valor = txtValor.Text.Trim();
            if (string.IsNullOrWhiteSpace(valor))
            {
                MostrarError("ERR_VACIO", "El valor a convertir no puede estar vacio.", "Ingrese un numero.");
                return;
            }

            string cleanDigits = valor.Replace("-", "").Replace(".", "").Replace(",", "");
            if (cleanDigits.Length > 15)
            {
                MostrarError("VAL_007", "El valor ingresado supera el limite maximo permitido (maximo 15 digitos).", "Ingrese un valor numerico razonable.");
                return;
            }

            var (origen, destino) = ObtenerUnidades(cmbConversion.SelectedIndex);
            if (string.IsNullOrEmpty(origen))
            {
                MostrarError("ERR_SEL", "Seleccione un tipo de conversion valido.", "");
                return;
            }

            btnConvertir.IsEnabled = false;
            btnConvertir.Content = "PROCESANDO...";

            try
            {
                var res = await RestClient.Instance.ConvertirMasaAsync(valor, origen, destino);
                if (res.Exitoso && res.Resultado != null)
                {
                    lblEquivalencia.Text = $"{res.Resultado.ValorOriginal} {res.Resultado.UnidadOrigen} equivalen a:";
                    lblValorGrande.Text = $"{res.Resultado.ValorConvertidoRedondeado} {res.Resultado.UnidadDestino}";
                    lblValorExacto.Text = $"Valor Exacto: {res.Resultado.ValorConvertidoExacto}";
                    lblFactor.Text = $"Factor: {res.Resultado.FactorConversion} | {res.Resultado.FechaConversion.ToLocalTime():HH:mm:ss}";

                    pnlResultado.Visibility = Visibility.Visible;
                    ConversionRealizada?.Invoke();
                }
                else
                {
                    string cod = res.Error?.CodigoError ?? "ERR";
                    string msg = res.Error?.Mensaje ?? "Error al procesar conversion";
                    string det = res.Error?.Detalles ?? "";
                    MostrarError(cod, msg, det);
                }
            }
            catch (Exception ex)
            {
                MostrarError("CONN_ERR", "Error de comunicacion con el servidor", ex.Message);
            }
            finally
            {
                btnConvertir.IsEnabled = true;
                btnConvertir.Content = "CONVERTIR MASA";
            }
        }

        private void MostrarError(string codigo, string mensaje, string detalles)
        {
            lblErrorTitulo.Text = $"Error en la Conversion [{codigo}]";
            lblErrorMensaje.Text = mensaje;
            lblErrorDetalles.Text = detalles;
            pnlError.Visibility = Visibility.Visible;
        }

        private static (string Origen, string Destino) ObtenerUnidades(int index) => index switch
        {
            0 => ("Kilogramo", "Libra"),
            1 => ("Libra", "Kilogramo"),
            2 => ("Kilogramo", "Onza"),
            3 => ("Onza", "Kilogramo"),
            4 => ("Libra", "Onza"),
            5 => ("Onza", "Libra"),
            6 => ("Gramo", "Kilogramo"),
            7 => ("Kilogramo", "Gramo"),
            _ => ("", "")
        };
    }
}
