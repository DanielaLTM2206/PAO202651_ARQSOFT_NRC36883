using ec.edu.monster.Cliente;
using ec.edu.monster.Controlador;
using ec.edu.monster.Modelo;
using ec.edu.monster.Util;

namespace ec.edu.monster.Vista;

public class LoginVista
{
    private readonly MensajesModelo mensajes = new();

    public async Task IniciarAsync()
    {
        mensajes.MostrarBanner();

        using var clienteWs = new ClienteWSConversor();
        var loginControlador = new LoginControlador(clienteWs);
        mensajes.MostrarDetalle("  Servidor SOAP: " + loginControlador.UrlServidor);

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
                    return;
                }
                mensajes.MostrarAviso("  Sesión cerrada.");
            }
            catch (ExcepcionClienteWS ex)
            {
                mensajes.MostrarError(ex.Message);
                if (ex.EsErrorConexion && !EntradaUtil.LeerSiNo("  ¿Desea reintentar?"))
                {
                    return;
                }
            }
            catch (ArgumentException ex)
            {
                mensajes.MostrarError(ex.Message);
            }
        }
    }
}
