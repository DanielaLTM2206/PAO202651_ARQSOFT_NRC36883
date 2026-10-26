using ec.edu.monster.Cliente;
using ec.edu.monster.Controlador;
using ec.edu.monster.Vista;

namespace ec.edu.monster
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            using var clienteWs = new ClienteWSConversor();
            Application.Run(new FormLogin(new LoginControlador(clienteWs)));
        }
    }
}
