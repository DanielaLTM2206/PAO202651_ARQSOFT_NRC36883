using System.Globalization;
using ec.edu.monster.Cliente;
using ReferenciaServicio;
using ec.edu.monster.Modelo;

namespace ec.edu.monster.Controlador;

public class ConversionControlador
{
    private readonly IContratoWSConversor clienteWs;

    public ConversionControlador(IContratoWSConversor clienteWs)
    {
        this.clienteWs = clienteWs;
    }

    // El servidor registra la conversión en el historial compartido al responder
    public async Task<double> ConvertirAsync(ConversionModelo modelo)
    {
        ValidarSolicitud(modelo);

        double valor = modelo.Valor;
        string unidadInicial = modelo.UnidadInicial!;
        string unidadFinal = modelo.UnidadFinal!;

        return await (modelo.TipoConversion switch
        {
            TipoConversion.Longitud => clienteWs.ConvertirLongitudAsync(valor, unidadInicial, unidadFinal),
            TipoConversion.Masa => clienteWs.ConvertirMasaAsync(valor, unidadInicial, unidadFinal),
            TipoConversion.Temperatura => clienteWs.ConvertirTemperaturaAsync(valor, unidadInicial, unidadFinal),
            _ => throw new ArgumentException("Debe seleccionar el tipo de conversión.")
        });
    }

    /// <summary>
    /// Historial compartido por todos los clientes: lo guarda el servidor, así que incluye
    /// las conversiones hechas desde el escritorio, la web, el móvil y la consola.
    /// </summary>
    public async Task<IReadOnlyList<RegistroConversion>> ObtenerHistorialAsync()
    {
        IReadOnlyList<RegistroConversionRespuesta> registros = await clienteWs.ConsultarHistorialAsync();

        var historial = new List<RegistroConversion>();
        foreach (RegistroConversionRespuesta registro in registros)
        {
            if (InterpretarRegistro(registro) is RegistroConversion convertido)
            {
                historial.Add(convertido);
            }
        }
        return historial;
    }

    // Borra el historial en el servidor: deja de verse en todos los clientes
    public Task<string> LimpiarHistorialAsync()
    {
        return clienteWs.LimpiarHistorialAsync();
    }

    // Convierte el registro del mensaje SOAP al modelo; si llega incompleto, se omite en lugar de fallar
    public static RegistroConversion? InterpretarRegistro(RegistroConversionRespuesta registro)
    {
        bool fechaValida = DateTime.TryParse(registro.FechaHora, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime fechaHora);
        bool tipoValido = Enum.TryParse(registro.Tipo, ignoreCase: true, out TipoConversion tipo)
                && Enum.IsDefined(tipo);

        if (!fechaValida || !tipoValido
                || string.IsNullOrWhiteSpace(registro.UnidadOrigen) || string.IsNullOrWhiteSpace(registro.UnidadDestino))
        {
            return null;
        }
        return new RegistroConversion(fechaHora, tipo, registro.Valor, registro.UnidadOrigen,
                registro.Resultado, registro.UnidadDestino);
    }

    // Las unidades se consultan al servidor (catálogo de unidades por categoría)
    public Task<IReadOnlyList<string>> ObtenerUnidadesAsync(TipoConversion tipo)
    {
        return clienteWs.ConsultarUnidadesAsync(tipo.Categoria());
    }

    /// <summary>
    /// Longitud y masa no admiten negativos. La temperatura sí, y el límite del cero
    /// absoluto lo valida el servidor porque depende de la unidad de origen.
    /// </summary>
    public void ValidarValor(TipoConversion? tipo, double valor)
    {
        if (tipo is null)
        {
            throw new ArgumentException("Debe seleccionar el tipo de conversión.");
        }
        if (!double.IsFinite(valor))
        {
            throw new ArgumentException("El valor debe ser un número válido.");
        }
        if (tipo != TipoConversion.Temperatura && valor < 0)
        {
            throw new ArgumentException($"La {tipo.Value.NombreVisible().ToLowerInvariant()} no puede ser negativa.");
        }
    }

    public void ValidarUnidades(string? unidadInicial, string? unidadFinal)
    {
        if (string.IsNullOrWhiteSpace(unidadInicial))
        {
            throw new ArgumentException("La unidad de origen es obligatoria.");
        }
        if (string.IsNullOrWhiteSpace(unidadFinal))
        {
            throw new ArgumentException("La unidad de destino es obligatoria.");
        }
        if (string.Equals(unidadInicial, unidadFinal, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("La unidad de destino debe ser diferente a la unidad de origen.");
        }
    }

    private void ValidarSolicitud(ConversionModelo modelo)
    {
        if (modelo is null)
        {
            throw new ArgumentException("La solicitud de conversión es obligatoria.");
        }
        ValidarValor(modelo.TipoConversion, modelo.Valor);
        ValidarUnidades(modelo.UnidadInicial, modelo.UnidadFinal);
    }
}
