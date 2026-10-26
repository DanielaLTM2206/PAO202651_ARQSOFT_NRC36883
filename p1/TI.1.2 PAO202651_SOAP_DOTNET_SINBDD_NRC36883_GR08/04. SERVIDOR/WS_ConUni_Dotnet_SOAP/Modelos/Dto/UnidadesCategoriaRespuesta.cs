using System.Runtime.Serialization;
using ec.edu.monster.Contratos;

namespace ec.edu.monster.Modelos.Dto;

[DataContract(Namespace = ConstantesWS.Espacio)]
public class UnidadesCategoriaRespuesta
{
    [DataMember(Order = 1)]
    public string Categoria { get; set; } = string.Empty;

    [DataMember(Order = 2)]
    public List<string> Unidades { get; set; } = [];
}
