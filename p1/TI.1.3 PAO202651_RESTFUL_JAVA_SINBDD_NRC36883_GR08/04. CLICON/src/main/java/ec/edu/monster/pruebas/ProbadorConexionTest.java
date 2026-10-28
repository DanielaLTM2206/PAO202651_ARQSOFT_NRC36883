package ec.edu.monster.pruebas;

import ec.edu.monster.cliente.ProbadorConexion;
import ec.edu.monster.modelo.ResultadoConexion;
import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpServer;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.io.IOException;
import java.io.OutputStream;
import java.net.InetSocketAddress;
import java.net.ServerSocket;
import java.nio.charset.StandardCharsets;
import java.util.concurrent.atomic.AtomicReference;

import static org.junit.jupiter.api.Assertions.*;

/**
 * Prueba la conexion contra un servidor HTTP local que imita las respuestas del servicio RESTful.
 */
public class ProbadorConexionTest {

    private HttpServer servidor;
    private String base;
    private final ProbadorConexion probador = new ProbadorConexion();

    private final AtomicReference<Integer> codigo = new AtomicReference<>(401);
    private final AtomicReference<String> cuerpo =
            new AtomicReference<>("{\"Mensaje\":\"Token de sesion requerido\"}");
    private final AtomicReference<String> rutaRecibida = new AtomicReference<>();

    @BeforeEach
    void iniciarServidor() throws IOException {
        servidor = HttpServer.create(new InetSocketAddress("localhost", 0), 0);
        servidor.createContext("/", intercambio -> {
            rutaRecibida.set(intercambio.getRequestURI().getPath());
            responder(intercambio, codigo.get(), cuerpo.get());
        });
        servidor.start();
        base = "http://localhost:" + servidor.getAddress().getPort() + "/WS_ConUni_Java_RESTFUL/api";
    }

    @AfterEach
    void detenerServidor() {
        servidor.stop(0);
    }

    @Test
    void deberiaAceptarElServicioAunqueRespondaSinToken() {
        ResultadoConexion resultado = probador.probar(base);

        assertTrue(resultado.exitosa());
        assertEquals("/WS_ConUni_Java_RESTFUL/api/unidades", rutaRecibida.get());
    }

    @Test
    void deberiaAceptarUnaRespuestaCorrectaDelCatalogo() {
        codigo.set(200);
        cuerpo.set("[{\"Categoria\":\"masa\",\"Unidades\":[\"KILOGRAMO\"]}]");

        assertTrue(probador.probar(base + "/").exitosa());
    }

    @Test
    void deberiaIndicarRutaEquivocadaConUn404() {
        codigo.set(404);
        cuerpo.set("<html>Not Found</html>");

        ResultadoConexion resultado = probador.probar(base);

        assertFalse(resultado.exitosa());
        assertTrue(resultado.mensaje().contains("/WS_ConUni_Java_RESTFUL/api"));
    }

    @Test
    void deberiaRechazarOtroServicio() {
        codigo.set(200);
        cuerpo.set("<html>Otra aplicacion</html>");

        ResultadoConexion resultado = probador.probar(base);

        assertFalse(resultado.exitosa());
        assertTrue(resultado.mensaje().contains("no es el servicio"));
    }

    @Test
    void deberiaInformarSiNoHayConexion() throws IOException {
        int puertoLibre;
        try (ServerSocket socket = new ServerSocket(0)) {
            puertoLibre = socket.getLocalPort();
        }

        ResultadoConexion resultado = probador.probar("http://localhost:" + puertoLibre + "/api");

        assertFalse(resultado.exitosa());
        assertTrue(resultado.mensaje().contains("No fue posible conectar"));
    }

    private static void responder(HttpExchange intercambio, int codigo, String json) throws IOException {
        byte[] bytes = json.getBytes(StandardCharsets.UTF_8);
        intercambio.getResponseHeaders().add("Content-Type", "application/json");
        intercambio.sendResponseHeaders(codigo, bytes.length);
        try (OutputStream salida = intercambio.getResponseBody()) {
            salida.write(bytes);
        }
    }
}
