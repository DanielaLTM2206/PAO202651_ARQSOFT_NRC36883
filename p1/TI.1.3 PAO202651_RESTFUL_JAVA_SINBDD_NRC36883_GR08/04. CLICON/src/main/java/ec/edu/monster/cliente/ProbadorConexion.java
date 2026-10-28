package ec.edu.monster.cliente;

import ec.edu.monster.modelo.ResultadoConexion;

import java.io.IOException;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.net.http.HttpTimeoutException;
import java.time.Duration;

/**
 * Comprueba que en una direccion responda el servicio RESTful WSConversorUnidades antes de guardarla.
 * Pide GET /unidades sin token: el servicio responde 401 con {"Mensaje": "..."}, lo que confirma que
 * esta activo. Distingue un servidor apagado o bloqueado por el firewall de una ruta equivocada (404).
 */
public class ProbadorConexion {

    private static final Duration TIEMPO_ESPERA = Duration.ofSeconds(5);

    private final HttpClient clienteHttp = HttpClient.newBuilder()
            .connectTimeout(TIEMPO_ESPERA)
            .build();

    public ResultadoConexion probar(String baseUrl) {
        String url = baseUrl.trim();
        while (url.endsWith("/")) {
            url = url.substring(0, url.length() - 1);
        }

        HttpResponse<String> respuesta;
        try {
            HttpRequest peticion = HttpRequest.newBuilder(URI.create(url + "/unidades"))
                    .header("Accept", "application/json")
                    .timeout(TIEMPO_ESPERA)
                    .GET()
                    .build();
            respuesta = clienteHttp.send(peticion, HttpResponse.BodyHandlers.ofString());
        } catch (HttpTimeoutException ex) {
            return new ResultadoConexion(false, "El servidor en " + url + " no respondio en "
                    + TIEMPO_ESPERA.toSeconds() + " segundos. Verifique la IP, que ambas computadoras esten "
                    + "en la misma red y que el firewall permita el puerto 8080.");
        } catch (IOException | IllegalArgumentException ex) {
            return new ResultadoConexion(false, "No fue posible conectar con " + url
                    + ". Verifique que Payara este encendido con el servicio desplegado, la IP y el puerto, "
                    + "y el firewall del servidor.");
        } catch (InterruptedException ex) {
            Thread.currentThread().interrupt();
            return new ResultadoConexion(false, "La prueba de conexion fue interrumpida.");
        }

        int codigo = respuesta.statusCode();
        String cuerpo = respuesta.body() == null ? "" : respuesta.body();
        boolean esElServicio = (codigo == 401 && cuerpo.contains("\"Mensaje\""))
                || (codigo == 200 && cuerpo.contains("\"Unidades\""));
        if (esElServicio) {
            return new ResultadoConexion(true, "Conexion correcta con el servidor REST en " + url + ".");
        }
        if (codigo == 404) {
            return new ResultadoConexion(false, "El equipo en " + url + " respondio con el codigo 404. "
                    + "Revise que la direccion termine en /WS_ConUni_Java_RESTFUL/api.");
        }
        return new ResultadoConexion(false,
                "La direccion " + url + " responde (codigo " + codigo + "), pero no es el servicio REST WSConversorUnidades.");
    }
}
