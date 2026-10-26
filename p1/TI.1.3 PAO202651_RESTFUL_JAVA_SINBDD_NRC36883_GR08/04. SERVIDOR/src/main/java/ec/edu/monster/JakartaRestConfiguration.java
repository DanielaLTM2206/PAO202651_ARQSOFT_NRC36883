package ec.edu.monster;

import jakarta.ws.rs.ApplicationPath;
import jakarta.ws.rs.core.Application;

/**
 * Configura Jakarta RESTful Web Services. Todos los recursos quedan bajo /api.
 */
@ApplicationPath("api")
public class JakartaRestConfiguration extends Application {

}
