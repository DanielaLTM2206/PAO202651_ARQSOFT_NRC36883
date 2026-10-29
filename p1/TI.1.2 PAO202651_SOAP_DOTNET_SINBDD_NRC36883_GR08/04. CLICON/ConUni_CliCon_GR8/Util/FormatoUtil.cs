using System.Globalization;
using ec.edu.monster.Modelo;

namespace ec.edu.monster.Util;

public static class FormatoUtil
{
    // 10 cifras significativas: suficiente precisión sin arrastrar errores de punto flotante
    private const int CifrasSignificativas = 10;
    private const int MaximoDecimales = 15;

    /// <summary>
    /// Muestra el número sin notación científica ni ceros sobrantes,
    /// de modo que valores muy pequeños (0.000000001) no se vean como 0.
    /// </summary>
    public static string FormatearNumero(double numero)
    {
        if (numero == 0 || !double.IsFinite(numero))
        {
            return numero == 0 ? "0" : numero.ToString(CultureInfo.InvariantCulture);
        }

        int magnitud = (int)Math.Floor(Math.Log10(Math.Abs(numero)));
        int decimales = Math.Clamp(CifrasSignificativas - magnitud - 1, 0, MaximoDecimales);
        double redondeado = Math.Round(numero, decimales);

        // Valores por debajo de 15 decimales: se muestran en notación científica para no perderlos
        if (redondeado == 0)
        {
            return numero.ToString("0.#########E+0", CultureInfo.InvariantCulture);
        }
        return redondeado.ToString("0." + new string('#', MaximoDecimales), CultureInfo.InvariantCulture);
    }

    public static string FormatearFechaHora(DateTime fechaHora)
    {
        return fechaHora.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
    }

    // Por ejemplo: 1000 METRO = 1 KILOMETRO
    public static string FormatearConversion(RegistroConversion registro)
    {
        return $"{FormatearNumero(registro.Valor)} {registro.UnidadOrigen} = "
                + $"{FormatearNumero(registro.Resultado)} {registro.UnidadDestino}";
    }
}
