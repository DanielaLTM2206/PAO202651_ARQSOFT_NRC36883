using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace UniversalConverter.Client.Models
{
    public class ConversionResponse
    {
        [JsonPropertyName("exitoso")]
        public bool Exitoso { get; set; }

        [JsonPropertyName("resultado")]
        public ResultData Resultado { get; set; }

        [JsonPropertyName("error")]
        public ErrorData Error { get; set; }
    }

    public class ResultData
    {
        [JsonPropertyName("valorOriginal")]
        public double ValorOriginal { get; set; }

        [JsonPropertyName("valorConvertidoExacto")]
        public double ValorConvertidoExacto { get; set; }

        [JsonPropertyName("valorConvertidoRedondeado")]
        public double ValorConvertidoRedondeado { get; set; }

        [JsonPropertyName("unidadOrigen")]
        public string UnidadOrigen { get; set; }

        [JsonPropertyName("unidadDestino")]
        public string UnidadDestino { get; set; }

        [JsonPropertyName("factorConversion")]
        public double FactorConversion { get; set; }

        [JsonPropertyName("fechaConversion")]
        public DateTime FechaConversion { get; set; }
    }

    public class ErrorData
    {
        [JsonPropertyName("codigoError")]
        public string CodigoError { get; set; }

        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; }

        [JsonPropertyName("tipoError")]
        public string TipoError { get; set; }

        [JsonPropertyName("valorProblematico")]
        public object ValorProblematico { get; set; }

        [JsonPropertyName("unidad")]
        public string Unidad { get; set; }

        [JsonPropertyName("fechaError")]
        public DateTime FechaError { get; set; }

        [JsonPropertyName("detalles")]
        public string Detalles { get; set; }
    }
}
