using CoreWCF;
using CoreWCF.Channels;
using ec.edu.monster.Excepciones;
using ec.edu.monster.Modelos.Dto;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ManejadorErroresPruebas
{
    [Fact]
    public void ErrorDeValidacionDeberiaTenerCodigo400()
    {
        ErrorServicio error = ManejadorErrores.CrearError(new ExcepcionValidacion("La masa no puede ser negativa"));

        Assert.Equal(400, error.Codigo);
        Assert.Equal("La masa no puede ser negativa", error.Mensaje);
    }

    [Fact]
    public void ErrorDeAutenticacionDeberiaTenerCodigo401()
    {
        ErrorServicio error = ManejadorErrores.CrearError(new ExcepcionAutenticacion("Credenciales incorrectas"));

        Assert.Equal(401, error.Codigo);
        Assert.Equal("Credenciales incorrectas", error.Mensaje);
    }

    [Fact]
    public void MensajeSoapMalFormadoDeberiaTenerCodigo400()
    {
        ErrorServicio error = ManejadorErrores.CrearError(new CommunicationException("XML inválido"));

        Assert.Equal(400, error.Codigo);
        Assert.Equal("La solicitud SOAP no es válida", error.Mensaje);
    }

    [Fact]
    public void ErrorInesperadoNoDeberiaRevelarDetallesInternos()
    {
        ErrorServicio error = ManejadorErrores.CrearError(new NullReferenceException("detalle interno"));

        Assert.Equal(500, error.Codigo);
        Assert.Equal("Error interno del servidor", error.Mensaje);
    }

    [Fact]
    public void ElFaultDeberiaLlevarElDetalleErrorServicio()
    {
        var detalle = new ErrorServicio { Codigo = 401, Mensaje = "Acceso denegado: Token inválido." };

        Message mensaje = ManejadorErrores.CrearMensajeFault(detalle, MessageVersion.Soap11);
        MessageFault fault = MessageFault.CreateFault(mensaje, int.MaxValue);
        ErrorServicio leido = fault.GetDetail<ErrorServicio>();

        Assert.True(mensaje.IsFault);
        Assert.Equal("Acceso denegado: Token inválido.", fault.Reason.GetMatchingTranslation().Text);
        Assert.Equal(401, leido.Codigo);
        Assert.Equal(detalle.Mensaje, leido.Mensaje);
    }
}
