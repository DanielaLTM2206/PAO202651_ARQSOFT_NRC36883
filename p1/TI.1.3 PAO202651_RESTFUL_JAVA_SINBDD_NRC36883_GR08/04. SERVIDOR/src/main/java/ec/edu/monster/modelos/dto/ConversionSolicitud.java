package ec.edu.monster.modelos.dto;

import jakarta.json.bind.annotation.JsonbProperty;

public class ConversionSolicitud {

    // Double y no double: si el cliente omite "Valor" llega null (400) en vez de un 0 silencioso
    @JsonbProperty("Valor")
    private Double valor;

    @JsonbProperty("UnidadOrigen")
    private String unidadOrigen;

    @JsonbProperty("UnidadDestino")
    private String unidadDestino;

    public ConversionSolicitud() {}

    public Double getValor() {
        return valor;
    }

    public void setValor(Double valor) {
        this.valor = valor;
    }

    public String getUnidadOrigen() {
        return unidadOrigen;
    }

    public void setUnidadOrigen(String unidadOrigen) {
        this.unidadOrigen = unidadOrigen;
    }

    public String getUnidadDestino() {
        return unidadDestino;
    }

    public void setUnidadDestino(String unidadDestino) {
        this.unidadDestino = unidadDestino;
    }
}
