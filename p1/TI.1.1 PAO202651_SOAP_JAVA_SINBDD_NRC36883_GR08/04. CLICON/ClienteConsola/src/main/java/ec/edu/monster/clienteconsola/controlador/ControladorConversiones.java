package ec.edu.monster.clienteconsola.controlador;

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
 * Controlador para Cliente de Consola (GR08).
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

    public boolean iniciarSesion(String usuario, String clave) {
        try {
            if (port == null) {
                this.ultimoError = "Servidor no disponible en " + ENDPOINT_URL;
                return false;
            }
            RespuestaAutenticacion resp = port.autenticar(usuario, clave);
            if ("OK".equalsIgnoreCase(resp.getEstado()) && resp.getToken() != null) {
                this.token = resp.getToken();
                return true;
            } else {
                this.ultimoError = resp.getMensaje() != null ? resp.getMensaje() : "Credenciales inválidas";
                return false;
            }
        } catch (Exception e) {
            this.ultimoError = "Error SOAP: " + e.getMessage();
            return false;
        }
    }

    public List<CategoriaDTO> consultarCatalogo() {
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
        RespuestaConversion res = port.convertirLongitud(valor, origen.toLowerCase(), destino.toLowerCase(), token);
        return res.getResultado() != null ? res.getResultado() : 0.0;
    }

    public double convertirMasa(double valor, String origen, String destino) throws Exception {
        RespuestaConversion res = port.convertirMasa(valor, origen.toLowerCase(), destino.toLowerCase(), token);
        return res.getResultado() != null ? res.getResultado() : 0.0;
    }

    public double convertirTemperatura(double valor, String origen, String destino) throws Exception {
        RespuestaConversion res = port.convertirTemperatura(valor, origen.toLowerCase(), destino.toLowerCase(), token);
        return res.getResultado() != null ? res.getResultado() : 0.0;
    }

    public RespuestaConversion convertir(double valor, String origen, String destino) {
        try {
            if (port == null) {
                RespuestaConversion err = new RespuestaConversion();
                err.setEstado("ERROR");
                err.setCodigo("CONN-001");
                err.setMensaje("No se pudo conectar con el servidor.");
                return err;
            }
            return port.convertir(valor, origen.toLowerCase(), destino.toLowerCase(), token);
        } catch (Exception e) {
            RespuestaConversion err = new RespuestaConversion();
            err.setEstado("ERROR");
            err.setCodigo("SOAP-ERR");
            err.setMensaje("No se pudo realizar la conversión en el servidor.");
            return err;
        }
    }

    public String getToken() {
        return token;
    }

    public String getUltimoError() {
        return ultimoError;
    }

    /**
     * Consulta el historial centralizado compartido de conversiones.
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

    /**
     * RF-08 / DS_07: Cambia la contraseña en el servidor central SOAP.
     * La actualización se propaga inmediatamente a todos los demás clientes (Web, Escritorio, Móvil).
     */
    public RespuestaAutenticacion cambiarContrasenia(String actual, String nueva) {
        try {
            if (port == null) {
                RespuestaAutenticacion err = new RespuestaAutenticacion();
                err.setEstado("ERROR");
                err.setCodigo("CONN-001");
                err.setMensaje("No se pudo conectar con el servidor.");
                return err;
            }
            if (token == null) {
                RespuestaAutenticacion err = new RespuestaAutenticacion();
                err.setEstado("ERROR");
                err.setCodigo("AUTH-001");
                err.setMensaje("No hay sesión activa.");
                return err;
            }
            return port.cambiarContrasenia(actual, nueva, token);
        } catch (Exception e) {
            RespuestaAutenticacion err = new RespuestaAutenticacion();
            err.setEstado("ERROR");
            err.setCodigo("SOAP-ERR");
            err.setMensaje("No se pudo actualizar la contraseña.");
            return err;
        }
    }
}
