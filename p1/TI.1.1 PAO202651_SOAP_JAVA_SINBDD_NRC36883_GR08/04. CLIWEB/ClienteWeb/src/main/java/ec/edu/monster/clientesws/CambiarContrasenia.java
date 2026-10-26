package ec.edu.monster.clientesws;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlType;

@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "cambiarContrasenia", propOrder = {
    "contraseniaActual",
    "contraseniaNueva",
    "token"
})
public class CambiarContrasenia {

    protected String contraseniaActual;
    protected String contraseniaNueva;
    protected String token;

    public String getContraseniaActual() {
        return contraseniaActual;
    }

    public void setContraseniaActual(String value) {
        this.contraseniaActual = value;
    }

    public String getContraseniaNueva() {
        return contraseniaNueva;
    }

    public void setContraseniaNueva(String value) {
        this.contraseniaNueva = value;
    }

    public String getToken() {
        return token;
    }

    public void setToken(String value) {
        this.token = value;
    }
}
