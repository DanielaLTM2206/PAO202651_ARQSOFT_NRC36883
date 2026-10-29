package ec.edu.monster.modelo;

import java.time.LocalDateTime;

/**
 * Una conversion realizada con exito, tal como se guarda en el historial.
 */
public record RegistroConversion(
        LocalDateTime fechaHora,
        TipoConversion tipo,
        double valor,
        String unidadOrigen,
        double resultado,
        String unidadDestino) {
}
