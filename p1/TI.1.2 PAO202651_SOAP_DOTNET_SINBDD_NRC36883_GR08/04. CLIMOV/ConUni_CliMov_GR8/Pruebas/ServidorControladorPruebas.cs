using ec.edu.monster.Cliente;
using ec.edu.monster.Controlador;
using ec.edu.monster.Modelo;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ServidorControladorPruebas
{
    private readonly ManejadorHttpFalso manejador = new();
    private readonly ServidorControlador controlador;

    public ServidorControladorPruebas()
    {
        controlador = new ServidorControlador(new ProbadorConexion(manejador));
    }

    [Fact]
    public void DeberiaCompletarLaDireccion()
    {
        Assert.Equal("http://10.0.2.2:5080/WSConversorUnidades.svc", controlador.CompletarUrl("10.0.2.2"));
    }

    [Fact]
    public async Task DeberiaProbarLaDireccionCompleta()
    {
        ResultadoConexion resultado = await controlador.ProbarAsync("192.168.1.25");

        Assert.True(resultado.Exitosa);
        Assert.Equal("http://192.168.1.25:5080/WSConversorUnidades.svc?wsdl", manejador.UltimaUrl!.ToString());
    }

    [Fact]
    public async Task DeberiaRechazarUnaDireccionInvalidaSinLlamarAlServidor()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => controlador.ProbarAsync("192.168.1"));
        Assert.Null(manejador.UltimaUrl);
    }
}
