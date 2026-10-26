package ec.edu.monster.modelos.dto;

import jakarta.json.bind.annotation.JsonbProperty;

public class LoginRespuesta {

    @JsonbProperty("Token")
    private String token;

    public LoginRespuesta() {}

    public LoginRespuesta(String token) {
        this.token = token;
    }

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        this.token = token;
    }
}
