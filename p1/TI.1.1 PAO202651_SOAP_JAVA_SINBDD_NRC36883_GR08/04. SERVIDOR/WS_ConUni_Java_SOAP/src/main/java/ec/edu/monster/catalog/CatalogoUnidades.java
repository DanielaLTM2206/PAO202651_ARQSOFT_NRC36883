package ec.edu.monster.catalog;

import ec.edu.monster.model.CategoriaDTO;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * RNF-03: Persistencia en memoria.
 * RF-02 (CU-02, CU-03): Consulta de Catálogo y Unidades.
 * RF-04 (CU-06): Validación de Parámetros y Coherencia de Categoría.
 * RF-07 (CU-10): Gestión de Unidades no Soportadas.
 * 
 * @author GR08 - Arquitectura de Software
 */
public class CatalogoUnidades {

    public static final String CAT_LONGITUD = "Longitud";
    public static final String CAT_MASA = "Masa";
    public static final String CAT_TEMPERATURA = "Temperatura";

    // Mapeo en memoria de Categoría -> CategoriaDTO
    private static final Map<String, CategoriaDTO> catalogo = new LinkedHashMap<>();

    // Mapeo rápido de unidad normalizada -> Categoría
    private static final Map<String, String> unidadACategoria = new LinkedHashMap<>();

    static {
        // 1. Longitud (Base: metros)
        List<String> unidadesLongitud = Arrays.asList("centimetros", "metros", "pies", "yardas", "millas");
        CategoriaDTO catLongitud = new CategoriaDTO(CAT_LONGITUD, "metros", unidadesLongitud);
        catalogo.put(CAT_LONGITUD.toLowerCase(), catLongitud);
        for (String u : unidadesLongitud) {
            unidadACategoria.put(u.toLowerCase(), CAT_LONGITUD);
        }

        // 2. Masa (Base: kilogramos)
        List<String> unidadesMasa = Arrays.asList("gramos", "kilogramos", "onzas", "libras", "quintales");
        CategoriaDTO catMasa = new CategoriaDTO(CAT_MASA, "kilogramos", unidadesMasa);
        catalogo.put(CAT_MASA.toLowerCase(), catMasa);
        for (String u : unidadesMasa) {
            unidadACategoria.put(u.toLowerCase(), CAT_MASA);
        }

        // 3. Temperatura (Base: celsius)
        List<String> unidadesTemperatura = Arrays.asList("celsius", "fahrenheit", "kelvin", "newton", "reaumur");
        CategoriaDTO catTemperatura = new CategoriaDTO(CAT_TEMPERATURA, "celsius", unidadesTemperatura);
        catalogo.put(CAT_TEMPERATURA.toLowerCase(), catTemperatura);
        for (String u : unidadesTemperatura) {
            unidadACategoria.put(u.toLowerCase(), CAT_TEMPERATURA);
        }
    }

    /**
     * CU-02: Retorna el catálogo completo con todas las categorías y sus unidades asociadas.
     */
    public List<CategoriaDTO> obtenerCatalogoCompleto() {
        return new ArrayList<>(catalogo.values());
    }

    /**
     * CU-03: Retorna las unidades de una categoría seleccionada.
     */
    public CategoriaDTO obtenerPorCategoria(String categoria) {
        if (categoria == null) {
            return null;
        }
        return catalogo.get(categoria.trim().toLowerCase());
    }

    /**
     * Normaliza la cadena de texto de una unidad (minúsculas y sin acentos ni espacios extremos).
     */
    public static String normalizar(String texto) {
        if (texto == null) {
            return "";
        }
        String s = texto.trim().toLowerCase();
        s = s.replace("á", "a").replace("é", "e").replace("í", "i").replace("ó", "o").replace("ú", "u");
        // Alias comunes
        if (s.equals("centimetro") || s.equals("cm")) return "centimetros";
        if (s.equals("metro") || s.equals("m")) return "metros";
        if (s.equals("pie") || s.equals("ft")) return "pies";
        if (s.equals("yarda") || s.equals("yd")) return "yardas";
        if (s.equals("milla") || s.equals("mi")) return "millas";
        if (s.equals("gramo") || s.equals("g")) return "gramos";
        if (s.equals("kilogramo") || s.equals("kilo") || s.equals("kg")) return "kilogramos";
        if (s.equals("onza") || s.equals("oz")) return "onzas";
        if (s.equals("libra") || s.equals("lb")) return "libras";
        if (s.equals("quintal") || s.equals("q")) return "quintales";
        if (s.equals("c") || s.equals("centigrados") || s.equals("centigrado")) return "celsius";
        if (s.equals("f") || s.equals("farenheit")) return "fahrenheit";
        if (s.equals("k")) return "kelvin";
        if (s.equals("n")) return "newton";
        if (s.equals("re") || s.equals("r")) return "reaumur";
        return s;
    }

    /**
     * CU-10: Verifica si una unidad existe en el catálogo.
     */
    public boolean esUnidadSoportada(String unidad) {
        if (unidad == null) {
            return false;
        }
        return unidadACategoria.containsKey(normalizar(unidad));
    }

    /**
     * Obtiene el nombre canónico de la categoría a la que pertenece la unidad.
     */
    public String obtenerCategoriaDeUnidad(String unidad) {
        if (unidad == null) {
            return null;
        }
        return unidadACategoria.get(normalizar(unidad));
    }

    /**
     * RF-04 (CU-06): Verifica que ambas unidades correspondan a la misma categoría física.
     */
    public boolean pertenecenMismaCategoria(String unidadOrigen, String unidadDestino) {
        String cat1 = obtenerCategoriaDeUnidad(unidadOrigen);
        String cat2 = obtenerCategoriaDeUnidad(unidadDestino);
        return cat1 != null && cat1.equalsIgnoreCase(cat2);
    }
}
