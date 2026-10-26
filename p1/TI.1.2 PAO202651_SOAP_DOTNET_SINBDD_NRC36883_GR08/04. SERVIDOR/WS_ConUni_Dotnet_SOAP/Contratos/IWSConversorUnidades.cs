using CoreWCF;
using ec.edu.monster.Modelos.Dto;

namespace ec.edu.monster.Contratos;

/// <summary>
/// Contrato SOAP del servicio. A partir de esta interfaz CoreWCF genera el WSDL.
/// Todas las operaciones, excepto Login, exigen la cabecera SOAP "Token" (la valida InspectorAutenticacion).
/// Los errores se devuelven como el fault tipado ErrorServicio {Codigo, Mensaje}.
/// </summary>
[ServiceContract(Namespace = ConstantesWS.Espacio)]
public interface IWSConversorUnidades
{
    // CU-01 Autenticarse (operación pública)
    [OperationContract]
    [FaultContract(typeof(ErrorServicio))]
    LoginRespuesta Login(
        [MessageParameter(Name = "Usuario")] string? usuario,
        [MessageParameter(Name = "Contrasena")] string? contrasena);

    [OperationContract]
    [FaultContract(typeof(ErrorServicio))]
    MensajeRespuesta CambiarContrasenia(
        [MessageParameter(Name = "ContrasenaActual")] string? contrasenaActual,
        [MessageParameter(Name = "NuevaContrasena")] string? nuevaContrasena);

    // CU-04 Realizar conversión
    [OperationContract]
    [FaultContract(typeof(ErrorServicio))]
    ConversionRespuesta ConvertirLongitud(
        [MessageParameter(Name = "Valor")] double valor,
        [MessageParameter(Name = "UnidadOrigen")] string? unidadOrigen,
        [MessageParameter(Name = "UnidadDestino")] string? unidadDestino);

    [OperationContract]
    [FaultContract(typeof(ErrorServicio))]
    ConversionRespuesta ConvertirMasa(
        [MessageParameter(Name = "Valor")] double valor,
        [MessageParameter(Name = "UnidadOrigen")] string? unidadOrigen,
        [MessageParameter(Name = "UnidadDestino")] string? unidadDestino);

    [OperationContract]
    [FaultContract(typeof(ErrorServicio))]
    ConversionRespuesta ConvertirTemperatura(
        [MessageParameter(Name = "Valor")] double valor,
        [MessageParameter(Name = "UnidadOrigen")] string? unidadOrigen,
        [MessageParameter(Name = "UnidadDestino")] string? unidadDestino);

    // CU-02 Consultar catálogo de unidades
    [OperationContract]
    [FaultContract(typeof(ErrorServicio))]
    List<UnidadesCategoriaRespuesta> ConsultarCatalogo();

    // CU-03 Consultar unidades por categoría
    [OperationContract]
    [FaultContract(typeof(ErrorServicio))]
    UnidadesCategoriaRespuesta ConsultarUnidadesPorCategoria(
        [MessageParameter(Name = "Categoria")] string? categoria);

    // Historial compartido: lo ven todos los clientes (escritorio, web y móvil)
    [OperationContract]
    [FaultContract(typeof(ErrorServicio))]
    List<RegistroConversionRespuesta> ConsultarHistorial();

    [OperationContract]
    [FaultContract(typeof(ErrorServicio))]
    MensajeRespuesta LimpiarHistorial();
}
