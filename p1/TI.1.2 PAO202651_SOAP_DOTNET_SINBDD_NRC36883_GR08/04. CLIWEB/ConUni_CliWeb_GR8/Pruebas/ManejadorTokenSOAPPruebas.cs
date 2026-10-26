using System.ServiceModel.Channels;
using ec.edu.monster.Cliente;
using Xunit;

namespace ec.edu.monster.Pruebas;

/// <summary>
/// Prueba el inspector del cliente con mensajes SOAP construidos en memoria, sin enviarlos.
/// </summary>
public class ManejadorTokenSOAPPruebas
{
    private readonly AlmacenTokenMemoria almacen = new();
    private readonly ManejadorTokenSOAP manejador;

    public ManejadorTokenSOAPPruebas()
    {
        manejador = new ManejadorTokenSOAP(almacen);
    }

    [Fact]
    public void DeberiaAgregarLaCabeceraConElTokenDeLaSesion()
    {
        almacen.Token = "abc-123";
        Message mensaje = CrearMensaje();

        manejador.BeforeSendRequest(ref mensaje, null!);

        int posicion = mensaje.Headers.FindHeader(ManejadorTokenSOAP.NombreCabecera, ManejadorTokenSOAP.EspacioNombres);
        Assert.True(posicion >= 0);
        Assert.Equal("abc-123", mensaje.Headers.GetHeader<string>(posicion));
    }

    [Fact]
    public void NoDeberiaAgregarCabeceraSinSesion()
    {
        Message mensaje = CrearMensaje();

        manejador.BeforeSendRequest(ref mensaje, null!);

        Assert.Equal(-1, mensaje.Headers.FindHeader(ManejadorTokenSOAP.NombreCabecera, ManejadorTokenSOAP.EspacioNombres));
    }

    [Fact]
    public void ExigirSesionSinTokenDeberiaSerErrorDeAutenticacion()
    {
        var error = Assert.Throws<ExcepcionClienteWS>(() => manejador.ExigirSesion());

        Assert.True(error.EsErrorAutenticacion);
    }

    [Fact]
    public void LimpiarTokenDeberiaQuitarloDeLaSesion()
    {
        manejador.GuardarToken("abc-123");

        manejador.LimpiarToken();

        Assert.Null(almacen.Token);
        Assert.False(manejador.HaySesionActiva);
    }

    private static Message CrearMensaje()
    {
        return Message.CreateMessage(MessageVersion.Soap11, ManejadorTokenSOAP.EspacioNombres + "/IWSConversorUnidades/ConvertirMasa");
    }
}
