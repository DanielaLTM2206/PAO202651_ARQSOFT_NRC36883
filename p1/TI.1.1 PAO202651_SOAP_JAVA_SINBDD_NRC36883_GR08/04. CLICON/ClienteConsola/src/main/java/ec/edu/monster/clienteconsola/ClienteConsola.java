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
                    case SALIR -> System.out.println("Saliendo...");
                }
            }
        } while (opcion != OpcionMenu.SALIR);
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