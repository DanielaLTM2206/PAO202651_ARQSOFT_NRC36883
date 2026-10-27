using System;
using System.Collections.Generic;

namespace ConUni_Client_DotNet.Models
{
    public class RegistroHistorial
    {
        public string Id { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string TipoConversion { get; set; } = string.Empty;
        public double ValorOriginal { get; set; }
        public string UnidadOrigen { get; set; } = string.Empty;
        public double ValorResultado { get; set; }
        public string UnidadDestino { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
    }

    public class RespuestaHistorial
    {
        public bool Exitoso { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public List<RegistroHistorial> Registros { get; set; } = new List<RegistroHistorial>();
    }
}
