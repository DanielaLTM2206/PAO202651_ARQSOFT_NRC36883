using ec.edu.monster.Util;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class DireccionServidorPruebas
{
    [Theory]
    [InlineData("192.168.1.25", "http://192.168.1.25:5080/WSConversorUnidades.svc")]
    [InlineData("  192.168.1.25  ", "http://192.168.1.25:5080/WSConversorUnidades.svc")]
    [InlineData("192.168.1.25:6000", "http://192.168.1.25:6000/WSConversorUnidades.svc")]
    [InlineData("http://192.168.1.25", "http://192.168.1.25:5080/WSConversorUnidades.svc")]
    [InlineData("http://192.168.1.25/", "http://192.168.1.25:5080/WSConversorUnidades.svc")]
    [InlineData("PC-STEVEN", "http://pc-steven:5080/WSConversorUnidades.svc")]
    [InlineData("localhost", "http://localhost:5080/WSConversorUnidades.svc")]
    [InlineData("http://192.168.1.25:5080/WSConversorUnidades.svc", "http://192.168.1.25:5080/WSConversorUnidades.svc")]
    [InlineData("http://192.168.1.25:5080/WSConversorUnidades.svc/", "http://192.168.1.25:5080/WSConversorUnidades.svc")]
    [InlineData("https://servidor.local/otra/ruta.svc", "https://servidor.local/otra/ruta.svc")]
    public void DeberiaCompletarLaDireccion(string entrada, string esperado)
    {
        Assert.Equal(esperado, DireccionServidor.Completar(entrada));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ftp://192.168.1.25")]
    [InlineData("192.168.1")]
    [InlineData("192.168.1.256")]
    [InlineData("192.168.1.25.7")]
    [InlineData("http://")]
    [InlineData("http://mi servidor")]
    [InlineData("192.168.1.25:abc")]
    public void DeberiaRechazarDireccionesInvalidas(string? entrada)
    {
        var error = Assert.Throws<ArgumentException>(() => DireccionServidor.Completar(entrada));
        Assert.Contains("192.168.1.25", error.Message);
    }

    [Theory]
    [InlineData("http://192.168.1.25:5080/WSConversorUnidades.svc", true)]
    [InlineData("https://localhost/WSConversorUnidades.svc", true)]
    [InlineData("192.168.1.25", false)]
    [InlineData("ftp://192.168.1.25", false)]
    [InlineData(null, false)]
    public void DeberiaValidarUrls(string? url, bool esperado)
    {
        Assert.Equal(esperado, DireccionServidor.EsUrlValida(url));
    }
}
