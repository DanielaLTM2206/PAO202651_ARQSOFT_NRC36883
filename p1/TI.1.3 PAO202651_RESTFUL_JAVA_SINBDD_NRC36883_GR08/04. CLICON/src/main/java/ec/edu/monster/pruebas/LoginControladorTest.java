package ec.edu.monster.pruebas;

import ec.edu.monster.controlador.LoginControlador;
import ec.edu.monster.modelo.LoginModelo;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.junit.jupiter.api.Assertions.*;

public class LoginControladorTest {

    private final ContratoFalso clienteWs = new ContratoFalso();
    private final LoginControlador controlador = new LoginControlador(clienteWs, "http://servidor-de-prueba/api");

    @Test
    void deberiaAutenticarYDevolverToken() {
        String token = controlador.autenticar(new LoginModelo("Monster", "Monster9"));

        assertEquals("token-de-prueba", token);
        assertEquals("login", clienteWs.operacionLlamada);
    }

    @ParameterizedTest
    @CsvSource(value = {
        "NULL, Monster9",
        "Monster, NULL",
        "'  ', Monster9",
        "Monster, '  '"
    }, nullValues = "NULL")
    void deberiaRechazarCredencialesVacias(String usuario, String contrasenia) {
        assertThrows(IllegalArgumentException.class,
                () -> controlador.autenticar(new LoginModelo(usuario, contrasenia)));

        assertNull(clienteWs.operacionLlamada);
    }

    @ParameterizedTest
    @CsvSource(value = {
        "NULL, Nueva123",
        "Monster9, NULL",
        "'  ', Nueva123"
    }, nullValues = "NULL")
    void deberiaRechazarCambioDeContraseniaIncompleto(String actual, String nueva) {
        assertThrows(IllegalArgumentException.class,
                () -> controlador.cambiarContrasenia(actual, nueva));

        assertNull(clienteWs.operacionLlamada);
    }

    @Test
    void deberiaRechazarNuevaContraseniaIgualALaActual() {
        IllegalArgumentException ex = assertThrows(IllegalArgumentException.class,
                () -> controlador.cambiarContrasenia("Monster9", "Monster9"));

        assertEquals("La nueva contrasenia debe ser diferente a la actual.", ex.getMessage());
        assertNull(clienteWs.operacionLlamada);
    }

    @Test
    void deberiaCerrarSesionEnElCliente() {
        controlador.cerrarSesion();

        assertEquals("cerrarSesion", clienteWs.operacionLlamada);
    }
}
