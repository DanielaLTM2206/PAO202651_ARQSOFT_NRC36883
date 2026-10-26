package ec.edu.monster.modelo;

public class MensajesModelo {

    // Códigos ANSI de color (la consola de NetBeans y la de Windows 10+ los interpretan)
    private static final String RESET = "\u001B[0m";
    private static final String NEGRITA = "\u001B[1m";
    private static final String CIAN = "\u001B[36m";
    private static final String VERDE = "\u001B[32m";
    private static final String ROJO = "\u001B[31m";
    private static final String AMARILLO = "\u001B[33m";
    private static final String GRIS = "\u001B[90m";

    private static final int ANCHO = 66;

    // Arte ASCII ("ConUni" + "GR08"); las barras invertidas van escapadas
    private static final String[] LOGO = {
        "  ____            _   _       _     ____ ____   ___   ___  ",
        " / ___|___  _ __ | | | |_ __ (_)   / ___|  _ \\ / _ \\ ( _ ) ",
        "| |   / _ \\| '_ \\| | | | '_ \\| |  | |  _| |_) | | | |/ _ \\ ",
        "| |__| (_) | | | | |_| | | | | |  | |_| |  _ <| |_| | (_) |",
        " \\____\\___/|_| |_|\\___/|_| |_|_|   \\____|_| \\_\\\\___/ \\___/ "
    };

    public void mostrarBanner() {
        System.out.println(CIAN + borde('=') + RESET);
        for (String linea : LOGO) {
            System.out.println(CIAN + NEGRITA + lineaCentrada(linea) + RESET);
        }
        System.out.println(CIAN + borde('=') + RESET);
        System.out.println(CIAN + lineaCentrada("SISTEMA DE CONVERSION DE UNIDADES") + RESET);
        System.out.println(CIAN + lineaCentrada("Cliente Consola  -  Servicio RESTFUL Java") + RESET);
        System.out.println(CIAN + lineaCentrada("Longitud  |  Masa  |  Temperatura") + RESET);
        System.out.println(CIAN + borde('=') + RESET);
    }

    public void mostrarTitulo(String texto) {
        System.out.println();
        System.out.println(CIAN + borde('-') + RESET);
        System.out.println(CIAN + NEGRITA + lineaCentrada(texto) + RESET);
        System.out.println(CIAN + borde('-') + RESET);
    }

    public void mostrarMensaje(String mensaje) {
        System.out.println(mensaje);
    }

    public void mostrarDetalle(String mensaje) {
        System.out.println(GRIS + mensaje + RESET);
    }

    public void mostrarAviso(String mensaje) {
        System.out.println(AMARILLO + mensaje + RESET);
    }

    public void mostrarExito(String mensaje) {
        System.out.println(VERDE + NEGRITA + "  [OK] " + mensaje + RESET);
    }

    // Se usa System.out (no System.err) para que los errores salgan en orden con el resto del texto
    public void mostrarError(String mensaje) {
        System.out.println(ROJO + "  [ERROR] " + mensaje + RESET);
    }

    private static String borde(char relleno) {
        return "+" + String.valueOf(relleno).repeat(ANCHO - 2) + "+";
    }

    private static String lineaCentrada(String texto) {
        int espacio = ANCHO - 4 - texto.length();
        int izquierda = Math.max(espacio / 2, 0);
        int derecha = Math.max(espacio - izquierda, 0);
        return "| " + " ".repeat(izquierda) + texto + " ".repeat(derecha) + " |";
    }
}
