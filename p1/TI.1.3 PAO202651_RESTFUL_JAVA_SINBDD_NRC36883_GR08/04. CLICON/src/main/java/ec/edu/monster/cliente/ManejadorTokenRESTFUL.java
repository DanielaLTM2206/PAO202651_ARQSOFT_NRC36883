package ec.edu.monster.cliente;

import java.net.http.HttpRequest;

/**
 * Equivalente REST del ManejadorTokenSOAP: guarda el token de la sesión y lo
 * adjunta a cada petición protegida en el encabezado "Authorization: Bearer".
 */
public class ManejadorTokenRESTFUL {

    private String token;

    public void guardarToken(String token) {
        this.token = token;
    }

    public String obtenerToken() {
        return token;
    }

    public boolean haySesionActiva() {
        return token != null && !token.isBlank();
    }

    public void limpiarToken() {
        this.token = null;
    }

    public HttpRequest.Builder adjuntarToken(HttpRequest.Builder peticion) {
        if (!haySesionActiva()) {
            throw new ExcepcionClienteWS(401, "No hay sesion activa. Debe autenticarse primero.");
        }
        return peticion.header("Authorization", "Bearer " + token);
    }
}
