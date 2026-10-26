using ec.edu.monster.Excepciones;
using ec.edu.monster.Seguridad;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class AdministradorTokensPruebas
{
    [Fact]
    public void ElTokenGeneradoDeberiaSerValido()
    {
        string token = AdministradorTokens.GenerarToken();

        AdministradorTokens.ValidarToken(token);
    }

    [Fact]
    public void CadaLoginDeberiaGenerarUnTokenDistinto()
    {
        Assert.NotEqual(AdministradorTokens.GenerarToken(), AdministradorTokens.GenerarToken());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void DeberiaRechazarTokenAusente(string? token)
    {
        var error = Assert.Throws<ExcepcionAutenticacion>(() => AdministradorTokens.ValidarToken(token));
        Assert.Contains("No se encontró el token", error.Message);
    }

    [Fact]
    public void DeberiaRechazarTokenInventado()
    {
        var error = Assert.Throws<ExcepcionAutenticacion>(() => AdministradorTokens.ValidarToken("token-falso"));
        Assert.Contains("Token inválido", error.Message);
    }

    [Fact]
    public void DeberiaRechazarTokenExpiradoYOlvidarlo()
    {
        string token = AdministradorTokens.GenerarToken();
        AdministradorTokens.Expirar(token);

        var error = Assert.Throws<ExcepcionAutenticacion>(() => AdministradorTokens.ValidarToken(token));
        Assert.Contains("expirado", error.Message);

        // Ya se eliminó: el segundo intento lo trata como inválido
        error = Assert.Throws<ExcepcionAutenticacion>(() => AdministradorTokens.ValidarToken(token));
        Assert.Contains("Token inválido", error.Message);
    }
}
