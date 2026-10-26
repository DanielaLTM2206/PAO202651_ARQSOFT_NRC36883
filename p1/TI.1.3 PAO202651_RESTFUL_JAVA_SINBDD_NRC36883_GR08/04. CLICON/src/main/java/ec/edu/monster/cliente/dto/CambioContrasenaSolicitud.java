package ec.edu.monster.cliente.dto;

import com.google.gson.annotations.SerializedName;

public class CambioContrasenaSolicitud {

    @SerializedName("ContrasenaActual")
    private String contrasenaActual;

    @SerializedName("NuevaContrasena")
    private String nuevaContrasena;

    public CambioContrasenaSolicitud() {
    }

    public CambioContrasenaSolicitud(String contrasenaActual, String nuevaContrasena) {
        this.contrasenaActual = contrasenaActual;
        this.nuevaContrasena = nuevaContrasena;
    }

    public String getContrasenaActual() {
        return contrasenaActual;
    }

    public String getNuevaContrasena() {
        return nuevaContrasena;
    }
}
