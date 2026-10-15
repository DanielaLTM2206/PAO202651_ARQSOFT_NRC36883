package ec.edu.monster.conuniclientemovil.model;

import org.ksoap2.SoapEnvelope;
import org.ksoap2.SoapFault;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;

import java.io.IOException;
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
    private static final String METHOD_CONVERTIR = "convertir";
    private static final String METHOD_CONVERTIR_LONGITUD = "convertirLongitud";
    private static final String METHOD_CONVERTIR_MASA = "convertirMasa";
    private static final String METHOD_CONVERTIR_TEMPERATURA = "convertirTemperatura";

    private static final int TIMEOUT_MS = 15000;

    private final ExecutorService executorService;
    private volatile String sessionToken;

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

            String endpoint = buildEndpoint(baseUrl);
            Object response = call(endpoint, METHOD_AUTENTICAR, request);

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

            String endpoint = buildEndpoint(baseUrl);
            Object response = call(endpoint, methodName, request);

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

    public String getSessionToken() {
        return sessionToken;
    }

    public void clearSessionToken() {
        sessionToken = null;
    }

    public void shutdown() {
        executorService.shutdown();
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
