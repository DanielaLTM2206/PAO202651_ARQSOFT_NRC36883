using ec.edu.monster.Servicios;
using ec.edu.monster.Models;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ServicioCuentaPruebas
{
    private readonly ContratoFalso clienteWs = new();
    private readonly ServicioCuenta servicio;

    public ServicioCuentaPruebas()
    {
        servicio = new ServicioCuenta(clienteWs);
    }

    [Fact]
    public async Task DeberiaAutenticarYDevolverToken()
    {
        string token = await servicio.AutenticarAsync(new LoginModelo("Monster", "Monster9"));

        Assert.Equal("token-de-prueba", token);
        Assert.Equal("Login", clienteWs.OperacionLlamada);
    }

    [Theory]
    [InlineData(null, "Monster9")]
    [InlineData("Monster", null)]
    [InlineData("  ", "Monster9")]
    [InlineData("Monster", "  ")]
    public async Task DeberiaRechazarCredencialesVacias(string? usuario, string? contrasenia)
    {
        await Assert.ThrowsAsync<ArgumentException>(
                () => servicio.AutenticarAsync(new LoginModelo(usuario, contrasenia)));

        Assert.Null(clienteWs.OperacionLlamada);
    }

    [Theory]
    [InlineData(null, "Nueva123")]
    [InlineData("Monster9", null)]
    [InlineData("  ", "Nueva123")]
    public async Task DeberiaRechazarCambioDeContraseniaIncompleto(string? actual, string? nueva)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => servicio.CambiarContraseniaAsync(actual, nueva));

        Assert.Null(clienteWs.OperacionLlamada);
    }

    [Fact]
    public async Task DeberiaRechazarNuevaContraseniaIgualALaActual()
    {
        var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => servicio.CambiarContraseniaAsync("Monster9", "Monster9"));

        Assert.Equal("La nueva contraseña debe ser diferente a la actual.", ex.Message);
        Assert.Null(clienteWs.OperacionLlamada);
    }

    [Fact]
    public void DeberiaCerrarSesionEnElCliente()
    {
        servicio.CerrarSesion();

        Assert.Equal("CerrarSesion", clienteWs.OperacionLlamada);
    }
}
