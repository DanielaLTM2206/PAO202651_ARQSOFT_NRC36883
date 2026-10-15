package ec.edu.monster.controlador;

import com.google.gson.Gson;
import ec.edu.monster.clientesws.RespuestaConversion;
import ec.edu.monster.clientesws.WSConversorUnidades;
import ec.edu.monster.clientesws.WSConversorUnidades_Service;
import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;
import jakarta.xml.ws.BindingProvider;
import java.io.IOException;

/**
 * Servlet de Conversión de Unidades (CU-04 / RF-04 / RF-06)
 * Consume la operación SOAP y retorna una Respuesta Estructurada en formato JSON.
 * 
 * @author GR08 - Arquitectura de Software
 */
@WebServlet(name = "ConversorServlet", urlPatterns = {"/ConversorServlet"})
public class ConversorServlet extends HttpServlet {

    private static final String DEFAULT_ENDPOINT = "http://localhost:8080/WS_ConUni_Java_SOAP/WSConversorUnidades";
    private final Gson gson = new Gson();

    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {

        response.setContentType("application/json;charset=UTF-8");

        HttpSession session = request.getSession(false);
        String token = (session != null) ? (String) session.getAttribute("token") : null;

        if (token == null || token.trim().isEmpty()) {
            RespuestaConversion errorRes = new RespuestaConversion();
            errorRes.setEstado("ERROR");
            errorRes.setCodigo("AUTH-001");
            errorRes.setMensaje("Sesión no iniciada o token expirado. Por favor ingrese nuevamente.");
            errorRes.setResultado(null);
            response.setStatus(HttpServletResponse.SC_UNAUTHORIZED);
            response.getWriter().write(gson.toJson(errorRes));
            return;
        }

        String categoria = request.getParameter("cmbTipo");
        String origen = request.getParameter("txtOrigen");
        String destino = request.getParameter("txtDestino");
        String valStr = request.getParameter("txtValor");

        if (categoria == null || origen == null || destino == null || valStr == null) {
            RespuestaConversion errorRes = new RespuestaConversion();
            errorRes.setEstado("ERROR");
            errorRes.setCodigo("PARAM-001");
            errorRes.setMensaje("Parámetros incompletos en la solicitud.");
            errorRes.setResultado(null);
            response.getWriter().write(gson.toJson(errorRes));
            return;
        }

        double valor;
        try {
            valor = Double.parseDouble(valStr.trim());
        } catch (NumberFormatException e) {
            RespuestaConversion errorRes = new RespuestaConversion();
            errorRes.setEstado("ERROR");
            errorRes.setCodigo("PARAM-001");
            errorRes.setMensaje("El valor numérico ingresado no es válido.");
            errorRes.setResultado(null);
            response.getWriter().write(gson.toJson(errorRes));
            return;
        }

        try {
            WSConversorUnidades_Service service = new WSConversorUnidades_Service();
            WSConversorUnidades port = service.getWSConversorUnidadesPort();
            ((BindingProvider) port).getRequestContext().put(
                    BindingProvider.ENDPOINT_ADDRESS_PROPERTY,
                    DEFAULT_ENDPOINT
            );

            RespuestaConversion res;
            if ("Longitud".equalsIgnoreCase(categoria)) {
                res = port.convertirLongitud(valor, origen, destino, token);
            } else if ("Masa".equalsIgnoreCase(categoria)) {
                res = port.convertirMasa(valor, origen, destino, token);
            } else if ("Temperatura".equalsIgnoreCase(categoria)) {
                res = port.convertirTemperatura(valor, origen, destino, token);
            } else {
                res = port.convertir(valor, origen, destino, token);
            }

            response.getWriter().write(gson.toJson(res));

        } catch (Exception e) {
            e.printStackTrace();
            RespuestaConversion errorRes = new RespuestaConversion();
            errorRes.setEstado("ERROR");
            errorRes.setCodigo("SYS-001");
            errorRes.setMensaje("Fallo al conectar con el servicio SOAP: " + e.getMessage());
            errorRes.setResultado(null);
            response.getWriter().write(gson.toJson(errorRes));
        }
    }
}
