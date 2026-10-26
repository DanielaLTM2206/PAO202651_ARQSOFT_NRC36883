using System.Runtime.Serialization;
using ec.edu.monster.Contratos;

namespace ec.edu.monster.Modelos.Dto;

[DataContract(Namespace = ConstantesWS.Espacio)]
public class LoginRespuesta
{
    [DataMember(Order = 1)]
    public string Token { get; set; } = string.Empty;
}
