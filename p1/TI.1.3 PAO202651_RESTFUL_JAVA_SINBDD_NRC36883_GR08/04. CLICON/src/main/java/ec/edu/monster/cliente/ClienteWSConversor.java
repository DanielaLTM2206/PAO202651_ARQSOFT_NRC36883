package ec.edu.monster.cliente;

import com.google.gson.Gson;
import com.google.gson.JsonParseException;
import ec.edu.monster.cliente.dto.CambioContrasenaSolicitud;
import ec.edu.monster.cliente.dto.ConversionRespuesta;
import ec.edu.monster.cliente.dto.ConversionSolicitud;
import ec.edu.monster.cliente.dto.LoginRespuesta;
import ec.edu.monster.cliente.dto.LoginSolicitud;
import ec.edu.monster.cliente.dto.MensajeRespuesta;
import ec.edu.monster.cliente.dto.RegistroConversionRespuesta;
import ec.edu.monster.cliente.dto.UnidadesCategoriaRespuesta;
import ec.edu.monster.util.ConfiguracionServidor;

import java.io.IOException;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.time.Duration;
import java.util.Arrays;
import java.util.List;

/**
 * Cliente HTTP del servicio RESTful WSConversorUnidades. Envía y recibe JSON.
 */
public class ClienteWSConversor implements ContratoWSConversor {

    private static final Duration TIEMPO_ESPERA = Duration.ofSeconds(10);
    private static final String TIPO_JSON = "application/json";

    private final HttpClient clienteHttp = HttpClient.newBuilder()
            .connectTimeout(TIEMPO_ESPERA)
            .build();
    private final Gson gson = new Gson();
    private final ManejadorTokenRESTFUL manejadorToken = new ManejadorTokenRESTFUL();
    private final String baseUrl;

    // Propiedad ws.base.url, variable WS_BASE_URL, direccion guardada por el usuario o localhost
    public ClienteWSConversor() {
        this(new ConfiguracionServidor().getUrlActual());
    }

    public ClienteWSConversor(String baseUrl) {
        this.baseUrl = normalizarBase(baseUrl);
    }

    @Override
    public String login(String usuario, String contrasenia) {
        LoginRespuesta respuesta = enviar(
                post("/seguridad/login", new LoginSolicitud(usuario, contrasenia)),
                LoginRespuesta.class);

        manejadorToken.guardarToken(respuesta.getToken());
        return respuesta.getToken();
    }

    @Override
    public String cambiarContrasenia(String contraseniaActual, String contraseniaNueva) {
        HttpRequest.Builder peticion = post("/seguridad/cambiar-contrasena",
                new CambioContrasenaSolicitud(contraseniaActual, contraseniaNueva));

        return enviar(manejadorToken.adjuntarToken(peticion), MensajeRespuesta.class).getMensaje();
    }

    @Override
    public double convertirLongitud(double valor, String unidadInicial, String unidadFinal) {
        return convertir("longitud", valor, unidadInicial, unidadFinal);
    }

    @Override
    public double convertirMasa(double valor, String unidadInicial, String unidadFinal) {
        return convertir("masa", valor, unidadInicial, unidadFinal);
    }

    @Override
    public double convertirTemperatura(double valor, String unidadInicial, String unidadFinal) {
        return convertir("temperatura", valor, unidadInicial, unidadFinal);
    }

    @Override
    public List<String> consultarUnidades(String categoria) {
        HttpRequest.Builder peticion = HttpRequest.newBuilder(URI.create(baseUrl + "/unidades/" + categoria))
                .header("Accept", TIPO_JSON)
                .GET();

        return enviar(manejadorToken.adjuntarToken(peticion), UnidadesCategoriaRespuesta.class).getUnidades();
    }

    @Override
    public List<RegistroConversionRespuesta> consultarHistorial() {
        HttpRequest.Builder peticion = HttpRequest.newBuilder(URI.create(baseUrl + "/conversiones/historial"))
                .header("Accept", TIPO_JSON)
                .GET();

        RegistroConversionRespuesta[] registros =
                enviar(manejadorToken.adjuntarToken(peticion), RegistroConversionRespuesta[].class);
        return registros == null ? List.of() : Arrays.asList(registros);
    }

    @Override
    public String limpiarHistorial() {
        HttpRequest.Builder peticion = HttpRequest.newBuilder(URI.create(baseUrl + "/conversiones/historial"))
                .header("Accept", TIPO_JSON)
                .DELETE();

        MensajeRespuesta respuesta = enviar(manejadorToken.adjuntarToken(peticion), MensajeRespuesta.class);
        return respuesta != null && respuesta.getMensaje() != null
                ? respuesta.getMensaje() : "Historial de conversiones borrado";
    }

    @Override
    public void cerrarSesion() {
        manejadorToken.limpiarToken();
    }

    public String getTokenSesion() {
        return manejadorToken.obtenerToken();
    }

    public String getBaseUrl() {
        return baseUrl;
    }

    private double convertir(String tipo, double valor, String unidadInicial, String unidadFinal) {
        HttpRequest.Builder peticion = post("/conversiones/" + tipo,
                new ConversionSolicitud(valor, unidadInicial, unidadFinal));

        return enviar(manejadorToken.adjuntarToken(peticion), ConversionRespuesta.class).getValorConvertido();
    }

    private HttpRequest.Builder post(String ruta, Object cuerpo) {
        return HttpRequest.newBuilder(URI.create(baseUrl + ruta))
                .header("Content-Type", TIPO_JSON)
                .header("Accept", TIPO_JSON)
                .POST(HttpRequest.BodyPublishers.ofString(gson.toJson(cuerpo)));
    }

    private <T> T enviar(HttpRequest.Builder peticion, Class<T> tipoRespuesta) {
        HttpResponse<String> respuesta;
        try {
            respuesta = clienteHttp.send(peticion.timeout(TIEMPO_ESPERA).build(),
                    HttpResponse.BodyHandlers.ofString());
        } catch (IOException ex) {
            throw new ExcepcionClienteWS(ExcepcionClienteWS.SIN_CONEXION,
                    "No fue posible conectar con el servidor REST en " + baseUrl
                    + ". Verifique que Payara este activo y el servicio desplegado.");
        } catch (InterruptedException ex) {
            Thread.currentThread().interrupt();
            throw new ExcepcionClienteWS(ExcepcionClienteWS.SIN_CONEXION, "La peticion fue interrumpida.");
        }

        int codigo = respuesta.statusCode();
        if (codigo >= 200 && codigo < 300) {
            return gson.fromJson(respuesta.body(), tipoRespuesta);
        }
        throw new ExcepcionClienteWS(codigo, extraerMensaje(respuesta));
    }

    // Los errores del servidor llegan como {"Mensaje": "..."}
    private String extraerMensaje(HttpResponse<String> respuesta) {
        try {
            MensajeRespuesta error = gson.fromJson(respuesta.body(), MensajeRespuesta.class);
            if (error != null && error.getMensaje() != null) {
                return error.getMensaje();
            }
        } catch (JsonParseException ex) {
            // El cuerpo no es JSON (por ejemplo, una página de error del servidor)
        }
        return "El servidor respondio con el codigo HTTP " + respuesta.statusCode();
    }

    private static String normalizarBase(String base) {
        String baseNormalizada = base.trim();
        while (baseNormalizada.endsWith("/")) {
            baseNormalizada = baseNormalizada.substring(0, baseNormalizada.length() - 1);
        }
        return baseNormalizada;
    }
}
