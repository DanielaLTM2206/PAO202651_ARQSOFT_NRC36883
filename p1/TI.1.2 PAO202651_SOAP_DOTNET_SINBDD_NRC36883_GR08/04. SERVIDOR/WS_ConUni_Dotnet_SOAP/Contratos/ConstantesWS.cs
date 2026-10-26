namespace ec.edu.monster.Contratos;

/// <summary>
/// Valores que el servidor y los clientes deben compartir para entenderse.
/// </summary>
public static class ConstantesWS
{
    // Espacio de nombres XML del contrato, de los DTO y de la cabecera del token
    public const string Espacio = "http://monster.edu.ec/ws";

    // Ruta del endpoint SOAP dentro del servidor
    public const string Ruta = "/WSConversorUnidades.svc";

    // Cabecera SOAP donde viaja el token: <Token xmlns="http://monster.edu.ec/ws">...</Token>
    public const string CabeceraToken = "Token";
}
