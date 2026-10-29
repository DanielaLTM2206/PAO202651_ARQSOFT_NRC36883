package ec.edu.monster.pruebas;

import ec.edu.monster.cliente.ClienteWSConversor;
import ec.edu.monster.cliente.ExcepcionClienteWS;
import ec.edu.monster.cliente.dto.RegistroConversionRespuesta;
import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpServer;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.io.IOException;
import java.io.OutputStream;
import java.net.InetSocketAddress;
import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.concurrent.atomic.AtomicReference;

import static org.junit.jupiter.api.Assertions.*;

/**
 * Prueba el cliente contra un servidor HTTP local que imita las respuestas del servicio RESTful.
 */
public class ClienteWSConversorTest {

    private HttpServer servidor;
    private ClienteWSConversor cliente;

    private final AtomicReference<String> cuerpoRecibido = new AtomicReference<>();
    private final AtomicReference<String> autorizacionRecibida = new AtomicReference<>();
    private final AtomicReference<String> metodoHistorial = new AtomicReference<>();

    @BeforeEach
    void iniciarServidor() throws IOException {
        servidor = HttpServer.create(new InetSocketAddress("localhost", 0), 0);

        servidor.createContext("/api/seguridad/login", intercambio -> {
            String cuerpo = leerCuerpo(intercambio);
            if (cuerpo.contains("\"Contrasena\":\"Monster9\"")) {
                responder(intercambio, 200, "{\"Token\":\"abc-123\"}");
            } else {
                responder(intercambio, 401, "{\"Mensaje\":\"Credenciales incorrectas\"}");
            }
        });
        servidor.createContext("/api/conversiones/longitud", intercambio -> {
            cuerpoRecibido.set(leerCuerpo(intercambio));
            autorizacionRecibida.set(intercambio.getRequestHeaders().getFirst("Authorization"));
            responder(intercambio, 200, "{\"ValorConvertido\":1.0}");
        });
        servidor.createContext("/api/conversiones/masa", intercambio ->
                responder(intercambio, 400, "{\"Mensaje\":\"Unidad de masa inválida: PIE\"}"));
        servidor.createContext("/api/unidades/temperatura", intercambio ->
                responder(intercambio, 200,
                        "{\"Categoria\":\"temperatura\",\"Unidades\":[\"CELSIUS\",\"FAHRENHEIT\",\"KELVIN\",\"RANKINE\"]}"));

        servidor.createContext("/api/conversiones/historial", intercambio -> {
            metodoHistorial.set(intercambio.getRequestMethod());
            autorizacionRecibida.set(intercambio.getRequestHeaders().getFirst("Authorization"));
            if ("DELETE".equals(intercambio.getRequestMethod())) {
                responder(intercambio, 200, "{\"Mensaje\":\"Historial de conversiones borrado\"}");
            } else {
                responder(intercambio, 200, "[{\"FechaHora\":\"2026-10-18T19:28:36\",\"Tipo\":\"Longitud\","
                        + "\"Valor\":1000.0,\"UnidadOrigen\":\"METRO\",\"Resultado\":1.0,\"UnidadDestino\":\"KILOMETRO\"}]");
            }
        });

        servidor.start();
        cliente = new ClienteWSConversor("http://localhost:" + servidor.getAddress().getPort() + "/api/");
    }

    @AfterEach
    void detenerServidor() {
        servidor.stop(0);
    }

    @Test
    void deberiaGuardarElTokenAlIniciarSesion() {
        String token = cliente.login("Monster", "Monster9");

        assertEquals("abc-123", token);
        assertEquals("abc-123", cliente.getTokenSesion());
    }

    @Test
    void deberiaEnviarJsonYTokenBearerAlConvertir() {
        cliente.login("Monster", "Monster9");

        double resultado = cliente.convertirLongitud(1000, "METRO", "KILOMETRO");

        assertEquals(1.0, resultado, 0.000001);
        assertEquals("Bearer abc-123", autorizacionRecibida.get());
        assertEquals("{\"Valor\":1000.0,\"UnidadOrigen\":\"METRO\",\"UnidadDestino\":\"KILOMETRO\"}",
                cuerpoRecibido.get());
    }

    @Test
    void deberiaLeerElCatalogoDeUnidades() {
        cliente.login("Monster", "Monster9");

        List<String> unidades = cliente.consultarUnidades("temperatura");

        assertEquals(List.of("CELSIUS", "FAHRENHEIT", "KELVIN", "RANKINE"), unidades);
    }

    @Test
    void deberiaTraducirCredencialesIncorrectasA401() {
        ExcepcionClienteWS ex = assertThrows(ExcepcionClienteWS.class,
                () -> cliente.login("Monster", "otra"));

        assertTrue(ex.esErrorAutenticacion());
        assertEquals("Credenciales incorrectas", ex.getMessage());
    }

    @Test
    void deberiaPropagarElMensajeDeErrorDelServidor() {
        cliente.login("Monster", "Monster9");

        ExcepcionClienteWS ex = assertThrows(ExcepcionClienteWS.class,
                () -> cliente.convertirMasa(1, "PIE", "GRAMO"));

        assertEquals(400, ex.getCodigoEstado());
        assertEquals("Unidad de masa inválida: PIE", ex.getMessage());
    }

    @Test
    void deberiaExigirSesionAntesDeConvertir() {
        ExcepcionClienteWS ex = assertThrows(ExcepcionClienteWS.class,
                () -> cliente.convertirLongitud(1, "METRO", "KILOMETRO"));

        assertTrue(ex.esErrorAutenticacion());
        assertNull(cuerpoRecibido.get());
    }

    @Test
    void deberiaLeerElHistorialCompartidoConElToken() {
        cliente.login("Monster", "Monster9");

        List<RegistroConversionRespuesta> historial = cliente.consultarHistorial();

        assertEquals("GET", metodoHistorial.get());
        assertEquals("Bearer abc-123", autorizacionRecibida.get());
        assertEquals(1, historial.size());
        assertEquals("Longitud", historial.get(0).getTipo());
        assertEquals("KILOMETRO", historial.get(0).getUnidadDestino());
        assertEquals(1000.0, historial.get(0).getValor(), 0.000001);
    }

    @Test
    void deberiaBorrarElHistorialConDelete() {
        cliente.login("Monster", "Monster9");

        String mensaje = cliente.limpiarHistorial();

        assertEquals("DELETE", metodoHistorial.get());
        assertEquals("Historial de conversiones borrado", mensaje);
    }

    @Test
    void deberiaExigirSesionAntesDeConsultarElHistorial() {
        ExcepcionClienteWS ex = assertThrows(ExcepcionClienteWS.class, () -> cliente.consultarHistorial());

        assertTrue(ex.esErrorAutenticacion());
        assertNull(metodoHistorial.get());
    }

    @Test
    void deberiaInformarCuandoNoHayServidor() {
        servidor.stop(0);

        ExcepcionClienteWS ex = assertThrows(ExcepcionClienteWS.class,
                () -> cliente.login("Monster", "Monster9"));

        assertTrue(ex.esErrorConexion());
    }

    private static String leerCuerpo(HttpExchange intercambio) throws IOException {
        return new String(intercambio.getRequestBody().readAllBytes(), StandardCharsets.UTF_8);
    }

    private static void responder(HttpExchange intercambio, int codigo, String json) throws IOException {
        byte[] bytes = json.getBytes(StandardCharsets.UTF_8);
        intercambio.getResponseHeaders().set("Content-Type", "application/json");
        intercambio.sendResponseHeaders(codigo, bytes.length);
        try (OutputStream salida = intercambio.getResponseBody()) {
            salida.write(bytes);
        }
    }
}
