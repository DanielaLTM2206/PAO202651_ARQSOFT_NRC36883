using ec.edu.monster.Cliente;
using ec.edu.monster.Filtros;
using ec.edu.monster.Models;
using ec.edu.monster.Servicios;
using ec.edu.monster.Sesion;
using ec.edu.monster.Util;
using Microsoft.AspNetCore.Mvc;

namespace ec.edu.monster.Controllers;

[RequiereSesion]
public class ConversorController : Controller
{
    private readonly ServicioConversion servicioConversion;
    private readonly AlmacenSesion sesion;

    public ConversorController(ServicioConversion servicioConversion, AlmacenSesion sesion)
    {
        this.servicioConversion = servicioConversion;
        this.sesion = sesion;
    }

    // Los parámetros opcionales permiten volver a cargar una conversión del historial
    [HttpGet]
    public async Task<IActionResult> Index(TipoConversion tipo = TipoConversion.Longitud,
            string? valor = null, string? origen = null, string? destino = null)
    {
        ConversorModelo modelo = new() { Tipo = tipo, Valor = valor };

        try
        {
            await CargarUnidadesAsync(modelo, origen, destino);
            await CargarHistorialAsync(modelo);
        }
        catch (ExcepcionClienteWS ex) when (ex.EsErrorAutenticacion)
        {
            return SesionNoValida(ex);
        }
        catch (ExcepcionClienteWS ex)
        {
            modelo.Error = ex.Message;
        }

        return View(modelo);
    }

    [HttpPost]
    public async Task<IActionResult> Convertir(ConversorModelo entrada)
    {
        ConversorModelo modelo = new() { Tipo = entrada.Tipo, Valor = entrada.Valor };

        try
        {
            await CargarUnidadesAsync(modelo, entrada.UnidadOrigen, entrada.UnidadDestino);

            double valor;
            try
            {
                valor = ValidacionUtil.InterpretarDecimal(entrada.Valor);
                servicioConversion.ValidarValor(entrada.Tipo, valor);
            }
            catch (ArgumentException ex)
            {
                return await VistaConErrorAsync(modelo, ex.Message, "valor");
            }

            try
            {
                servicioConversion.ValidarUnidades(modelo.UnidadOrigen, modelo.UnidadDestino);
            }
            catch (ArgumentException ex)
            {
                return await VistaConErrorAsync(modelo, ex.Message, "unidades");
            }

            var conversion = new ConversionModelo(entrada.Tipo, valor, modelo.UnidadOrigen, modelo.UnidadDestino);
            double resultado = await servicioConversion.ConvertirAsync(conversion);

            modelo.ResultadoTitulo = $"{FormatoUtil.FormatearNumero(valor)} {modelo.UnidadOrigen} equivale a:";
            modelo.ResultadoValor = $"{FormatoUtil.FormatearNumero(resultado)} {modelo.UnidadDestino}";

            // El servidor ya registró la conversión en el historial compartido
            await CargarHistorialAsync(modelo);
        }
        catch (ExcepcionClienteWS ex) when (ex.EsErrorAutenticacion)
        {
            return SesionNoValida(ex);
        }
        catch (Exception ex) when (ex is ExcepcionClienteWS or ArgumentException)
        {
            return await VistaConErrorAsync(modelo, ex.Message, null);
        }

        return View(nameof(Index), modelo);
    }

    // Borra el historial en el servidor: deja de verse en todos los clientes
    [HttpPost]
    public async Task<IActionResult> LimpiarHistorial(TipoConversion tipo = TipoConversion.Longitud)
    {
        try
        {
            TempData["Exito"] = await servicioConversion.LimpiarHistorialAsync() + ".";
        }
        catch (ExcepcionClienteWS ex) when (ex.EsErrorAutenticacion)
        {
            return SesionNoValida(ex);
        }
        catch (ExcepcionClienteWS ex)
        {
            TempData["Aviso"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), new { tipo });
    }

    // Consulta al servidor las unidades de la categoría y conserva las elegidas si son válidas
    private async Task CargarUnidadesAsync(ConversorModelo modelo, string? origen, string? destino)
    {
        modelo.Unidades = await servicioConversion.ObtenerUnidadesAsync(modelo.Tipo);

        modelo.UnidadOrigen = modelo.Unidades.Contains(origen ?? string.Empty)
                ? origen
                : modelo.Unidades.FirstOrDefault();
        modelo.UnidadDestino = modelo.Unidades.Contains(destino ?? string.Empty)
                ? destino
                : modelo.Unidades.Skip(1).FirstOrDefault();
    }

    // El historial lo guarda el servidor: incluye lo convertido desde el escritorio, el móvil y la consola
    private async Task CargarHistorialAsync(ConversorModelo modelo)
    {
        modelo.Historial = await servicioConversion.ObtenerHistorialAsync();
    }

    private async Task<IActionResult> VistaConErrorAsync(ConversorModelo modelo, string mensaje, string? campo)
    {
        modelo.Error = mensaje;
        modelo.CampoConError = campo;
        try
        {
            await CargarHistorialAsync(modelo);
        }
        catch (ExcepcionClienteWS ex) when (ex.EsErrorAutenticacion)
        {
            return SesionNoValida(ex);
        }
        catch (ExcepcionClienteWS)
        {
            // Si el historial no se puede consultar, se muestra el error principal igualmente
        }
        return View(nameof(Index), modelo);
    }

    // Un 401 significa token inválido o sesión expirada: se vuelve al inicio de sesión
    private RedirectToActionResult SesionNoValida(ExcepcionClienteWS ex)
    {
        sesion.Limpiar();
        TempData["Aviso"] = ex.Message + " Inicie sesión nuevamente.";
        return RedirectToAction("Login", "Cuenta");
    }
}
