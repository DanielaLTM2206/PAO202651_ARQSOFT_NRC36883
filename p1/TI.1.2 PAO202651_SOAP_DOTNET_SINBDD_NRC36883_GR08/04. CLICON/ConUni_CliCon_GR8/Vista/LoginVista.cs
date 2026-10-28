using ec.edu.monster.Cliente;
using ec.edu.monster.Controlador;
using ec.edu.monster.Modelo;
using ec.edu.monster.Util;

namespace ec.edu.monster.Vista;

public class LoginVista
{
    private readonly MensajesModelo mensajes = new();
    private readonly ServidorControlador servidorControlador = new();

    public async Task IniciarAsync()
    {
        mensajes.MostrarBanner();

        while (true)
        {
            // Se crea un cliente nuevo cada vez que se cambia la dirección del servidor
            using var clienteWs = new ClienteWSConversor(servidorControlador.UrlActual);
            var loginControlador = new LoginControlador(clienteWs);
            mensajes.MostrarDetalle("  Servidor SOAP: " + loginControlador.UrlServidor);

            if (await AtenderInicioSesionAsync(loginControlador))
            {
                return;
            }
            await new ServidorVista(servidorControlador).EjecutarAsync();
        }
    }

    /// <returns>true si el programa debe terminar; false si el usuario quiere configurar el servidor.</returns>
    private async Task<bool> AtenderInicioSesionAsync(LoginControlador loginControlador)
    {
        while (true)
        {
            mensajes.MostrarTitulo("INICIO DE SESIÓN");
            mensajes.MostrarMensaje("  Ingrese sus credenciales para acceder al sistema.");
            mensajes.MostrarMensaje("");
            string usuario = EntradaUtil.LeerTextoNoVacio("  > Usuario     : ");
            string contrasenia = EntradaUtil.LeerTextoNoVacio("  > Contraseña  : ");

            try
            {
                string token = await loginControlador.AutenticarAsync(new LoginModelo(usuario, contrasenia));
                mensajes.MostrarMensaje("");
                mensajes.MostrarExito($"Bienvenido, {usuario}. Sesión iniciada correctamente.");
                mensajes.MostrarDetalle("  Token de sesión: " + token);

                bool salir = await new ConversionVista(
                        loginControlador.CrearControladorConversion(), loginControlador).EjecutarMenuAsync();

                loginControlador.CerrarSesion();
                if (salir)
                {
                    mensajes.MostrarMensaje("");
                    mensajes.MostrarExito("Sesión finalizada. Hasta pronto.");
                    return true;
                }
                mensajes.MostrarAviso("  Sesión cerrada.");
            }
            catch (ExcepcionClienteWS ex)
            {
                mensajes.MostrarError(ex.Message);
                if (ex.EsErrorConexion)
                {
                    // Si el servidor está en otra computadora, aquí se cambia su dirección
                    if (EntradaUtil.LeerSiNo("  ¿Desea configurar la dirección del servidor?"))
                    {
                        return false;
                    }
                    if (!EntradaUtil.LeerSiNo("  ¿Desea reintentar?"))
                    {
                        return true;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                mensajes.MostrarError(ex.Message);
            }
        }
    }
}
