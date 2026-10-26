using System.Runtime.Serialization;
using ec.edu.monster.Contratos;

namespace ec.edu.monster.Modelos.Dto;

/// <summary>
/// Detalle del SOAP Fault que devuelve el servicio. Codigo sigue la convención HTTP del proyecto
/// RESTful para que los clientes reaccionen igual: 400 dato inválido, 401 sesión, 500 error interno.
/// </summary>
[DataContract(Namespace = ConstantesWS.Espacio)]
public class ErrorServicio
{
    public const int Validacion = 400;
    public const int Autenticacion = 401;
    public const int Interno = 500;

    [DataMember(Order = 1)]
    public int Codigo { get; set; }

    [DataMember(Order = 2)]
    public string Mensaje { get; set; } = string.Empty;
}
