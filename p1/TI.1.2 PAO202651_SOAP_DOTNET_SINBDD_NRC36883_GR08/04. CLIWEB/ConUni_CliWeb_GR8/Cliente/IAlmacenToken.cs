namespace ec.edu.monster.Cliente;

/// <summary>
/// Lugar donde se guarda el token de la sesión. En la aplicación web es la sesión del navegador
/// de cada usuario (así no se mezclan los tokens de distintas personas); en las pruebas, memoria.
/// </summary>
public interface IAlmacenToken
{
    string? Token { get; set; }
}
