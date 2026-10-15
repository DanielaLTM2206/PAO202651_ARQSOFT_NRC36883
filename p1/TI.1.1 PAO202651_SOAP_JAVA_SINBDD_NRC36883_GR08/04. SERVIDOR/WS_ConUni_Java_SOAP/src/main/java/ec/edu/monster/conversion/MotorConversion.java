package ec.edu.monster.conversion;

import ec.edu.monster.catalog.CatalogoUnidades;
import ec.edu.monster.model.RespuestaConversion;
import ec.edu.monster.security.ResultadoValidacionToken;
import ec.edu.monster.security.ServicioSeguridad;

/**
 * RF-04: Validación de Parámetros y Coherencia de Entrada (CU-06).
 * RF-05: Ejecución del Motor Matemático de Conversión (CU-04).
 * RF-06: Construcción de Respuesta Estructurada Uniforme (CU-07).
 * RF-07: Gestión de Excepciones y Flujos Alternativos (CU-08 a CU-11).
 * RFP-02 / RFP-03: Procesamiento robusto del lado servidor que siempre retorna RespuestaConversion.
 * 
 * @author GR08 - Arquitectura de Software
 */
public class MotorConversion {

    private final ServicioSeguridad servicioSeguridad;
    private final CatalogoUnidades catalogoUnidades;
    private final ConversorLongitud conversorLongitud;
    private final ConversorMasa conversorMasa;
    private final ConversorTemperatura conversorTemperatura;

    public MotorConversion() {
        this.servicioSeguridad = new ServicioSeguridad();
        this.catalogoUnidades = new CatalogoUnidades();
        this.conversorLongitud = new ConversorLongitud();
        this.conversorMasa = new ConversorMasa();
        this.conversorTemperatura = new ConversorTemperatura();
    }

    public MotorConversion(ServicioSeguridad servicioSeguridad, CatalogoUnidades catalogoUnidades) {
        this.servicioSeguridad = servicioSeguridad;
        this.catalogoUnidades = catalogoUnidades;
        this.conversorLongitud = new ConversorLongitud();
        this.conversorMasa = new ConversorMasa();
        this.conversorTemperatura = new ConversorTemperatura();
    }

    /**
     * Procesa una conversión validando seguridad, parámetros, coherencia física y cálculo matemático.
     */
    public RespuestaConversion procesarConversion(Double valor, String unidadOrigen, String unidadDestino, String token) {
        // 1. RF-03 (CU-05), CU-08, CU-09: Validación Obligatoria de Token de Seguridad
        ResultadoValidacionToken valToken = servicioSeguridad.validarToken(token);
        if (!valToken.isValido()) {
            return RespuestaConversion.error(valToken.getCodigo(), valToken.getMensaje());
        }

        // 2. RF-04 (CU-06), CU-11: Validación de Parámetros no nulos
        if (valor == null || Double.isNaN(valor) || Double.isInfinite(valor)) {
            return RespuestaConversion.error("PARAM-001", "El valor numérico a convertir es inválido o no fue proporcionado (CU-11).");
        }

        if (unidadOrigen == null || unidadOrigen.trim().isEmpty() ||
            unidadDestino == null || unidadDestino.trim().isEmpty()) {
            return RespuestaConversion.error("PARAM-001", "Las unidades de origen y destino son obligatorias y no pueden estar vacías (CU-11).");
        }

        String origenLimpio = unidadOrigen.trim();
        String destinoLimpio = unidadDestino.trim();

        // 3. CU-10: Validación de que ambas unidades pertenezcan al catálogo
        if (!catalogoUnidades.esUnidadSoportada(origenLimpio)) {
            return RespuestaConversion.error("UNIT-001", "La unidad de origen '" + origenLimpio + "' no se encuentra soportada en el catálogo (CU-10).");
        }
        if (!catalogoUnidades.esUnidadSoportada(destinoLimpio)) {
            return RespuestaConversion.error("UNIT-001", "La unidad de destino '" + destinoLimpio + "' no se encuentra soportada en el catálogo (CU-10).");
        }

        // 4. RF-04 (CU-06), CU-11: Coherencia de Magnitud (Misma categoría física)
        String catOrigen = catalogoUnidades.obtenerCategoriaDeUnidad(origenLimpio);
        String catDestino = catalogoUnidades.obtenerCategoriaDeUnidad(destinoLimpio);

        if (catOrigen == null || catDestino == null || !catOrigen.equalsIgnoreCase(catDestino)) {
            return RespuestaConversion.error("PARAM-001", 
                    "Inconsistencia de magnitudes físicas: No es posible convertir entre categorías diferentes ('" 
                    + catOrigen + "' -> '" + catDestino + "') (CU-11).");
        }

        // 5. RF-05 (CU-04): Ejecución del Motor Matemático según categoría
        try {
            double resultado;
            switch (catOrigen) {
                case CatalogoUnidades.CAT_LONGITUD:
                    resultado = conversorLongitud.convertir(valor, origenLimpio, destinoLimpio);
                    break;
                case CatalogoUnidades.CAT_MASA:
                    resultado = conversorMasa.convertir(valor, origenLimpio, destinoLimpio);
                    break;
                case CatalogoUnidades.CAT_TEMPERATURA:
                    resultado = conversorTemperatura.convertir(valor, origenLimpio, destinoLimpio);
                    break;
                default:
                    return RespuestaConversion.error("UNIT-001", "Categoría no reconocida: " + catOrigen);
            }

            // 6. RF-06 (CU-07): Construcción de Respuesta Estructurada Homogénea Exitosa
            return RespuestaConversion.exitosa(resultado, catOrigen, origenLimpio, destinoLimpio, valor);

        } catch (Exception ex) {
            // Manejo de errores de cálculo o desbordamiento
            return RespuestaConversion.error("MATH-001", "Error durante el cálculo matemático de conversión: " + ex.getMessage());
        }
    }
}
