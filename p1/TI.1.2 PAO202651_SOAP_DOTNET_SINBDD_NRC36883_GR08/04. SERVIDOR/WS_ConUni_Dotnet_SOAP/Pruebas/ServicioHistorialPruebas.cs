using ec.edu.monster.Modelos.Dto;
using ec.edu.monster.Servicios;
using Xunit;

namespace ec.edu.monster.Pruebas;

[Collection(EstadoCompartido.Nombre)]
public class ServicioHistorialPruebas
{
    private static readonly DateTime Fecha = new(2026, 10, 18, 19, 28, 36, 123);

    public ServicioHistorialPruebas()
    {
        ServicioHistorial.Limpiar();
    }

    [Fact]
    public void DeberiaIniciarVacio()
    {
        Assert.Empty(ServicioHistorial.Obtener());
    }

    [Theory]
    [InlineData("Longitud", 1000, "METRO", 1, "KILOMETRO")]
    [InlineData("Masa", 1, "ONZA", 28.3495, "GRAMO")]
    [InlineData("Temperatura", 100, "CELSIUS", 212, "FAHRENHEIT")]
    public void DeberiaRegistrarLaConversion(string tipo, double valor, string origen, double resultado, string destino)
    {
        ServicioHistorial.Registrar(Fecha, tipo, valor, origen, resultado, destino);

        RegistroConversionRespuesta registro = ServicioHistorial.Obtener()[0];
        Assert.Equal("2026-10-18T19:28:36", registro.FechaHora);
        Assert.Equal(tipo, registro.Tipo);
        Assert.Equal(valor, registro.Valor, 6);
        Assert.Equal(origen, registro.UnidadOrigen);
        Assert.Equal(resultado, registro.Resultado, 6);
        Assert.Equal(destino, registro.UnidadDestino);
    }

    [Fact]
    public void DeberiaMostrarPrimeroLaMasReciente()
    {
        ServicioHistorial.Registrar(Fecha, "Longitud", 1, "METRO", 1.0936, "YARDA");
        ServicioHistorial.Registrar(Fecha, "Masa", 2, "GRAMO", 0.0705, "ONZA");
        ServicioHistorial.Registrar(Fecha, "Temperatura", 3, "CELSIUS", 276.15, "KELVIN");

        var tipos = ServicioHistorial.Obtener().Select(r => r.Tipo).ToList();
        Assert.Equal(["Temperatura", "Masa", "Longitud"], tipos);
    }

    [Fact]
    public void DeberiaConservarSoloLosRegistrosMasRecientes()
    {
        for (int i = 1; i <= ServicioHistorial.CapacidadMaxima + 5; i++)
        {
            ServicioHistorial.Registrar(Fecha, "Masa", i, "GRAMO", i, "KILOGRAMO");
        }

        var registros = ServicioHistorial.Obtener();
        Assert.Equal(ServicioHistorial.CapacidadMaxima, registros.Count);
        Assert.Equal(ServicioHistorial.CapacidadMaxima + 5, registros[0].Valor);
        Assert.Equal(6, registros[^1].Valor);
    }

    [Fact]
    public void DeberiaVaciarseAlLimpiar()
    {
        ServicioHistorial.Registrar(Fecha, "Longitud", 1, "METRO", 100, "CENTIMETRO");

        ServicioHistorial.Limpiar();

        Assert.Empty(ServicioHistorial.Obtener());
    }

    [Fact]
    public void LaListaDevueltaNoDeberiaPermitirModificarElHistorial()
    {
        ServicioHistorial.Registrar(Fecha, "Longitud", 1, "METRO", 100, "CENTIMETRO");

        var lista = (ICollection<RegistroConversionRespuesta>)ServicioHistorial.Obtener();
        Assert.Throws<NotSupportedException>(() => lista.Clear());
        Assert.Single(ServicioHistorial.Obtener());
    }
}
