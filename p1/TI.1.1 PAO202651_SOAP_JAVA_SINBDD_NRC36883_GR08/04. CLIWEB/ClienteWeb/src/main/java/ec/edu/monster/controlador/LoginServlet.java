package ec.edu.monster.controlador;

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
 * Servlet de Autenticación de Usuario (CU-01 / RF-01)
 * Consume el servicio SOAP del GR08 y gestiona la sesión web.
 * 
 * @author GR08 - Arquitectura de Software
 */
@WebServlet(name = "LoginServlet", urlPatterns = {"/LoginServlet"})
public class LoginServlet extends HttpServlet {

    private static final String DEFAULT_ENDPOINT = "http://localhost:8080/WS_ConUni_Java_SOAP/WSConversorUnidades";

    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {

        String user = request.getParameter("txtUsuario");
        String pass = request.getParameter("txtClave");

        if (user == null || user.trim().isEmpty() || pass == null || pass.trim().isEmpty()) {
            request.setAttribute("error", "Por favor ingresa usuario y contraseña.");
            request.getRequestDispatcher("index.jsp").forward(request, response);
            return;
        }

        try {
            WSConversorUnidades_Service service = new WSConversorUnidades_Service();
            WSConversorUnidades port = service.getWSConversorUnidadesPort();
            ((BindingProvider) port).getRequestContext().put(
                    BindingProvider.ENDPOINT_ADDRESS_PROPERTY,
                    DEFAULT_ENDPOINT
            );

            RespuestaAutenticacion res = port.autenticar(user.trim(), pass.trim());

            if ("OK".equalsIgnoreCase(res.getEstado()) && res.getToken() != null) {
                HttpSession session = request.getSession(true);
                session.setAttribute("token", res.getToken());
                session.setAttribute("usuario", user.trim());
                response.sendRedirect("conversor.jsp");
            } else {
                String errorMsg = res.getMensaje() != null ? res.getMensaje() : "Credenciales incorrectas. Verifica tu usuario y contraseña.";
                request.setAttribute("error", errorMsg);
                request.getRequestDispatcher("index.jsp").forward(request, response);
            }

        } catch (Exception e) {
            e.printStackTrace();
            request.setAttribute("error", "No se pudo conectar con el servidor. Verifica que el servicio esté activo e inténtalo de nuevo.");
            request.getRequestDispatcher("index.jsp").forward(request, response);
        }
    }
}
