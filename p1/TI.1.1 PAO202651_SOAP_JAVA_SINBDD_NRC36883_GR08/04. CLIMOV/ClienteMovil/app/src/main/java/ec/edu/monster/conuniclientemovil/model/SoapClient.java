package ec.edu.monster.conuniclientemovil.model;

import android.os.Build;

import org.ksoap2.SoapEnvelope;
import org.ksoap2.SoapFault;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

/**
 * Cliente SOAP para Android (KSoap2) adaptado a la arquitectura SOA GR08.
 * Consume el Web Service JAX-WS 'WSConversorUnidades' y procesa Respuestas Estructuradas (RF-06).
 *
 * @author GR08 - Arquitectura de Software
 */
public class SoapClient {

    private static final String NAMESPACE = "http://monster.edu.ec/services";
    private static final String SERVICE_NAME = "WSConversorUnidades";

    private static final String METHOD_AUTENTICAR = "autenticar";
    private static final String METHOD_CAMBIAR_CONTRASENIA = "cambiarContrasenia";
    private static final String METHOD_CONVERTIR = "convertir";
    private static final String METHOD_CONVERTIR_LONGITUD = "convertirLongitud";
    private static final String METHOD_CONVERTIR_MASA = "convertirMasa";
    private static final String METHOD_CONVERTIR_TEMPERATURA = "convertirTemperatura";

    private static final int TIMEOUT_MS = 5000;

    private static boolean isRunningOnEmulator() {
        return Build.FINGERPRINT.startsWith("generic")
                || Build.MODEL.contains("google_sdk")
                || Build.MODEL.contains("Emulator")
                || Build.HARDWARE.contains("goldfish")
                || Build.HARDWARE.contains("ranchu");
    }

    private static List<String> getCandidateHosts() {
        List<String> list = new ArrayList<>();
        if (isRunningOnEmulator()) {
            list.add("http://10.0.2.2:8080/WS_ConUni_Java_SOAP/");
            list.add("http://127.0.0.1:8080/WS_ConUni_Java_SOAP/");
        } else {
            list.add("http://127.0.0.1:8080/WS_ConUni_Java_SOAP/");
        }
        return list;
    }

    private final ExecutorService executorService;
    private volatile String sessionToken;
    private volatile String resolvedBaseUrl = null;

    public SoapClient() {
        this(Executors.newFixedThreadPool(2));
    }

    public SoapClient(ExecutorService executorService) {
        this.executorService = executorService;
    }

    public Future<LoginResult> login(String baseUrl, String usuario, String clave) {
        return executorService.submit(() -> {
            SoapObject request = new SoapObject(NAMESPACE, METHOD_AUTENTICAR);
            request.addProperty("usuario", usuario);
            request.addProperty("clave", clave);

            Object response = callWithFallback(baseUrl, METHOD_AUTENTICAR, request);

            if (response instanceof SoapObject) {
                SoapObject obj = (SoapObject) response;
                String estado = getProperty(obj, "estado");
                String codigo = getProperty(obj, "codigo");
                String mensaje = getProperty(obj, "mensaje");
                String token = getProperty(obj, "token");

                if ("OK".equalsIgnoreCase(estado) && token != null) {
                    sessionToken = token;
                    return new LoginResult(true, token, codigo, null);
                } else {
                    return LoginResult.error(codigo, mensaje != null ? mensaje : "Credenciales inválidas");
                }
            } else if (response != null) {
                String tokenStr = response.toString();
                if (tokenStr.contains("ERROR")) {
                    return LoginResult.error("AUTH-001", tokenStr);
                }
                sessionToken = tokenStr;
                return LoginResult.success(tokenStr);
            }

            return LoginResult.error("AUTH-001", "Respuesta vacía del servidor.");
        });
    }

    /**
     * RF-08 / DS_07: Cambia la contraseña en el servidor central SOAP.
     * Sincronizado para todos los clientes en memoria.
     */
    public Future<LoginResult> cambiarContrasenia(String baseUrl, String contraseniaActual, String contraseniaNueva, String token) {
        return executorService.submit(() -> {
            SoapObject request = new SoapObject(NAMESPACE, METHOD_CAMBIAR_CONTRASENIA);
            request.addProperty("contraseniaActual", contraseniaActual);
            request.addProperty("contraseniaNueva", contraseniaNueva);
            request.addProperty("token", token != null ? token : sessionToken);

            Object response = callWithFallback(baseUrl, METHOD_CAMBIAR_CONTRASENIA, request);

            if (response instanceof SoapObject) {
                SoapObject obj = (SoapObject) response;
                String estado = getProperty(obj, "estado");
                String codigo = getProperty(obj, "codigo");
                String mensaje = getProperty(obj, "mensaje");

                if ("OK".equalsIgnoreCase(estado)) {
                    return new LoginResult(true, sessionToken, codigo, mensaje);
                } else {
                    return LoginResult.error(codigo, mensaje != null ? mensaje : "Error al cambiar contraseña");
                }
            } else if (response != null) {
                String resStr = response.toString();
                if (resStr.contains("ERROR")) {
                    return LoginResult.error("AUTH-002", resStr);
                }
                return LoginResult.success(sessionToken);
            }

            return LoginResult.error("AUTH-002", "Respuesta vacía del servidor.");
        });
    }

    public Future<ConversionResult> convertir(
            String baseUrl,
            ServiceType serviceType,
            double valor,
            String unidadOrigen,
            String unidadDestino,
            String token
    ) {
        return executorService.submit(() -> {
            String methodName;
            switch (serviceType) {
                case LONGITUD:
                    methodName = METHOD_CONVERTIR_LONGITUD;
                    break;
                case MASA:
                    methodName = METHOD_CONVERTIR_MASA;
                    break;
                case TEMPERATURA:
                    methodName = METHOD_CONVERTIR_TEMPERATURA;
                    break;
                default:
                    methodName = METHOD_CONVERTIR;
                    break;
            }

            SoapObject request = new SoapObject(NAMESPACE, methodName);
            request.addProperty("valor", String.valueOf(valor));
            request.addProperty("unidadOrigen", unidadOrigen);
            request.addProperty("unidadDestino", unidadDestino);
            request.addProperty("token", token);

            Object response = callWithFallback(baseUrl, methodName, request);

            if (response instanceof SoapObject) {
                SoapObject obj = (SoapObject) response;
                String estado = getProperty(obj, "estado");
                String codigo = getProperty(obj, "codigo");
                String mensaje = getProperty(obj, "mensaje");
                Double resVal = null;

                if (obj.hasProperty("resultado")) {
                    try {
                        resVal = Double.parseDouble(obj.getPropertyAsString("resultado"));
                    } catch (Exception ignored) {}
                }

                return new ConversionResult(estado, codigo, mensaje, resVal);
            } else if (response != null) {
                try {
                    double val = Double.parseDouble(response.toString());
                    return ConversionResult.success("200", "Conversión exitosa", val);
                } catch (NumberFormatException e) {
                    return ConversionResult.error("SYS-001", response.toString());
                }
            }

            return ConversionResult.error("SYS-001", "Respuesta vacía del servicio SOAP.");
        });
    }

    /**
     * Consulta el historial global de conversiones compartido (Web, Móvil, Escritorio, Consola).
     */
    public Future<List<RegistroHistorial>> consultarHistorial(String baseUrl, String token) {
        return executorService.submit(() -> {
            SoapObject request = new SoapObject(NAMESPACE, "consultarHistorial");
            request.addProperty("token", token);

            Object response = callWithFallback(baseUrl, "consultarHistorial", request);

            List<RegistroHistorial> items = new ArrayList<>();
            if (response instanceof SoapObject) {
                SoapObject obj = (SoapObject) response;
                if (obj.hasProperty("return") && obj.getProperty("return") instanceof SoapObject) {
                    obj = (SoapObject) obj.getProperty("return");
                }
                String estadoHist = getProperty(obj, "estado");
                if (estadoHist != null && !"OK".equalsIgnoreCase(estadoHist)) {
                    String msg = getProperty(obj, "mensaje");
                    throw new IOException(msg != null ? msg : "El servidor rechazó la consulta del historial.");
                }
                for (int i = 0; i < obj.getPropertyCount(); i++) {
                    Object p = obj.getProperty(i);
                    if (p instanceof SoapObject) {
                        SoapObject so = (SoapObject) p;
                        int id = 0;
                        try { id = Integer.parseInt(getProperty(so, "id")); } catch (Exception ignored) {}
                        String fechaHora = getProperty(so, "fechaHora");
                        String categoria = getProperty(so, "categoria");
                        double valorOriginal = 0;
                        try { valorOriginal = Double.parseDouble(getProperty(so, "valorOriginal")); } catch (Exception ignored) {}
                        String unidadOrigen = getProperty(so, "unidadOrigen");
                        double resultado = 0;
                        try { resultado = Double.parseDouble(getProperty(so, "resultado")); } catch (Exception ignored) {}
                        String unidadDestino = getProperty(so, "unidadDestino");

                        items.add(new RegistroHistorial(id, fechaHora, categoria, valorOriginal, unidadOrigen, resultado, unidadDestino));
                    }
                }
            }
            return items;
        });
    }

    public String getSessionToken() {
        return sessionToken;
    }

    public void clearSessionToken() {
        sessionToken = null;
    }

    public String getResolvedBaseUrl() {
        return resolvedBaseUrl;
    }

    public void shutdown() {
        executorService.shutdown();
    }

    private Object callWithFallback(String baseUrl, String methodName, SoapObject request) throws Exception {
        List<String> hostsToTry = new ArrayList<>();

        if (baseUrl != null && !baseUrl.trim().isEmpty()) {
            String norm = baseUrl.trim();
            if (!isRunningOnEmulator() && norm.contains("10.0.2.2")) {
                // Descartar 10.0.2.2 en dispositivos fisicos
            } else {
                hostsToTry.add(norm);
            }
        } else if (resolvedBaseUrl != null && !resolvedBaseUrl.trim().isEmpty()) {
            hostsToTry.add(resolvedBaseUrl.trim());
        }

        // Solo recurrir a candidatos por defecto si no se especifico ninguna IP
        if (hostsToTry.isEmpty()) {
            for (String candidate : getCandidateHosts()) {
                if (!hostsToTry.contains(candidate)) {
                    hostsToTry.add(candidate);
                }
            }
        }
        Exception lastException = null;
        for (String host : hostsToTry) {
            try {
                String endpoint = buildEndpoint(host);
                Object resp = call(endpoint, methodName, request);
                resolvedBaseUrl = host;
                return resp;
            } catch (Exception ex) {
                lastException = ex;
                if (ex.getMessage() != null && ex.getMessage().contains("SOAP Fault")) {
                    throw ex;
                }
            }
        }
        throw lastException != null ? lastException : new IOException("No se pudo conectar a ningún servidor.");
    }

    private String buildEndpoint(String baseUrl) {
        if (baseUrl == null || baseUrl.trim().isEmpty()) {
            baseUrl = "http://10.0.2.2:8080/WS_ConUni_Java_SOAP/";
        }

        String normalized = baseUrl.trim();
        if (!normalized.endsWith("/")) {
            normalized = normalized + "/";
        }

        if (!normalized.endsWith(SERVICE_NAME + "/") && !normalized.endsWith(SERVICE_NAME)) {
            normalized = normalized + SERVICE_NAME;
        }

        return normalized;
    }

    private Object call(String endpoint, String methodName, SoapObject request) throws Exception {
        SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(SoapEnvelope.VER11);
        envelope.dotNet = false;
        envelope.implicitTypes = true;
        envelope.setOutputSoapObject(request);

        HttpTransportSE httpTransport = new HttpTransportSE(endpoint, TIMEOUT_MS);
        String soapAction = NAMESPACE + "/" + methodName;
        httpTransport.call(soapAction, envelope);

        if (envelope.bodyIn instanceof SoapFault) {
            SoapFault fault = (SoapFault) envelope.bodyIn;
            throw new IOException("SOAP Fault: " + fault.faultstring);
        }

        Object response = envelope.getResponse();
        if (response instanceof SoapPrimitive) {
            return ((SoapPrimitive) response).toString();
        }

        return response;
    }

    private String getProperty(SoapObject obj, String name) {
        if (obj != null && obj.hasProperty(name)) {
            Object val = obj.getProperty(name);
            return val != null ? val.toString() : null;
        }
        return null;
    }
}
