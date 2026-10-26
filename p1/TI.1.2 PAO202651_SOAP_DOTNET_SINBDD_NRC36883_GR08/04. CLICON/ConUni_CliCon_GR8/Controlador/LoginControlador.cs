using ec.edu.monster.Cliente;
using ec.edu.monster.Modelo;

namespace ec.edu.monster.Controlador;

public class LoginControlador
{
    private readonly IContratoWSConversor clienteWs;

    public string UrlServidor { get; }

    public LoginControlador() : this(new ClienteWSConversor())
    {
    }

    public LoginControlador(ClienteWSConversor clienteWs) : this(clienteWs, clienteWs.UrlServicio)
    {
    }

    public LoginControlador(IContratoWSConversor clienteWs, string urlServidor)
    {
        this.clienteWs = clienteWs;
        UrlServidor = urlServidor;
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

    public ConversionControlador CrearControladorConversion() => new(clienteWs);

    public void CerrarSesion()
    {
        clienteWs.CerrarSesion();
    }

    private static bool EsVacio(string? texto) => string.IsNullOrWhiteSpace(texto);
}
