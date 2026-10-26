using System.Diagnostics;
using ec.edu.monster.Cliente;
using ec.edu.monster.Filtros;
using ec.edu.monster.Models;
using ec.edu.monster.Servicios;
using ec.edu.monster.Sesion;
using Microsoft.AspNetCore.Mvc;

namespace ec.edu.monster.Controllers;

public class CuentaController : Controller
{
    private readonly ServicioCuenta servicioCuenta;
    private readonly AlmacenSesion sesion;
    private readonly string urlServidor;

    public CuentaController(ServicioCuenta servicioCuenta, AlmacenSesion sesion, IConfiguration configuracion)
    {
        this.servicioCuenta = servicioCuenta;
        this.sesion = sesion;
        urlServidor = configuracion["ServidorRest:UrlBase"] ?? string.Empty;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (sesion.HaySesionActiva)
        {
            return RedirectToAction("Index", "Conversor");
        }
        return View(new LoginModelo { UrlServidor = urlServidor });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModelo modelo)
    {
        try
        {
            await servicioCuenta.AutenticarAsync(modelo);

            sesion.Usuario = modelo.Usuario!.Trim();
            return RedirectToAction("Index", "Conversor");
        }
        catch (Exception ex) when (ex is ExcepcionClienteWS or ArgumentException)
        {
            modelo.Error = ex.Message;
        }

        // Por seguridad, la contraseña no se devuelve a la página
        modelo.Contrasenia = null;
        modelo.UrlServidor = urlServidor;
        return View(modelo);
    }

    [HttpPost]
    public IActionResult CerrarSesion()
    {
        servicioCuenta.CerrarSesion();
        sesion.Limpiar();
        TempData["Exito"] = "Sesión cerrada correctamente.";
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    [RequiereSesion]
    public IActionResult CambiarContrasenia()
    {
        return View(new CambioContraseniaModelo());
    }

    [HttpPost]
    [RequiereSesion]
    public async Task<IActionResult> CambiarContrasenia(CambioContraseniaModelo modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.ContraseniaActual) || string.IsNullOrWhiteSpace(modelo.ContraseniaNueva)
                || string.IsNullOrWhiteSpace(modelo.Confirmacion))
        {
            return VistaConError(modelo, "Complete los tres campos.");
        }
        if (modelo.ContraseniaNueva != modelo.Confirmacion)
        {
            return VistaConError(modelo, "La confirmación no coincide con la nueva contraseña.");
        }

        try
        {
            string mensaje = await servicioCuenta.CambiarContraseniaAsync(modelo.ContraseniaActual, modelo.ContraseniaNueva);
            TempData["Exito"] = mensaje;
            return RedirectToAction("Index", "Conversor");
        }
        catch (ExcepcionClienteWS ex) when (ex.EsErrorAutenticacion)
        {
            sesion.Limpiar();
            TempData["Aviso"] = ex.Message + " Inicie sesión nuevamente.";
            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex) when (ex is ExcepcionClienteWS or ArgumentException)
        {
            return VistaConError(modelo, ex.Message);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Las contraseñas no se devuelven a la página: el usuario las vuelve a escribir
    private ViewResult VistaConError(CambioContraseniaModelo modelo, string mensaje)
    {
        return View(nameof(CambiarContrasenia), new CambioContraseniaModelo { Error = mensaje });
    }
}
