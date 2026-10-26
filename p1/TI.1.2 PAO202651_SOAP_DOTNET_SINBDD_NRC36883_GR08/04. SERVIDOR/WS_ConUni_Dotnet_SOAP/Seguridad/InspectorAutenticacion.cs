using System.Collections.ObjectModel;
using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Description;
using CoreWCF.Dispatcher;
using ec.edu.monster.Contratos;
using ec.edu.monster.Excepciones;
using ec.edu.monster.Modelos.Dto;

namespace ec.edu.monster.Seguridad;

/// <summary>
/// Equivalente SOAP del FiltroAutenticacion del proyecto RESTful: antes de ejecutar cualquier
/// operación lee la cabecera SOAP "Token" y la valida con AdministradorTokens.
/// Login es la única operación que no la exige. Si el token falta, es inválido o expiró, lo anota en el
/// EstadoSolicitud, la operación no se ejecuta y, al responder, se envía un fault de código 401.
/// Los errores de las operaciones (datos o credenciales incorrectas) también se responden aquí.
/// </summary>
public class InspectorAutenticacion : IDispatchMessageInspector, IServiceBehavior
{
    private const string OperacionPublica = nameof(IWSConversorUnidades.Login);

    public object? AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
    {
        // El estado viaja como propiedad del mensaje: la operación lo lee con EstadoSolicitud.Actual
        var estado = new EstadoSolicitud();
        request.Properties[EstadoSolicitud.Clave] = estado;

        if (!EsOperacionPublica(request.Headers.Action)
                && !AdministradorTokens.EsTokenValido(LeerToken(request.Headers), out string mensajeError))
        {
            estado.RegistrarError(ErrorServicio.Autenticacion, mensajeError);
        }
        return estado;
    }

    // Si la solicitud terminó con un error, la respuesta normal se reemplaza por el fault ErrorServicio
    public void BeforeSendReply(ref Message reply, object? correlationState)
    {
        if (reply is not null && correlationState is EstadoSolicitud { Error: ErrorServicio error })
        {
            reply = ManejadorErrores.CrearMensajeFault(error, reply.Version);
        }
    }

    // La acción SOAP termina en el nombre de la operación: http://monster.edu.ec/ws/IWSConversorUnidades/Login
    public static bool EsOperacionPublica(string? accion)
    {
        return accion is not null && accion.EndsWith("/" + OperacionPublica, StringComparison.Ordinal);
    }

    // Una cabecera Token mal formada se trata igual que una ausente
    public static string? LeerToken(MessageHeaders cabeceras)
    {
        int posicion = cabeceras.FindHeader(ConstantesWS.CabeceraToken, ConstantesWS.Espacio);
        if (posicion < 0)
        {
            return null;
        }
        try
        {
            return cabeceras.GetHeader<string>(posicion);
        }
        catch (Exception ex) when (ex is System.Runtime.Serialization.SerializationException or System.Xml.XmlException)
        {
            return null;
        }
    }

    // Se registra solo en el endpoint del contrato, no en el que publica el WSDL
    public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
        foreach (ChannelDispatcher canal in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>())
        {
            foreach (EndpointDispatcher endpoint in canal.Endpoints)
            {
                if (EsContratoDelServicio(endpoint))
                {
                    endpoint.DispatchRuntime.MessageInspectors.Add(this);
                }
            }
        }
    }

    internal static bool EsContratoDelServicio(EndpointDispatcher endpoint)
    {
        return endpoint.ContractName == nameof(IWSConversorUnidades)
                && endpoint.ContractNamespace == ConstantesWS.Espacio;
    }

    public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
    {
    }

    public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
    }
}
