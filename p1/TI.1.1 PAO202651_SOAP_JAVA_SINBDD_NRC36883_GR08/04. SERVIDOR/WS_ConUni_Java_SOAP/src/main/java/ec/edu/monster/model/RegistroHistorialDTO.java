package ec.edu.monster.model;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;
import java.io.Serializable;

/**
 * Representa una entrada individual en el historial global de conversiones.
 * Almacena los detalles completos de la operación realizada por cualquier cliente.
 * 
 * @author GR08 - Arquitectura de Software
 */
@XmlRootElement(name = "RegistroHistorialDTO")
@XmlAccessorType(XmlAccessType.FIELD)
public class RegistroHistorialDTO implements Serializable {

    private static final long serialVersionUID = 1L;

    @XmlElement(name = "id", required = true)
    private int id;

    @XmlElement(name = "fechaHora", required = true)
    private String fechaHora;

    @XmlElement(name = "categoria", required = true)
    private String categoria;

    @XmlElement(name = "valorOriginal", required = true)
    private Double valorOriginal;

    @XmlElement(name = "unidadOrigen", required = true)
    private String unidadOrigen;

    @XmlElement(name = "resultado", required = true)
    private Double resultado;

    @XmlElement(name = "unidadDestino", required = true)
    private String unidadDestino;

    public RegistroHistorialDTO() {
    }

    public RegistroHistorialDTO(int id, String fechaHora, String categoria, Double valorOriginal, 
                               String unidadOrigen, Double resultado, String unidadDestino) {
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

    public Double getValorOriginal() {
        return valorOriginal;
    }

    public void setValorOriginal(Double valorOriginal) {
        this.valorOriginal = valorOriginal;
    }

    public String getUnidadOrigen() {
        return unidadOrigen;
    }

    public void setUnidadOrigen(String unidadOrigen) {
        this.unidadOrigen = unidadOrigen;
    }

    public Double getResultado() {
        return resultado;
    }

    public void setResultado(Double resultado) {
        this.resultado = resultado;
    }

    public String getUnidadDestino() {
        return unidadDestino;
    }

    public void setUnidadDestino(String unidadDestino) {
        this.unidadDestino = unidadDestino;
    }

    @Override
    public String toString() {
        return "RegistroHistorialDTO{" +
                "id=" + id +
                ", fechaHora='" + fechaHora + '\'' +
                ", categoria='" + categoria + '\'' +
                ", valorOriginal=" + valorOriginal +
                ", unidadOrigen='" + unidadOrigen + '\'' +
                ", resultado=" + resultado +
                ", unidadDestino='" + unidadDestino + '\'' +
                '}';
    }
}
