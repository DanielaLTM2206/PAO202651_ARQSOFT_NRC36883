package ec.edu.monster.vista;

import ec.edu.monster.cliente.ExcepcionClienteWS;
import ec.edu.monster.controlador.ConversionControlador;
import ec.edu.monster.controlador.LoginControlador;
import ec.edu.monster.modelo.ConversionModelo;
import ec.edu.monster.modelo.MensajesModelo;
import ec.edu.monster.modelo.TipoConversion;
import ec.edu.monster.util.EntradaUtil;
import ec.edu.monster.util.FormatoUtil;

import java.util.List;

public class ConversionVista {

    private final MensajesModelo mensajes = new MensajesModelo();
    private final ConversionControlador conversionControlador;
    private final LoginControlador loginControlador;
    private boolean sesionActiva = true;

    public ConversionVista(ConversionControlador conversionControlador, LoginControlador loginControlador) {
        this.conversionControlador = conversionControlador;
        this.loginControlador = loginControlador;
    }

    /**
     * @return true si el usuario eligió salir del programa; false si debe volver al inicio de sesión.
     */
    public boolean ejecutarMenu() {
        while (sesionActiva) {
            mensajes.mostrarTitulo("MENU PRINCIPAL");
            mensajes.mostrarMensaje("  [1] Convertir Longitud");
            mensajes.mostrarMensaje("  [2] Convertir Masa");
            mensajes.mostrarMensaje("  [3] Convertir Temperatura");
            mensajes.mostrarMensaje("  [4] Cambiar contrasenia");
            mensajes.mostrarMensaje("  [5] Cerrar sesion");
            mensajes.mostrarMensaje("  [0] Salir");

            int opcion = EntradaUtil.leerEntero("  > Seleccione una opcion: ");

            switch (opcion) {
                case 1 -> procesarConversion(TipoConversion.LONGITUD);
                case 2 -> procesarConversion(TipoConversion.MASA);
                case 3 -> procesarConversion(TipoConversion.TEMPERATURA);
                case 4 -> procesarCambioContrasenia();
                case 5 -> {
                    mensajes.mostrarAviso("  Cerrando sesion...");
                    return false;
                }
                case 0 -> {
                    return true;
                }
                default -> mensajes.mostrarError("Opcion no valida. Intente nuevamente.");
            }
        }
        return false;
    }

    private void procesarConversion(TipoConversion tipo) {
        mensajes.mostrarTitulo("CONVERSION DE " + tipo.getNombreVisible().toUpperCase());

        try {
            List<String> unidadesPermitidas = conversionControlador.obtenerUnidades(tipo);

            double valor = solicitarValor(tipo);
            String unidadInicial = solicitarUnidad("inicial", unidadesPermitidas);
            String unidadFinal = solicitarUnidadFinal(unidadInicial, unidadesPermitidas);

            ConversionModelo modelo = new ConversionModelo(tipo, valor, unidadInicial, unidadFinal);
            double resultado = conversionControlador.convertir(modelo);

            mensajes.mostrarExito("Resultado: " + FormatoUtil.formatearNumero(valor) + " " + unidadInicial
                    + " = " + FormatoUtil.formatearNumero(resultado) + " " + unidadFinal);
        } catch (ExcepcionClienteWS ex) {
            manejarErrorServicio(ex);
        } catch (IllegalArgumentException ex) {
            mensajes.mostrarError(ex.getMessage());
        }
    }

    private void procesarCambioContrasenia() {
        mensajes.mostrarTitulo("CAMBIAR CONTRASENIA");

        String actual = EntradaUtil.leerTextoNoVacio("  > Contrasenia actual        : ");
        String nueva = EntradaUtil.leerTextoNoVacio("  > Nueva contrasenia         : ");
        String confirmacion = EntradaUtil.leerTextoNoVacio("  > Confirme la contrasenia  : ");

        if (!nueva.equals(confirmacion)) {
            mensajes.mostrarError("La confirmacion no coincide con la nueva contrasenia.");
            return;
        }

        try {
            mensajes.mostrarExito(loginControlador.cambiarContrasenia(actual, nueva));
        } catch (ExcepcionClienteWS ex) {
            manejarErrorServicio(ex);
        } catch (IllegalArgumentException ex) {
            mensajes.mostrarError(ex.getMessage());
        }
    }

    private double solicitarValor(TipoConversion tipo) {
        while (true) {
            double valor = EntradaUtil.leerDecimal("  > Valor a convertir: ");
            try {
                conversionControlador.validarValor(tipo, valor);
                return valor;
            } catch (IllegalArgumentException ex) {
                mensajes.mostrarError(ex.getMessage());
            }
        }
    }

    private String solicitarUnidadFinal(String unidadInicial, List<String> unidadesPermitidas) {
        while (true) {
            String unidadFinal = solicitarUnidad("final", unidadesPermitidas);
            try {
                conversionControlador.validarUnidades(unidadInicial, unidadFinal);
                return unidadFinal;
            } catch (IllegalArgumentException ex) {
                mensajes.mostrarError(ex.getMessage());
            }
        }
    }

    private String solicitarUnidad(String tipoUnidad, List<String> unidadesPermitidas) {
        mensajes.mostrarMensaje("  Seleccione la unidad " + tipoUnidad + ":");
        for (int indice = 0; indice < unidadesPermitidas.size(); indice++) {
            mensajes.mostrarMensaje("  [" + (indice + 1) + "] " + unidadesPermitidas.get(indice));
        }

        while (true) {
            int opcion = EntradaUtil.leerEntero("  > Opcion de unidad: ");
            if (opcion >= 1 && opcion <= unidadesPermitidas.size()) {
                return unidadesPermitidas.get(opcion - 1);
            }
            mensajes.mostrarError("Unidad no valida. Elija una opcion de la lista.");
        }
    }

    // Un 401 significa token inválido o sesión expirada: se vuelve al inicio de sesión
    private void manejarErrorServicio(ExcepcionClienteWS ex) {
        mensajes.mostrarError(ex.getMessage());
        if (ex.esErrorAutenticacion()) {
            mensajes.mostrarAviso("  Debe iniciar sesion nuevamente.");
            sesionActiva = false;
        }
    }
}
