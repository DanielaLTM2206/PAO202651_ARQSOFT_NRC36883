package ec.edu.monster.clientesws;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlType;

@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "registroHistorialDTO", propOrder = {
    "id",
    "fechaHora",
    "categoria",
    "valorOriginal",
    "unidadOrigen",
    "resultado",
    "unidadDestino"
})
public class RegistroHistorialDTO {

    protected int id;
    @XmlElement(required = true)
    protected String fechaHora;
    @XmlElement(required = true)
    protected String categoria;
    @XmlElement(required = true)
    protected Double valorOriginal;
    @XmlElement(required = true)
    protected String unidadOrigen;
    @XmlElement(required = true)
    protected Double resultado;
    @XmlElement(required = true)
    protected String unidadDestino;

    public int getId() {
        return id;
    }

    public void setId(int value) {
        this.id = value;
    }

    public String getFechaHora() {
        return fechaHora;
    }

    public void setFechaHora(String value) {
        this.fechaHora = value;
    }

    public String getCategoria() {
        return categoria;
    }

    public void setCategoria(String value) {
        this.categoria = value;
    }

    public Double getValorOriginal() {
        return valorOriginal;
    }

    public void setValorOriginal(Double value) {
        this.valorOriginal = value;
    }

    public String getUnidadOrigen() {
        return unidadOrigen;
    }

    public void setUnidadOrigen(String value) {
        this.unidadOrigen = value;
    }

    public Double getResultado() {
        return resultado;
    }

    public void setResultado(Double value) {
        this.resultado = value;
    }

    public String getUnidadDestino() {
        return unidadDestino;
    }

    public void setUnidadDestino(String value) {
        this.unidadDestino = value;
    }
}
