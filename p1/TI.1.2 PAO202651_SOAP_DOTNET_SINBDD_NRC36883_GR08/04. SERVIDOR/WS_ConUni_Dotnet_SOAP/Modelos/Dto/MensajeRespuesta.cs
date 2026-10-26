using System.Runtime.Serialization;
using ec.edu.monster.Contratos;

namespace ec.edu.monster.Modelos.Dto;

[DataContract(Namespace = ConstantesWS.Espacio)]
public class MensajeRespuesta
{
    [DataMember(Order = 1)]
    public string Mensaje { get; set; } = string.Empty;
}
