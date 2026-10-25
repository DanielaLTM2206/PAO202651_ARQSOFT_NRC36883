package ec.edu.monster.controlador;

import com.google.gson.Gson;
import ec.edu.monster.clientesws.RespuestaAutenticacion;
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
 * RF-08 / DS_07: Cambio de contraseña centralizado en el servidor SOAP.
 * Si se cambia aquí, se actualiza en memoria para todos los canales (Consola, Swing, Web, Móvil).
 * 
 * @author GR08 - Arquitectura de Software
 */
@WebServlet(name = "CambiarClaveServlet", urlPatterns = {"/CambiarClaveServlet"})
public class CambiarClaveServlet extends HttpServlet {

    private static final String DEFAULT_ENDPOINT = "http://localhost:8080/WS_ConUni_Java_SOAP/WSConversorUnidades";
    private final Gson gson = new Gson();

    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {

        response.setContentType("application/json;charset=UTF-8");

        HttpSession session = request.getSession(false);
        String token = (session != null) ? (String) session.getAttribute("token") : null;

        if (token == null || token.trim().isEmpty()) {
            RespuestaAutenticacion errorRes = new RespuestaAutenticacion();
            errorRes.setEstado("ERROR");
            errorRes.setCodigo("AUTH-001");
            errorRes.setMensaje("Sesión no iniciada o token expirado. Por favor ingrese nuevamente.");
            response.setStatus(HttpServletResponse.SC_UNAUTHORIZED);
            response.getWriter().write(gson.toJson(errorRes));
            return;
        }

        String claveActual = request.getParameter("txtClaveActual");
        String claveNueva = request.getParameter("txtClaveNueva");
        String claveConfirmar = request.getParameter("txtClaveConfirmar");

        if (claveActual == null || claveActual.trim().isEmpty() ||
            claveNueva == null || claveNueva.trim().isEmpty()) {
            RespuestaAutenticacion errorRes = new RespuestaAutenticacion();
            errorRes.setEstado("ERROR");
            errorRes.setCodigo("PARAM-001");
            errorRes.setMensaje("Por favor completa todos los campos de contraseña.");
            response.getWriter().write(gson.toJson(errorRes));
            return;
        }

        if (claveConfirmar != null && !claveNueva.trim().equals(claveConfirmar.trim())) {
            RespuestaAutenticacion errorRes = new RespuestaAutenticacion();
            errorRes.setEstado("ERROR");
            errorRes.setCodigo("PARAM-001");
            errorRes.setMensaje("La confirmación no coincide con la nueva contraseña.");
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

            RespuestaAutenticacion res = port.cambiarContrasenia(claveActual.trim(), claveNueva.trim(), token);
            response.getWriter().write(gson.toJson(res));

        } catch (Exception e) {
            e.printStackTrace();
            RespuestaAutenticacion errorRes = new RespuestaAutenticacion();
            errorRes.setEstado("ERROR");
            errorRes.setCodigo("SYS-001");
            errorRes.setMensaje("No se pudo conectar con el servidor. Inténtalo de nuevo más tarde.");
            response.getWriter().write(gson.toJson(errorRes));
        }
    }
}
