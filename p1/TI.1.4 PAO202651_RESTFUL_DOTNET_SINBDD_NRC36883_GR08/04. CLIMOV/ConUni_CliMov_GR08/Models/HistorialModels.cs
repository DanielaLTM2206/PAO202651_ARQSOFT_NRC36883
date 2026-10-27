using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UniversalConverter.Client.Models
{
    public class RegistroHistorial
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("tipoConversion")]
        public string TipoConversion { get; set; } = string.Empty;

        [JsonPropertyName("valorOriginal")]
        public double ValorOriginal { get; set; }

        [JsonPropertyName("unidadOrigen")]
        public string UnidadOrigen { get; set; } = string.Empty;

        [JsonPropertyName("valorResultado")]
        public double ValorResultado { get; set; }

        [JsonPropertyName("unidadDestino")]
        public string UnidadDestino { get; set; } = string.Empty;

        [JsonPropertyName("factorConversion")]
        public double FactorConversion { get; set; }

        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }

        [JsonPropertyName("usuario")]
        public string Usuario { get; set; } = string.Empty;
    }

    public class HistorialResponse
    {
        [JsonPropertyName("exitoso")]
        public bool Exitoso { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("registros")]
        public List<RegistroHistorial> Registros { get; set; } = new();

        [JsonPropertyName("mensaje")]
        public string? Mensaje { get; set; }
    }
}
