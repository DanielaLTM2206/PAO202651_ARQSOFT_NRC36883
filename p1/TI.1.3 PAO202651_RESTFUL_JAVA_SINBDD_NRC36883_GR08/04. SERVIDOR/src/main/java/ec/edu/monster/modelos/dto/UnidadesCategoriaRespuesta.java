package ec.edu.monster.modelos.dto;

import jakarta.json.bind.annotation.JsonbProperty;

import java.util.List;

public class UnidadesCategoriaRespuesta {

    @JsonbProperty("Categoria")
    private String categoria;

    @JsonbProperty("Unidades")
    private List<String> unidades;

    public UnidadesCategoriaRespuesta() {}

    public UnidadesCategoriaRespuesta(String categoria, List<String> unidades) {
        this.categoria = categoria;
        this.unidades = unidades;
    }

    public String getCategoria() {
        return categoria;
    }

    public void setCategoria(String categoria) {
        this.categoria = categoria;
    }

    public List<String> getUnidades() {
        return unidades;
    }

    public void setUnidades(List<String> unidades) {
        this.unidades = unidades;
    }
}
