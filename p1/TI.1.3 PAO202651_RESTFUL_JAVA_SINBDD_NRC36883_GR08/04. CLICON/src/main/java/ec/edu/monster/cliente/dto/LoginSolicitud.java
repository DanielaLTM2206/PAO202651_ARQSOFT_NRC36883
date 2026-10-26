package ec.edu.monster.cliente.dto;

import com.google.gson.annotations.SerializedName;

public class LoginSolicitud {

    @SerializedName("Usuario")
    private String usuario;

    @SerializedName("Contrasena")
    private String contrasena;

    public LoginSolicitud() {
    }

    public LoginSolicitud(String usuario, String contrasena) {
        this.usuario = usuario;
        this.contrasena = contrasena;
    }

    public String getUsuario() {
        return usuario;
    }

    public String getContrasena() {
        return contrasena;
    }
}
