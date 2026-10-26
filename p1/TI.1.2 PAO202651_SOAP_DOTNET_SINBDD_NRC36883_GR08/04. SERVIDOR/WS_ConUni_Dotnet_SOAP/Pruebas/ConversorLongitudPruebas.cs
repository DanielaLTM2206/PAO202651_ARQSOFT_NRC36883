using ec.edu.monster.Excepciones;
using ec.edu.monster.Modelos.Utilidades;
using ec.edu.monster.Modelos.Utilidades.Enums;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ConversorLongitudPruebas
{
    private readonly ConversorLongitud conversor = new();

    [Theory]
    [InlineData(1000, UnidadLongitud.METRO, UnidadLongitud.KILOMETRO, 1.0)]
    [InlineData(1, UnidadLongitud.KILOMETRO, UnidadLongitud.METRO, 1000.0)]
    [InlineData(100, UnidadLongitud.CENTIMETRO, UnidadLongitud.METRO, 1.0)]
    [InlineData(1000, UnidadLongitud.MILIMETRO, UnidadLongitud.METRO, 1.0)]
    [InlineData(1, UnidadLongitud.METRO, UnidadLongitud.MILIMETRO, 1000.0)]
    [InlineData(1, UnidadLongitud.YARDA, UnidadLongitud.METRO, 0.9144)]
    [InlineData(1, UnidadLongitud.KILOMETRO, UnidadLongitud.YARDA, 1093.6133)]
    [InlineData(91.44, UnidadLongitud.CENTIMETRO, UnidadLongitud.YARDA, 1.0)]
    [InlineData(5, UnidadLongitud.METRO, UnidadLongitud.METRO, 5.0)]
    [InlineData(0, UnidadLongitud.KILOMETRO, UnidadLongitud.MILIMETRO, 0.0)]
    public void DeberiaConvertirCorrectamente(double valor, UnidadLongitud origen, UnidadLongitud destino, double esperado)
    {
        double resultado = conversor.Convertir(valor, origen, destino);

        Assert.Equal(esperado, resultado, 4);
    }

    [Theory]
    [InlineData(-1, UnidadLongitud.METRO, UnidadLongitud.KILOMETRO)]
    [InlineData(-0.5, UnidadLongitud.YARDA, UnidadLongitud.METRO)]
    [InlineData(-1000, UnidadLongitud.MILIMETRO, UnidadLongitud.MILIMETRO)]
    public void DeberiaRechazarLongitudNegativa(double valor, UnidadLongitud origen, UnidadLongitud destino)
    {
        Assert.Throws<ExcepcionValidacion>(() => conversor.Convertir(valor, origen, destino));
    }
}
