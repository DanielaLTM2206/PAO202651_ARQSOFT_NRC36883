package ec.edu.monster.modelos.utilidades.mapeadores;

import ec.edu.monster.excepciones.ExcepcionValidacion;
import ec.edu.monster.modelos.utilidades.enums.UnidadTemperatura;
import ec.edu.monster.modelos.utilidades.enums.UnidadMasa;
import ec.edu.monster.modelos.utilidades.enums.UnidadLongitud;

import java.util.Locale;

public class UnidadMapper {

    public static UnidadLongitud toLongitud(String unidad) {
        return convertir(UnidadLongitud.class, unidad, "longitud");
    }

    public static UnidadMasa toMasa(String unidad) {
        return convertir(UnidadMasa.class, unidad, "masa");
    }

    public static UnidadTemperatura toTemperatura(String unidad) {
        return convertir(UnidadTemperatura.class, unidad, "temperatura");
    }

    private static <E extends Enum<E>> E convertir(Class<E> tipo, String unidad, String categoria) {
        if (unidad == null || unidad.isBlank()) {
            throw new ExcepcionValidacion("La unidad de " + categoria + " es obligatoria");
        }
        try {
            return Enum.valueOf(tipo, unidad.trim().toUpperCase(Locale.ROOT));
        } catch (IllegalArgumentException e) {
            throw new ExcepcionValidacion("Unidad de " + categoria + " inválida: " + unidad);
        }
    }
}
