package ec.edu.monster.seguridad;

import ec.edu.monster.excepciones.ExcepcionValidacion;

public final class AdministradorCredenciales {

    private static final String USUARIO = "Monster";
    private static String contrasenia = "Monster9";

    private AdministradorCredenciales() {
    }

    public static synchronized boolean validarCredenciales(String usuario, String contraseniaIngresada) {
        return USUARIO.equals(usuario) && contrasenia.equals(contraseniaIngresada);
    }

    /**
     * Una contraseña actual incorrecta se trata como error de validación (400) y no
     * de autenticación (401): ante un 401 los clientes descartan el token y vuelven al login.
     */
    public static synchronized void cambiarContrasenia(String contraseniaActual, String contraseniaNueva) {
        if (!contrasenia.equals(contraseniaActual)) {
            throw new ExcepcionValidacion("La contraseña actual es incorrecta");
        }

        if (contraseniaNueva == null || contraseniaNueva.isBlank()) {
            throw new ExcepcionValidacion("La nueva contraseña es obligatoria");
        }

        if (contraseniaNueva.equals(contrasenia)) {
            throw new ExcepcionValidacion("La nueva contraseña debe ser diferente a la actual");
        }

        contrasenia = contraseniaNueva;
    }
}
