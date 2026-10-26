package ec.edu.monster.cliente.dto;

import com.google.gson.annotations.SerializedName;

public class ConversionSolicitud {

    @SerializedName("Valor")
    private double valor;

    @SerializedName("UnidadOrigen")
    private String unidadOrigen;

    @SerializedName("UnidadDestino")
    private String unidadDestino;

    public ConversionSolicitud() {
    }

    public ConversionSolicitud(double valor, String unidadOrigen, String unidadDestino) {
        this.valor = valor;
        this.unidadOrigen = unidadOrigen;
        this.unidadDestino = unidadDestino;
    }

    public double getValor() {
        return valor;
    }

    public String getUnidadOrigen() {
        return unidadOrigen;
    }

    public String getUnidadDestino() {
        return unidadDestino;
    }
}
