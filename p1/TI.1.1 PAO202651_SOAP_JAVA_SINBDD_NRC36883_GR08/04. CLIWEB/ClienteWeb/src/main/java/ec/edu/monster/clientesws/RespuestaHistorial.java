package ec.edu.monster.clientesws;

import java.util.ArrayList;
import java.util.List;
import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlType;

@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "respuestaHistorial", propOrder = {
    "estado",
    "codigo",
    "mensaje",
    "item"
})
public class RespuestaHistorial {

    @XmlElement(required = true)
    protected String estado;
    @XmlElement(required = true)
    protected String codigo;
    @XmlElement(required = true)
    protected String mensaje;
    @XmlElement(name = "item")
    protected List<RegistroHistorialDTO> item;

    public String getEstado() {
        return estado;
    }

    public void setEstado(String value) {
        this.estado = value;
    }

    public String getCodigo() {
        return codigo;
    }

    public void setCodigo(String value) {
        this.codigo = value;
    }

    public String getMensaje() {
        return mensaje;
    }

    public void setMensaje(String value) {
        this.mensaje = value;
    }

    public List<RegistroHistorialDTO> getItem() {
        if (item == null) {
            item = new ArrayList<>();
        }
        return this.item;
    }

    public void setItem(List<RegistroHistorialDTO> item) {
        this.item = item;
    }
}
