using ec.edu.monster.Controlador;
using ec.edu.monster.Modelo;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class LoginControladorPruebas
{
    private readonly ContratoFalso contrato = new();
    private readonly LoginControlador controlador;

    public LoginControladorPruebas()
    {
        controlador = new LoginControlador(contrato, "http://servidor-de-prueba");
    }

    [Fact]
    public async Task DeberiaAutenticarYDevolverToken()
    {
        string token = await controlador.AutenticarAsync(new LoginModelo("Monster", "Monster9"));

        Assert.Equal("token-de-prueba", token);
        Assert.Equal("Login", contrato.OperacionLlamada);
    }

    [Theory]
    [InlineData(null, "Monster9")]
    [InlineData("Monster", null)]
    [InlineData("  ", "Monster9")]
    [InlineData("Monster", "")]
    public async Task DeberiaRechazarCredencialesVacias(string? usuario, string? contrasenia)
    {
        await Assert.ThrowsAsync<ArgumentException>(
                () => controlador.AutenticarAsync(new LoginModelo(usuario, contrasenia)));
        Assert.Null(contrato.OperacionLlamada);
    }

    [Theory]
    [InlineData(null, "Nueva123")]
    [InlineData("Monster9", " ")]
    public async Task DeberiaRechazarCambioDeContraseniaIncompleto(string? actual, string? nueva)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => controlador.CambiarContraseniaAsync(actual, nueva));
    }

    [Fact]
    public async Task DeberiaRechazarNuevaContraseniaIgualALaActual()
    {
        var error = await Assert.ThrowsAsync<ArgumentException>(
                () => controlador.CambiarContraseniaAsync("Monster9", "Monster9"));

        Assert.Equal("La nueva contraseña debe ser diferente a la actual.", error.Message);
    }

    [Fact]
    public async Task DeberiaCambiarLaContrasenia()
    {
        string mensaje = await controlador.CambiarContraseniaAsync("Monster9", "Nueva123");

        Assert.Equal("Contraseña actualizada correctamente", mensaje);
        Assert.Equal("CambiarContrasenia", contrato.OperacionLlamada);
    }

    [Fact]
    public void DeberiaCerrarSesionEnElCliente()
    {
        controlador.CerrarSesion();

        Assert.Equal("CerrarSesion", contrato.OperacionLlamada);
    }
}
