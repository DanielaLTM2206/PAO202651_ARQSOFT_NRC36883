using System;

namespace WS_ConUni_DotNet_REST.Models
{
    /// <summary>
    /// Modelo de solicitud para conversiones de unidades
    /// </summary>
    public class ConversionRequest
    {
        /// <summary>
        /// Valor numérico a convertir en formato string o número
        /// </summary>
        public string Valor { get; set; } = string.Empty;

        /// <summary>
        /// Unidad de origen (ej: Milla, Metro, Kilogramo, Celsius)
        /// </summary>
        public string UnidadOrigen { get; set; } = string.Empty;

        /// <summary>
        /// Unidad de destino (ej: Metro, Pulgada, Libra, Fahrenheit)
        /// </summary>
        public string UnidadDestino { get; set; } = string.Empty;

        /// <summary>
        /// Token opcional en el cuerpo (si no se envía por encabezado Authorization)
        /// </summary>
        public string? Token { get; set; }
    }

    /// <summary>
    /// Modelo del resultado exitoso de una conversión
    /// </summary>
    public class UnidadConversionModel
    {
        public double ValorOriginal { get; set; }
        public double ValorConvertidoExacto { get; set; }
        public double ValorConvertidoRedondeado { get; set; }
        public string UnidadOrigen { get; set; } = string.Empty;
        public string UnidadDestino { get; set; } = string.Empty;
        public string TipoConversion { get; set; } = string.Empty;
        public double FactorConversion { get; set; }
        public DateTime FechaConversion { get; set; } = DateTime.UtcNow;

        public UnidadConversionModel() { }

        public UnidadConversionModel(
            double valorOriginal,
            double valorConvertidoExacto,
            string unidadOrigen,
            string unidadDestino,
            string tipoConversion,
            double factorConversion)
        {
            ValorOriginal = valorOriginal;
            ValorConvertidoExacto = valorConvertidoExacto;
            ValorConvertidoRedondeado = Math.Round(valorConvertidoExacto, 4);
            UnidadOrigen = unidadOrigen;
            UnidadDestino = unidadDestino;
            TipoConversion = tipoConversion;
            FactorConversion = factorConversion;
            FechaConversion = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Modelo de error de validación o del sistema
    /// </summary>
    public class ConversionErrorModel
    {
        public string CodigoError { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string TipoError { get; set; } = string.Empty;
        public double? ValorProblematico { get; set; }
        public string Unidad { get; set; } = string.Empty;
        public DateTime FechaError { get; set; } = DateTime.UtcNow;
        public string Detalles { get; set; } = string.Empty;

        public ConversionErrorModel() { }

        public ConversionErrorModel(
            string codigoError,
            string mensaje,
            string tipoError,
            double? valorProblematico = null,
            string unidad = "",
            string detalles = "")
        {
            CodigoError = codigoError;
            Mensaje = mensaje;
            TipoError = tipoError;
            ValorProblematico = valorProblematico;
            Unidad = unidad;
            Detalles = detalles;
            FechaError = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Modelo estándar de respuesta de conversión (compatible con GR01 y GR08)
    /// </summary>
    public class ConversionResultModel
    {
        public bool Exitoso { get; set; }
        public UnidadConversionModel? Resultado { get; set; }
        public ConversionErrorModel? Error { get; set; }

        public static ConversionResultModel Exito(UnidadConversionModel resultado)
        {
            return new ConversionResultModel
            {
                Exitoso = true,
                Resultado = resultado,
                Error = null
            };
        }

        public static ConversionResultModel Fallo(ConversionErrorModel error)
        {
            return new ConversionResultModel
            {
                Exitoso = false,
                Resultado = null,
                Error = error
            };
        }
    }
}
