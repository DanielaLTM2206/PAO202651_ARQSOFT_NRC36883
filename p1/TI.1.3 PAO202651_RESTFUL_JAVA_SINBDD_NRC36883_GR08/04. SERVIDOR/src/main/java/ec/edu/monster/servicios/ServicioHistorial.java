package ec.edu.monster.servicios;

import ec.edu.monster.modelos.dto.RegistroConversionRespuesta;

import java.time.LocalDateTime;
import java.time.temporal.ChronoUnit;
import java.util.ArrayDeque;
import java.util.Deque;
import java.util.List;

/**
 * Historial de conversiones compartido por todos los clientes (escritorio, web, móvil y consola).
 * Vive en memoria del servidor, igual que las credenciales y los tokens (no hay base de datos):
 * se pierde al reiniciar Payara. Guarda las más recientes primero, como máximo CAPACIDAD_MAXIMA.
 */
public final class ServicioHistorial {

    public static final int CAPACIDAD_MAXIMA = 100;

    private static final Deque<RegistroConversionRespuesta> registros = new ArrayDeque<>();

    private ServicioHistorial() {
    }

    public static void registrar(String tipo, double valor, String unidadOrigen, double resultado, String unidadDestino) {
        registrar(LocalDateTime.now(), tipo, valor, unidadOrigen, resultado, unidadDestino);
    }

    // La fecha se puede indicar en las pruebas para obtener registros predecibles
    public static synchronized void registrar(LocalDateTime fechaHora, String tipo, double valor,
                                              String unidadOrigen, double resultado, String unidadDestino) {
        String fecha = fechaHora.truncatedTo(ChronoUnit.SECONDS).toString();
        registros.addFirst(new RegistroConversionRespuesta(fecha, tipo, valor, unidadOrigen, resultado, unidadDestino));

        while (registros.size() > CAPACIDAD_MAXIMA) {
            registros.removeLast();
        }
    }

    public static synchronized List<RegistroConversionRespuesta> obtener() {
        return List.copyOf(registros);
    }

    public static synchronized void limpiar() {
        registros.clear();
    }
}
