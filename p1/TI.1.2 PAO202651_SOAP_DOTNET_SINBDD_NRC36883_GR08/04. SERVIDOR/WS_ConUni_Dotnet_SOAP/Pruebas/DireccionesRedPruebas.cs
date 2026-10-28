using System.Net;
using ec.edu.monster.Util;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class DireccionesRedPruebas
{
    [Fact]
    public void DeberiaArmarLaUrlDelServicioPorCadaIPv4DeLaRed()
    {
        IPAddress[] direcciones = [IPAddress.Parse("192.168.1.25"), IPAddress.Parse("10.0.0.7")];

        IReadOnlyList<string> urls = DireccionesRed.CrearUrlsServicio(direcciones, 5080);

        Assert.Equal(
            ["http://192.168.1.25:5080/WSConversorUnidades.svc", "http://10.0.0.7:5080/WSConversorUnidades.svc"],
            urls);
    }

    [Theory]
    [InlineData("127.0.0.1")]
    [InlineData("169.254.10.20")]
    [InlineData("::1")]
    [InlineData("fe80::1")]
    public void DeberiaDescartarDireccionesQueNoSirvenDesdeOtraComputadora(string direccion)
    {
        Assert.Empty(DireccionesRed.CrearUrlsServicio([IPAddress.Parse(direccion)], 5080));
    }

    [Fact]
    public void DeberiaNoRepetirDirecciones()
    {
        IPAddress ip = IPAddress.Parse("192.168.1.25");

        Assert.Single(DireccionesRed.CrearUrlsServicio([ip, ip], 5080));
    }

    [Theory]
    [InlineData("http://[::]:5080", 5080)]
    [InlineData("http://0.0.0.0:6000", 6000)]
    [InlineData("http://localhost:5080/", 5080)]
    public void DeberiaObtenerElPuertoDeLaDireccionDeEscucha(string direccion, int esperado)
    {
        Assert.Equal(esperado, DireccionesRed.ObtenerPuerto(direccion));
    }

    [Theory]
    [InlineData("http://localhost")]
    [InlineData("texto")]
    [InlineData("http://localhost:99999")]
    public void DeberiaDevolverNullSiNoHayPuerto(string direccion)
    {
        Assert.Null(DireccionesRed.ObtenerPuerto(direccion));
    }

    [Fact]
    public void DeberiaListarLasIPDeEstaComputadoraSinFallar()
    {
        IReadOnlyList<IPAddress> direcciones = DireccionesRed.ObtenerIPv4Locales();

        Assert.NotNull(direcciones);
    }
}
