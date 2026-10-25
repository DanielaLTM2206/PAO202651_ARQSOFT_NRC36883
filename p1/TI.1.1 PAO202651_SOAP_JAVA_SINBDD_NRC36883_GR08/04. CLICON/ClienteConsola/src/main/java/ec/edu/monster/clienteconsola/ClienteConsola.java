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
        System.out.println("\n--- CAMBIO DE CONTRASEÑA ---");
        System.out.println("El cambio se sincronizará inmediatamente en todos los clientes (Web, Escritorio, Móvil).");
        String actual = interfaz.pedirTexto("Ingresa tu contraseña actual: ");
        String nueva = interfaz.pedirTexto("Ingresa tu nueva contraseña: ");
        String confirmar = interfaz.pedirTexto("Confirma tu nueva contraseña: ");

        if (actual.isEmpty() || nueva.isEmpty() || confirmar.isEmpty()) {
            System.out.println("❌ Error: Por favor completa todos los campos de contraseña.\n");
            return;
        }
        if (nueva.length() < 4) {
            System.out.println("❌ Error: La nueva contraseña debe tener al menos 4 caracteres.\n");
            return;
        }
        if (actual.equals(nueva)) {
            System.out.println("❌ Error: La nueva contraseña debe ser diferente a la contraseña actual.\n");
            return;
        }
        if (!nueva.equals(confirmar)) {
            System.out.println("❌ Error: La confirmación no coincide con la nueva contraseña.\n");
            return;
        }

        System.out.println("[Actualizando contraseña en el servidor...]");
        var resp = controlador.cambiarContrasenia(actual, nueva);
        if ("OK".equalsIgnoreCase(resp.getEstado())) {
            System.out.println("✅ Éxito: " + (resp.getMensaje() != null ? resp.getMensaje() : "Contraseña actualizada exitosamente."));
            System.out.println("La nueva clave ya se encuentra sincronizada en todos los clientes.\n");
        } else {
            System.out.println("❌ Error: " + (resp.getMensaje() != null ? resp.getMensaje() : "No se pudo actualizar la contraseña.") + "\n");
        }
    }

    private static void mostrarHistorial(ControladorConversiones controlador) {
        System.out.println("\n==========================================================================");
        System.out.println("     HISTORIAL GLOBAL COMPARTIDO DE CONVERSIONES      ");
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

        // Validar coherencia física
        String origStr = origen.name().toLowerCase();
        if (origStr.contains("kelvin") && valor < 0) {
            System.out.println("❌ Error: La temperatura en Kelvin no puede ser menor a 0 K (cero absoluto).\n");
            return;
        }
        if (origStr.contains("celsius") && valor < -273.15) {
            System.out.println("❌ Error: La temperatura en Celsius no puede ser menor a -273.15 °C (cero absoluto).\n");
            return;
        }
        if (origStr.contains("fahrenheit") && valor < -459.67) {
            System.out.println("❌ Error: La temperatura en Fahrenheit no puede ser menor a -459.67 °F (cero absoluto).\n");
            return;
        }
        if ((origStr.contains("metro") || origStr.contains("centimetro") || origStr.contains("pie") || origStr.contains("yarda") || origStr.contains("milla")) && valor < 0) {
            System.out.println("❌ Error: La longitud no puede ser un valor negativo.\n");
            return;
        }
        if ((origStr.contains("gramo") || origStr.contains("kilogramo") || origStr.contains("libra") || origStr.contains("onza") || origStr.contains("quintal")) && valor < 0) {
            System.out.println("❌ Error: La masa no puede ser un valor negativo.\n");
            return;
        }

        // Mismas unidades
        if (origen == destino) {
            System.out.printf("ℹ️  Nota: Las unidades de origen y destino son iguales (%s).%n", origen.name());
            System.out.printf("Resultado = %.4f %s%n%n", valor, destino.name());
            return;
        }

        System.out.println("\n[Procesando conversión...] " + valor + " " + origen.name() + " ➔ " + destino.name());
        
        try {
            double result = operacion.ejecutar(valor, origen.name(), destino.name());
            System.out.printf("Resultado = %.4f %s%n%n", result, destino.name());
        } catch (Exception ex) {
            System.err.println("❌ No se pudo completar la conversión: " + ex.getMessage());
        }
    }
}