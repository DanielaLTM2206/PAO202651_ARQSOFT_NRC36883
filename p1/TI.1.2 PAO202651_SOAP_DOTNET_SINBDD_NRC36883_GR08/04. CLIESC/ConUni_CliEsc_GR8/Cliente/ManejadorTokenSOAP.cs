using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
// WinForms también tiene una clase Message: aquí se usa la del mensaje SOAP
using Message = System.ServiceModel.Channels.Message;

namespace ec.edu.monster.Cliente;

/// <summary>
/// Equivalente SOAP del ManejadorTokenRESTFUL: guarda el token de la sesión y, como inspector
/// de mensajes del cliente WCF, lo agrega a cada petición en la cabecera SOAP
/// &lt;Token xmlns="http://monster.edu.ec/ws"&gt;...&lt;/Token&gt;, que valida el InspectorAutenticacion del servidor.
/// </summary>
public class ManejadorTokenSOAP : IClientMessageInspector, IEndpointBehavior
{
    // Deben coincidir con ConstantesWS del servidor
    public const string NombreCabecera = "Token";
    public const string EspacioNombres = "http://monster.edu.ec/ws";

    private string? token;

    public void GuardarToken(string token)
    {
        this.token = token;
    }

    public string? ObtenerToken() => token;

    public bool HaySesionActiva => !string.IsNullOrWhiteSpace(token);

    public void LimpiarToken()
    {
        token = null;
    }

    // Se llama antes de cada operación protegida: sin token ni siquiera se envía la petición
    public void ExigirSesion()
    {
        if (!HaySesionActiva)
        {
            throw new ExcepcionClienteWS(401, "No hay sesión activa. Debe autenticarse primero.");
        }
    }

    public object? BeforeSendRequest(ref Message request, IClientChannel channel)
    {
        if (HaySesionActiva)
        {
            request.Headers.Add(MessageHeader.CreateHeader(NombreCabecera, EspacioNombres, token));
        }
        return null;
    }

    public void AfterReceiveReply(ref Message reply, object? correlationState)
    {
    }

    // Se registra como comportamiento del endpoint para que WCF lo ejecute en cada mensaje
    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
        clientRuntime.ClientMessageInspectors.Add(this);
    }

    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
    {
    }

    public void Validate(ServiceEndpoint endpoint)
    {
    }
}
