namespace ec.edu.monster.Models;

/// <summary>
/// Datos de la página del conversor: lo que el usuario ingresa y lo que la vista muestra.
/// </summary>
public class ConversorModelo
{
    // Datos que envía el formulario
    public TipoConversion Tipo { get; set; } = TipoConversion.Longitud;
    public string? Valor { get; set; }
    public string? UnidadOrigen { get; set; }
    public string? UnidadDestino { get; set; }

    // Datos que se muestran en la vista
    public IReadOnlyList<string> Unidades { get; set; } = [];
    public string? ResultadoTitulo { get; set; }
    public string? ResultadoValor { get; set; }
    public string? Error { get; set; }
    public string? CampoConError { get; set; }
    public IReadOnlyList<RegistroConversion> Historial { get; set; } = [];
}
