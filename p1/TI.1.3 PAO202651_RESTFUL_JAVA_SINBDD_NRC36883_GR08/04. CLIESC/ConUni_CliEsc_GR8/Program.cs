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

            // La dirección del servidor REST se puede cambiar desde "Configurar servidor" en el inicio de sesión
            Application.Run(new FormLogin(new ServidorControlador()));
        }
    }
}
