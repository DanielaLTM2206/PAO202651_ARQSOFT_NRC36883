package ec.edu.monster.clientesws;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlType;

@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "cambiarContraseniaResponse", propOrder = {
    "_return"
})
public class CambiarContraseniaResponse {

    @XmlElement(name = "return")
    protected RespuestaAutenticacion _return;

    public RespuestaAutenticacion getReturn() {
        return _return;
    }

    public void setReturn(RespuestaAutenticacion value) {
        this._return = value;
    }
}
