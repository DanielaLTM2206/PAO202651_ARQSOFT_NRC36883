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
 * RF-02: Consulta de Catálogo y Unidades (CU-02, CU-03)
 * 
 * @author GR08 - Arquitectura de Software
 */
@XmlRootElement(name = "RespuestaCatalogo")
@XmlAccessorType(XmlAccessType.FIELD)
public class RespuestaCatalogo implements Serializable {

    private static final long serialVersionUID = 1L;

    @XmlElement(name = "estado", required = true)
    private String estado; // "OK" o "ERROR"

    @XmlElement(name = "codigo", required = true)
    private String codigo; // "200" o códigos de error

    @XmlElement(name = "mensaje", required = true)
    private String mensaje;

    @XmlElement(name = "categoria")
    private List<CategoriaDTO> categorias = new ArrayList<>();

    public RespuestaCatalogo() {
    }

    public RespuestaCatalogo(String estado, String codigo, String mensaje) {
        this.estado = estado;
        this.codigo = codigo;
        this.mensaje = mensaje;
    }

    public static RespuestaCatalogo exitoso(List<CategoriaDTO> categorias) {
        RespuestaCatalogo r = new RespuestaCatalogo("OK", "200", "Catálogo consultado exitosamente");
        r.setCategorias(categorias);
        return r;
    }

    public static RespuestaCatalogo error(String codigo, String mensaje) {
        return new RespuestaCatalogo("ERROR", codigo, mensaje);
    }

    public String getEstado() {
        return estado;
    }

    public void setEstado(String estado) {
        this.estado = estado;
    }

    public String getCodigo() {
        return codigo;
    }

    public void setCodigo(String codigo) {
        this.codigo = codigo;
    }

    public String getMensaje() {
        return mensaje;
    }

    public void setMensaje(String mensaje) {
        this.mensaje = mensaje;
    }

    public List<CategoriaDTO> getCategorias() {
        return categorias;
    }

    public void setCategorias(List<CategoriaDTO> categorias) {
        this.categorias = categorias != null ? new ArrayList<>(categorias) : new ArrayList<>();
    }
}
