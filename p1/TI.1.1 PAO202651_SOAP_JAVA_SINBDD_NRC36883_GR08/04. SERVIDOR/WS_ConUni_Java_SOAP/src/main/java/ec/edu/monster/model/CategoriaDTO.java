package ec.edu.monster.model;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlElementWrapper;
import jakarta.xml.bind.annotation.XmlRootElement;
import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

/**
 * Representa una categoría física de conversión con su unidad base y unidades soportadas.
 * 
 * @author GR08 - Arquitectura de Software
 */
@XmlRootElement(name = "CategoriaDTO")
@XmlAccessorType(XmlAccessType.FIELD)
public class CategoriaDTO implements Serializable {

    private static final long serialVersionUID = 1L;

    @XmlElement(name = "nombre", required = true)
    private String nombre; // "Longitud", "Masa", "Temperatura"

    @XmlElement(name = "unidadBase", required = true)
    private String unidadBase;

    @XmlElementWrapper(name = "unidades")
    @XmlElement(name = "unidad")
    private List<String> unidades = new ArrayList<>();

    public CategoriaDTO() {
    }

    public CategoriaDTO(String nombre, String unidadBase, List<String> unidades) {
        this.nombre = nombre;
        this.unidadBase = unidadBase;
        this.unidades = unidades != null ? new ArrayList<>(unidades) : new ArrayList<>();
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public String getUnidadBase() {
        return unidadBase;
    }

    public void setUnidadBase(String unidadBase) {
        this.unidadBase = unidadBase;
    }

    public List<String> getUnidades() {
        return unidades;
    }

    public void setUnidades(List<String> unidades) {
        this.unidades = unidades;
    }
}
