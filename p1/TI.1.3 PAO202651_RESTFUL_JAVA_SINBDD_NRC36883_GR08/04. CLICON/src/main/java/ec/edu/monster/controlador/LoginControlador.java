package ec.edu.monster.controlador;

import ec.edu.monster.cliente.ClienteWSConversor;
import ec.edu.monster.cliente.ContratoWSConversor;
import ec.edu.monster.modelo.LoginModelo;

public class LoginControlador {

    private final ContratoWSConversor clienteWs;
    private final String urlServidor;

    public LoginControlador() {
        this(new ClienteWSConversor());
    }

    public LoginControlador(ClienteWSConversor clienteWs) {
        this(clienteWs, clienteWs.getBaseUrl());
    }

    public LoginControlador(ContratoWSConversor clienteWs, String urlServidor) {
        this.clienteWs = clienteWs;
        this.urlServidor = urlServidor;
    }

    public String autenticar(LoginModelo loginModelo) {
        if (loginModelo == null || esVacio(loginModelo.getUsuario()) || esVacio(loginModelo.getContrasenia())) {
            throw new IllegalArgumentException("El usuario y la contrasenia son obligatorios.");
        }
        return clienteWs.login(loginModelo.getUsuario(), loginModelo.getContrasenia());
    }

    public String cambiarContrasenia(String contraseniaActual, String contraseniaNueva) {
        if (esVacio(contraseniaActual) || esVacio(contraseniaNueva)) {
            throw new IllegalArgumentException("La contrasenia actual y la nueva son obligatorias.");
        }
        if (contraseniaActual.equals(contraseniaNueva)) {
            throw new IllegalArgumentException("La nueva contrasenia debe ser diferente a la actual.");
        }
        return clienteWs.cambiarContrasenia(contraseniaActual, contraseniaNueva);
    }

    public ConversionControlador crearControladorConversion() {
        return new ConversionControlador(clienteWs);
    }

    public String getUrlServidor() {
        return urlServidor;
    }

    public void cerrarSesion() {
        clienteWs.cerrarSesion();
    }

    private static boolean esVacio(String texto) {
        return texto == null || texto.isBlank();
    }
}
