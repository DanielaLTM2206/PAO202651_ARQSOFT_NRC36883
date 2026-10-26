package ec.edu.monster.cliente.dto;

import com.google.gson.annotations.SerializedName;

import java.util.List;

public class UnidadesCategoriaRespuesta {

    @SerializedName("Categoria")
    private String categoria;

    @SerializedName("Unidades")
    private List<String> unidades;

    public UnidadesCategoriaRespuesta() {
    }

    public String getCategoria() {
        return categoria;
    }

    public List<String> getUnidades() {
        return unidades;
    }
}
