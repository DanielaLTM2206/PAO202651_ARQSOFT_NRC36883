package ec.edu.monster.controladores.ws;

import ec.edu.monster.modelos.utilidades.enums.UnidadTemperatura;
import ec.edu.monster.modelos.utilidades.enums.UnidadMasa;
import ec.edu.monster.modelos.utilidades.enums.UnidadLongitud;
import ec.edu.monster.modelos.utilidades.ConversorTemperatura;
import ec.edu.monster.modelos.utilidades.ConversorMasa;
import ec.edu.monster.modelos.utilidades.ConversorLongitud;
import ec.edu.monster.servicios.ServicioConversor;
import ec.edu.monster.servicios.ServicioUnidades;
import ec.edu.monster.servicios.ServicioHistorial;
import ec.edu.monster.modelos.utilidades.mapeadores.UnidadMapper;
import ec.edu.monster.seguridad.AdministradorCredenciales;
import ec.edu.monster.seguridad.AdministradorTokens;
import ec.edu.monster.modelos.dto.LoginSolicitud;
import ec.edu.monster.modelos.dto.LoginRespuesta;
import ec.edu.monster.modelos.dto.CambioContrasenaSolicitud;
import ec.edu.monster.modelos.dto.ConversionSolicitud;
import ec.edu.monster.modelos.dto.ConversionRespuesta;
import ec.edu.monster.modelos.dto.MensajeRespuesta;
import ec.edu.monster.modelos.dto.UnidadesCategoriaRespuesta;
import ec.edu.monster.modelos.dto.RegistroConversionRespuesta;
import ec.edu.monster.controladores.filtros.Asegurado;
import ec.edu.monster.excepciones.ExcepcionAutenticacion;
import ec.edu.monster.excepciones.ExcepcionValidacion;

import jakarta.ws.rs.Path;
import jakarta.ws.rs.POST;
import jakarta.ws.rs.GET;
import jakarta.ws.rs.DELETE;
import jakarta.ws.rs.PathParam;
import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.MediaType;

import java.util.List;
import java.util.Locale;

@Path("/")
@Consumes(MediaType.APPLICATION_JSON)
@Produces(MediaType.APPLICATION_JSON)
public class WSConversorUnidades {

    private final ServicioConversor<UnidadLongitud> servicioLongitud =
            new ServicioConversor<>(new ConversorLongitud());

    private final ServicioConversor<UnidadMasa> servicioMasa =
            new ServicioConversor<>(new ConversorMasa());

    private final ServicioConversor<UnidadTemperatura> servicioTemperatura =
            new ServicioConversor<>(new ConversorTemperatura());

    private final ServicioUnidades servicioUnidades = new ServicioUnidades();

    // CU-01 Autenticarse (sin @Asegurado: es la única operación pública)
    @POST
    @Path("seguridad/login")
    public LoginRespuesta login(LoginSolicitud solicitud) {

        if (solicitud == null || esVacio(solicitud.getUsuario()) || esVacio(solicitud.getContrasena())) {
            throw new ExcepcionValidacion("El usuario y la contraseña son obligatorios");
        }
        if (!AdministradorCredenciales.validarCredenciales(solicitud.getUsuario(), solicitud.getContrasena())) {
            throw new ExcepcionAutenticacion("Credenciales incorrectas");
        }
        return new LoginRespuesta(AdministradorTokens.generarToken());
    }

    @POST
    @Path("seguridad/cambiar-contrasena")
    @Asegurado
    public MensajeRespuesta cambiarContrasenia(CambioContrasenaSolicitud solicitud) {

        if (solicitud == null || esVacio(solicitud.getContrasenaActual())) {
            throw new ExcepcionValidacion("La contraseña actual es obligatoria");
        }
        AdministradorCredenciales.cambiarContrasenia(solicitud.getContrasenaActual(), solicitud.getNuevaContrasena());
        return new MensajeRespuesta("Contraseña actualizada correctamente");
    }

    // CU-04 Realizar conversión
    @POST
    @Path("conversiones/longitud")
    @Asegurado
    public ConversionRespuesta convertirLongitud(ConversionSolicitud solicitud) {
        validarSolicitud(solicitud);

        UnidadLongitud origen = UnidadMapper.toLongitud(solicitud.getUnidadOrigen());
        UnidadLongitud destino = UnidadMapper.toLongitud(solicitud.getUnidadDestino());

        double resultado = servicioLongitud.convertir(solicitud.getValor(), origen, destino);
        ServicioHistorial.registrar("Longitud", solicitud.getValor(), origen.name(), resultado, destino.name());
        return new ConversionRespuesta(resultado);
    }

    @POST
    @Path("conversiones/masa")
    @Asegurado
    public ConversionRespuesta convertirMasa(ConversionSolicitud solicitud) {
        validarSolicitud(solicitud);

        UnidadMasa origen = UnidadMapper.toMasa(solicitud.getUnidadOrigen());
        UnidadMasa destino = UnidadMapper.toMasa(solicitud.getUnidadDestino());

        double resultado = servicioMasa.convertir(solicitud.getValor(), origen, destino);
        ServicioHistorial.registrar("Masa", solicitud.getValor(), origen.name(), resultado, destino.name());
        return new ConversionRespuesta(resultado);
    }

    @POST
    @Path("conversiones/temperatura")
    @Asegurado
    public ConversionRespuesta convertirTemperatura(ConversionSolicitud solicitud) {
        validarSolicitud(solicitud);

        UnidadTemperatura origen = UnidadMapper.toTemperatura(solicitud.getUnidadOrigen());
        UnidadTemperatura destino = UnidadMapper.toTemperatura(solicitud.getUnidadDestino());

        double resultado = servicioTemperatura.convertir(solicitud.getValor(), origen, destino);
        ServicioHistorial.registrar("Temperatura", solicitud.getValor(), origen.name(), resultado, destino.name());
        return new ConversionRespuesta(resultado);
    }

    // Historial compartido: lo ven todos los clientes (escritorio, web y móvil)
    @GET
    @Path("conversiones/historial")
    @Asegurado
    public List<RegistroConversionRespuesta> consultarHistorial() {
        return ServicioHistorial.obtener();
    }

    @DELETE
    @Path("conversiones/historial")
    @Asegurado
    public MensajeRespuesta limpiarHistorial() {
        ServicioHistorial.limpiar();
        return new MensajeRespuesta("Historial de conversiones borrado");
    }

    // CU-02 Consultar catálogo de unidades
    @GET
    @Path("unidades")
    @Asegurado
    public List<UnidadesCategoriaRespuesta> consultarCatalogo() {
        return ServicioUnidades.CATEGORIAS.stream()
                .map(this::consultarUnidadesPorCategoria)
                .toList();
    }

    // CU-03 Consultar unidades por categoría
    @GET
    @Path("unidades/{categoria}")
    @Asegurado
    public UnidadesCategoriaRespuesta consultarUnidadesPorCategoria(@PathParam("categoria") String categoria) {
        List<String> unidades = servicioUnidades.obtenerUnidades(categoria);
        return new UnidadesCategoriaRespuesta(categoria.trim().toLowerCase(Locale.ROOT), unidades);
    }

    // CU-06 Validar datos de conversión (las unidades las valida UnidadMapper)
    private static void validarSolicitud(ConversionSolicitud solicitud) {
        if (solicitud == null) {
            throw new ExcepcionValidacion("El cuerpo de la solicitud es obligatorio");
        }
        if (solicitud.getValor() == null) {
            throw new ExcepcionValidacion("El valor a convertir es obligatorio");
        }
    }

    private static boolean esVacio(String texto) {
        return texto == null || texto.isBlank();
    }
}
