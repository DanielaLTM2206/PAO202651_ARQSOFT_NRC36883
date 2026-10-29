using System;
using System.Collections.Generic;

namespace WS_ConUni_DotNet_REST.Models
{
    public class UnidadDTO
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Simbolo { get; set; } = string.Empty;
        public double FactorBase { get; set; }
    }

    public class CategoriaDTO
    {
        public string Categoria { get; set; } = string.Empty;
        public string UnidadBase { get; set; } = string.Empty;
        public List<UnidadDTO> Unidades { get; set; } = new List<UnidadDTO>();
    }

    public class RespuestaCatalogo
    {
        public bool Exitoso { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public List<CategoriaDTO> Categorias { get; set; } = new List<CategoriaDTO>();

        public static RespuestaCatalogo Ok(List<CategoriaDTO> categorias)
        {
            return new RespuestaCatalogo
            {
                Exitoso = true,
                Codigo = "CAT_OK",
                Mensaje = "Catálogo consultado con éxito",
                Categorias = categorias
            };
        }

        public static RespuestaCatalogo Error(string codigo, string mensaje)
        {
            return new RespuestaCatalogo
            {
                Exitoso = false,
                Codigo = codigo,
                Mensaje = mensaje,
                Categorias = new List<CategoriaDTO>()
            };
        }
    }

    public class RegistroHistorialDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string TipoConversion { get; set; } = string.Empty;
        public double ValorOriginal { get; set; }
        public string UnidadOrigen { get; set; } = string.Empty;
        public double ValorResultado { get; set; }
        public string UnidadDestino { get; set; } = string.Empty;
        public string Usuario { get; set; } = "monster";
    }

    public class RespuestaHistorial
    {
        public bool Exitoso { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public List<RegistroHistorialDTO> Registros { get; set; } = new List<RegistroHistorialDTO>();

        public static RespuestaHistorial Ok(List<RegistroHistorialDTO> registros)
        {
            return new RespuestaHistorial
            {
                Exitoso = true,
                Codigo = "HIST_OK",
                Mensaje = "Historial consultado con éxito",
                Registros = registros
            };
        }

        public static RespuestaHistorial Error(string codigo, string mensaje)
        {
            return new RespuestaHistorial
            {
                Exitoso = false,
                Codigo = codigo,
                Mensaje = mensaje,
                Registros = new List<RegistroHistorialDTO>()
            };
        }
    }
}
