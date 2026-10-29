package ec.edu.monster.pruebas;

import ec.edu.monster.cliente.ContratoWSConversor;
import ec.edu.monster.cliente.dto.RegistroConversionRespuesta;

import java.util.ArrayList;
import java.util.List;

/**
 * Implementación de prueba del contrato: registra qué operación se llamó y con qué datos,
 * sin hacer peticiones HTTP.
 */
class ContratoFalso implements ContratoWSConversor {

    String operacionLlamada;
    double valorRecibido;
    String unidadInicialRecibida;
    String unidadFinalRecibida;
    double resultado = 42.0;

    @Override
    public String login(String usuario, String contrasenia) {
        operacionLlamada = "login";
        return "token-de-prueba";
    }

    @Override
    public String cambiarContrasenia(String contraseniaActual, String contraseniaNueva) {
        operacionLlamada = "cambiarContrasenia";
        return "Contraseña actualizada correctamente";
    }

    @Override
    public double convertirLongitud(double valor, String unidadInicial, String unidadFinal) {
        return registrar("convertirLongitud", valor, unidadInicial, unidadFinal);
    }

    @Override
    public double convertirMasa(double valor, String unidadInicial, String unidadFinal) {
        return registrar("convertirMasa", valor, unidadInicial, unidadFinal);
    }

    @Override
    public double convertirTemperatura(double valor, String unidadInicial, String unidadFinal) {
        return registrar("convertirTemperatura", valor, unidadInicial, unidadFinal);
    }

    @Override
    public List<String> consultarUnidades(String categoria) {
        operacionLlamada = "consultarUnidades:" + categoria;
        return List.of("A", "B");
    }

    // Historial que "guarda el servidor" en las pruebas
    final List<RegistroConversionRespuesta> historialServidor = new ArrayList<>();

    @Override
    public List<RegistroConversionRespuesta> consultarHistorial() {
        operacionLlamada = "consultarHistorial";
        return List.copyOf(historialServidor);
    }

    @Override
    public String limpiarHistorial() {
        operacionLlamada = "limpiarHistorial";
        historialServidor.clear();
        return "Historial de conversiones borrado";
    }

    @Override
    public void cerrarSesion() {
        operacionLlamada = "cerrarSesion";
    }

    private double registrar(String operacion, double valor, String unidadInicial, String unidadFinal) {
        operacionLlamada = operacion;
        valorRecibido = valor;
        unidadInicialRecibida = unidadInicial;
        unidadFinalRecibida = unidadFinal;
        return resultado;
    }
}
