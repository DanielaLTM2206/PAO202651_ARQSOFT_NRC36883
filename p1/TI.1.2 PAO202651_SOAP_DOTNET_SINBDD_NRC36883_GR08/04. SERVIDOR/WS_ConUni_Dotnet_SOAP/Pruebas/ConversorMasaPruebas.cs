using ec.edu.monster.Excepciones;
using ec.edu.monster.Modelos.Utilidades;
using ec.edu.monster.Modelos.Utilidades.Enums;
using Xunit;

namespace ec.edu.monster.Pruebas;

public class ConversorMasaPruebas
{
    private readonly ConversorMasa conversor = new();

    [Theory]
    [InlineData(1000, UnidadMasa.GRAMO, UnidadMasa.KILOGRAMO, 1.0)]
    [InlineData(1, UnidadMasa.KILOGRAMO, UnidadMasa.GRAMO, 1000.0)]
    [InlineData(500, UnidadMasa.GRAMO, UnidadMasa.KILOGRAMO, 0.5)]
    [InlineData(1000, UnidadMasa.MILIGRAMO, UnidadMasa.GRAMO, 1.0)]
    [InlineData(1, UnidadMasa.TONELADA, UnidadMasa.KILOGRAMO, 1000.0)]
    [InlineData(2500, UnidadMasa.KILOGRAMO, UnidadMasa.TONELADA, 2.5)]
    [InlineData(1, UnidadMasa.ONZA, UnidadMasa.GRAMO, 28.3495)]
    [InlineData(28.3495, UnidadMasa.GRAMO, UnidadMasa.ONZA, 1.0)]
    [InlineData(7, UnidadMasa.GRAMO, UnidadMasa.GRAMO, 7.0)]
    public void DeberiaConvertirCorrectamente(double valor, UnidadMasa origen, UnidadMasa destino, double esperado)
    {
        double resultado = conversor.Convertir(valor, origen, destino);

        Assert.Equal(esperado, resultado, 4);
    }

    [Theory]
    [InlineData(-1, UnidadMasa.GRAMO, UnidadMasa.KILOGRAMO)]
    [InlineData(-28.3495, UnidadMasa.ONZA, UnidadMasa.GRAMO)]
    [InlineData(-0.001, UnidadMasa.TONELADA, UnidadMasa.TONELADA)]
    public void DeberiaRechazarMasaNegativa(double valor, UnidadMasa origen, UnidadMasa destino)
    {
        Assert.Throws<ExcepcionValidacion>(() => conversor.Convertir(valor, origen, destino));
    }
}
