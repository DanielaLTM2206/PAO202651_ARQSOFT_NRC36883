using ec.edu.monster.Excepciones;
using ec.edu.monster.Seguridad;
using ec.edu.monster.Servicios;
using Xunit;

namespace ec.edu.monster.Pruebas;

/// <summary>
/// Prueba las operaciones del servicio directamente (sin HTTP). La validación del token
/// la hace InspectorAutenticacion antes de llegar aquí, y se prueba por separado.
/// </summary>
[Collection(EstadoCompartido.Nombre)]
public class WSConversorUnidadesPruebas : IDisposable
{
    private readonly WSConversorUnidades servicio = new();

    public WSConversorUnidadesPruebas()
    {
        ServicioHistorial.Limpiar();
        AdministradorCredenciales.Restablecer();
    }

    public void Dispose()
    {
        AdministradorCredenciales.Restablecer();
    }

    [Fact]
    public void LoginCorrectoDeberiaDevolverUnTokenValido()
    {
        string token = servicio.Login("Monster", "Monster9").Token;

        Assert.False(string.IsNullOrWhiteSpace(token));
        AdministradorTokens.ValidarToken(token);
    }

    [Fact]
    public void LoginIncorrectoDeberiaSerErrorDeAutenticacion()
    {
        Assert.Throws<ExcepcionAutenticacion>(() => servicio.Login("Monster", "mala"));
    }

    [Theory]
    [InlineData(null, "Monster9")]
    [InlineData("Monster", "")]
    public void LoginSinDatosDeberiaSerErrorDeValidacion(string? usuario, string? contrasena)
    {
        Assert.Throws<ExcepcionValidacion>(() => servicio.Login(usuario, contrasena));
    }

    [Fact]
    public void LaConversionDeberiaQuedarEnElHistorial()
    {
        double resultado = servicio.ConvertirLongitud(1000, "metro", "KILOMETRO").ValorConvertido;

        Assert.Equal(1.0, resultado, 6);
        var registro = Assert.Single(servicio.ConsultarHistorial());
        Assert.Equal("Longitud", registro.Tipo);
        Assert.Equal("METRO", registro.UnidadOrigen);
        Assert.Equal("KILOMETRO", registro.UnidadDestino);
    }

    [Fact]
    public void UnaConversionFallidaNoDeberiaQuedarEnElHistorial()
    {
        Assert.Throws<ExcepcionValidacion>(() => servicio.ConvertirMasa(-1, "GRAMO", "KILOGRAMO"));
        Assert.Throws<ExcepcionValidacion>(() => servicio.ConvertirTemperatura(10, "CELSIUS", "LIBRA"));

        Assert.Empty(servicio.ConsultarHistorial());
    }

    [Fact]
    public void ValorNoFinitoDeberiaSerRechazado()
    {
        Assert.Throws<ExcepcionValidacion>(() => servicio.ConvertirMasa(double.PositiveInfinity, "GRAMO", "ONZA"));
    }

    [Fact]
    public void LimpiarDeberiaVaciarElHistorial()
    {
        servicio.ConvertirTemperatura(100, "CELSIUS", "FAHRENHEIT");

        string mensaje = servicio.LimpiarHistorial().Mensaje;

        Assert.Equal("Historial de conversiones borrado", mensaje);
        Assert.Empty(servicio.ConsultarHistorial());
    }

    [Fact]
    public void ElCatalogoDeberiaTenerLasTresCategorias()
    {
        var catalogo = servicio.ConsultarCatalogo();

        Assert.Equal(["longitud", "masa", "temperatura"], catalogo.Select(c => c.Categoria));
        Assert.Contains("YARDA", catalogo[0].Unidades);
        Assert.Contains("ONZA", catalogo[1].Unidades);
        Assert.Equal(4, catalogo[2].Unidades.Count);
    }

    [Fact]
    public void CategoriaSeDeberiaNormalizar()
    {
        Assert.Equal("masa", servicio.ConsultarUnidadesPorCategoria("  MASA ").Categoria);
    }

    [Fact]
    public void CategoriaInexistenteDeberiaSerErrorDeValidacion()
    {
        Assert.Throws<ExcepcionValidacion>(() => servicio.ConsultarUnidadesPorCategoria("volumen"));
    }

    [Fact]
    public void CambiarContraseniaDeberiaPermitirIniciarSesionConLaNueva()
    {
        servicio.CambiarContrasenia("Monster9", "Nueva123");

        Assert.False(string.IsNullOrWhiteSpace(servicio.Login("Monster", "Nueva123").Token));
    }

    // Dentro de WCF los errores esperados se anotan en el estado en lugar de salir como excepción

    [Fact]
    public void CredencialesIncorrectasDeberianAnotarseComoError401SinLanzarExcepcion()
    {
        var estado = new EstadoSolicitud();

        var respuesta = WSConversorUnidades.EjecutarConEstado(estado, () => servicio.Login("Monster", "mala"));

        Assert.Null(respuesta);
        Assert.Equal(401, estado.Error!.Codigo);
        Assert.Equal("Credenciales incorrectas", estado.Error.Mensaje);
    }

    [Fact]
    public void DatoInvalidoDeberiaAnotarseComoError400SinLanzarExcepcion()
    {
        var estado = new EstadoSolicitud();

        WSConversorUnidades.EjecutarConEstado(estado, () => servicio.ConvertirMasa(-1, "GRAMO", "ONZA"));

        Assert.Equal(400, estado.Error!.Codigo);
        Assert.Equal("La masa no puede ser negativa", estado.Error.Mensaje);
        Assert.Empty(ServicioHistorial.Obtener());
    }

    [Fact]
    public void ConTokenInvalidoLaOperacionNoDeberiaEjecutarse()
    {
        var estado = new EstadoSolicitud();
        estado.RegistrarError(401, "Acceso denegado: Token inválido.");

        WSConversorUnidades.EjecutarConEstado(estado, () => servicio.LimpiarHistorial());
        WSConversorUnidades.EjecutarConEstado(estado, () => servicio.ConvertirLongitud(1, "METRO", "YARDA"));

        Assert.Empty(ServicioHistorial.Obtener());
        Assert.Equal("Acceso denegado: Token inválido.", estado.Error!.Mensaje);
    }

    [Fact]
    public void SinErroresDeberiaDevolverLaRespuestaNormal()
    {
        var estado = new EstadoSolicitud();

        var respuesta = WSConversorUnidades.EjecutarConEstado(estado,
                () => servicio.ConvertirTemperatura(100, "CELSIUS", "FAHRENHEIT"));

        Assert.False(estado.TieneError);
        Assert.Equal(212.0, respuesta.ValorConvertido, 6);
    }
}
