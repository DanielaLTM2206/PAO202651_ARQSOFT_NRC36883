using System.Net.Http.Headers;

namespace ec.edu.monster.Cliente;

/// <summary>
/// Guarda el token de la sesión y lo adjunta a cada petición protegida
/// en el encabezado "Authorization: Bearer".
/// </summary>
public class ManejadorTokenRESTFUL
{
    private readonly IAlmacenToken almacen;

    public ManejadorTokenRESTFUL(IAlmacenToken almacen)
    {
        this.almacen = almacen;
    }

    public void GuardarToken(string token)
    {
        almacen.Token = token;
    }

    public string? ObtenerToken() => almacen.Token;

    public bool HaySesionActiva => !string.IsNullOrWhiteSpace(almacen.Token);

    public void LimpiarToken()
    {
        almacen.Token = null;
    }

    public void AdjuntarToken(HttpRequestMessage peticion)
    {
        if (!HaySesionActiva)
        {
            throw new ExcepcionClienteWS(401, "No hay sesión activa. Debe autenticarse primero.");
        }
        peticion.Headers.Authorization = new AuthenticationHeaderValue("Bearer", almacen.Token);
    }
}
