package ec.edu.monster.service;

import ec.edu.monster.catalog.CatalogoUnidades;
import ec.edu.monster.conversion.MotorConversion;
import ec.edu.monster.history.GestorHistorial;
import ec.edu.monster.model.CategoriaDTO;
import ec.edu.monster.model.RespuestaAutenticacion;
import ec.edu.monster.model.RespuestaCatalogo;
import ec.edu.monster.model.RespuestaConversion;
import ec.edu.monster.model.RespuestaHistorial;
import ec.edu.monster.security.ResultadoValidacionToken;
import ec.edu.monster.security.ServicioSeguridad;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import jakarta.jws.WebService;
import java.util.Collections;
import java.util.List;

/**
 * RNF-01: Arquitectura Orientada a Servicios (SOA).
 * RNF-02: Web Service SOAP implementado en Java con especificación JAX-WS para Payara Server.
 * RNF-04: Multi-Canalidad (consumido por Consola, Swing, Web y Móvil Android).
 * RNF-05: Desacoplamiento de Capas (service -> security / catalog / conversion).
 * 
 * @author GR08 - Arquitectura de Software
 */
@WebService(
    serviceName = "WSConversorUnidades",
    portName = "WSConversorUnidadesPort",
    targetNamespace = "http://monster.edu.ec/services"
)
public class WSConversorUnidades {

    private final ServicioSeguridad servicioSeguridad = new ServicioSeguridad();
    private final CatalogoUnidades catalogoUnidades = new CatalogoUnidades();
    private final MotorConversion motorConversion = new MotorConversion(servicioSeguridad, catalogoUnidades);

    /**
     * RF-01: Autenticación de Usuario y Emisión de Token (CU-01).
     * El usuario se autentica una sola vez y recibe su token de seguridad.
     */
    @WebMethod(operationName = "autenticar")
    public RespuestaAutenticacion autenticar(
            @WebParam(name = "usuario") String usuario, 
            @WebParam(name = "clave") String clave) {
        return servicioSeguridad.autenticar(usuario, clave);
    }

    /**
     * RF-02: Consulta de Catálogo y Unidades (CU-02, CU-03).
     * Retorna todas las categorías con sus unidades de medida asociadas.
     */
    @WebMethod(operationName = "consultarCatalogo")
    public RespuestaCatalogo consultarCatalogo(
            @WebParam(name = "token") String token) {
        
        // Validación obligatoria de token (CU-05, CU-08, CU-09)
        ResultadoValidacionToken val = servicioSeguridad.validarToken(token);
        if (!val.isValido()) {
            return RespuestaCatalogo.error(val.getCodigo(), val.getMensaje());
        }

        List<CategoriaDTO> categorias = catalogoUnidades.obtenerCatalogoCompleto();
        return RespuestaCatalogo.exitoso(categorias);
    }

    /**
     * RF-02: Consulta de unidades pertenecientes a una categoría específica (CU-03).
     */
    @WebMethod(operationName = "consultarUnidadesPorCategoria")
    public RespuestaCatalogo consultarUnidadesPorCategoria(
            @WebParam(name = "categoria") String categoria, 
            @WebParam(name = "token") String token) {
        
        ResultadoValidacionToken val = servicioSeguridad.validarToken(token);
        if (!val.isValido()) {
            return RespuestaCatalogo.error(val.getCodigo(), val.getMensaje());
        }

        CategoriaDTO cat = catalogoUnidades.obtenerPorCategoria(categoria);
        if (cat == null) {
            return RespuestaCatalogo.error("CAT-001", "La categoría '" + categoria + "' no existe en el catálogo.");
        }

        return RespuestaCatalogo.exitoso(Collections.singletonList(cat));
    }

    /**
     * RF-05: Operación unificada de conversión (CU-04).
     * Valida token (CU-05), parámetros (CU-06), ejecuta motor y retorna respuesta estructurada (CU-07).
     */
    @WebMethod(operationName = "convertir")
    public RespuestaConversion convertir(
            @WebParam(name = "valor") double valor,
            @WebParam(name = "unidadOrigen") String unidadOrigen,
            @WebParam(name = "unidadDestino") String unidadDestino,
            @WebParam(name = "token") String token) {
        return motorConversion.procesarConversion(valor, unidadOrigen, unidadDestino, token);
    }

    /**
     * Operación especializada para conversión de Longitud.
     */
    @WebMethod(operationName = "convertirLongitud")
    public RespuestaConversion convertirLongitud(
            @WebParam(name = "valor") double valor,
            @WebParam(name = "unidadOrigen") String unidadOrigen,
            @WebParam(name = "unidadDestino") String unidadDestino,
            @WebParam(name = "token") String token) {
        return motorConversion.procesarConversion(valor, unidadOrigen, unidadDestino, token);
    }

    /**
     * Operación especializada para conversión de Masa.
     */
    @WebMethod(operationName = "convertirMasa")
    public RespuestaConversion convertirMasa(
            @WebParam(name = "valor") double valor,
            @WebParam(name = "unidadOrigen") String unidadOrigen,
            @WebParam(name = "unidadDestino") String unidadDestino,
            @WebParam(name = "token") String token) {
        return motorConversion.procesarConversion(valor, unidadOrigen, unidadDestino, token);
    }

    /**
     * Operación especializada para conversión de Temperatura.
     */
    @WebMethod(operationName = "convertirTemperatura")
    public RespuestaConversion convertirTemperatura(
            @WebParam(name = "valor") double valor,
            @WebParam(name = "unidadOrigen") String unidadOrigen,
            @WebParam(name = "unidadDestino") String unidadDestino,
            @WebParam(name = "token") String token) {
        return motorConversion.procesarConversion(valor, unidadOrigen, unidadDestino, token);
    }

    /**
     * Consulta el historial global de conversiones sincronizado para todos los clientes (Web, Móvil, Escritorio, Consola).
     */
    @WebMethod(operationName = "consultarHistorial")
    public RespuestaHistorial consultarHistorial(
            @WebParam(name = "token") String token) {
        
        ResultadoValidacionToken val = servicioSeguridad.validarToken(token);
        if (!val.isValido()) {
            return RespuestaHistorial.error(val.getCodigo(), val.getMensaje());
        }

        List<ec.edu.monster.model.RegistroHistorialDTO> items = GestorHistorial.getInstance().obtenerHistorial();
        return RespuestaHistorial.exitoso(items);
    }

    /**
     * Limpia el historial global de conversiones.
     */
    @WebMethod(operationName = "limpiarHistorial")
    public RespuestaConversion limpiarHistorial(
            @WebParam(name = "token") String token) {
        
        ResultadoValidacionToken val = servicioSeguridad.validarToken(token);
        if (!val.isValido()) {
            return RespuestaConversion.error(val.getCodigo(), val.getMensaje());
        }

        GestorHistorial.getInstance().limpiarHistorial();
        return new RespuestaConversion("OK", "200", "Historial limpiado correctamente.", 0.0);
    }
}
