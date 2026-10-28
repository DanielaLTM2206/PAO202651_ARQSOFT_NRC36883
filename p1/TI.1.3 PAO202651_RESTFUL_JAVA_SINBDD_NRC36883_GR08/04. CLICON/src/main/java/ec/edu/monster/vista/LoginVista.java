package ec.edu.monster.vista;

import ec.edu.monster.cliente.ClienteWSConversor;
import ec.edu.monster.cliente.ExcepcionClienteWS;
import ec.edu.monster.controlador.LoginControlador;
import ec.edu.monster.controlador.ServidorControlador;
import ec.edu.monster.modelo.LoginModelo;
import ec.edu.monster.modelo.MensajesModelo;
import ec.edu.monster.util.EntradaUtil;

public class LoginVista {

    private final MensajesModelo mensajes = new MensajesModelo();
    private final ServidorControlador servidorControlador = new ServidorControlador();

    public void iniciar() {
        mensajes.mostrarBanner();

        while (true) {
            // Se crea un cliente nuevo cada vez que se cambia la direccion del servidor
            LoginControlador loginControlador =
                    new LoginControlador(new ClienteWSConversor(servidorControlador.getUrlActual()));
            mensajes.mostrarDetalle("  Servidor REST: " + loginControlador.getUrlServidor());

            if (atenderInicioSesion(loginControlador)) {
                return;
            }
            new ServidorVista(servidorControlador).ejecutar();
        }
    }

    /**
     * @return true si el programa debe terminar; false si el usuario quiere configurar el servidor.
     */
    private boolean atenderInicioSesion(LoginControlador loginControlador) {
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
                    return true;
                }
                mensajes.mostrarAviso("  Sesion cerrada.");
            } catch (ExcepcionClienteWS ex) {
                mensajes.mostrarError(ex.getMessage());
                if (ex.esErrorConexion()) {
                    // Si el servidor esta en otra computadora, aqui se cambia su direccion
                    if (EntradaUtil.leerSiNo("Desea configurar la direccion del servidor?")) {
                        return false;
                    }
                    if (!EntradaUtil.leerSiNo("Desea reintentar?")) {
                        return true;
                    }
                }
            } catch (IllegalArgumentException ex) {
                mensajes.mostrarError(ex.getMessage());
            }
        }
    }
}
