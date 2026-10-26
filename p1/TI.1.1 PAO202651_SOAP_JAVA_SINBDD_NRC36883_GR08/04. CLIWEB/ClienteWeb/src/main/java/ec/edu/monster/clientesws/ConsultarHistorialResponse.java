package ec.edu.monster.clientesws;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlType;

@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "consultarHistorialResponse", propOrder = {
    "_return"
})
public class ConsultarHistorialResponse {

    @XmlElement(name = "return")
    protected RespuestaHistorial _return;

    public RespuestaHistorial getReturn() {
        return _return;
    }

    public void setReturn(RespuestaHistorial value) {
        this._return = value;
    }
}
