using System.Text;
using ec.edu.monster.Vista;

// Cliente de consola del servicio SOAP WSConversorUnidades (patrón MVC).
// La URL del servicio se puede cambiar con la variable de entorno WS_SOAP_URL.
Console.OutputEncoding = Encoding.UTF8;
Console.Title = "ConUni GR08 - Cliente Consola SOAP";

try
{
    await new LoginVista().IniciarAsync();
}
catch (EndOfStreamException)
{
    // El usuario cerró la entrada (Ctrl+Z): se termina sin error
}
