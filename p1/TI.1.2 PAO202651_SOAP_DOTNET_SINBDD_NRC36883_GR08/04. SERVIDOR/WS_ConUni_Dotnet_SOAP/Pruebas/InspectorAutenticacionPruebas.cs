using CoreWCF;
using CoreWCF.Channels;
using ec.edu.monster.Contratos;
using ec.edu.monster.Modelos.Dto;
using ec.edu.monster.Seguridad;
using Xunit;

namespace ec.edu.monster.Pruebas;

/// <summary>
/// Prueba el inspector con mensajes SOAP construidos en memoria, sin levantar el servidor.
/// </summary>
public class InspectorAutenticacionPruebas
{
    private const string Prefijo = ConstantesWS.Espacio + "/IWSConversorUnidades/";

    private readonly InspectorAutenticacion inspector = new();

    [Fact]
    public void LoginNoDeberiaExigirToken()
    {
        EstadoSolicitud estado = Recibir("Login", token: null);

        Assert.False(estado.TieneError);
    }

    [Fact]
    public void DeberiaRegistrarError401SinCabeceraToken()
    {
        EstadoSolicitud estado = Recibir("ConvertirLongitud", token: null);

        Assert.Equal(401, estado.Error!.Codigo);
        Assert.Contains("No se encontró el token", estado.Error.Mensaje);
    }

    [Fact]
    public void DeberiaRegistrarError401ConTokenInvalido()
    {
        EstadoSolicitud estado = Recibir("ConsultarHistorial", token: "token-falso");

        Assert.Equal(401, estado.Error!.Codigo);
        Assert.Contains("Token inválido", estado.Error.Mensaje);
    }

    [Fact]
    public void DeberiaAceptarTokenValidoEnLaCabecera()
    {
        string token = AdministradorTokens.GenerarToken();

        EstadoSolicitud estado = Recibir("ConsultarCatalogo", token);

        Assert.False(estado.TieneError);
    }

    [Fact]
    public void ElEstadoDeberiaViajarComoPropiedadDelMensaje()
    {
        Message mensaje = CrearMensaje("Login", token: null);

        object? estado = inspector.AfterReceiveRequest(ref mensaje, null!, null!);

        Assert.Same(estado, mensaje.Properties[EstadoSolicitud.Clave]);
    }

    [Fact]
    public void ConErrorDeberiaResponderConElFaultErrorServicio()
    {
        var estado = new EstadoSolicitud();
        estado.RegistrarError(401, "Credenciales incorrectas");
        Message respuesta = Message.CreateMessage(MessageVersion.Soap11, Prefijo + "LoginResponse");

        inspector.BeforeSendReply(ref respuesta, estado);

        Assert.True(respuesta.IsFault);
        ErrorServicio detalle = MessageFault.CreateFault(respuesta, int.MaxValue).GetDetail<ErrorServicio>();
        Assert.Equal(401, detalle.Codigo);
        Assert.Equal("Credenciales incorrectas", detalle.Mensaje);
    }

    [Fact]
    public void SinErrorNoDeberiaCambiarLaRespuesta()
    {
        Message respuesta = Message.CreateMessage(MessageVersion.Soap11, Prefijo + "LoginResponse");
        Message original = respuesta;

        inspector.BeforeSendReply(ref respuesta, new EstadoSolicitud());

        Assert.Same(original, respuesta);
        Assert.False(respuesta.IsFault);
    }

    [Theory]
    [InlineData(Prefijo + "Login", true)]
    [InlineData(Prefijo + "ConvertirMasa", false)]
    [InlineData(Prefijo + "LimpiarHistorial", false)]
    [InlineData(null, false)]
    public void SoloLoginDeberiaSerPublica(string? accion, bool esperado)
    {
        Assert.Equal(esperado, InspectorAutenticacion.EsOperacionPublica(accion));
    }

    private EstadoSolicitud Recibir(string operacion, string? token)
    {
        Message mensaje = CrearMensaje(operacion, token);
        return Assert.IsType<EstadoSolicitud>(inspector.AfterReceiveRequest(ref mensaje, null!, null!));
    }

    private static Message CrearMensaje(string operacion, string? token)
    {
        Message mensaje = Message.CreateMessage(MessageVersion.Soap11, Prefijo + operacion);
        if (token is not null)
        {
            mensaje.Headers.Add(MessageHeader.CreateHeader(ConstantesWS.CabeceraToken, ConstantesWS.Espacio, token));
        }
        return mensaje;
    }
}
