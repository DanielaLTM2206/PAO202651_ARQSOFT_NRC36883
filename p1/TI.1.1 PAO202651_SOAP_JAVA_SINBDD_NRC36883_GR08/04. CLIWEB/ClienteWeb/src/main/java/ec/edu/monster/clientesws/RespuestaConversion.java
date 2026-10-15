
package ec.edu.monster.clientesws;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlType;


/**
 * <p>Clase Java para respuestaConversion complex type.
 * 
 * <p>El siguiente fragmento de esquema especifica el contenido que se espera que haya en esta clase.
 * 
 * <pre>{@code
 * <complexType name="respuestaConversion">
 *   <complexContent>
 *     <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       <sequence>
 *         <element name="estado" type="{http://www.w3.org/2001/XMLSchema}string"/>
 *         <element name="codigo" type="{http://www.w3.org/2001/XMLSchema}string"/>
 *         <element name="mensaje" type="{http://www.w3.org/2001/XMLSchema}string"/>
 *         <element name="resultado" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/>
 *         <element name="categoria" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         <element name="unidadOrigen" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         <element name="unidadDestino" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         <element name="valorOriginal" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/>
 *       </sequence>
 *     </restriction>
 *   </complexContent>
 * </complexType>
 * }</pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "respuestaConversion", propOrder = {
    "estado",
    "codigo",
    "mensaje",
    "resultado",
    "categoria",
    "unidadOrigen",
    "unidadDestino",
    "valorOriginal"
})
public class RespuestaConversion {

    @XmlElement(required = true)
    protected String estado;
    @XmlElement(required = true)
    protected String codigo;
    @XmlElement(required = true)
    protected String mensaje;
    protected Double resultado;
    protected String categoria;
    protected String unidadOrigen;
    protected String unidadDestino;
    protected Double valorOriginal;

    /**
     * Obtiene el valor de la propiedad estado.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getEstado() {
        return estado;
    }

    /**
     * Define el valor de la propiedad estado.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setEstado(String value) {
        this.estado = value;
    }

    /**
     * Obtiene el valor de la propiedad codigo.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getCodigo() {
        return codigo;
    }

    /**
     * Define el valor de la propiedad codigo.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setCodigo(String value) {
        this.codigo = value;
    }

    /**
     * Obtiene el valor de la propiedad mensaje.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getMensaje() {
        return mensaje;
    }

    /**
     * Define el valor de la propiedad mensaje.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setMensaje(String value) {
        this.mensaje = value;
    }

    /**
     * Obtiene el valor de la propiedad resultado.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getResultado() {
        return resultado;
    }

    /**
     * Define el valor de la propiedad resultado.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setResultado(Double value) {
        this.resultado = value;
    }

    /**
     * Obtiene el valor de la propiedad categoria.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getCategoria() {
        return categoria;
    }

    /**
     * Define el valor de la propiedad categoria.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setCategoria(String value) {
        this.categoria = value;
    }

    /**
     * Obtiene el valor de la propiedad unidadOrigen.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getUnidadOrigen() {
        return unidadOrigen;
    }

    /**
     * Define el valor de la propiedad unidadOrigen.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setUnidadOrigen(String value) {
        this.unidadOrigen = value;
    }

    /**
     * Obtiene el valor de la propiedad unidadDestino.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getUnidadDestino() {
        return unidadDestino;
    }

    /**
     * Define el valor de la propiedad unidadDestino.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setUnidadDestino(String value) {
        this.unidadDestino = value;
    }

    /**
     * Obtiene el valor de la propiedad valorOriginal.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getValorOriginal() {
        return valorOriginal;
    }

    /**
     * Define el valor de la propiedad valorOriginal.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setValorOriginal(Double value) {
        this.valorOriginal = value;
    }

}
