package ec.edu.monster.vista;

import ec.edu.monster.cliente.ExcepcionClienteWS;
import ec.edu.monster.controlador.LoginControlador;
import ec.edu.monster.modelo.LoginModelo;
import ec.edu.monster.modelo.MensajesModelo;
import ec.edu.monster.util.EntradaUtil;

public class LoginVista {

    private final MensajesModelo mensajes = new MensajesModelo();
    private LoginControlador loginControlador;

    public void iniciar() {
        mensajes.mostrarBanner();

        loginControlador = new LoginControlador();
        mensajes.mostrarDetalle("  Servidor REST: " + loginControlador.getUrlServidor());

        while (true) {
            mensajes.mostrarTitulo("INICIO DE SESION");
            mensajes.mostrarMensaje("  Ingrese sus credenciales para acceder al sistema.");
            mensajes.mostrarMensaje("");
            String usuario = EntradaUtil.leerTextoNoVacio("  > Usuario     : ");
            String contrasenia = EntradaUtil.leerTextoNoVacio("  > Contrasenia : ");

            try {
                String token = loginControlador.autenticar(new LoginModelo(usuario, contrasenia));
                mensajes.mostrarMensaje("");
                mensajes.mostrarExito("Bienvenido, " + usuario + ". Sesion iniciada correctamente.");
                mensajes.mostrarDetalle("  Token de sesion: " + token);

                boolean salir = new ConversionVista(
                        loginControlador.crearControladorConversion(), loginControlador).ejecutarMenu();

                loginControlador.cerrarSesion();
                if (salir) {
                    mensajes.mostrarMensaje("");
                    mensajes.mostrarExito("Sesion finalizada. Hasta pronto.");
                    return;
                }
                mensajes.mostrarAviso("  Sesion cerrada.");
            } catch (ExcepcionClienteWS ex) {
                mensajes.mostrarError(ex.getMessage());
                if (ex.esErrorConexion() && !EntradaUtil.leerSiNo("Desea reintentar?")) {
                    return;
                }
            } catch (IllegalArgumentException ex) {
                mensajes.mostrarError(ex.getMessage());
            }
        }
    }
}
