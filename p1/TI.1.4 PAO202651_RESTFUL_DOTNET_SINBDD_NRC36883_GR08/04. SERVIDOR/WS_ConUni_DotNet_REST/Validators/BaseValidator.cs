using System;
using System.Globalization;
using WS_ConUni_DotNet_REST.Constants;
using WS_ConUni_DotNet_REST.Models;

namespace WS_ConUni_DotNet_REST.Validators
{
    public static class BaseValidator
    {
        public static ConversionErrorModel? ValidarStringPositivo(string valorString, string unidad, out double valor)
        {
            valor = 0;

            if (string.IsNullOrWhiteSpace(valorString))
            {
                return new ConversionErrorModel(
                    ErrorConstants.VALOR_VACIO,
                    ErrorConstants.MSG_VALOR_VACIO,
                    ErrorConstants.TIPO_VALIDACION,
                    null,
                    unidad,
                    "El campo no puede estar vacío ni contener solo espacios.");
            }

            // Aceptar tanto coma como punto decimal
            string normalized = valorString.Trim().Replace(',', '.');

            // Validar longitud máxima de caracteres para evitar desbordamiento
            string cleanDigits = normalized.Replace("-", "").Replace(".", "");
            if (cleanDigits.Length > 15)
            {
                return new ConversionErrorModel(
                    ErrorConstants.VALOR_FUERA_DE_RANGO,
                    "El valor ingresado supera el límite máximo permitido (máximo 15 dígitos numéricos).",
                    ErrorConstants.TIPO_VALIDACION,
                    null,
                    unidad,
                    "Por favor ingrese un valor dentro de los límites numéricos reales.");
            }

            if (!double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out valor))
            {
                return new ConversionErrorModel(
                    ErrorConstants.VALOR_NO_NUMERICO,
                    ErrorConstants.MSG_VALOR_NO_NUMERICO,
                    ErrorConstants.TIPO_VALIDACION,
                    null,
                    unidad,
                    $"El valor '{valorString}' no se pudo convertir a un número válido.");
            }

            if (valor > 1_000_000_000_000.0)
            {
                return new ConversionErrorModel(
                    ErrorConstants.VALOR_FUERA_DE_RANGO,
                    "El valor numérico supera el límite máximo permitido (1,000,000,000,000).",
                    ErrorConstants.TIPO_VALIDACION,
                    valor,
                    unidad,
                    "Valores superiores a 1 billón causan desbordamiento.");
            }

            return ValidarValorPositivo(valor, unidad);
        }

        public static ConversionErrorModel? ValidarValorPositivo(double valor, string unidad)
        {
            if (double.IsNaN(valor))
            {
                return new ConversionErrorModel(
                    ErrorConstants.VALOR_NAN,
                    ErrorConstants.MSG_VALOR_NAN,
                    ErrorConstants.TIPO_VALIDACION,
                    valor,
                    unidad);
            }

            if (double.IsInfinity(valor))
            {
                return new ConversionErrorModel(
                    ErrorConstants.VALOR_INFINITO,
                    ErrorConstants.MSG_VALOR_INFINITO,
                    ErrorConstants.TIPO_VALIDACION,
                    valor,
                    unidad);
            }

            if (valor < 0)
            {
                return new ConversionErrorModel(
                    ErrorConstants.VALOR_NEGATIVO,
                    ErrorConstants.MSG_VALOR_NEGATIVO,
                    ErrorConstants.TIPO_VALIDACION,
                    valor,
                    unidad,
                    "Magnitudes físicas de longitud y masa deben ser mayores o iguales a cero.");
            }

            return null;
        }

        public static ConversionErrorModel? ValidarStringTemperatura(string valorString, string unidad, out double valor)
        {
            valor = 0;

            if (string.IsNullOrWhiteSpace(valorString))
            {
                return new ConversionErrorModel(
                    ErrorConstants.VALOR_VACIO,
                    ErrorConstants.MSG_VALOR_VACIO,
                    ErrorConstants.TIPO_VALIDACION,
                    null,
                    unidad,
                    "El valor de temperatura no puede estar vacío.");
            }

            string normalized = valorString.Trim().Replace(',', '.');

            string cleanDigits = normalized.Replace("-", "").Replace(".", "");
            if (cleanDigits.Length > 15)
            {
                return new ConversionErrorModel(
                    ErrorConstants.VALOR_FUERA_DE_RANGO,
                    "El valor térmico supera el límite máximo permitido (máximo 15 dígitos numéricos).",
                    ErrorConstants.TIPO_VALIDACION,
                    null,
                    unidad,
                    "Por favor ingrese un valor dentro de los límites numéricos reales.");
            }

            if (!double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out valor))
            {
                return new ConversionErrorModel(
                    ErrorConstants.VALOR_NO_NUMERICO,
                    ErrorConstants.MSG_VALOR_NO_NUMERICO,
                    ErrorConstants.TIPO_VALIDACION,
                    null,
                    unidad,
                    $"El valor '{valorString}' no es un valor térmico válido.");
            }

            if (Math.Abs(valor) > 1_000_000_000_000.0)
            {
                return new ConversionErrorModel(
                    ErrorConstants.VALOR_FUERA_DE_RANGO,
                    "El valor de temperatura supera el límite físico y numérico permitido (1,000,000,000,000).",
                    ErrorConstants.TIPO_VALIDACION,
                    valor,
                    unidad);
            }

            return ValidarTemperatura(valor, unidad);
        }

        public static ConversionErrorModel? ValidarTemperatura(double valor, string unidad)
        {
            if (double.IsNaN(valor))
            {
                return new ConversionErrorModel(
                    ErrorConstants.VALOR_NAN,
                    ErrorConstants.MSG_VALOR_NAN,
                    ErrorConstants.TIPO_VALIDACION,
                    valor,
                    unidad);
            }

            if (double.IsInfinity(valor))
            {
                return new ConversionErrorModel(
                    ErrorConstants.VALOR_INFINITO,
                    ErrorConstants.MSG_VALOR_INFINITO,
                    ErrorConstants.TIPO_VALIDACION,
                    valor,
                    unidad);
            }

            string u = unidad.Trim().ToLowerInvariant();

            if (u.StartsWith("cel") && valor < TemperaturaConstants.CERO_ABSOLUTO_CELSIUS)
            {
                return new ConversionErrorModel(
                    ErrorConstants.TEMPERATURA_BAJO_CERO_ABSOLUTO,
                    ErrorConstants.MSG_TEMPERATURA_INVALIDA,
                    ErrorConstants.TIPO_VALIDACION,
                    valor,
                    unidad,
                    $"El valor no puede ser menor al cero absoluto (-273.15 °C).");
            }

            if (u.StartsWith("fah") && valor < TemperaturaConstants.CERO_ABSOLUTO_FAHRENHEIT)
            {
                return new ConversionErrorModel(
                    ErrorConstants.TEMPERATURA_BAJO_CERO_ABSOLUTO,
                    ErrorConstants.MSG_TEMPERATURA_INVALIDA,
                    ErrorConstants.TIPO_VALIDACION,
                    valor,
                    unidad,
                    $"El valor no puede ser menor al cero absoluto (-459.67 °F).");
            }

            if (u.StartsWith("kel") && valor < TemperaturaConstants.CERO_ABSOLUTO_KELVIN)
            {
                return new ConversionErrorModel(
                    ErrorConstants.TEMPERATURA_BAJO_CERO_ABSOLUTO,
                    ErrorConstants.MSG_TEMPERATURA_INVALIDA,
                    ErrorConstants.TIPO_VALIDACION,
                    valor,
                    unidad,
                    $"El valor no puede ser menor a cero Kelvin (0 K).");
            }

            return null;
        }
    }
}
