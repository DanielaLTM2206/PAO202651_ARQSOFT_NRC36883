using ec.edu.monster.Cliente;
using ec.edu.monster.Controlador;
using ec.edu.monster.Util;
using ec.edu.monster.Vista;

namespace ec.edu.monster
{
    public partial class App : Application
    {
        private ClienteWSConversor? clienteWs;

        public App()
        {
            InitializeComponent();
            // La app usa siempre el tema claro, igual que los clientes de escritorio y web
            UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new LoginPagina(CrearLoginControlador())) { Title = "ConUni SOAP GR08" };
        }

        public static App Actual => (App)Current!;

        // Crea el cliente SOAP con la dirección del servidor configurada en ese momento
        public LoginControlador CrearLoginControlador()
        {
            clienteWs?.Dispose();
            clienteWs = new ClienteWSConversor(ConfiguracionServidor.UrlActual);
            return new LoginControlador(clienteWs);
        }

        public void MostrarInicioSesion(string? aviso = null)
        {
            Windows[0].Page = new LoginPagina(CrearLoginControlador(), aviso);
        }

        public void MostrarConversor(LoginControlador loginControlador, string usuario)
        {
            Windows[0].Page = new NavigationPage(new ConversorPagina(loginControlador, usuario))
            {
                BarBackgroundColor = Color.FromArgb("#005493"),
                BarTextColor = Colors.White
            };
        }
    }
}
