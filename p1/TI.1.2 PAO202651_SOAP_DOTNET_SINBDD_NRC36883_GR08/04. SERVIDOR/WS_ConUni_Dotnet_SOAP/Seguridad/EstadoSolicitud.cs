using CoreWCF;
using CoreWCF.Channels;
using ec.edu.monster.Modelos.Dto;

namespace ec.edu.monster.Seguridad;

/// <summary>
/// Resultado de procesar una solicitud SOAP. Lo crea InspectorAutenticacion al recibir el mensaje y lo
/// comparte con la operación: si al final queda un error (token inválido, credenciales o datos incorrectos),
/// el inspector responde con el fault ErrorServicio en lugar de la respuesta normal.
/// Así los errores esperados no salen del servicio como excepciones: el servidor sigue atendiendo
/// y Visual Studio no se detiene en ellos al depurar.
/// </summary>
public class EstadoSolicitud
{
    // Nombre de la propiedad del mensaje SOAP donde se guarda el estado de la solicitud
    public const string Clave = "ConUni.EstadoSolicitud";

    public ErrorServicio? Error { get; private set; }

    public bool TieneError => Error is not null;

    // Se conserva el primer error: es el que explica por qué falló la solicitud
    public void RegistrarError(int codigo, string mensaje)
    {
        Error ??= new ErrorServicio { Codigo = codigo, Mensaje = mensaje };
    }

    // Estado de la solicitud que se está atendiendo (null fuera de WCF, por ejemplo en las pruebas)
    public static EstadoSolicitud? Actual
    {
        get
        {
            MessageProperties? propiedades = OperationContext.Current?.IncomingMessageProperties;
            return propiedades is not null && propiedades.TryGetValue(Clave, out object? valor)
                    ? valor as EstadoSolicitud
                    : null;
        }
    }
}
