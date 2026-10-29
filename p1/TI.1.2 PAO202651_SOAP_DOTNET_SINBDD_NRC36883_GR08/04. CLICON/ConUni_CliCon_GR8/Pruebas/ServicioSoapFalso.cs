using ReferenciaServicio;

namespace ec.edu.monster.Pruebas;

/// <summary>
/// Reemplaza al proxy SOAP generado (IWSConversorUnidades) para probar ClienteWSConversor
/// sin servidor. Registra la última operación y, si se asigna Error, falla con esa excepción
/// (por ejemplo, un FaultException&lt;ErrorServicio&gt; como los que envía el servidor).
/// </summary>
internal class ServicioSoapFalso : IWSConversorUnidades
{
    public string? OperacionLlamada { get; private set; }
    public object?[] Argumentos { get; private set; } = [];
    public Exception? Error { get; set; }

    public string Token { get; set; } = "token-de-prueba";
    public double ValorConvertido { get; set; } = 1.0;
    public string[] Unidades { get; set; } = ["METRO", "KILOMETRO"];
    public RegistroConversionRespuesta[] Historial { get; set; } = [];

    public Task<LoginRespuesta> LoginAsync(string Usuario, string Contrasena)
        => Responder(new LoginRespuesta { Token = Token }, "Login", Usuario, Contrasena);

    public Task<MensajeRespuesta> CambiarContraseniaAsync(string ContrasenaActual, string NuevaContrasena)
        => Responder(new MensajeRespuesta { Mensaje = "Contraseña actualizada correctamente" },
                "CambiarContrasenia", ContrasenaActual, NuevaContrasena);

    public Task<ConversionRespuesta> ConvertirLongitudAsync(double Valor, string UnidadOrigen, string UnidadDestino)
        => Responder(new ConversionRespuesta { ValorConvertido = ValorConvertido },
                "ConvertirLongitud", Valor, UnidadOrigen, UnidadDestino);

    public Task<ConversionRespuesta> ConvertirMasaAsync(double Valor, string UnidadOrigen, string UnidadDestino)
        => Responder(new ConversionRespuesta { ValorConvertido = ValorConvertido },
                "ConvertirMasa", Valor, UnidadOrigen, UnidadDestino);

    public Task<ConversionRespuesta> ConvertirTemperaturaAsync(double Valor, string UnidadOrigen, string UnidadDestino)
        => Responder(new ConversionRespuesta { ValorConvertido = ValorConvertido },
                "ConvertirTemperatura", Valor, UnidadOrigen, UnidadDestino);

    public Task<UnidadesCategoriaRespuesta[]> ConsultarCatalogoAsync()
        => Responder(Array.Empty<UnidadesCategoriaRespuesta>(), "ConsultarCatalogo");

    public Task<UnidadesCategoriaRespuesta> ConsultarUnidadesPorCategoriaAsync(string Categoria)
        => Responder(new UnidadesCategoriaRespuesta { Categoria = Categoria, Unidades = Unidades },
                "ConsultarUnidadesPorCategoria", Categoria);

    public Task<RegistroConversionRespuesta[]> ConsultarHistorialAsync()
        => Responder(Historial, "ConsultarHistorial");

    public Task<MensajeRespuesta> LimpiarHistorialAsync()
    {
        Historial = [];
        return Responder(new MensajeRespuesta { Mensaje = "Historial de conversiones borrado" }, "LimpiarHistorial");
    }

    private Task<T> Responder<T>(T respuesta, string operacion, params object?[] argumentos)
    {
        OperacionLlamada = operacion;
        Argumentos = argumentos;
        return Error is null ? Task.FromResult(respuesta) : Task.FromException<T>(Error);
    }
}
