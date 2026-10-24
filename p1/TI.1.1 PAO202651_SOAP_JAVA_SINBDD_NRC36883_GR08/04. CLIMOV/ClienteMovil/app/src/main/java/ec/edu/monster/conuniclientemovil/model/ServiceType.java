package ec.edu.monster.conuniclientemovil.model;

import java.util.Arrays;
import java.util.Collections;
import java.util.List;

/**
 * Tipos de servicio de conversión disponibles.
 * Cada tipo define sus unidades válidas.
 */
public enum ServiceType {

    LONGITUD("Longitud",
            Arrays.asList("centimetros", "metros", "pies", "yardas", "millas")),

    MASA("Masa",
            Arrays.asList("gramos", "kilogramos", "onzas", "libras", "quintales")),

    TEMPERATURA("Temperatura",
            Arrays.asList("fahrenheit", "kelvin", "celsius", "newton", "reaumur"));

    private final String displayName;
    private final List<String> units;

    ServiceType(String displayName, List<String> units) {
        this.displayName = displayName;
        this.units = Collections.unmodifiableList(units);
    }

    public String getDisplayName() {
        return displayName;
    }

    public List<String> getUnits() {
        return units;
    }

    /**
     * Busca un ServiceType por su nombre para mostrar.
     */
    public static ServiceType fromDisplayName(String name) {
        for (ServiceType type : values()) {
            if (type.displayName.equals(name)) {
                return type;
            }
        }
        throw new IllegalArgumentException("Servicio no soportado: " + name);
    }
}
