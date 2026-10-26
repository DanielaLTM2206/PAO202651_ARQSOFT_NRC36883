namespace ec.edu.monster.Cliente;

/// <summary>
/// Operaciones del servicio SOAP WSConversorUnidades que usa el cliente de consola.
/// Los controladores dependen de esta interfaz y no del proxy WCF generado.
/// La consola no muestra el historial, por eso no incluye esas operaciones.
/// </summary>
public interface IContratoWSConversor
{
    // Operación SOAP Login (la única pública)
    Task<string> LoginAsync(string usuario, string contrasenia);

    // Operación SOAP CambiarContrasenia
    Task<string> CambiarContraseniaAsync(string contraseniaActual, string contraseniaNueva);

    // Operaciones SOAP ConvertirLongitud, ConvertirMasa y ConvertirTemperatura
    Task<double> ConvertirLongitudAsync(double valor, string unidadInicial, string unidadFinal);

    Task<double> ConvertirMasaAsync(double valor, string unidadInicial, string unidadFinal);

    Task<double> ConvertirTemperaturaAsync(double valor, string unidadInicial, string unidadFinal);

    // Operación SOAP ConsultarUnidadesPorCategoria
    Task<IReadOnlyList<string>> ConsultarUnidadesAsync(string categoria);

    void CerrarSesion();
}
