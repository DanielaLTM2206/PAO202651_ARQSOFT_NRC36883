using ec.edu.monster.Cliente;
using ReferenciaServicio;

namespace ec.edu.monster.Pruebas;

/// <summary>
/// Implementación de prueba del contrato: registra qué operación se llamó y con qué datos,
/// sin hacer peticiones SOAP.
/// </summary>
internal class ContratoFalso : IContratoWSConversor
{
    public string? OperacionLlamada { get; private set; }
    public double ValorRecibido { get; private set; }
    public string? UnidadInicialRecibida { get; private set; }
    public string? UnidadFinalRecibida { get; private set; }
    public double Resultado { get; set; } = 42.0;

    public Task<string> LoginAsync(string usuario, string contrasenia)
    {
        OperacionLlamada = "Login";
        return Task.FromResult("token-de-prueba");
    }

    public Task<string> CambiarContraseniaAsync(string contraseniaActual, string contraseniaNueva)
    {
        OperacionLlamada = "CambiarContrasenia";
        return Task.FromResult("Contraseña actualizada correctamente");
    }

    public Task<double> ConvertirLongitudAsync(double valor, string unidadInicial, string unidadFinal)
        => Registrar("ConvertirLongitud", valor, unidadInicial, unidadFinal);

    public Task<double> ConvertirMasaAsync(double valor, string unidadInicial, string unidadFinal)
        => Registrar("ConvertirMasa", valor, unidadInicial, unidadFinal);

    public Task<double> ConvertirTemperaturaAsync(double valor, string unidadInicial, string unidadFinal)
        => Registrar("ConvertirTemperatura", valor, unidadInicial, unidadFinal);

    public Task<IReadOnlyList<string>> ConsultarUnidadesAsync(string categoria)
    {
        OperacionLlamada = "ConsultarUnidades:" + categoria;
        return Task.FromResult<IReadOnlyList<string>>(["A", "B"]);
    }

    // Historial que "guarda el servidor" en las pruebas
    public List<RegistroConversionRespuesta> HistorialServidor { get; } = new();

    public Task<IReadOnlyList<RegistroConversionRespuesta>> ConsultarHistorialAsync()
    {
        OperacionLlamada = "ConsultarHistorial";
        return Task.FromResult<IReadOnlyList<RegistroConversionRespuesta>>(HistorialServidor.ToList());
    }

    public Task<string> LimpiarHistorialAsync()
    {
        OperacionLlamada = "LimpiarHistorial";
        HistorialServidor.Clear();
        return Task.FromResult("Historial de conversiones borrado");
    }

    public void CerrarSesion()
    {
        OperacionLlamada = "CerrarSesion";
    }

    private Task<double> Registrar(string operacion, double valor, string unidadInicial, string unidadFinal)
    {
        OperacionLlamada = operacion;
        ValorRecibido = valor;
        UnidadInicialRecibida = unidadInicial;
        UnidadFinalRecibida = unidadFinal;
        return Task.FromResult(Resultado);
    }
}
