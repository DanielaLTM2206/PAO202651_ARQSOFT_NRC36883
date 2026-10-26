using ec.edu.monster.Cliente;
using ec.edu.monster.Models;

namespace ec.edu.monster.Servicios;

/// <summary>
/// Inicio de sesión y cambio de contraseña: valida los datos y llama al servicio SOAP.
/// </summary>
public class ServicioCuenta
{
    private readonly IContratoWSConversor clienteWs;

    public ServicioCuenta(IContratoWSConversor clienteWs)
    {
        this.clienteWs = clienteWs;
    }

    public async Task<string> AutenticarAsync(LoginModelo loginModelo)
    {
        if (loginModelo is null || EsVacio(loginModelo.Usuario) || EsVacio(loginModelo.Contrasenia))
        {
            throw new ArgumentException("El usuario y la contraseña son obligatorios.");
        }
        return await clienteWs.LoginAsync(loginModelo.Usuario!.Trim(), loginModelo.Contrasenia!);
    }

    public async Task<string> CambiarContraseniaAsync(string? contraseniaActual, string? contraseniaNueva)
    {
        if (EsVacio(contraseniaActual) || EsVacio(contraseniaNueva))
        {
            throw new ArgumentException("La contraseña actual y la nueva son obligatorias.");
        }
        if (contraseniaActual == contraseniaNueva)
        {
            throw new ArgumentException("La nueva contraseña debe ser diferente a la actual.");
        }
        return await clienteWs.CambiarContraseniaAsync(contraseniaActual!, contraseniaNueva!);
    }

    public void CerrarSesion()
    {
        clienteWs.CerrarSesion();
    }

    private static bool EsVacio(string? texto) => string.IsNullOrWhiteSpace(texto);
}
