using CoreWCF;
using ec.edu.monster.Contratos;
using ec.edu.monster.Excepciones;
using ec.edu.monster.Modelos.Dto;
using ec.edu.monster.Modelos.Utilidades;
using ec.edu.monster.Modelos.Utilidades.Enums;
using ec.edu.monster.Modelos.Utilidades.Mapeadores;
using ec.edu.monster.Seguridad;

namespace ec.edu.monster.Servicios;

/// <summary>
/// Implementación del servicio SOAP. El token ya llega validado por InspectorAutenticacion.
/// Cada operación se ejecuta dentro de Ejecutar, que convierte los errores esperados en el fault ErrorServicio.
/// </summary>
[ServiceBehavior(Name = "WSConversorUnidades", Namespace = ConstantesWS.Espacio,
        InstanceContextMode = InstanceContextMode.PerCall)]
public class WSConversorUnidades : IWSConversorUnidades
{
    private readonly ServicioConversor<UnidadLongitud> servicioLongitud = new(new ConversorLongitud());
    private readonly ServicioConversor<UnidadMasa> servicioMasa = new(new ConversorMasa());
    private readonly ServicioConversor<UnidadTemperatura> servicioTemperatura = new(new ConversorTemperatura());

    // CU-01 Autenticarse (única operación pública)
    public LoginRespuesta Login(string? usuario, string? contrasena) => Ejecutar(() =>
    {
        if (EsVacio(usuario) || EsVacio(contrasena))
        {
            throw new ExcepcionValidacion("El usuario y la contraseña son obligatorios");
        }
        if (!AdministradorCredenciales.ValidarCredenciales(usuario!, contrasena!))
        {
            throw new ExcepcionAutenticacion("Credenciales incorrectas");
        }
        return new LoginRespuesta { Token = AdministradorTokens.GenerarToken() };
    });

    public MensajeRespuesta CambiarContrasenia(string? contrasenaActual, string? nuevaContrasena) => Ejecutar(() =>
    {
        if (EsVacio(contrasenaActual))
        {
            throw new ExcepcionValidacion("La contraseña actual es obligatoria");
        }
        AdministradorCredenciales.CambiarContrasenia(contrasenaActual!, nuevaContrasena);
        return new MensajeRespuesta { Mensaje = "Contraseña actualizada correctamente" };
    });

    // CU-04 Realizar conversión (las unidades las valida UnidadMapper)
    public ConversionRespuesta ConvertirLongitud(double valor, string? unidadOrigen, string? unidadDestino) => Ejecutar(() =>
    {
        UnidadLongitud origen = UnidadMapper.ALongitud(unidadOrigen);
        UnidadLongitud destino = UnidadMapper.ALongitud(unidadDestino);

        double resultado = servicioLongitud.Convertir(valor, origen, destino);
        ServicioHistorial.Registrar("Longitud", valor, origen.ToString(), resultado, destino.ToString());
        return new ConversionRespuesta { ValorConvertido = resultado };
    });

    public ConversionRespuesta ConvertirMasa(double valor, string? unidadOrigen, string? unidadDestino) => Ejecutar(() =>
    {
        UnidadMasa origen = UnidadMapper.AMasa(unidadOrigen);
        UnidadMasa destino = UnidadMapper.AMasa(unidadDestino);

        double resultado = servicioMasa.Convertir(valor, origen, destino);
        ServicioHistorial.Registrar("Masa", valor, origen.ToString(), resultado, destino.ToString());
        return new ConversionRespuesta { ValorConvertido = resultado };
    });

    public ConversionRespuesta ConvertirTemperatura(double valor, string? unidadOrigen, string? unidadDestino) => Ejecutar(() =>
    {
        UnidadTemperatura origen = UnidadMapper.ATemperatura(unidadOrigen);
        UnidadTemperatura destino = UnidadMapper.ATemperatura(unidadDestino);

        double resultado = servicioTemperatura.Convertir(valor, origen, destino);
        ServicioHistorial.Registrar("Temperatura", valor, origen.ToString(), resultado, destino.ToString());
        return new ConversionRespuesta { ValorConvertido = resultado };
    });

    // CU-02 Consultar catálogo de unidades
    public List<UnidadesCategoriaRespuesta> ConsultarCatalogo() => Ejecutar(() =>
    {
        return ServicioUnidades.Categorias.Select(CrearCategoria).ToList();
    });

    // CU-03 Consultar unidades por categoría
    public UnidadesCategoriaRespuesta ConsultarUnidadesPorCategoria(string? categoria) => Ejecutar(() =>
    {
        return CrearCategoria(categoria);
    });

    // Historial compartido: lo ven todos los clientes (escritorio, web y móvil)
    public List<RegistroConversionRespuesta> ConsultarHistorial() => Ejecutar(() =>
    {
        return ServicioHistorial.Obtener().ToList();
    });

    public MensajeRespuesta LimpiarHistorial() => Ejecutar(() =>
    {
        ServicioHistorial.Limpiar();
        return new MensajeRespuesta { Mensaje = "Historial de conversiones borrado" };
    });

    private static UnidadesCategoriaRespuesta CrearCategoria(string? categoria)
    {
        List<string> unidades = ServicioUnidades.ObtenerUnidades(categoria);
        return new UnidadesCategoriaRespuesta
        {
            Categoria = ServicioUnidades.NormalizarCategoria(categoria),
            Unidades = unidades
        };
    }

    private static T Ejecutar<T>(Func<T> operacion) where T : class
    {
        EstadoSolicitud? estado = EstadoSolicitud.Actual;

        // Dentro de WCF el inspector siempre crea el estado; si faltara, no se atiende la solicitud sin validar el token
        if (estado is null && OperationContext.Current is not null)
        {
            throw new InvalidOperationException("La solicitud no pasó por InspectorAutenticacion.");
        }
        return EjecutarConEstado(estado, operacion);
    }

    /// <summary>
    /// Ejecuta la operación y anota los errores esperados en el estado de la solicitud, sin dejar escapar
    /// la excepción: InspectorAutenticacion responde con el fault ErrorServicio. Fuera de WCF (estado null,
    /// por ejemplo en las pruebas) las excepciones se propagan normalmente.
    /// </summary>
    internal static T EjecutarConEstado<T>(EstadoSolicitud? estado, Func<T> operacion) where T : class
    {
        if (estado is null)
        {
            return operacion();
        }
        // Token ausente, inválido o expirado: la operación no se ejecuta
        if (estado.TieneError)
        {
            return null!;
        }

        try
        {
            return operacion();
        }
        catch (ExcepcionValidacion ex)
        {
            estado.RegistrarError(ErrorServicio.Validacion, ex.Message);
        }
        catch (ExcepcionAutenticacion ex)
        {
            estado.RegistrarError(ErrorServicio.Autenticacion, ex.Message);
        }
        return null!;
    }

    private static bool EsVacio(string? texto) => string.IsNullOrWhiteSpace(texto);
}
