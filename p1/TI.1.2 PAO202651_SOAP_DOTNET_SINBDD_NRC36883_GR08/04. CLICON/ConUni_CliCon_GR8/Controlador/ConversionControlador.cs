using ec.edu.monster.Cliente;
using ec.edu.monster.Modelo;

namespace ec.edu.monster.Controlador;

public class ConversionControlador
{
    private readonly IContratoWSConversor clienteWs;

    public ConversionControlador(IContratoWSConversor clienteWs)
    {
        this.clienteWs = clienteWs;
    }

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

    // Las unidades se consultan al servidor (operación SOAP ConsultarUnidadesPorCategoria)
    public Task<IReadOnlyList<string>> ObtenerUnidadesAsync(TipoConversion? tipo)
    {
        if (tipo is null)
        {
            throw new ArgumentException("Debe seleccionar el tipo de conversión.");
        }
        return clienteWs.ConsultarUnidadesAsync(tipo.Value.Categoria());
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
            throw new ArgumentException("La unidad inicial es obligatoria.");
        }
        if (string.IsNullOrWhiteSpace(unidadFinal))
        {
            throw new ArgumentException("La unidad final es obligatoria.");
        }
        if (string.Equals(unidadInicial, unidadFinal, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("La unidad final debe ser diferente a la unidad inicial.");
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
