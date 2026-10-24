package ec.edu.monster.clienteescritorio.controlador;

import ec.edu.monster.clientesws.CategoriaDTO;
import ec.edu.monster.clientesws.RespuestaAutenticacion;
import ec.edu.monster.clientesws.RespuestaCatalogo;
import ec.edu.monster.clientesws.RespuestaConversion;
import ec.edu.monster.clientesws.WSConversorUnidades;
import ec.edu.monster.clientesws.WSConversorUnidades_Service;
import jakarta.xml.ws.BindingProvider;
import java.net.URL;
import java.util.Collections;
import java.util.List;

/**
 * Controlador de comunicación con el servicio SOAP del GR08.
 * Desacopla la lógica de red de la interfaz gráfica Swing.
 * 
 * @author GR08 - Arquitectura de Software
 */
public class ControladorConversiones {

    private static final String ENDPOINT_URL = "http://localhost:8080/WS_ConUni_Java_SOAP/WSConversorUnidades";
    private static final String WSDL_URL = ENDPOINT_URL + "?wsdl";

    private WSConversorUnidades port;
    private String token;
    private String ultimoError;

    public ControladorConversiones() {
        try {
            URL url = new URL(WSDL_URL);
            WSConversorUnidades_Service service = new WSConversorUnidades_Service(url);
            this.port = service.getWSConversorUnidadesPort();
            ((BindingProvider) port).getRequestContext().put(BindingProvider.ENDPOINT_ADDRESS_PROPERTY, ENDPOINT_URL);
        } catch (Exception e) {
            this.ultimoError = "Error conectando con el servicio SOAP: " + e.getMessage();
        }
    }

    /**
     * RF-01 (CU-01): Autentica credenciales y almacena el token.
     */
    public boolean iniciarSesion(String usuario, String clave) {
        try {
            if (port == null) {
                this.ultimoError = "No se pudo conectar al servidor SOAP. ¿Payara está iniciado?";
                return false;
            }
            RespuestaAutenticacion resp = port.autenticar(usuario, clave);
            if ("OK".equalsIgnoreCase(resp.getEstado()) && resp.getToken() != null) {
                this.token = resp.getToken();
                this.ultimoError = null;
                return true;
            } else {
                this.ultimoError = resp.getMensaje() != null ? resp.getMensaje() : "Credenciales inválidas";
                return false;
            }
        } catch (Exception e) {
            this.ultimoError = "Fallo de comunicación SOAP: " + e.getMessage();
            return false;
        }
    }

    /**
     * RF-02 (CU-02): Consulta el catálogo de categorías y unidades desde el servidor.
     */
    public List<CategoriaDTO> obtenerCatalogo() {
        try {
            if (port != null && token != null) {
                RespuestaCatalogo cat = port.consultarCatalogo(token);
                if ("OK".equalsIgnoreCase(cat.getEstado()) && cat.getCategoria() != null) {
                    return cat.getCategoria();
                }
            }
        } catch (Exception e) {
            System.err.println("Error al consultar catálogo: " + e.getMessage());
        }
        return Collections.emptyList();
    }

    public double convertirLongitud(double valor, String origen, String destino) throws Exception {
        RespuestaConversion res = port.convertirLongitud(valor, origen, destino, token);
        return res.getResultado() != null ? res.getResultado() : 0.0;
    }

    public double convertirMasa(double valor, String origen, String destino) throws Exception {
        RespuestaConversion res = port.convertirMasa(valor, origen, destino, token);
        return res.getResultado() != null ? res.getResultado() : 0.0;
    }

    public double convertirTemperatura(double valor, String origen, String destino) throws Exception {
        RespuestaConversion res = port.convertirTemperatura(valor, origen, destino, token);
        return res.getResultado() != null ? res.getResultado() : 0.0;
    }

    /**
     * RF-05 (CU-04): Ejecuta la conversión enviando el token emitido con Respuesta Estructurada.
     */
    public RespuestaConversion convertir(double valor, String origen, String destino) {
        try {
            if (port == null) {
                RespuestaConversion err = new RespuestaConversion();
                err.setEstado("ERROR");
                err.setCodigo("CONN-001");
                err.setMensaje("No hay conexión con el servidor SOAP");
                return err;
            }
            return port.convertir(valor, origen, destino, token);
        } catch (Exception e) {
            RespuestaConversion err = new RespuestaConversion();
            err.setEstado("ERROR");
            err.setCodigo("SOAP-ERR");
            err.setMensaje("Excepción SOAP: " + e.getMessage());
            return err;
        }
    }

    public void cerrarSesion() {
        this.token = null;
    }

    public boolean tieneSesionActiva() {
        return this.token != null && !this.token.isEmpty();
    }

    public String getToken() {
        return token;
    }

    public String getUltimoError() {
        return ultimoError;
    }

    /**
     * Consulta el historial global de conversiones compartido por todos los canales (Web, Móvil, Escritorio, Consola).
     */
    public List<ec.edu.monster.clientesws.RegistroHistorialDTO> obtenerHistorial() {
        try {
            if (port != null && token != null) {
                ec.edu.monster.clientesws.RespuestaHistorial res = port.consultarHistorial(token);
                if ("OK".equalsIgnoreCase(res.getEstado()) && res.getItem() != null) {
                    return res.getItem();
                }
            }
        } catch (Exception e) {
            System.err.println("Error al consultar historial: " + e.getMessage());
        }
        return Collections.emptyList();
    }
}
