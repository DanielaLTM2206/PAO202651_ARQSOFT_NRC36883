using System.Runtime.Serialization;
using ec.edu.monster.Contratos;

namespace ec.edu.monster.Modelos.Dto;

/// <summary>
/// Una conversión realizada con éxito, tal como se guarda en el historial compartido.
/// FechaHora va en formato ISO-8601 (por ejemplo, 2026-10-18T19:28:36).
/// </summary>
[DataContract(Namespace = ConstantesWS.Espacio)]
public class RegistroConversionRespuesta
{
    [DataMember(Order = 1)]
    public string FechaHora { get; set; } = string.Empty;

    [DataMember(Order = 2)]
    public string Tipo { get; set; } = string.Empty;

    [DataMember(Order = 3)]
    public double Valor { get; set; }

    [DataMember(Order = 4)]
    public string UnidadOrigen { get; set; } = string.Empty;

    [DataMember(Order = 5)]
    public double Resultado { get; set; }

    [DataMember(Order = 6)]
    public string UnidadDestino { get; set; } = string.Empty;
}
