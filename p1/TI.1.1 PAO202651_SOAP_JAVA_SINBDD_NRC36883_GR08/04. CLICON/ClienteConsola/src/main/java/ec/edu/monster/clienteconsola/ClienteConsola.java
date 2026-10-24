package ec.edu.monster.clienteconsola;

import ec.edu.monster.clienteconsola.controlador.ControladorConversiones;
import ec.edu.monster.clienteconsola.vista.InterfazConsola;
import ec.edu.monster.clienteconsola.vista.OpcionMenu;
import ec.edu.monster.clienteconsola.modelo.CatalogoUnidades;


public class ClienteConsola {

    public static void main(String[] args) {
        InterfazConsola interfaz = new InterfazConsola();
        // Instanciamos nuestro nuevo controlador
        ControladorConversiones controlador = new ControladorConversiones(); 
        
        System.out.println("=== BIENVENIDO AL SISTEMA DE CONVERSIONES ===");
        boolean autenticado = false;
        
        while (!autenticado) {
            // Nota: Debes implementar estos dos métodos en InterfazConsola
            String usuario = interfaz.pedirTexto("Ingresa tu usuario: ");
            String clave = interfaz.pedirTexto("Ingresa tu clave: ");
            
            System.out.println("[Autenticando...]");
            autenticado = controlador.iniciarSesion(usuario, clave);
            
            if (!autenticado) {
                System.out.println("Credenciales incorrectas o error de red. Intenta nuevamente.\n");
            }
        }
        
        System.out.println("Sesion iniciada con exito\n");
        
        
        OpcionMenu opcion;

        do {
            opcion = interfaz.seleccionarConversion();
            
            if (opcion != null) {
                switch (opcion) {
                    case CONVERTIR_LONGITUD -> ejecutarFlujoConversion(
                            interfaz, 
                            CatalogoUnidades.Longitud.values(), 
                            (v, o, d) -> controlador.convertirLongitud(v, o, d)
                    );
                    case CONVERTIR_TEMPERATURA -> ejecutarFlujoConversion(
                            interfaz, 
                            CatalogoUnidades.Temperatura.values(), 
                            (v, o, d) -> controlador.convertirTemperatura(v, o, d)
                    );
                    case CONVERTIR_MASA -> ejecutarFlujoConversion(
                            interfaz, 
                            CatalogoUnidades.Masa.values(), 
                            (v, o, d) -> controlador.convertirMasa(v, o, d)
                    );
                    case VER_HISTORIAL -> mostrarHistorial(controlador);
                    case CAMBIAR_CONTRASENIA -> ejecutarFlujoCambioClave(interfaz, controlador);
                    case SALIR -> System.out.println("Saliendo...");
                }
            }
        } while (opcion != OpcionMenu.SALIR);
    }

    private static void ejecutarFlujoCambioClave(InterfazConsola interfaz, ControladorConversiones controlador) {
        System.out.println("\n--- CAMBIO DE CONTRASEÑA CENTRALIZADO (SIN BDD - MEMORIA) ---");
        System.out.println("Nota: El cambio se sincronizará inmediatamente en todos los clientes (Web, Escritorio, Móvil).");
        String actual = interfaz.pedirTexto("Ingresa tu contraseña ACTUAL: ");
        String nueva = interfaz.pedirTexto("Ingresa tu NUEVA contraseña: ");
        String confirmar = interfaz.pedirTexto("Confirma tu NUEVA contraseña: ");

        if (!nueva.equals(confirmar)) {
            System.out.println("❌ ERROR: La confirmación no coincide con la nueva contraseña.\n");
            return;
        }

        System.out.println("[Enviando solicitud SOAP al servidor central...]");
        var resp = controlador.cambiarContrasenia(actual, nueva);
        if ("OK".equalsIgnoreCase(resp.getEstado())) {
            System.out.println("✅ ÉXITO: " + resp.getMensaje());
            System.out.println("La nueva clave ya está activa en memoria para todos los canales.\n");
        } else {
            System.out.println("❌ ERROR [" + resp.getCodigo() + "]: " + resp.getMensaje() + "\n");
        }
    }

    private static void mostrarHistorial(ControladorConversiones controlador) {
        System.out.println("\n==========================================================================");
        System.out.println("     HISTORIAL GLOBAL COMPARTIDO DE CONVERSIONES (SIN BDD - MEMORIA)      ");
        System.out.println("==========================================================================");
        var items = controlador.obtenerHistorial();
        if (items == null || items.isEmpty()) {
            System.out.println("No hay conversiones registradas actualmente en el servidor.");
        } else {
            System.out.printf("%-4s | %-19s | %-12s | %s%n", "ID", "FECHA Y HORA", "CATEGORÍA", "CONVERSIÓN REALIZADA");
            System.out.println("--------------------------------------------------------------------------");
            for (var r : items) {
                String conv = String.format(java.util.Locale.US, "%.2f %s ➜ %.4f %s",
                        r.getValorOriginal(), r.getUnidadOrigen(), r.getResultado(), r.getUnidadDestino());
                System.out.printf("%-4d | %-19s | %-12s | %s%n",
                        r.getId(), r.getFechaHora(), r.getCategoria(), conv);
            }
        }
        System.out.println("==========================================================================\n");
    }

    /**
     * Interfaz funcional simplificada. Ya no maneja credenciales.
     */
    @FunctionalInterface
    private interface OperacionSOAP {
        double ejecutar(double valor, String origen, String destino) throws Exception;
    }

    /**
     * Flujo de UI genérico.
     */
    private static <T extends Enum<T>> void ejecutarFlujoConversion(
            InterfazConsola interfaz, 
            T[] valoresCatalogo, 
            OperacionSOAP operacion) {
        
        T origen = interfaz.seleccionarUnidad("Selecciona la unidad de ORIGEN", valoresCatalogo);
        if (origen == null) return;
        
        T destino = interfaz.seleccionarUnidad("Selecciona la unidad de DESTINO", valoresCatalogo);
        if (destino == null) return;

        Double valor = interfaz.pedirValorAConvertir();
        if (valor == null) return;

        System.out.println("\n[Llamando Controlador...] Convirtiendo " + valor + " de " + origen.name() + " a " + destino.name());
        
        try {
            // El controlador maneja la complejidad subyacente
            double result = operacion.ejecutar(valor, origen.name(), destino.name());
            System.out.println("Resultado = " + result);
        } catch (Exception ex) {
            System.err.println("Error durante la conversion: " + ex.getMessage());
        }
    }
}