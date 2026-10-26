using ec.edu.monster.Excepciones;
using ec.edu.monster.Seguridad;
using Xunit;

namespace ec.edu.monster.Pruebas;

[Collection(EstadoCompartido.Nombre)]
public class AdministradorCredencialesPruebas : IDisposable
{
    public AdministradorCredencialesPruebas()
    {
        AdministradorCredenciales.Restablecer();
    }

    public void Dispose()
    {
        AdministradorCredenciales.Restablecer();
    }

    [Theory]
    [InlineData("Monster", "Monster9", true)]
    [InlineData("Monster", "otra", false)]
    [InlineData("monster", "Monster9", false)]
    public void DeberiaValidarLasCredenciales(string usuario, string contrasenia, bool esperado)
    {
        Assert.Equal(esperado, AdministradorCredenciales.ValidarCredenciales(usuario, contrasenia));
    }

    [Fact]
    public void DeberiaCambiarLaContrasenia()
    {
        AdministradorCredenciales.CambiarContrasenia("Monster9", "Nueva123");

        Assert.True(AdministradorCredenciales.ValidarCredenciales("Monster", "Nueva123"));
        Assert.False(AdministradorCredenciales.ValidarCredenciales("Monster", "Monster9"));
    }

    [Theory]
    [InlineData("incorrecta", "Nueva123", "La contraseña actual es incorrecta")]
    [InlineData("Monster9", " ", "La nueva contraseña es obligatoria")]
    [InlineData("Monster9", "Monster9", "La nueva contraseña debe ser diferente a la actual")]
    public void DeberiaRechazarCambioInvalidoSinModificarLaContrasenia(string actual, string nueva, string mensaje)
    {
        var error = Assert.Throws<ExcepcionValidacion>(() => AdministradorCredenciales.CambiarContrasenia(actual, nueva));

        Assert.Equal(mensaje, error.Message);
        Assert.True(AdministradorCredenciales.ValidarCredenciales("Monster", "Monster9"));
    }
}
