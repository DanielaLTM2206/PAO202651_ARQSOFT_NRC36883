using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace UniversalConverter.Client.Models
{
    // Modelo para la petición REST. Lo adaptaremos para SOAP.
    public class ConversionRequest
    {
        [JsonPropertyName("valor")]
        public string Valor { get; set; }

        [JsonPropertyName("unidadOrigen")]
        public string UnidadOrigen { get; set; }

        [JsonPropertyName("unidadDestino")]
        public string UnidadDestino { get; set; }

        // Propiedad adicional para saber qué tipo de conversión es
        public ConversionType TipoConversion { get; set; }
    }

    public enum ConversionType
    {
        Longitud,
        Masa,
        Temperatura
    }
}
