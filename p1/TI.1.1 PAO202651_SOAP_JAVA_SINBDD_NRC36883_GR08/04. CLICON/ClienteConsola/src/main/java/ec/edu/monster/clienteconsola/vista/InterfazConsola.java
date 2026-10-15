package ec.edu.monster.clienteconsola.vista;

import java.util.Scanner;

/**
 *
 * @author joanc
 */
public class InterfazConsola {
    private final Scanner entrada;

    public InterfazConsola() {
        this.entrada = new Scanner(System.in);
    }

    public OpcionMenu seleccionarConversion() {
        System.out.println("\n--- Menu de Conversion de Unidades ---");
        
        for (OpcionMenu opcion : OpcionMenu.values()) {
            System.out.println(opcion.getNumero() + ". " + opcion.getDescripcion());
        }
        
        System.out.print("Selecciona una opcion: ");
        String seleccion = entrada.nextLine();
        
        for (OpcionMenu opcion : OpcionMenu.values()) {
            if (opcion.getNumero().equals(seleccion)) {
                return opcion;
            }
        }
        
        System.out.println("Opcion invalida. Intenta de nuevo.");
        return null;
    }
    
    public <T extends Enum<T>> T seleccionarUnidad(String titulo, T[] opciones) {
        System.out.println("\n--- " + titulo + " ---");
        for (int i = 0; i < opciones.length; i++) {
            System.out.println((i + 1) + ". " + opciones[i].name());
        }
        System.out.print("Elige una unidad: ");
        
        try {
            int seleccion = Integer.parseInt(entrada.nextLine());
            if (seleccion >= 1 && seleccion <= opciones.length) {
                return opciones[seleccion - 1];
            }
        } catch (NumberFormatException e) {
            // Ignorar para que caiga en el error de abajo
        }
        
        System.out.println("Unidad invalida. Operacion cancelada.");
        return null;
    }
    
    public Double pedirValorAConvertir() {
        System.out.print("\nIngresa el valor a convertir: ");
        try {
            return Double.valueOf(entrada.nextLine());
        } catch (NumberFormatException e) {
            System.out.println("Debe ser un numero valido.");
            return null;
        }
    }
    
    public String pedirTexto(String mensaje) {
        System.out.print(mensaje);
        return entrada.nextLine().trim();
    }
}
