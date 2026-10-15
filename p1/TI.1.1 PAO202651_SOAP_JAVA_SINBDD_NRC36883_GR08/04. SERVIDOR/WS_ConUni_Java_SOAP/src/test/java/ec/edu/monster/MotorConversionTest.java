package ec.edu.monster;

import ec.edu.monster.model.RespuestaAutenticacion;
import ec.edu.monster.model.RespuestaConversion;
import ec.edu.monster.service.WSConversorUnidades;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

/**
 * Pruebas unitarias para validar todos los Requerimientos Funcionales (RF-01 a RF-07)
 * y flujos alternativos (CU-08 a CU-11).
 * 
 * @author GR08 - Arquitectura de Software
 */
public class MotorConversionTest {

    private WSConversorUnidades servicio;
    private String tokenValido;

    @BeforeEach
    public void setUp() {
        servicio = new WSConversorUnidades();
        RespuestaAutenticacion auth = servicio.autenticar("monster", "monster9");
        assertEquals("OK", auth.getEstado());
        assertEquals("200", auth.getCodigo());
        assertNotNull(auth.getToken());
        this.tokenValido = auth.getToken();
    }

    @Test
    public void testRF01_AutenticacionCredencialesInvalidas() {
        // CU-01: Credenciales incorrectas
        RespuestaAutenticacion auth = servicio.autenticar("usuario_falso", "clave_falsa");
        assertEquals("ERROR", auth.getEstado());
        assertEquals("AUTH-001", auth.getCodigo());
        assertNull(auth.getToken());
    }

    @Test
    public void testRF02_ConsultarCatalogo() {
        // CU-02: Consultar catálogo completo
        var res = servicio.consultarCatalogo(tokenValido);
        assertEquals("OK", res.getEstado());
        assertEquals("200", res.getCodigo());
        assertEquals(3, res.getCategorias().size());
    }

    @Test
    public void testRF03_ValidarTokenInvalido_CU08() {
        // CU-08: Token alterado o no coincidente
        RespuestaConversion res = servicio.convertir(10.0, "metros", "pies", "token-inventado-12345");
        assertEquals("ERROR", res.getEstado());
        assertEquals("AUTH-001", res.getCodigo());
        assertNull(res.getResultado());
    }

    @Test
    public void testRF04_InconsistenciaDeMagnitudes_CU11() {
        // CU-11: Magnitudes incompatibles (Longitud a Masa)
        RespuestaConversion res = servicio.convertir(10.0, "metros", "kilogramos", tokenValido);
        assertEquals("ERROR", res.getEstado());
        assertEquals("PARAM-001", res.getCodigo());
        assertNull(res.getResultado());
        assertTrue(res.getMensaje().contains("Inconsistencia de magnitudes"));
    }

    @Test
    public void testRF05_ConversionLongitudLineal() {
        // 1 metro = 100 centímetros
        RespuestaConversion res = servicio.convertirLongitud(1.0, "metros", "centimetros", tokenValido);
        assertEquals("OK", res.getEstado());
        assertEquals("200", res.getCodigo());
        assertNotNull(res.getResultado());
        assertEquals(100.0, res.getResultado(), 0.001);
    }

    @Test
    public void testRF05_ConversionMasaLineal() {
        // 1 kilogramo = 1000 gramos
        RespuestaConversion res = servicio.convertirMasa(1.0, "kilogramos", "gramos", tokenValido);
        assertEquals("OK", res.getEstado());
        assertEquals("200", res.getCodigo());
        assertNotNull(res.getResultado());
        assertEquals(1000.0, res.getResultado(), 0.001);
    }

    @Test
    public void testRF05_ConversionTemperaturaNoLineal() {
        // 0 Celsius = 32 Fahrenheit
        RespuestaConversion res = servicio.convertirTemperatura(0.0, "celsius", "fahrenheit", tokenValido);
        assertEquals("OK", res.getEstado());
        assertEquals("200", res.getCodigo());
        assertNotNull(res.getResultado());
        assertEquals(32.0, res.getResultado(), 0.001);

        // 100 Celsius = 373.15 Kelvin
        RespuestaConversion resK = servicio.convertirTemperatura(100.0, "celsius", "kelvin", tokenValido);
        assertEquals(373.15, resK.getResultado(), 0.001);
    }

    @Test
    public void testRF07_UnidadNoSoportada_CU10() {
        // CU-10: Unidad inexistente
        RespuestaConversion res = servicio.convertir(5.0, "parsecs", "metros", tokenValido);
        assertEquals("ERROR", res.getEstado());
        assertEquals("UNIT-001", res.getCodigo());
        assertNull(res.getResultado());
    }
}
