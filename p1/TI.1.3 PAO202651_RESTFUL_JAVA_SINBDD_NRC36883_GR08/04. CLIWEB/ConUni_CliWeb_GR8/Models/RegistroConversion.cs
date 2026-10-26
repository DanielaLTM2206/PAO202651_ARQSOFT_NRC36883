namespace ec.edu.monster.Models;

/// <summary>
/// Una conversión realizada con éxito, tal como se guarda en el historial.
/// </summary>
public record RegistroConversion(
    DateTime FechaHora,
    TipoConversion Tipo,
    double Valor,
    string UnidadOrigen,
    double Resultado,
    string UnidadDestino);
