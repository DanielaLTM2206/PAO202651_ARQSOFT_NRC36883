using ec.edu.monster.Util;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class DireccionServidorPruebas
{
    [Theory]
    [InlineData("192.168.1.25", "http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api")]
    [InlineData("  192.168.1.25  ", "http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api")]
    [InlineData("192.168.1.25:6000", "http://192.168.1.25:6000/WS_ConUni_Java_RESTFUL/api")]
    [InlineData("http://192.168.1.25", "http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api")]
    [InlineData("http://192.168.1.25/", "http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api")]
    [InlineData("PC-STEVEN", "http://pc-steven:8080/WS_ConUni_Java_RESTFUL/api")]
    [InlineData("localhost", "http://localhost:8080/WS_ConUni_Java_RESTFUL/api")]
    [InlineData("http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api", "http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api")]
    [InlineData("http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api/", "http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api")]
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
    [InlineData("http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api", true)]
    [InlineData("https://localhost/WS_ConUni_Java_RESTFUL/api", true)]
    [InlineData("192.168.1.25", false)]
    [InlineData("ftp://192.168.1.25", false)]
    [InlineData(null, false)]
    public void DeberiaValidarUrls(string? url, bool esperado)
    {
        Assert.Equal(esperado, DireccionServidor.EsUrlValida(url));
    }
}
