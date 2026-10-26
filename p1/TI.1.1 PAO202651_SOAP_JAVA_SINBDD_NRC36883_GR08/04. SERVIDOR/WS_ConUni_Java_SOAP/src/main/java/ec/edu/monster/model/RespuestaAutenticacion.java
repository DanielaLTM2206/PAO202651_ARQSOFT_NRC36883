package ec.edu.monster.model;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;
import java.io.Serializable;

/**
 * RF-01: Autenticación de Usuario y Emisión de Token (CU-01)
 * 
 * @author GR08 - Arquitectura de Software
 */
@XmlRootElement(name = "RespuestaAutenticacion")
@XmlAccessorType(XmlAccessType.FIELD)
public class RespuestaAutenticacion implements Serializable {

    private static final long serialVersionUID = 1L;

    @XmlElement(name = "estado", required = true)
    private String estado; // "OK" o "ERROR"

    @XmlElement(name = "codigo", required = true)
    private String codigo; // "200" o "AUTH-001"

    @XmlElement(name = "mensaje", required = true)
    private String mensaje;

    @XmlElement(name = "token", required = false)
    private String token;

    public RespuestaAutenticacion() {
    }

    public RespuestaAutenticacion(String estado, String codigo, String mensaje, String token) {
        this.estado = estado;
        this.codigo = codigo;
        this.mensaje = mensaje;
        this.token = token;
    }

    public static RespuestaAutenticacion exitosa(String token) {
        return new RespuestaAutenticacion("OK", "200", "Autenticación exitosa. Token de sesión generado.", token);
    }

    public static RespuestaAutenticacion error(String mensaje) {
        return new RespuestaAutenticacion("ERROR", "AUTH-001", mensaje, null);
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

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        this.token = token;
    }

    @Override
    public String toString() {
        return "RespuestaAutenticacion{" +
                "estado='" + estado + '\'' +
                ", codigo='" + codigo + '\'' +
                ", mensaje='" + mensaje + '\'' +
                ", token='" + token + '\'' +
                '}';
    }
}
