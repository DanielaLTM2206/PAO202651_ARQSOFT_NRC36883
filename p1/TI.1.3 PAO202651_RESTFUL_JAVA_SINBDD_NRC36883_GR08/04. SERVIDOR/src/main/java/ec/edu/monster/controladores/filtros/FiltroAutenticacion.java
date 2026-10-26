package ec.edu.monster.controladores.filtros;

import ec.edu.monster.excepciones.ExcepcionAutenticacion;
import ec.edu.monster.seguridad.AdministradorTokens;

import jakarta.annotation.Priority;
import jakarta.ws.rs.Priorities;
import jakarta.ws.rs.container.ContainerRequestContext;
import jakarta.ws.rs.container.ContainerRequestFilter;
import jakarta.ws.rs.core.HttpHeaders;
import jakarta.ws.rs.ext.Provider;

/**
 * Equivalente REST del ManejadorAutenticacion (SOAPHandler) del servicio SOAP:
 * exige el header "Authorization: Bearer &lt;token&gt;" en los recursos marcados con {@link Asegurado}.
 * Si falla, la acción del controlador no se ejecuta y el ManejadorExcepciones responde 401.
 */
@Provider
@Asegurado
@Priority(Priorities.AUTHENTICATION)
public class FiltroAutenticacion implements ContainerRequestFilter {

    private static final String PREFIJO_BEARER = "Bearer ";

    @Override
    public void filter(ContainerRequestContext contexto) {

        String encabezado = contexto.getHeaderString(HttpHeaders.AUTHORIZATION);

        if (encabezado == null
                || !encabezado.regionMatches(true, 0, PREFIJO_BEARER, 0, PREFIJO_BEARER.length())) {
            throw new ExcepcionAutenticacion("Acceso denegado: No se encontró el token. Inicie sesión primero.");
        }

        String token = encabezado.substring(PREFIJO_BEARER.length()).trim();
        AdministradorTokens.validarToken(token);
    }
}
