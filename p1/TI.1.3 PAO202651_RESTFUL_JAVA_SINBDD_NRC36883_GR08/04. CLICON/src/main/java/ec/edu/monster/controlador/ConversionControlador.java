package ec.edu.monster.controlador;

import ec.edu.monster.cliente.ContratoWSConversor;
import ec.edu.monster.cliente.dto.RegistroConversionRespuesta;
import ec.edu.monster.modelo.ConversionModelo;
import ec.edu.monster.modelo.RegistroConversion;
import ec.edu.monster.modelo.TipoConversion;

import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.List;

public class ConversionControlador {

    private final ContratoWSConversor clienteWs;

    public ConversionControlador(ContratoWSConversor clienteWs) {
        this.clienteWs = clienteWs;
    }

    public double convertir(ConversionModelo modelo) {
        validarSolicitud(modelo);

        TipoConversion tipo = modelo.getTipoConversion();

        return switch (tipo) {
            case LONGITUD -> clienteWs.convertirLongitud(
                    modelo.getValor(),
                    modelo.getUnidadInicial(),
                    modelo.getUnidadFinal());
            case MASA -> clienteWs.convertirMasa(
                    modelo.getValor(),
                    modelo.getUnidadInicial(),
                    modelo.getUnidadFinal());
            case TEMPERATURA -> clienteWs.convertirTemperatura(
                    modelo.getValor(),
                    modelo.getUnidadInicial(),
                    modelo.getUnidadFinal());
        };
    }

    /**
     * Longitud y masa no admiten negativos. La temperatura sí, y el límite del cero
     * absoluto lo valida el servidor porque depende de la unidad de origen.
     */
    public void validarValor(TipoConversion tipo, double valor) {
        if (tipo == null) {
            throw new IllegalArgumentException("Debe seleccionar el tipo de conversion.");
        }
        if (!Double.isFinite(valor)) {
            throw new IllegalArgumentException("El valor debe ser un numero valido.");
        }
        if (tipo != TipoConversion.TEMPERATURA && valor < 0) {
            throw new IllegalArgumentException("La " + tipo.getNombreVisible().toLowerCase() + " no puede ser negativa.");
        }
    }

    public void validarUnidades(String unidadInicial, String unidadFinal) {
        if (unidadInicial == null || unidadInicial.isBlank()) {
            throw new IllegalArgumentException("La unidad inicial es obligatoria.");
        }
        if (unidadFinal == null || unidadFinal.isBlank()) {
            throw new IllegalArgumentException("La unidad final es obligatoria.");
        }
        if (unidadInicial.equalsIgnoreCase(unidadFinal)) {
            throw new IllegalArgumentException("La unidad final debe ser diferente a la unidad inicial.");
        }
    }

    // Las unidades se consultan al servidor (catálogo de unidades por categoría)
    public List<String> obtenerUnidades(TipoConversion tipo) {
        if (tipo == null) {
            throw new IllegalArgumentException("Debe seleccionar el tipo de conversion.");
        }
        return clienteWs.consultarUnidades(tipo.getCategoria());
    }

    /**
     * Historial compartido por todos los clientes: lo guarda el servidor, asi que incluye
     * las conversiones hechas desde la consola, el escritorio, la web y el movil.
     */
    public List<RegistroConversion> obtenerHistorial() {
        List<RegistroConversion> historial = new ArrayList<>();
        for (RegistroConversionRespuesta registro : clienteWs.consultarHistorial()) {
            RegistroConversion convertido = interpretarRegistro(registro);
            if (convertido != null) {
                historial.add(convertido);
            }
        }
        return historial;
    }

    // Borra el historial en el servidor: deja de verse en todos los clientes
    public String limpiarHistorial() {
        return clienteWs.limpiarHistorial();
    }

    // Convierte el registro del JSON al modelo; si llega incompleto, se omite en lugar de fallar
    public static RegistroConversion interpretarRegistro(RegistroConversionRespuesta registro) {
        if (registro == null || esVacio(registro.getFechaHora()) || esVacio(registro.getUnidadOrigen())
                || esVacio(registro.getUnidadDestino())) {
            return null;
        }
        TipoConversion tipo = buscarTipo(registro.getTipo());
        if (tipo == null) {
            return null;
        }
        try {
            LocalDateTime fechaHora = LocalDateTime.parse(registro.getFechaHora().trim());
            return new RegistroConversion(fechaHora, tipo, registro.getValor(), registro.getUnidadOrigen(),
                    registro.getResultado(), registro.getUnidadDestino());
        } catch (DateTimeParseException ex) {
            return null;
        }
    }

    // El servidor envia el tipo con su nombre visible ("Longitud", "Masa", "Temperatura")
    private static TipoConversion buscarTipo(String texto) {
        for (TipoConversion tipo : TipoConversion.values()) {
            if (tipo.getNombreVisible().equalsIgnoreCase(texto == null ? "" : texto.trim())) {
                return tipo;
            }
        }
        return null;
    }

    private static boolean esVacio(String texto) {
        return texto == null || texto.isBlank();
    }

    private void validarSolicitud(ConversionModelo modelo) {
        if (modelo == null) {
            throw new IllegalArgumentException("La solicitud de conversion es obligatoria.");
        }
        validarValor(modelo.getTipoConversion(), modelo.getValor());
        validarUnidades(modelo.getUnidadInicial(), modelo.getUnidadFinal());
    }
}
