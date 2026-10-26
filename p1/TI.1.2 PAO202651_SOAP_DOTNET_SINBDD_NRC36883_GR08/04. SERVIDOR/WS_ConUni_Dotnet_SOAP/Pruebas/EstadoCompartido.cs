using Xunit;

namespace ec.edu.monster.Pruebas;

/// <summary>
/// El historial y las credenciales son estáticos (viven en memoria del servidor).
/// Las clases de prueba que los modifican usan esta colección para no ejecutarse en paralelo.
/// </summary>
[CollectionDefinition(Nombre)]
public class EstadoCompartido
{
    public const string Nombre = "EstadoCompartido";
}
