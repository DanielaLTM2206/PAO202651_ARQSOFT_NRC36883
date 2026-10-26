package ec.edu.monster.conuniclientemovil.model;

import java.io.Serializable;

/**
 * Representa un registro en el historial de conversiones para el cliente móvil Android.
 * 
 * @author GR08 - Arquitectura de Software
 */
public class RegistroHistorial implements Serializable {

    private int id;
    private String fechaHora;
    private String categoria;
    private double valorOriginal;
    private String unidadOrigen;
    private double resultado;
    private String unidadDestino;

    public RegistroHistorial() {
    }

    public RegistroHistorial(int id, String fechaHora, String categoria, double valorOriginal, 
                             String unidadOrigen, double resultado, String unidadDestino) {
        this.id = id;
        this.fechaHora = fechaHora;
        this.categoria = categoria;
        this.valorOriginal = valorOriginal;
        this.unidadOrigen = unidadOrigen;
        this.resultado = resultado;
        this.unidadDestino = unidadDestino;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getFechaHora() {
        return fechaHora;
    }

    public void setFechaHora(String fechaHora) {
        this.fechaHora = fechaHora;
    }

    public String getCategoria() {
        return categoria;
    }

    public void setCategoria(String categoria) {
        this.categoria = categoria;
    }

    public double getValorOriginal() {
        return valorOriginal;
    }

    public void setValorOriginal(double valorOriginal) {
        this.valorOriginal = valorOriginal;
    }

    public String getUnidadOrigen() {
        return unidadOrigen;
    }

    public void setUnidadOrigen(String unidadOrigen) {
        this.unidadOrigen = unidadOrigen;
    }

    public double getResultado() {
        return resultado;
    }

    public void setResultado(double resultado) {
        this.resultado = resultado;
    }

    public String getUnidadDestino() {
        return unidadDestino;
    }

    public void setUnidadDestino(String unidadDestino) {
        this.unidadDestino = unidadDestino;
    }

    public String getResumenConversion() {
        return String.format(java.util.Locale.US, "%.1f %s → %.2f %s", valorOriginal, unidadOrigen, resultado, unidadDestino);
    }
}
