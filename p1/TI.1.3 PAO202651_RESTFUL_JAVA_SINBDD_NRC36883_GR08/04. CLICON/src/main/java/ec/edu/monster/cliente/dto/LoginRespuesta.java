package ec.edu.monster.cliente.dto;

import com.google.gson.annotations.SerializedName;

public class LoginRespuesta {

    @SerializedName("Token")
    private String token;

    public LoginRespuesta() {
    }

    public String getToken() {
        return token;
    }
}
