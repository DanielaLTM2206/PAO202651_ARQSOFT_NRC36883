package ec.edu.monster.modelos.dto;

import jakarta.json.bind.annotation.JsonbProperty;

/**
 * Una conversión realizada con éxito, tal como se guarda en el historial compartido.
 * FechaHora va en formato ISO-8601 (por ejemplo, 2026-10-18T19:28:36).
 */
public class RegistroConversionRespuesta {

    @JsonbProperty("FechaHora")
    private String fechaHora;

    @JsonbProperty("Tipo")
    private String tipo;

    @JsonbProperty("Valor")
    private double valor;

    @JsonbProperty("UnidadOrigen")
    private String unidadOrigen;

    @JsonbProperty("Resultado")
    private double resultado;

    @JsonbProperty("UnidadDestino")
    private String unidadDestino;

    public RegistroConversionRespuesta() {}

    public RegistroConversionRespuesta(String fechaHora, String tipo, double valor,
                                       String unidadOrigen, double resultado, String unidadDestino) {
        this.fechaHora = fechaHora;
        this.tipo = tipo;
        this.valor = valor;
        this.unidadOrigen = unidadOrigen;
        this.resultado = resultado;
        this.unidadDestino = unidadDestino;
    }

    public String getFechaHora() {
        return fechaHora;
    }

    public void setFechaHora(String fechaHora) {
        this.fechaHora = fechaHora;
    }

    public String getTipo() {
        return tipo;
    }

    public void setTipo(String tipo) {
        this.tipo = tipo;
    }

    public double getValor() {
        return valor;
    }

    public void setValor(double valor) {
        this.valor = valor;
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
}
