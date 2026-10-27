using System;
using System.Text;
using System.Threading.Tasks;
using ConUni_Client_DotNet.Models;
using ConUni_Client_DotNet.Services;

namespace ConUni_Client_DotNet
{
    class Program
    {
        private const string DEFAULT_SERVER_URL = "http://localhost:5000/api";
        private static RestClient _client = null!;

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            if (!Console.IsOutputRedirected)
            {
                try { Console.Title = "Monsters Inc. - Cliente Consola RESTful (GR08)"; } catch { }
            }

            _client = new RestClient(DEFAULT_SERVER_URL);

            MostrarBannerBienvenida();

            // Flujo de autenticación obligatorio contra el servidor REST
            bool autenticado = await EjecutarLoginAsync();
            if (!autenticado)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nAcceso denegado. No se pudo autenticar con el servidor REST.");
                Console.WriteLine("El programa finalizara.\n");
                Console.ResetColor();
                return;
            }

            // Menú interactivo
            bool salir = false;
            while (!salir)
            {
                MostrarMenuPrincipal();
                Console.Write("Seleccione una opcion [1-7]: ");
                string? opcion = Console.ReadLine()?.Trim();

                Console.WriteLine();
                switch (opcion)
                {
                    case "1":
                        await MenuConversionLongitudAsync();
                        break;
                    case "2":
                        await MenuConversionMasaAsync();
                        break;
                    case "3":
                        await MenuConversionTemperaturaAsync();
                        break;
                    case "4":
                        await ConsultarHistorialAsync();
                        break;
                    case "5":
                        await LimpiarHistorialAsync();
                        break;
                    case "6":
                        await CambiarContrasenaAsync();
                        break;
                    case "7":
                        _client.CerrarSesion();
                        Console.WriteLine("Sesion finalizada correctamente. Hasta pronto.");
                        salir = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Opcion invalida. Ingrese un numero del 1 al 7.");
                        Console.ResetColor();
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione ENTER para continuar...");
                    Console.ReadLine();
                }
            }
        }

        private static void MostrarBannerBienvenida()
        {
            if (!Console.IsOutputRedirected && !Console.IsInputRedirected)
            {
                try { Console.Clear(); } catch { }
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("+---------------------------------------------------------------+");
            Console.WriteLine("|       MONSTERS INC. - CLIENTE CONSOLA RESTFUL .NET 8          |");
            Console.WriteLine("|       Grupo 08 - Arquitectura de Software (SINBDD)            |");
            Console.WriteLine("+---------------------------------------------------------------+");
            Console.ResetColor();
            Console.WriteLine($"Conectado a: {DEFAULT_SERVER_URL}\n");
        }

        private static async Task<bool> EjecutarLoginAsync()
        {
            Console.WriteLine("+---------------------------------------------------------------+");
            Console.WriteLine("|                   AUTENTICACION DE USUARIO                    |");
            Console.WriteLine("+---------------------------------------------------------------+");

            int intentos = 0;
            const int maxIntentos = 3;

            while (intentos < maxIntentos)
            {
                Console.Write("Usuario [Default: MONSTER]: ");
                string? usuario = Console.ReadLine()?.Trim('\uFEFF', ' ', '\r', '\n', '\t');
                if (string.IsNullOrWhiteSpace(usuario)) usuario = "MONSTER";

                Console.Write("Contrasena [Default: MONSTER9]: ");
                string clave = LeerContrasenaOculta().Trim('\uFEFF', ' ', '\r', '\n', '\t');
                if (string.IsNullOrWhiteSpace(clave)) clave = "MONSTER9";

                Console.WriteLine("\nVerificando credenciales con la API REST...");
                var res = await _client.IniciarSesionAsync(usuario, clave);

                if (res.Exitoso)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Autenticacion exitosa.");
                    Console.WriteLine($"Bienvenido: {res.Usuario}");
                    Console.WriteLine($"Token de sesion emitido: {res.Token?.Substring(0, 16)}...\n");
                    Console.ResetColor();
                    return true;
                }
                else
                {
                    intentos++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error [{res.Codigo}]: {res.Mensaje}");
                    Console.ResetColor();

                    int restantes = maxIntentos - intentos;
                    if (restantes > 0)
                    {
                        Console.WriteLine($"Intentos restantes: {restantes}\n");
                    }
                }
            }

            return false;
        }

        private static void MostrarMenuPrincipal()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("+---------------------------------------------------------------+");
            Console.WriteLine($"|  PANEL PRINCIPAL - Sesion: {_client.Usuario,-34} |");
            Console.WriteLine("+---------------------------------------------------------------+");
            Console.ResetColor();
            Console.WriteLine("|  1. Conversion de Longitud (Milla, Metro, Pulgada, etc.)      |");
            Console.WriteLine("|  2. Conversion de Masa (Kilogramo, Libra, Onza, Gramo)        |");
            Console.WriteLine("|  3. Conversion de Temperatura (Celsius, Fahrenheit, Kelvin)   |");
            Console.WriteLine("|  4. Consultar Historial Sincronizado en Memoria               |");
            Console.WriteLine("|  5. Limpiar Historial Global                                  |");
            Console.WriteLine("|  6. Cambiar Contrasena Corporativa                            |");
            Console.WriteLine("|  7. Cerrar Sesion y Salir                                     |");
            Console.WriteLine("+---------------------------------------------------------------+");
        }

        private static async Task MenuConversionLongitudAsync()
        {
            Console.WriteLine("--- CONVERSION DE LONGITUD ---");
            Console.WriteLine("1. Milla -> Metro");
            Console.WriteLine("2. Metro -> Milla");
            Console.WriteLine("3. Milla -> Pulgada");
            Console.WriteLine("4. Pulgada -> Milla");
            Console.WriteLine("5. Metro -> Pulgada");
            Console.WriteLine("6. Pulgada -> Metro");
            Console.WriteLine("7. Kilometro -> Metro");
            Console.WriteLine("8. Metro -> Kilometro");
            Console.Write("Seleccione tipo [1-8]: ");
            string? sel = Console.ReadLine()?.Trim();

            (string origen, string destino) = sel switch
            {
                "1" => ("Milla", "Metro"),
                "2" => ("Metro", "Milla"),
                "3" => ("Milla", "Pulgada"),
                "4" => ("Pulgada", "Milla"),
                "5" => ("Metro", "Pulgada"),
                "6" => ("Pulgada", "Metro"),
                "7" => ("Kilometro", "Metro"),
                "8" => ("Metro", "Kilometro"),
                _ => ("", "")
            };

            if (string.IsNullOrEmpty(origen))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Seleccion de unidades no valida.");
                Console.ResetColor();
                return;
            }

            string? valor = PedirValorNumerico();
            if (valor == null) return;

            Console.WriteLine($"Consultando API REST: Convirtiendo {valor} {origen} a {destino}...");
            var res = await _client.ConvertirLongitudAsync(valor, origen, destino);
            MostrarResultadoConversion(res);
        }

        private static async Task MenuConversionMasaAsync()
        {
            Console.WriteLine("--- CONVERSION DE MASA ---");
            Console.WriteLine("1. Kilogramo -> Libra");
            Console.WriteLine("2. Libra -> Kilogramo");
            Console.WriteLine("3. Kilogramo -> Onza");
            Console.WriteLine("4. Onza -> Kilogramo");
            Console.WriteLine("5. Libra -> Onza");
            Console.WriteLine("6. Onza -> Libra");
            Console.WriteLine("7. Gramo -> Kilogramo");
            Console.WriteLine("8. Kilogramo -> Gramo");
            Console.Write("Seleccione tipo [1-8]: ");
            string? sel = Console.ReadLine()?.Trim();

            (string origen, string destino) = sel switch
            {
                "1" => ("Kilogramo", "Libra"),
                "2" => ("Libra", "Kilogramo"),
                "3" => ("Kilogramo", "Onza"),
                "4" => ("Onza", "Kilogramo"),
                "5" => ("Libra", "Onza"),
                "6" => ("Onza", "Libra"),
                "7" => ("Gramo", "Kilogramo"),
                "8" => ("Kilogramo", "Gramo"),
                _ => ("", "")
            };

            if (string.IsNullOrEmpty(origen))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Seleccion no valida.");
                Console.ResetColor();
                return;
            }

            string? valor = PedirValorNumerico();
            if (valor == null) return;

            Console.WriteLine($"Consultando API REST: Convirtiendo {valor} {origen} a {destino}...");
            var res = await _client.ConvertirMasaAsync(valor, origen, destino);
            MostrarResultadoConversion(res);
        }

        private static async Task MenuConversionTemperaturaAsync()
        {
            Console.WriteLine("--- CONVERSION DE TEMPERATURA ---");
            Console.WriteLine("1. Celsius -> Fahrenheit");
            Console.WriteLine("2. Fahrenheit -> Celsius");
            Console.WriteLine("3. Celsius -> Kelvin");
            Console.WriteLine("4. Kelvin -> Celsius");
            Console.WriteLine("5. Fahrenheit -> Kelvin");
            Console.WriteLine("6. Kelvin -> Fahrenheit");
            Console.Write("Seleccione tipo [1-6]: ");
            string? sel = Console.ReadLine()?.Trim();

            (string origen, string destino) = sel switch
            {
                "1" => ("Celsius", "Fahrenheit"),
                "2" => ("Fahrenheit", "Celsius"),
                "3" => ("Celsius", "Kelvin"),
                "4" => ("Kelvin", "Celsius"),
                "5" => ("Fahrenheit", "Kelvin"),
                "6" => ("Kelvin", "Fahrenheit"),
                _ => ("", "")
            };

            if (string.IsNullOrEmpty(origen))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Seleccion no valida.");
                Console.ResetColor();
                return;
            }

            string? valor = PedirValorNumerico();
            if (valor == null) return;

            Console.WriteLine($"Consultando API REST: Convirtiendo {valor} {origen} a {destino}...");
            var res = await _client.ConvertirTemperaturaAsync(valor, origen, destino);
            MostrarResultadoConversion(res);
        }

        private static string? PedirValorNumerico()
        {
            Console.Write("Ingrese valor numerico a convertir (max 15 digitos): ");
            string? valor = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(valor))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("El valor no puede estar vacio.");
                Console.ResetColor();
                return null;
            }

            // Validar longitud en cliente
            string digitosLimpios = valor.Replace("-", "").Replace(".", "").Replace(",", "");
            if (digitosLimpios.Length > 15)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error [VAL_007]: El valor ingresado supera el limite maximo permitido (maximo 15 digitos).");
                Console.ResetColor();
                return null;
            }

            return valor;
        }

        private static void MostrarResultadoConversion(ConversionResult res)
        {
            Console.WriteLine();
            if (res.Exitoso && res.Resultado != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("+---------------------------------------------------------------+");
                Console.WriteLine("| RESULTADO DE LA CONVERSION                                     |");
                Console.WriteLine("+---------------------------------------------------------------+");
                Console.ResetColor();
                Console.WriteLine($"  Valor Original:     {res.Resultado.ValorOriginal} {res.Resultado.UnidadOrigen}");
                Console.WriteLine($"  Valor Redondeado:   {res.Resultado.ValorConvertidoRedondeado} {res.Resultado.UnidadDestino}");
                Console.WriteLine($"  Valor Exacto:       {res.Resultado.ValorConvertidoExacto}");
                Console.WriteLine($"  Factor Aplicado:    {res.Resultado.FactorConversion}");
                Console.WriteLine($"  Fecha/Hora:         {res.Resultado.FechaConversion.ToLocalTime():yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine("+---------------------------------------------------------------+");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("+---------------------------------------------------------------+");
                Console.WriteLine("| ERROR EN LA CONVERSION                                        |");
                Console.WriteLine("+---------------------------------------------------------------+");
                Console.WriteLine($"  Codigo:   {res.Error?.CodigoError ?? "ERR"}");
                Console.WriteLine($"  Mensaje:  {res.Error?.Mensaje ?? "Error desconocido"}");
                if (!string.IsNullOrEmpty(res.Error?.Detalles))
                {
                    Console.WriteLine($"  Detalle:  {res.Error.Detalles}");
                }
                Console.WriteLine("+---------------------------------------------------------------+");
                Console.ResetColor();
            }
        }

        private static async Task ConsultarHistorialAsync()
        {
            Console.WriteLine("Consultando historial sincronizado desde el servidor REST...\n");
            var res = await _client.ObtenerHistorialAsync();

            if (res.Exitoso && res.Registros != null)
            {
                if (res.Registros.Count == 0)
                {
                    Console.WriteLine("No hay conversiones registradas en el servidor.");
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("+---------------------+-------------+-------------------+-------------------+----------+");
                Console.WriteLine("| Fecha/Hora          | Tipo        | Origen            | Destino           | Usuario  |");
                Console.WriteLine("+---------------------+-------------+-------------------+-------------------+----------+");
                Console.ResetColor();

                foreach (var r in res.Registros)
                {
                    string f = r.Fecha.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                    string orig = $"{r.ValorOriginal} {r.UnidadOrigen}";
                    string dest = $"{r.ValorResultado} {r.UnidadDestino}";
                    Console.WriteLine($"| {f,-19} | {r.TipoConversion,-11} | {orig,-17} | {dest,-17} | {r.Usuario,-8} |");
                }
                Console.WriteLine("+---------------------+-------------+-------------------+-------------------+----------+");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error al consultar historial: {res.Mensaje}");
                Console.ResetColor();
            }
        }

        private static async Task LimpiarHistorialAsync()
        {
            Console.Write("Esta seguro de limpiar el historial en memoria? [S/N]: ");
            string? conf = Console.ReadLine()?.Trim().ToUpperInvariant();
            if (conf == "S" || conf == "SI")
            {
                var res = await _client.LimpiarHistorialAsync();
                if (res.Exitoso)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Historial limpiado exitosamente en el servidor REST.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {res.Mensaje}");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("Operacion cancelada.");
            }
        }

        private static async Task CambiarContrasenaAsync()
        {
            Console.WriteLine("--- CAMBIO DE CONTRASENA CORPORATIVA ---");
            Console.Write("Ingrese su contrasena actual: ");
            string actual = LeerContrasenaOculta();

            Console.Write("Ingrese su nueva contrasena (min 4 car): ");
            string nueva = LeerContrasenaOculta();

            Console.Write("Confirme su nueva contrasena: ");
            string confirmar = LeerContrasenaOculta();

            if (nueva != confirmar)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nLa nueva contrasena y su confirmacion no coinciden.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("\nActualizando contrasena en el servidor REST...");
            var res = await _client.CambiarClaveAsync(actual, nueva);

            if (res.Exitoso)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Exito: {res.Mensaje}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error [{res.Codigo}]: {res.Mensaje}");
                Console.ResetColor();
            }
        }

        private static string LeerContrasenaOculta()
        {
            if (Console.IsInputRedirected)
            {
                return Console.ReadLine()?.Trim() ?? "";
            }

            var sb = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        sb.Length--;
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    sb.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
            return sb.ToString();
        }
    }
}
