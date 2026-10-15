package ec.edu.monster.model;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;
import java.io.Serializable;

/**
 * RF-06: Construcción de Respuesta Estructurada Uniforme (CU-07)
 * Toda invocación al servicio de conversión genera este XML de respuesta estructurada.
 * 
 * @author GR08 - Arquitectura de Software
 */
@XmlRootElement(name = "RespuestaConversion")
@XmlAccessorType(XmlAccessType.FIELD)
public class RespuestaConversion implements Serializable {

    private static final long serialVersionUID = 1L;

    @XmlElement(name = "estado", required = true)
    private String estado; // "OK" o "ERROR"

    @XmlElement(name = "codigo", required = true)
    private String codigo; // "200", "AUTH-001", "UNIT-001", "PARAM-001", "MATH-001"

    @XmlElement(name = "mensaje", required = true)
    private String mensaje;

    @XmlElement(name = "resultado", required = false)
    private Double resultado;

    @XmlElement(name = "categoria", required = false)
    private String categoria;

    @XmlElement(name = "unidadOrigen", required = false)
    private String unidadOrigen;

    @XmlElement(name = "unidadDestino", required = false)
    private String unidadDestino;

    @XmlElement(name = "valorOriginal", required = false)
    private Double valorOriginal;

    public RespuestaConversion() {
    }

    public RespuestaConversion(String estado, String codigo, String mensaje, Double resultado) {
        this.estado = estado;
        this.codigo = codigo;
        this.mensaje = mensaje;
        this.resultado = resultado;
    }

    public static RespuestaConversion exitosa(double resultado, String categoria, String origen, String destino, double valorOriginal) {
        RespuestaConversion r = new RespuestaConversion("OK", "200", "Conversión realizada exitosamente", resultado);
        r.setCategoria(categoria);
        r.setUnidadOrigen(origen);
        r.setUnidadDestino(destino);
        r.setValorOriginal(valorOriginal);
        return r;
    }

    public static RespuestaConversion error(String codigo, String mensaje) {
        return new RespuestaConversion("ERROR", codigo, mensaje, null);
    }

    // Getters y Setters
    public String getEstado() {
        return estado;
    }

    public void setEstado(String estado) {
        this.estado = estado;
    }

    public String getCodigo() {
        return codigo;
    }

    public void setCodigo(String codigo) {
        this.codigo = codigo;
    }

    public String getMensaje() {
        return mensaje;
    }

    public void setMensaje(String mensaje) {
        this.mensaje = mensaje;
    }

    public Double getResultado() {
        return resultado;
    }

    public void setResultado(Double resultado) {
        this.resultado = resultado;
    }

    public String getCategoria() {
        return categoria;
    }

    public void setCategoria(String categoria) {
        this.categoria = categoria;
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

    public Double getValorOriginal() {
        return valorOriginal;
    }

    public void setValorOriginal(Double valorOriginal) {
        this.valorOriginal = valorOriginal;
    }

    @Override
    public String toString() {
        return "RespuestaConversion{" +
                "estado='" + estado + '\'' +
                ", codigo='" + codigo + '\'' +
                ", mensaje='" + mensaje + '\'' +
                ", resultado=" + resultado +
                '}';
    }
}
