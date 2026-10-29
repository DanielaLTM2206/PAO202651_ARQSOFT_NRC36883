using ec.edu.monster.Cliente;
using ec.edu.monster.Controlador;
using ec.edu.monster.Modelo;
using ec.edu.monster.Util;

namespace ec.edu.monster.Vista;

public class ConversionVista
{
    private readonly MensajesModelo mensajes = new();
    private readonly ConversionControlador conversionControlador;
    private readonly LoginControlador loginControlador;
    private bool sesionActiva = true;

    public ConversionVista(ConversionControlador conversionControlador, LoginControlador loginControlador)
    {
        this.conversionControlador = conversionControlador;
        this.loginControlador = loginControlador;
    }

    /// <returns>true si el usuario eligió salir del programa; false si debe volver al inicio de sesión.</returns>
    public async Task<bool> EjecutarMenuAsync()
    {
        while (sesionActiva)
        {
            mensajes.MostrarTitulo("MENÚ PRINCIPAL");
            mensajes.MostrarMensaje("  [1] Convertir Longitud");
            mensajes.MostrarMensaje("  [2] Convertir Masa");
            mensajes.MostrarMensaje("  [3] Convertir Temperatura");
            mensajes.MostrarMensaje("  [4] Ver historial de conversiones");
            mensajes.MostrarMensaje("  [5] Borrar historial");
            mensajes.MostrarMensaje("  [6] Cambiar contraseña");
            mensajes.MostrarMensaje("  [7] Cerrar sesión");
            mensajes.MostrarMensaje("  [0] Salir");

            int opcion = EntradaUtil.LeerEntero("  > Seleccione una opción: ");

            switch (opcion)
            {
                case 1:
                    await ProcesarConversionAsync(TipoConversion.Longitud);
                    break;
                case 2:
                    await ProcesarConversionAsync(TipoConversion.Masa);
                    break;
                case 3:
                    await ProcesarConversionAsync(TipoConversion.Temperatura);
                    break;
                case 4:
                    await ProcesarHistorialAsync(historial => historial.MostrarAsync());
                    break;
                case 5:
                    await ProcesarHistorialAsync(historial => historial.BorrarAsync());
                    break;
                case 6:
                    await ProcesarCambioContraseniaAsync();
                    break;
                case 7:
                    mensajes.MostrarAviso("  Cerrando sesión...");
                    return false;
                case 0:
                    return true;
                default:
                    mensajes.MostrarError("Opción no válida. Intente nuevamente.");
                    break;
            }
        }
        return false;
    }

    private async Task ProcesarConversionAsync(TipoConversion tipo)
    {
        mensajes.MostrarTitulo("CONVERSIÓN DE " + tipo.NombreVisible().ToUpperInvariant());

        try
        {
            IReadOnlyList<string> unidadesPermitidas = await conversionControlador.ObtenerUnidadesAsync(tipo);

            double valor = SolicitarValor(tipo);
            string unidadInicial = SolicitarUnidad("inicial", unidadesPermitidas);
            string unidadFinal = SolicitarUnidadFinal(unidadInicial, unidadesPermitidas);

            var modelo = new ConversionModelo(tipo, valor, unidadInicial, unidadFinal);
            double resultado = await conversionControlador.ConvertirAsync(modelo);

            mensajes.MostrarExito($"Resultado: {FormatoUtil.FormatearNumero(valor)} {unidadInicial}"
                    + $" = {FormatoUtil.FormatearNumero(resultado)} {unidadFinal}");
        }
        catch (ExcepcionClienteWS ex)
        {
            ManejarErrorServicio(ex);
        }
        catch (ArgumentException ex)
        {
            mensajes.MostrarError(ex.Message);
        }
    }

    private async Task ProcesarHistorialAsync(Func<HistorialVista, Task> accion)
    {
        try
        {
            await accion(new HistorialVista(conversionControlador));
        }
        catch (ExcepcionClienteWS ex)
        {
            ManejarErrorServicio(ex);
        }
    }

    private async Task ProcesarCambioContraseniaAsync()
    {
        mensajes.MostrarTitulo("CAMBIAR CONTRASEÑA");

        string actual = EntradaUtil.LeerTextoNoVacio("  > Contraseña actual       : ");
        string nueva = EntradaUtil.LeerTextoNoVacio("  > Nueva contraseña        : ");
        string confirmacion = EntradaUtil.LeerTextoNoVacio("  > Confirme la contraseña  : ");

        if (nueva != confirmacion)
        {
            mensajes.MostrarError("La confirmación no coincide con la nueva contraseña.");
            return;
        }

        try
        {
            mensajes.MostrarExito(await loginControlador.CambiarContraseniaAsync(actual, nueva));
        }
        catch (ExcepcionClienteWS ex)
        {
            ManejarErrorServicio(ex);
        }
        catch (ArgumentException ex)
        {
            mensajes.MostrarError(ex.Message);
        }
    }

    private double SolicitarValor(TipoConversion tipo)
    {
        while (true)
        {
            double valor = EntradaUtil.LeerDecimal("  > Valor a convertir: ");
            try
            {
                conversionControlador.ValidarValor(tipo, valor);
                return valor;
            }
            catch (ArgumentException ex)
            {
                mensajes.MostrarError(ex.Message);
            }
        }
    }

    private string SolicitarUnidadFinal(string unidadInicial, IReadOnlyList<string> unidadesPermitidas)
    {
        while (true)
        {
            string unidadFinal = SolicitarUnidad("final", unidadesPermitidas);
            try
            {
                conversionControlador.ValidarUnidades(unidadInicial, unidadFinal);
                return unidadFinal;
            }
            catch (ArgumentException ex)
            {
                mensajes.MostrarError(ex.Message);
            }
        }
    }

    private string SolicitarUnidad(string tipoUnidad, IReadOnlyList<string> unidadesPermitidas)
    {
        mensajes.MostrarMensaje($"  Seleccione la unidad {tipoUnidad}:");
        for (int indice = 0; indice < unidadesPermitidas.Count; indice++)
        {
            mensajes.MostrarMensaje($"  [{indice + 1}] {unidadesPermitidas[indice]}");
        }

        while (true)
        {
            int opcion = EntradaUtil.LeerEntero("  > Opción de unidad: ");
            if (opcion >= 1 && opcion <= unidadesPermitidas.Count)
            {
                return unidadesPermitidas[opcion - 1];
            }
            mensajes.MostrarError("Unidad no válida. Elija una opción de la lista.");
        }
    }

    // Un 401 significa token inválido o sesión expirada: se vuelve al inicio de sesión
    private void ManejarErrorServicio(ExcepcionClienteWS ex)
    {
        mensajes.MostrarError(ex.Message);
        if (ex.EsErrorAutenticacion)
        {
            mensajes.MostrarAviso("  Debe iniciar sesión nuevamente.");
            sesionActiva = false;
        }
    }
}
