package ec.edu.monster.modelos.dto;

import jakarta.json.bind.annotation.JsonbProperty;

public class LoginSolicitud {

    @JsonbProperty("Usuario")
    private String usuario;

    @JsonbProperty("Contrasena")
    private String contrasena;

    public LoginSolicitud() {}

    public String getUsuario() {
        return usuario;
    }

    public void setUsuario(String usuario) {
        this.usuario = usuario;
    }

    public String getContrasena() {
        return contrasena;
    }

    public void setContrasena(String contrasena) {
        this.contrasena = contrasena;
    }
}
