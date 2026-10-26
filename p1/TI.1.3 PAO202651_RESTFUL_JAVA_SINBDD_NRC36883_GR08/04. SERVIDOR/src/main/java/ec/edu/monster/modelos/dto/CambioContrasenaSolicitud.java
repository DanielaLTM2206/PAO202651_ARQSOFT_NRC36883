package ec.edu.monster.modelos.dto;

import jakarta.json.bind.annotation.JsonbProperty;

public class CambioContrasenaSolicitud {

    @JsonbProperty("ContrasenaActual")
    private String contrasenaActual;

    @JsonbProperty("NuevaContrasena")
    private String nuevaContrasena;

    public CambioContrasenaSolicitud() {}

    public String getContrasenaActual() {
        return contrasenaActual;
    }

    public void setContrasenaActual(String contrasenaActual) {
        this.contrasenaActual = contrasenaActual;
    }

    public String getNuevaContrasena() {
        return nuevaContrasena;
    }

    public void setNuevaContrasena(String nuevaContrasena) {
        this.nuevaContrasena = nuevaContrasena;
    }
}
