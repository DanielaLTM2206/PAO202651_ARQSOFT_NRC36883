using System.Runtime.Serialization;
using ec.edu.monster.Contratos;

namespace ec.edu.monster.Modelos.Dto;

[DataContract(Namespace = ConstantesWS.Espacio)]
public class ConversionRespuesta
{
    [DataMember(Order = 1)]
    public double ValorConvertido { get; set; }
}
