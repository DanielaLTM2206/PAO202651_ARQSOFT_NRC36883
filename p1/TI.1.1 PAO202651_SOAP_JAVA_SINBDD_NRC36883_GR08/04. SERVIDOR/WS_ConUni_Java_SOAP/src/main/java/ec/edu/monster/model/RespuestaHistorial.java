package ec.edu.monster.model;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;
import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

/**
 * Respuesta estructurada homogénea para la consulta del historial de conversiones.
 * 
 * @author GR08 - Arquitectura de Software
 */
@XmlRootElement(name = "RespuestaHistorial")
@XmlAccessorType(XmlAccessType.FIELD)
public class RespuestaHistorial implements Serializable {

    private static final long serialVersionUID = 1L;

    @XmlElement(name = "estado", required = true)
    private String estado; // "OK" o "ERROR"

    @XmlElement(name = "codigo", required = true)
    private String codigo; // "200" o código de error

    @XmlElement(name = "mensaje", required = true)
    private String mensaje;

    @XmlElement(name = "item")
    private List<RegistroHistorialDTO> items = new ArrayList<>();

    public RespuestaHistorial() {
    }

    public RespuestaHistorial(String estado, String codigo, String mensaje) {
        this.estado = estado;
        this.codigo = codigo;
        this.mensaje = mensaje;
    }

    public static RespuestaHistorial exitoso(List<RegistroHistorialDTO> items) {
        RespuestaHistorial r = new RespuestaHistorial("OK", "200", "Historial consultado exitosamente");
        r.setItems(items);
        return r;
    }

    public static RespuestaHistorial error(String codigo, String mensaje) {
        return new RespuestaHistorial("ERROR", codigo, mensaje);
    }

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

    public List<RegistroHistorialDTO> getItems() {
        return items;
    }

    public void setItems(List<RegistroHistorialDTO> items) {
        this.items = items != null ? new ArrayList<>(items) : new ArrayList<>();
    }
}
