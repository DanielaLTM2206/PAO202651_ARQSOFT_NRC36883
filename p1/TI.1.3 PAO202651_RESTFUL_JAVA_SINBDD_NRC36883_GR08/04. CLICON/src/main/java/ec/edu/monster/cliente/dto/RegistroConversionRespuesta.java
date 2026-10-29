package ec.edu.monster.cliente.dto;

import com.google.gson.annotations.SerializedName;

/**
 * Una conversion del historial compartido, tal como la envia GET /conversiones/historial.
 */
public class RegistroConversionRespuesta {

    @SerializedName("FechaHora")
    private String fechaHora;

    @SerializedName("Tipo")
    private String tipo;

    @SerializedName("Valor")
    private double valor;

    @SerializedName("UnidadOrigen")
    private String unidadOrigen;

    @SerializedName("Resultado")
    private double resultado;

    @SerializedName("UnidadDestino")
    private String unidadDestino;

    public RegistroConversionRespuesta() {
    }

    public RegistroConversionRespuesta(String fechaHora, String tipo, double valor, String unidadOrigen,
            double resultado, String unidadDestino) {
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

    public String getTipo() {
        return tipo;
    }

    public double getValor() {
        return valor;
    }

    public String getUnidadOrigen() {
        return unidadOrigen;
    }

    public double getResultado() {
        return resultado;
    }

    public String getUnidadDestino() {
        return unidadDestino;
    }
}
