package ec.edu.monster.cliente.dto;

import com.google.gson.annotations.SerializedName;

public class ConversionRespuesta {

    @SerializedName("ValorConvertido")
    private double valorConvertido;

    public ConversionRespuesta() {
    }

    public double getValorConvertido() {
        return valorConvertido;
    }
}
