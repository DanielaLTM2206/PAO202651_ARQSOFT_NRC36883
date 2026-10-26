package ec.edu.monster.seguridad;

import ec.edu.monster.excepciones.ExcepcionAutenticacion;

import java.time.Duration;
import java.time.Instant;
import java.util.Map;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;

/**
 * Tokens activos en memoria. Cada token vence tras 30 minutos sin uso
 * (mismo valor que session-timeout en web.xml); cada uso válido renueva el plazo.
 */
public final class AdministradorTokens {

    private static final Duration DURACION_SESION = Duration.ofMinutes(30);

    // token -> instante de vencimiento
    private static final Map<String, Instant> tokensActivos = new ConcurrentHashMap<>();

    private AdministradorTokens() {
    }

    public static String generarToken() {
        Instant ahora = Instant.now();
        tokensActivos.values().removeIf(vencimiento -> vencimiento.isBefore(ahora));

        String token = UUID.randomUUID().toString();
        tokensActivos.put(token, ahora.plus(DURACION_SESION));
        return token;
    }

    public static void validarToken(String token) {
        if (token == null || token.isBlank()) {
            throw new ExcepcionAutenticacion("Acceso denegado: No se encontró el token. Inicie sesión primero.");
        }

        Instant vencimiento = tokensActivos.get(token);
        if (vencimiento == null) {
            throw new ExcepcionAutenticacion("Acceso denegado: Token inválido.");
        }

        Instant ahora = Instant.now();
        if (vencimiento.isBefore(ahora)) {
            tokensActivos.remove(token);
            throw new ExcepcionAutenticacion("Acceso denegado: La sesión ha expirado. Inicie sesión nuevamente.");
        }

        tokensActivos.put(token, ahora.plus(DURACION_SESION));
    }
}
