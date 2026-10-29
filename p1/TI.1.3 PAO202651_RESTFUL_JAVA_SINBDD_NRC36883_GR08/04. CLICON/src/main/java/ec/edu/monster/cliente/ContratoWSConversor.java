package ec.edu.monster.cliente;

import ec.edu.monster.cliente.dto.RegistroConversionRespuesta;

import java.util.List;

/**
 * Operaciones que expone el servicio RESTful WSConversorUnidades.
 * Los controladores dependen de esta interfaz y no de la implementación HTTP.
 */
public interface ContratoWSConversor {

    // POST /seguridad/login
    String login(String usuario, String contrasenia);

    // POST /seguridad/cambiar-contrasena
    String cambiarContrasenia(String contraseniaActual, String contraseniaNueva);

    // POST /conversiones/longitud
    double convertirLongitud(double valor, String unidadInicial, String unidadFinal);

    // POST /conversiones/masa
    double convertirMasa(double valor, String unidadInicial, String unidadFinal);

    // POST /conversiones/temperatura
    double convertirTemperatura(double valor, String unidadInicial, String unidadFinal);

    // GET /unidades/{categoria}
    List<String> consultarUnidades(String categoria);

    // GET /conversiones/historial (historial compartido por todos los clientes)
    List<RegistroConversionRespuesta> consultarHistorial();

    // DELETE /conversiones/historial
    String limpiarHistorial();

    void cerrarSesion();
}
