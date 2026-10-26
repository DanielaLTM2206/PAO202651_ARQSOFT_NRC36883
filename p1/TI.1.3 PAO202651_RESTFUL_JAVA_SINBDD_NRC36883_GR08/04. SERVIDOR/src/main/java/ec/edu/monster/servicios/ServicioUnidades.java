package ec.edu.monster.servicios;

import ec.edu.monster.excepciones.ExcepcionValidacion;
import ec.edu.monster.modelos.utilidades.enums.UnidadLongitud;
import ec.edu.monster.modelos.utilidades.enums.UnidadMasa;
import ec.edu.monster.modelos.utilidades.enums.UnidadTemperatura;

import java.util.Arrays;
import java.util.List;
import java.util.Locale;

/**
 * Catálogo de unidades soportadas, obtenido directamente de los enums.
 */
public class ServicioUnidades {

    public static final List<String> CATEGORIAS = List.of("longitud", "masa", "temperatura");

    public List<String> obtenerUnidades(String categoria) {
        String clave = categoria == null ? "" : categoria.trim().toLowerCase(Locale.ROOT);

        return switch (clave) {
            case "longitud" -> nombres(UnidadLongitud.values());
            case "masa" -> nombres(UnidadMasa.values());
            case "temperatura" -> nombres(UnidadTemperatura.values());
            default -> throw new ExcepcionValidacion("Categoría no soportada: " + categoria
                    + ". Categorías válidas: " + String.join(", ", CATEGORIAS));
        };
    }

    private static List<String> nombres(Enum<?>[] unidades) {
        return Arrays.stream(unidades).map(Enum::name).toList();
    }
}
