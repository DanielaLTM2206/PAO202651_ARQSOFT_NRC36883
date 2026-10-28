package ec.edu.monster.pruebas;

import ec.edu.monster.util.ConfiguracionServidor;
import ec.edu.monster.util.OrigenUrl;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.io.TempDir;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.junit.jupiter.params.provider.NullAndEmptySource;
import org.junit.jupiter.params.provider.ValueSource;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;

import static org.junit.jupiter.api.Assertions.*;

public class ConfiguracionServidorTest {

    @TempDir
    Path carpeta;

    private String propiedad;
    private String variable;

    private Path rutaArchivo() {
        return carpeta.resolve("ConUni_GR8").resolve("servidor_rest.json");
    }

    private ConfiguracionServidor crear() {
        return new ConfiguracionServidor(rutaArchivo(), () -> propiedad, () -> variable);
    }

    @Test
    void deberiaUsarLocalhostSiNoHayNadaConfigurado() {
        ConfiguracionServidor configuracion = crear();

        assertEquals(ConfiguracionServidor.URL_POR_DEFECTO, configuracion.getUrlActual());
        assertEquals(OrigenUrl.POR_DEFECTO, configuracion.getOrigen());
    }

    @Test
    void deberiaGuardarYLeerLaDireccion() {
        crear().guardar("http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api");

        ConfiguracionServidor otraInstancia = crear();
        assertEquals("http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api", otraInstancia.getUrlActual());
        assertEquals(OrigenUrl.GUARDADA, otraInstancia.getOrigen());
    }

    @Test
    void deberiaDarPrioridadALaVariableYLuegoALaPropiedad() {
        crear().guardar("http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api");
        variable = "http://10.0.0.7:8080/WS_ConUni_Java_RESTFUL/api";

        assertEquals("http://10.0.0.7:8080/WS_ConUni_Java_RESTFUL/api", crear().getUrlActual());
        assertEquals(OrigenUrl.VARIABLE_ENTORNO, crear().getOrigen());

        propiedad = "http://10.0.0.8:8080/WS_ConUni_Java_RESTFUL/api";
        assertEquals("http://10.0.0.8:8080/WS_ConUni_Java_RESTFUL/api", crear().getUrlActual());
    }

    @ParameterizedTest
    @ValueSource(strings = {"", "   ", "no es una url", "ftp://192.168.1.25"})
    void deberiaIgnorarUnaVariableInvalida(String valor) {
        variable = valor;

        assertEquals(ConfiguracionServidor.URL_POR_DEFECTO, crear().getUrlActual());
    }

    @Test
    void deberiaVolverALocalhostAlRestablecer() {
        ConfiguracionServidor configuracion = crear();
        configuracion.guardar("http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api");

        configuracion.restablecer();

        assertEquals(ConfiguracionServidor.URL_POR_DEFECTO, configuracion.getUrlActual());
        assertFalse(Files.exists(rutaArchivo()));
    }

    @ParameterizedTest
    @ValueSource(strings = {"{ esto no es json", "{\"Url\":\"texto cualquiera\"}", "{}"})
    void deberiaIgnorarUnArchivoDaniado(String contenido) throws IOException {
        Files.createDirectories(rutaArchivo().getParent());
        Files.writeString(rutaArchivo(), contenido);

        assertEquals(ConfiguracionServidor.URL_POR_DEFECTO, crear().getUrlActual());
    }

    @Test
    void deberiaRechazarGuardarUnaDireccionInvalida() {
        assertThrows(IllegalArgumentException.class, () -> crear().guardar("no es una url"));
        assertFalse(Files.exists(rutaArchivo()));
    }

    @ParameterizedTest
    @CsvSource({
        "192.168.1.25, http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api",
        "'  192.168.1.25  ', http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api",
        "192.168.1.25:9090, http://192.168.1.25:9090/WS_ConUni_Java_RESTFUL/api",
        "http://192.168.1.25, http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api",
        "http://192.168.1.25/, http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api",
        "PC-STEVEN, http://pc-steven:8080/WS_ConUni_Java_RESTFUL/api",
        "localhost, http://localhost:8080/WS_ConUni_Java_RESTFUL/api",
        "http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api, http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api",
        "http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api/, http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api",
        "https://servidor.local/otra/api, https://servidor.local/otra/api"
    })
    void deberiaCompletarLaDireccion(String entrada, String esperado) {
        assertEquals(esperado, ConfiguracionServidor.completar(entrada));
    }

    @ParameterizedTest
    @NullAndEmptySource
    @ValueSource(strings = {"   ", "ftp://192.168.1.25", "192.168.1", "192.168.1.256", "192.168.1.25.7",
        "http://", "http://mi servidor", "192.168.1.25:abc"})
    void deberiaRechazarDireccionesInvalidas(String entrada) {
        IllegalArgumentException error = assertThrows(IllegalArgumentException.class,
                () -> ConfiguracionServidor.completar(entrada));
        assertTrue(error.getMessage().contains("192.168.1.25"));
    }

    @ParameterizedTest
    @CsvSource(value = {
        "http://192.168.1.25:8080/WS_ConUni_Java_RESTFUL/api, true",
        "https://localhost/api, true",
        "192.168.1.25, false",
        "ftp://192.168.1.25, false",
        "NULL, false"
    }, nullValues = "NULL")
    void deberiaValidarUrls(String url, boolean esperado) {
        assertEquals(esperado, ConfiguracionServidor.esUrlValida(url));
    }
}
