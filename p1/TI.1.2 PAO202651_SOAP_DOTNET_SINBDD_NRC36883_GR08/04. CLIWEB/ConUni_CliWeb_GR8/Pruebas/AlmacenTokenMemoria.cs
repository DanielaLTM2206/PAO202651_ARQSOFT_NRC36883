using ec.edu.monster.Cliente;

namespace ec.edu.monster.Pruebas;

/// <summary>
/// Guarda el token en memoria para probar el cliente sin una sesión de navegador.
/// </summary>
internal class AlmacenTokenMemoria : IAlmacenToken
{
    public string? Token { get; set; }
}
