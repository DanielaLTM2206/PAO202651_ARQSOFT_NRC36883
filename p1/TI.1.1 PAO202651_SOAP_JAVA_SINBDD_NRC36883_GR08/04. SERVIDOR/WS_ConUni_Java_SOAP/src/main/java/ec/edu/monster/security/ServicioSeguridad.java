package ec.edu.monster.security;

import ec.edu.monster.model.RespuestaAutenticacion;
import java.time.Duration;
import java.time.Instant;
import java.util.Map;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;

/**
 * RNF-03: Persistencia en memoria.
 * RNF-05: Capa de Seguridad (security).
 * RF-01 (CU-01): Autenticación de Usuario y Emisión de Token.
 * RF-03 (CU-05): Validación Obligatoria de Token.
 * RF-07 (CU-08, CU-09): Gestión de Token Inválido y Sesión Expirada.
 * 
 * @author GR08 - Arquitectura de Software
 */
public class ServicioSeguridad {

    // Credenciales válidas para el actor Monster
    private static final String USUARIO_VALIDO = "monster";
    private static volatile String claveActual = "monster9";

    // Tiempo de vida del token (30 minutos según diseño de sesión)
    private static final Duration TOKEN_TTL = Duration.ofMinutes(30);

    // Registro de sesiones activas en memoria: token -> SesionInfo
    private static final Map<String, SesionInfo> sesionesActivas = new ConcurrentHashMap<>();

    private static final class SesionInfo {
        private final String usuario;
        private final Instant fechaCreacion;
        private Instant ultimaActividad;

        public SesionInfo(String usuario) {
            this.usuario = usuario;
            this.fechaCreacion = Instant.now();
            this.ultimaActividad = this.fechaCreacion;
        }

        public boolean estaExpirada() {
            return Duration.between(ultimaActividad, Instant.now()).compareTo(TOKEN_TTL) > 0;
        }

        public void renovarActividad() {
            this.ultimaActividad = Instant.now();
        }

        public String getUsuario() {
            return usuario;
        }
    }

    /**
     * RF-01 / CU-01: Autentica credenciales y genera token de seguridad.
     */
    public RespuestaAutenticacion autenticar(String usuario, String clave) {
        if (usuario == null || clave == null) {
            return RespuestaAutenticacion.error("Las credenciales (usuario y contraseña) no pueden ser nulas.");
        }

        String userTrim = usuario.trim();
        String passTrim = clave.trim();

        if (USUARIO_VALIDO.equals(userTrim) && claveActual.equals(passTrim)) {
            String token = UUID.randomUUID().toString();
            sesionesActivas.put(token, new SesionInfo(userTrim));
            return RespuestaAutenticacion.exitosa(token);
        } else {
            return RespuestaAutenticacion.error("Credenciales incorrectas. Acceso denegado (AUTH-001).");
        }
    }

    /**
     * RF-08: Cambio de contraseña centralizado en memoria.
     * Si un cliente cambia la contraseña, el cambio se refleja inmediatamente para todos los demás clientes.
     */
    public RespuestaAutenticacion cambiarContrasenia(String contraseniaActual, String contraseniaNueva, String token) {
        ResultadoValidacionToken val = validarToken(token);
        if (!val.isValido()) {
            return RespuestaAutenticacion.error(val.getMensaje());
        }

        if (contraseniaActual == null || contraseniaActual.trim().isEmpty() ||
            contraseniaNueva == null || contraseniaNueva.trim().isEmpty()) {
            return RespuestaAutenticacion.error("Las contraseñas no pueden estar vacías.");
        }

        String actualTrim = contraseniaActual.trim();
        String nuevaTrim = contraseniaNueva.trim();

        if (!claveActual.equals(actualTrim)) {
            return RespuestaAutenticacion.error("La contraseña actual es incorrecta (AUTH-002).");
        }

        if (claveActual.equals(nuevaTrim)) {
            return RespuestaAutenticacion.error("La nueva contraseña no puede ser idéntica a la actual.");
        }

        claveActual = nuevaTrim;
        return new RespuestaAutenticacion("OK", "200", "Contraseña cambiada exitosamente para todos los clientes.", token);
    }

    /**
     * RF-03 (CU-05), CU-08, CU-09: Valida vigencia y coherencia del token.
     */
    public ResultadoValidacionToken validarToken(String token) {
        if (token == null || token.trim().isEmpty()) {
            return ResultadoValidacionToken.invalido("Token de seguridad no proporcionado (AUTH-001).");
        }

        String tokenLimpio = token.trim();
        SesionInfo sesion = sesionesActivas.get(tokenLimpio);

        // CU-08: Token no coincide o no existe
        if (sesion == null) {
            return ResultadoValidacionToken.invalido("Token inválido o no reconocido por el servidor (AUTH-001).");
        }

        // CU-09: Token expirado por inactividad
        if (sesion.estaExpirada()) {
            sesionesActivas.remove(tokenLimpio);
            return ResultadoValidacionToken.expirado("La sesión del token ha expirado. Por favor autentíquese nuevamente (AUTH-001).");
        }

        // Token válido: se renueva la marca de actividad
        sesion.renovarActividad();
        return ResultadoValidacionToken.exitoso();
    }

    /**
     * Permite cerrar sesión y liberar el token de memoria.
     */
    public boolean cerrarSesion(String token) {
        if (token != null) {
            return sesionesActivas.remove(token.trim()) != null;
        }
        return false;
    }
}
