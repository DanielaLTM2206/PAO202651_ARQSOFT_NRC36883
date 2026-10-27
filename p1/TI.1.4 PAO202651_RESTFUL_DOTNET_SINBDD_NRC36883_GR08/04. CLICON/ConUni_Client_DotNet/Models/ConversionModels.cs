using System;

namespace ConUni_Client_DotNet.Models
{
    public class ConversionRequest
    {
        public string Valor { get; set; } = string.Empty;
        public string UnidadOrigen { get; set; } = string.Empty;
        public string UnidadDestino { get; set; } = string.Empty;
        public string? Token { get; set; }

        public ConversionRequest() { }

        public ConversionRequest(string valor, string unidadOrigen, string unidadDestino, string? token = null)
        {
            Valor = valor;
            UnidadOrigen = unidadOrigen;
            UnidadDestino = unidadDestino;
            Token = token;
        }
    }

    public class UnidadConversion
    {
        public double ValorOriginal { get; set; }
        public double ValorConvertidoExacto { get; set; }
        public double ValorConvertidoRedondeado { get; set; }
        public string UnidadOrigen { get; set; } = string.Empty;
        public string UnidadDestino { get; set; } = string.Empty;
        public string TipoConversion { get; set; } = string.Empty;
        public double FactorConversion { get; set; }
        public DateTime FechaConversion { get; set; }
    }

    public class ConversionError
    {
        public string CodigoError { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string TipoError { get; set; } = string.Empty;
        public double? ValorProblematico { get; set; }
        public string Unidad { get; set; } = string.Empty;
        public DateTime FechaError { get; set; }
        public string Detalles { get; set; } = string.Empty;
    }

    public class ConversionResult
    {
        public bool Exitoso { get; set; }
        public UnidadConversion? Resultado { get; set; }
        public ConversionError? Error { get; set; }
    }
}
