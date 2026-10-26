package ec.edu.monster.modelos.dto;

import jakarta.json.bind.annotation.JsonbProperty;

public class ConversionRespuesta {

    @JsonbProperty("ValorConvertido")
    private double valorConvertido;

    public ConversionRespuesta() {}

    public ConversionRespuesta(double valorConvertido) {
        this.valorConvertido = valorConvertido;
    }

    public double getValorConvertido() {
        return valorConvertido;
    }

    public void setValorConvertido(double valorConvertido) {
        this.valorConvertido = valorConvertido;
    }
}
