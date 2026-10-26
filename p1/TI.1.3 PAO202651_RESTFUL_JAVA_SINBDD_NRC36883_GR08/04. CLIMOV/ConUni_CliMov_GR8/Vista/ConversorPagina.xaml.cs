using ec.edu.monster.Cliente;
using ec.edu.monster.Controlador;
using ec.edu.monster.Modelo;
using ec.edu.monster.Util;

namespace ec.edu.monster.Vista;

public partial class ConversorPagina : ContentPage
{
    private static readonly Color ColorPrimario = Color.FromArgb("#005493");
    private static readonly Color ColorSuave = Color.FromArgb("#E1EAF4");

    private readonly LoginControlador loginControlador;
    private readonly ConversionControlador conversionControlador;
    private readonly string usuario;
    private readonly Dictionary<Button, TipoConversion> pestanias;
    private TipoConversion? tipoSeleccionado;
    private bool enPeticion;
    private bool cargado;
    private string textoValorAnterior = string.Empty;

    public ConversorPagina(LoginControlador loginControlador, string usuario)
    {
        InitializeComponent();
        this.loginControlador = loginControlador;
        this.usuario = usuario;
        conversionControlador = loginControlador.CrearControladorConversion();

        btnUsuario.Text = $"{usuario}  ▾";
        pestanias = new Dictionary<Button, TipoConversion>
        {
            [btnLongitud] = TipoConversion.Longitud,
            [btnMasa] = TipoConversion.Masa,
            [btnTemperatura] = TipoConversion.Temperatura
        };

        ResaltadoCampo.Aplicar(txtValor, bordeValor);
        ResaltadoCampo.Aplicar(cmbOrigen, bordeOrigen);
        ResaltadoCampo.Aplicar(cmbDestino, bordeDestino);
        PintarPestanias();
    }

    // Al volver a esta pantalla también se recarga el historial: puede haber cambios desde otros clientes
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!cargado)
        {
            cargado = true;
            await SeleccionarTipoAsync(TipoConversion.Longitud);
        }
        await CargarHistorialAsync();
    }

    private async void BtnTipo_Clicked(object? sender, EventArgs e)
    {
        if (!enPeticion && sender is Button boton && pestanias.TryGetValue(boton, out TipoConversion tipo)
                && tipo != tipoSeleccionado)
        {
            await SeleccionarTipoAsync(tipo);
        }
    }

    private async Task SeleccionarTipoAsync(TipoConversion tipo)
    {
        tipoSeleccionado = tipo;
        PintarPestanias();
        MostrarError(null);

        CambiarEstadoOcupado(true);
        try
        {
            IReadOnlyList<string> unidades = await conversionControlador.ObtenerUnidadesAsync(tipo);

            cmbOrigen.ItemsSource = unidades.ToList();
            cmbDestino.ItemsSource = unidades.ToList();
            if (unidades.Count > 1)
            {
                cmbOrigen.SelectedIndex = 0;
                cmbDestino.SelectedIndex = 1;
            }
            LimpiarResultado();
        }
        catch (ExcepcionClienteWS ex)
        {
            await ManejarErrorServicioAsync(ex);
        }
        finally
        {
            CambiarEstadoOcupado(false);
        }
    }

    // La pestaña activa se pinta en azul; las demás en tono suave
    private void PintarPestanias()
    {
        foreach ((Button boton, TipoConversion tipo) in pestanias)
        {
            bool activa = tipo == tipoSeleccionado;
            boton.BackgroundColor = activa ? ColorPrimario : ColorSuave;
            boton.TextColor = activa ? Colors.White : ColorPrimario;
        }
    }

    // Bloquea letras y símbolos: si el texto deja de ser un número, se restaura el anterior
    private void TxtValor_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (ValidacionUtil.EsTextoNumericoParcial(e.NewTextValue))
        {
            textoValorAnterior = e.NewTextValue ?? string.Empty;
            ResaltadoCampo.QuitarError(bordeValor);
            return;
        }
        txtValor.Text = textoValorAnterior;
    }

    private void CmbUnidad_SelectedIndexChanged(object? sender, EventArgs e)
    {
        ResaltadoCampo.QuitarError(bordeDestino);
    }

    private void BtnIntercambiar_Clicked(object? sender, EventArgs e)
    {
        int origen = cmbOrigen.SelectedIndex;
        cmbOrigen.SelectedIndex = cmbDestino.SelectedIndex;
        cmbDestino.SelectedIndex = origen;
    }

    private async void BtnConvertir_Clicked(object? sender, EventArgs e)
    {
        if (enPeticion || tipoSeleccionado is not TipoConversion tipo)
        {
            return;
        }
        MostrarError(null);

        double valor;
        try
        {
            valor = ValidacionUtil.InterpretarDecimal(txtValor.Text);
            conversionControlador.ValidarValor(tipo, valor);
        }
        catch (ArgumentException ex)
        {
            MostrarError(ex.Message);
            ResaltadoCampo.MarcarError(bordeValor);
            return;
        }

        string? unidadOrigen = cmbOrigen.SelectedItem as string;
        string? unidadDestino = cmbDestino.SelectedItem as string;
        try
        {
            conversionControlador.ValidarUnidades(unidadOrigen, unidadDestino);
        }
        catch (ArgumentException ex)
        {
            MostrarError(ex.Message);
            ResaltadoCampo.MarcarError(bordeDestino);
            return;
        }

        txtValor.Unfocus();
        CambiarEstadoOcupado(true);
        try
        {
            var modelo = new ConversionModelo(tipo, valor, unidadOrigen, unidadDestino);
            double resultado = await conversionControlador.ConvertirAsync(modelo);
            MostrarResultado(valor, unidadOrigen!, resultado, unidadDestino!);

            // El servidor ya registró la conversión: se recarga el historial compartido
            await CargarHistorialAsync();
        }
        catch (ExcepcionClienteWS ex)
        {
            await ManejarErrorServicioAsync(ex);
        }
        catch (ArgumentException ex)
        {
            MostrarError(ex.Message);
        }
        finally
        {
            CambiarEstadoOcupado(false);
        }
    }

    // Las opciones de la cuenta se agrupan en un menú para no ocupar espacio en la pantalla
    private async void BtnUsuario_Clicked(object? sender, EventArgs e)
    {
        const string cambiarContrasenia = "Cambiar contraseña";
        const string cerrarSesion = "Cerrar sesión";

        string opcion = await DisplayActionSheetAsync(usuario, "Cancelar", null, cambiarContrasenia, cerrarSesion);

        if (opcion == cambiarContrasenia)
        {
            await Navigation.PushAsync(new CambiarContraseniaPagina(loginControlador));
        }
        else if (opcion == cerrarSesion)
        {
            bool confirmar = await DisplayAlertAsync("¿Cerrar sesión?",
                    "Tendrá que volver a ingresar su usuario y contraseña para seguir usando el conversor.",
                    "Sí, cerrar sesión", "Cancelar");
            if (confirmar)
            {
                loginControlador.CerrarSesion();
                App.Actual.MostrarInicioSesion("Sesión cerrada correctamente.");
            }
        }
    }

    private async void BtnActualizarHistorial_Clicked(object? sender, EventArgs e)
    {
        await CargarHistorialAsync();
    }

    private async void BtnLimpiarHistorial_Clicked(object? sender, EventArgs e)
    {
        bool confirmar = await DisplayAlertAsync("¿Borrar el historial?",
                "Se borrarán las conversiones de todos los clientes (escritorio, web y móvil). "
                + "Esta acción no se puede deshacer.",
                "Sí, borrar", "Cancelar");
        if (!confirmar)
        {
            return;
        }

        try
        {
            await conversionControlador.LimpiarHistorialAsync();
        }
        catch (ExcepcionClienteWS ex)
        {
            await ManejarErrorServicioAsync(ex);
            return;
        }
        await CargarHistorialAsync();
    }

    // "Usar": vuelve a cargar una conversión del historial en el formulario
    private async Task ReutilizarAsync(RegistroConversion registro)
    {
        if (enPeticion)
        {
            return;
        }
        if (registro.Tipo != tipoSeleccionado)
        {
            await SeleccionarTipoAsync(registro.Tipo);
        }
        txtValor.Text = FormatoUtil.FormatearNumero(registro.Valor);
        cmbOrigen.SelectedItem = registro.UnidadOrigen;
        cmbDestino.SelectedItem = registro.UnidadDestino;
        MostrarResultado(registro.Valor, registro.UnidadOrigen, registro.Resultado, registro.UnidadDestino);
    }

    // El historial lo guarda el servidor: incluye lo convertido desde el escritorio, la web y la consola
    private async Task CargarHistorialAsync()
    {
        IReadOnlyList<RegistroConversion> historial;
        btnActualizarHistorial.IsEnabled = false;
        try
        {
            historial = await conversionControlador.ObtenerHistorialAsync();
        }
        catch (ExcepcionClienteWS ex)
        {
            await ManejarErrorServicioAsync(ex);
            return;
        }
        finally
        {
            btnActualizarHistorial.IsEnabled = true;
        }

        listaHistorial.Children.Clear();
        foreach (RegistroConversion registro in historial)
        {
            listaHistorial.Children.Add(CrearFilaHistorial(registro));
        }

        bool hayRegistros = historial.Count > 0;
        pnlHistorialVacio.IsVisible = !hayRegistros;
        btnLimpiarHistorial.IsVisible = hayRegistros;
        lblContadorHistorial.Text = historial.Count switch
        {
            0 => "Aún no hay conversiones",
            1 => "1 conversión en todos los clientes",
            int cantidad => $"{cantidad} conversiones en todos los clientes"
        };
    }

    private View CrearFilaHistorial(RegistroConversion registro)
    {
        var fila = new Grid
        {
            ColumnDefinitions = [new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto)],
            Padding = new Thickness(0, 10),
            ColumnSpacing = 8
        };

        var datos = new VerticalStackLayout { Spacing = 2 };
        datos.Children.Add(new Label
        {
            Text = $"{registro.FechaHora:HH:mm:ss} · {registro.Tipo.NombreVisible()}",
            TextColor = Color.FromArgb("#6C757D"),
            FontSize = 12
        });
        datos.Children.Add(new Label
        {
            Text = $"{FormatoUtil.FormatearNumero(registro.Valor)} {registro.UnidadOrigen}",
            TextColor = Color.FromArgb("#212529"),
            FontSize = 15
        });
        datos.Children.Add(new Label
        {
            Text = $"→ {FormatoUtil.FormatearNumero(registro.Resultado)} {registro.UnidadDestino}",
            TextColor = ColorPrimario,
            FontSize = 15,
            FontAttributes = FontAttributes.Bold
        });

        var botonUsar = new Button
        {
            Text = "Usar",
            BackgroundColor = ColorSuave,
            TextColor = ColorPrimario,
            FontSize = 14,
            FontAttributes = FontAttributes.Bold,
            CornerRadius = 16,
            HeightRequest = 34,
            Padding = new Thickness(14, 0),
            VerticalOptions = LayoutOptions.Center
        };
        botonUsar.Clicked += async (_, _) => await ReutilizarAsync(registro);
        SemanticProperties.SetDescription(botonUsar, "Volver a cargar esta conversión");

        fila.Add(datos);
        fila.Add(botonUsar, 1);

        var contenedor = new VerticalStackLayout();
        contenedor.Children.Add(fila);
        contenedor.Children.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#E9ECEF") });
        return contenedor;
    }

    private void MostrarResultado(double valor, string unidadOrigen, double resultado, string unidadDestino)
    {
        lblResultadoTitulo.Text = $"{FormatoUtil.FormatearNumero(valor)} {unidadOrigen} equivale a:";
        lblResultado.Text = $"{FormatoUtil.FormatearNumero(resultado)} {unidadDestino}";
    }

    private void LimpiarResultado()
    {
        lblResultadoTitulo.Text = "Resultado";
        lblResultado.Text = "—";
    }

    private void MostrarError(string? mensaje)
    {
        lblError.Text = mensaje;
        pnlError.IsVisible = !string.IsNullOrEmpty(mensaje);
    }

    // Un 401 significa token inválido o sesión expirada: se vuelve al inicio de sesión
    private async Task ManejarErrorServicioAsync(ExcepcionClienteWS ex)
    {
        if (ex.EsErrorAutenticacion)
        {
            await DisplayAlertAsync("Sesión no válida", ex.Message + "\n\nDebe iniciar sesión nuevamente.", "Entendido");
            loginControlador.CerrarSesion();
            App.Actual.MostrarInicioSesion("Inicie sesión para continuar.");
            return;
        }

        if (ex.EsErrorConexion)
        {
            await DisplayAlertAsync("Sin conexión con el servidor", ex.Message, "Entendido");
            return;
        }
        MostrarError(ex.Message);
    }

    private void CambiarEstadoOcupado(bool ocupado)
    {
        enPeticion = ocupado;
        btnConvertir.IsEnabled = !ocupado;
        btnUsuario.IsEnabled = !ocupado;
        btnConvertir.Text = ocupado ? "Procesando..." : "Convertir";
        indicadorCarga.IsRunning = ocupado;
        indicadorCarga.IsVisible = ocupado;
    }
}
