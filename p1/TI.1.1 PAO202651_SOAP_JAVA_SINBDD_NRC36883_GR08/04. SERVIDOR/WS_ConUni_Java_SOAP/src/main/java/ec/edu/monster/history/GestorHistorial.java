package ec.edu.monster.history;

import ec.edu.monster.model.RegistroHistorialDTO;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;

/**
 * Gestor centralizado en memoria del historial global de conversiones.
 * Al no contar con base de datos (SIN BDD), mantiene el registro unificado en memoria 
 * para garantizar la sincronización multi-canal entre todos los clientes (Web, Móvil, Escritorio, Consola).
 * 
 * Thread-safe para entornos concurrentes como Payara Server.
 * 
 * @author GR08 - Arquitectura de Software
 */
public class GestorHistorial {

    private static final GestorHistorial INSTANCIA = new GestorHistorial();
    private static final int MAX_REGISTROS = 100;
    private static final DateTimeFormatter FORMATO_FECHA = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");

    private final List<RegistroHistorialDTO> historial = Collections.synchronizedList(new ArrayList<>());
    private final AtomicInteger contadorId = new AtomicInteger(1);

    private GestorHistorial() {
    }

    public static GestorHistorial getInstance() {
        return INSTANCIA;
    }

    /**
     * Registra una conversión exitosa en la cima del historial global.
     */
    public RegistroHistorialDTO registrar(String categoria, double valorOriginal, String unidadOrigen, 
                                          double resultado, String unidadDestino) {
        int id = contadorId.getAndIncrement();
        String fechaHora = LocalDateTime.now().format(FORMATO_FECHA);
        
        RegistroHistorialDTO registro = new RegistroHistorialDTO(
                id, fechaHora, categoria, valorOriginal, unidadOrigen, resultado, unidadDestino
        );

        synchronized (historial) {
            historial.add(0, registro); // Más reciente primero
            if (historial.size() > MAX_REGISTROS) {
                historial.remove(historial.size() - 1);
            }
        }

        return registro;
    }

    /**
     * Retorna una instantánea inmutable del historial acumulado.
     */
    public List<RegistroHistorialDTO> obtenerHistorial() {
        synchronized (historial) {
            return new ArrayList<>(historial);
        }
    }

    /**
     * Limpia el historial acumulado.
     */
    public void limpiarHistorial() {
        synchronized (historial) {
            historial.clear();
        }
    }

    public int totalRegistros() {
        return historial.size();
    }
}
