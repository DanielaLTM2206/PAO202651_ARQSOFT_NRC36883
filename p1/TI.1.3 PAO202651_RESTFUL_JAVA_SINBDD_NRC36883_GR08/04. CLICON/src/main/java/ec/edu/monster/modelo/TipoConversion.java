package ec.edu.monster.modelo;

public enum TipoConversion {
    LONGITUD("Longitud", "longitud"),
    MASA("Masa", "masa"),
    TEMPERATURA("Temperatura", "temperatura");

    private final String nombreVisible;
    private final String categoria;

    TipoConversion(String nombreVisible, String categoria) {
        this.nombreVisible = nombreVisible;
        this.categoria = categoria;
    }

    public String getNombreVisible() {
        return nombreVisible;
    }

    // Categoría tal como la espera el servicio en /unidades/{categoria}
    public String getCategoria() {
        return categoria;
    }
}
