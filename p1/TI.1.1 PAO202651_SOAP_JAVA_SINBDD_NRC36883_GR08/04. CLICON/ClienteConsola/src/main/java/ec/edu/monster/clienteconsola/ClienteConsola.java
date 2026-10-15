package ec.edu.monster.clienteconsola;

import ec.edu.monster.clienteconsola.controlador.ControladorConversiones;
import ec.edu.monster.clientesws.CategoriaDTO;
import ec.edu.monster.clientesws.RespuestaConversion;
import java.util.List;
import java.util.Scanner;

/**
 * Cliente de Consola para el Web Service SOAP de Conversión de Unidades (GR08).
 * 
 * @author GR08 - Arquitectura de Software
 */
public class ClienteConsola {

    private static final Scanner scanner = new Scanner(System.in);
    private static final ControladorConversiones controlador = new ControladorConversiones();

    public static void main(String[] args) {
        mostrarBannerSullivan();

        // 1. Fase de Autenticación (RF-01)
        boolean autenticado = false;
        while (!autenticado) {
            System.out.println("\n========================================================");
            System.out.println("            INICIO DE SESIÓN - MONSTER INC             ");
            System.out.println("========================================================");
            System.out.print("Ingrese Usuario: ");
            String usuario = scanner.nextLine().trim();
            System.out.print("Ingrese Clave: ");
            String clave = scanner.nextLine().trim();

            System.out.println("\n[INFO] Conectando con el Servidor SOAP...");
            if (controlador.iniciarSesion(usuario, clave)) {
                autenticado = true;
                System.out.println(">> ¡AUTENTICACIÓN EXITOSA! Token obtenido.");
                System.out.println(">> Token de Sesión: " + controlador.getToken());
            } else {
                System.out.println(">> [ERROR] " + controlador.getUltimoError());
                System.out.print("¿Desea reintentar? (s/n): ");
                String op = scanner.nextLine().trim();
                if (!op.equalsIgnoreCase("s")) {
                    System.out.println("Saliendo del cliente de consola...");
                    return;
                }
            }
        }

        // 2. Menú Principal de Operaciones (CU-02, CU-04)
        boolean continuar = true;
        while (continuar) {
            System.out.println("\n========================================================");
            System.out.println("       MENÚ DE CONVERSIONES - MONSTER INC (GR08)        ");
            System.out.println("========================================================");
            System.out.println("1. Consultar Catálogo Completo (CU-02)");
            System.out.println("2. Realizar Conversión de Unidades (CU-04 / RF-05)");
            System.out.println("3. Probar Error: Token Inválido (CU-08)");
            System.out.println("4. Probar Error: Inconsistencia de Magnitudes (CU-11)");
            System.out.println("5. Salir");
            System.out.print("Seleccione una opción (1-5): ");

            String opcion = scanner.nextLine().trim();

            switch (opcion) {
                case "1":
                    consultarCatalogo();
                    break;
                case "2":
                    ejecutarConversion();
                    break;
                case "3":
                    probarTokenInvalido();
                    break;
                case "4":
                    probarInconsistencia();
                    break;
                case "5":
                    continuar = false;
                    System.out.println("\n¡Gracias por usar Monster Conversor GR08! Hasta pronto.");
                    break;
                default:
                    System.out.println(">> Opción no válida. Intente de nuevo.");
            }
        }
    }

    private static void consultarCatalogo() {
        System.out.println("\n--- CATÁLOGO DE MAGNITUDES Y UNIDADES DISPONIBLES ---");
        List<CategoriaDTO> categorias = controlador.consultarCatalogo();
        if (categorias.isEmpty()) {
            System.out.println("No se pudo obtener el catálogo del servidor.");
            return;
        }
        for (CategoriaDTO cat : categorias) {
            System.out.println("\n• Categoría: " + cat.getNombre() + " (Unidad Base: " + cat.getUnidadBase() + ")");
            List<String> lista = (cat.getUnidades() != null) ? cat.getUnidades().getUnidad() : java.util.Collections.emptyList();
            System.out.println("  Unidades soportadas: " + String.join(", ", lista));
        }
    }

    private static void ejecutarConversion() {
        System.out.println("\n--- NUEVA CONVERSIÓN ---");
        System.out.print("Ingrese valor numérico: ");
        String valStr = scanner.nextLine().trim();
        double valor;
        try {
            valor = Double.parseDouble(valStr);
        } catch (NumberFormatException e) {
            System.out.println(">> [ERROR] El valor ingresado no es un número válido.");
            return;
        }

        System.out.print("Unidad de Origen (ej. metros, kilogramos, celsius): ");
        String origen = scanner.nextLine().trim();

        System.out.print("Unidad de Destino (ej. pies, libras, fahrenheit): ");
        String destino = scanner.nextLine().trim();

        System.out.println("\n[INFO] Enviando petición SOAP al servidor...");
        RespuestaConversion resp = controlador.convertir(valor, origen, destino);
        imprimirRespuestaEstructurada(resp);
    }

    private static void probarTokenInvalido() {
        System.out.println("\n[PRUEBA] Enviando petición con Token alterado 'token-invalido-xyz'...");
        // Invocación directa con token erróneo
        ControladorConversiones controlErr = new ControladorConversiones();
        // Usar controlador con token no asignado
        RespuestaConversion resp = controlErr.convertir(10.0, "metros", "pies");
        imprimirRespuestaEstructurada(resp);
    }

    private static void probarInconsistencia() {
        System.out.println("\n[PRUEBA] Intentando convertir entre categorías incompatibles: 'metros' (Longitud) a 'kilogramos' (Masa)...");
        RespuestaConversion resp = controlador.convertir(50.0, "metros", "kilogramos");
        imprimirRespuestaEstructurada(resp);
    }

    private static void imprimirRespuestaEstructurada(RespuestaConversion resp) {
        System.out.println("\n--------------------------------------------------------");
        System.out.println("         RESPUESTA ESTRUCTURADA SOAP (RF-06)            ");
        System.out.println("--------------------------------------------------------");
        System.out.println("  ESTADO   : " + resp.getEstado());
        System.out.println("  CÓDIGO   : " + resp.getCodigo());
        System.out.println("  MENSAJE  : " + resp.getMensaje());
        if ("OK".equalsIgnoreCase(resp.getEstado()) && resp.getResultado() != null) {
            System.out.printf("  RESULTADO: %.6f\n", resp.getResultado());
            if (resp.getCategoria() != null) {
                System.out.println("  DETALLES : " + resp.getValorOriginal() + " " + resp.getUnidadOrigen() + " ➜ " + resp.getUnidadDestino() + " (" + resp.getCategoria() + ")");
            }
        }
        System.out.println("--------------------------------------------------------");
    }

    private static void mostrarBannerSullivan() {
        System.out.println("  ______________________________________________________");
        System.out.println(" /                                                      \\");
        System.out.println("|       MONSTERS INC. - CONVERSOR DE UNIDADES GR08       |");
        System.out.println("|       James P. 'Sulley' Sullivan - Arquitectura SOA    |");
        System.out.println(" \\______________________________________________________/");
        System.out.println("                      \\   ^__^");
        System.out.println("                       \\  (oo)\\_______");
        System.out.println("                          (__)\\       )\\/\\");
        System.out.println("                              ||----w |");
        System.out.println("                              ||     ||");
        System.out.println("                [SULLEY MONSTER LOGIN ACTIVO]            ");
    }
}
