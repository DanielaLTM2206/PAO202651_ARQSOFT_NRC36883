using System.Collections.ObjectModel;
using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Description;
using CoreWCF.Dispatcher;
using ec.edu.monster.Contratos;
using ec.edu.monster.Modelos.Dto;
using ec.edu.monster.Seguridad;

namespace ec.edu.monster.Excepciones;

/// <summary>
/// Equivalente SOAP del ManejadorExcepciones del proyecto RESTful: traduce cualquier excepción
/// al fault tipado ErrorServicio {Codigo, Mensaje} declarado en el contrato con [FaultContract].
/// </summary>
public class ManejadorErrores : IErrorHandler, IServiceBehavior
{
    public const string AccionFault = ConstantesWS.Espacio + "/ErrorServicio";

    private readonly ILogger<ManejadorErrores> registro;

    public ManejadorErrores(ILogger<ManejadorErrores> registro)
    {
        this.registro = registro;
    }

    public static ErrorServicio CrearError(Exception error)
    {
        return error switch
        {
            ExcepcionValidacion => Nuevo(ErrorServicio.Validacion, error.Message),
            ExcepcionAutenticacion => Nuevo(ErrorServicio.Autenticacion, error.Message),
            // Errores del propio WCF al leer el mensaje (XML mal formado, "abc" en un número, acción desconocida)
            FaultException or CommunicationException => Nuevo(ErrorServicio.Validacion, "La solicitud SOAP no es válida"),
            _ => Nuevo(ErrorServicio.Interno, "Error interno del servidor")
        };
    }

    public static Message CrearMensajeFault(ErrorServicio detalle, MessageVersion version)
    {
        var codigo = new FaultCode(detalle.Codigo >= ErrorServicio.Interno ? "Receiver" : "Sender");
        var excepcion = new FaultException<ErrorServicio>(detalle, new FaultReason(detalle.Mensaje), codigo, AccionFault);
        return Message.CreateMessage(version, excepcion.CreateMessageFault(), AccionFault);
    }

    public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
    {
        fault = CrearMensajeFault(CrearError(error), version);
    }

    public bool HandleError(Exception error)
    {
        if (CrearError(error).Codigo >= ErrorServicio.Interno)
        {
            registro.LogError(error, "Error no controlado en el servicio SOAP");
        }
        return true;
    }

    // Se registra solo en el canal del contrato, no en el que publica el WSDL
    public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
        foreach (ChannelDispatcher canal in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>())
        {
            if (canal.Endpoints.Any(InspectorAutenticacion.EsContratoDelServicio))
            {
                canal.ErrorHandlers.Add(this);
            }
        }
    }

    public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
    {
    }

    public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
    }

    private static ErrorServicio Nuevo(int codigo, string mensaje) => new() { Codigo = codigo, Mensaje = mensaje };
}
