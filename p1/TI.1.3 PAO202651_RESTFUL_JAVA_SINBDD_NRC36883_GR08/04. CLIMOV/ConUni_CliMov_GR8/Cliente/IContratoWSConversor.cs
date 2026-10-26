namespace ec.edu.monster.Cliente;

/// <summary>
/// Operaciones que expone el servicio RESTful WSConversorUnidades.
/// Los controladores dependen de esta interfaz y no de la implementación HTTP.
/// </summary>
public interface IContratoWSConversor
{
    // POST /seguridad/login
    Task<string> LoginAsync(string usuario, string contrasenia);

    // POST /seguridad/cambiar-contrasena
    Task<string> CambiarContraseniaAsync(string contraseniaActual, string contraseniaNueva);

    // POST /conversiones/longitud
    Task<double> ConvertirLongitudAsync(double valor, string unidadInicial, string unidadFinal);

    // POST /conversiones/masa
    Task<double> ConvertirMasaAsync(double valor, string unidadInicial, string unidadFinal);

    // POST /conversiones/temperatura
    Task<double> ConvertirTemperaturaAsync(double valor, string unidadInicial, string unidadFinal);

    // GET /unidades/{categoria}
    Task<IReadOnlyList<string>> ConsultarUnidadesAsync(string categoria);

    // GET /conversiones/historial (historial compartido por todos los clientes)
    Task<IReadOnlyList<Dto.RegistroConversionRespuesta>> ConsultarHistorialAsync();

    // DELETE /conversiones/historial
    Task<string> LimpiarHistorialAsync();

    void CerrarSesion();
}
