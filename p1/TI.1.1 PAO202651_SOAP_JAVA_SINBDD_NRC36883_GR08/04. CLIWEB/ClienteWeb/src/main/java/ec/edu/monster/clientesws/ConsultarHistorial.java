package ec.edu.monster.clientesws;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlType;

@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "consultarHistorial", propOrder = {
    "token"
})
public class ConsultarHistorial {

    protected String token;

    public String getToken() {
        return token;
    }

    public void setToken(String value) {
        this.token = value;
    }
}
