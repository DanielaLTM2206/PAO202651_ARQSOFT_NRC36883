
package ec.edu.monster.clientesws;

import java.util.ArrayList;
import java.util.List;
import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlType;


/**
 * <p>Clase Java para categoriaDTO complex type.
 * 
 * <p>El siguiente fragmento de esquema especifica el contenido que se espera que haya en esta clase.
 * 
 * <pre>{@code
 * <complexType name="categoriaDTO">
 *   <complexContent>
 *     <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       <sequence>
 *         <element name="nombre" type="{http://www.w3.org/2001/XMLSchema}string"/>
 *         <element name="unidadBase" type="{http://www.w3.org/2001/XMLSchema}string"/>
 *         <element name="unidades" minOccurs="0">
 *           <complexType>
 *             <complexContent>
 *               <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *                 <sequence>
 *                   <element name="unidad" type="{http://www.w3.org/2001/XMLSchema}string" maxOccurs="unbounded" minOccurs="0"/>
 *                 </sequence>
 *               </restriction>
 *             </complexContent>
 *           </complexType>
 *         </element>
 *       </sequence>
 *     </restriction>
 *   </complexContent>
 * </complexType>
 * }</pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "categoriaDTO", propOrder = {
    "nombre",
    "unidadBase",
    "unidades"
})
public class CategoriaDTO {

    @XmlElement(required = true)
    protected String nombre;
    @XmlElement(required = true)
    protected String unidadBase;
    protected CategoriaDTO.Unidades unidades;

    /**
     * Obtiene el valor de la propiedad nombre.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getNombre() {
        return nombre;
    }

    /**
     * Define el valor de la propiedad nombre.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setNombre(String value) {
        this.nombre = value;
    }

    /**
     * Obtiene el valor de la propiedad unidadBase.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getUnidadBase() {
        return unidadBase;
    }

    /**
     * Define el valor de la propiedad unidadBase.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setUnidadBase(String value) {
        this.unidadBase = value;
    }

    /**
     * Obtiene el valor de la propiedad unidades.
     * 
     * @return
     *     possible object is
     *     {@link CategoriaDTO.Unidades }
     *     
     */
    public CategoriaDTO.Unidades getUnidades() {
        return unidades;
    }

    /**
     * Define el valor de la propiedad unidades.
     * 
     * @param value
     *     allowed object is
     *     {@link CategoriaDTO.Unidades }
     *     
     */
    public void setUnidades(CategoriaDTO.Unidades value) {
        this.unidades = value;
    }


    /**
     * <p>Clase Java para anonymous complex type.
     * 
     * <p>El siguiente fragmento de esquema especifica el contenido que se espera que haya en esta clase.
     * 
     * <pre>{@code
     * <complexType>
     *   <complexContent>
     *     <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
     *       <sequence>
     *         <element name="unidad" type="{http://www.w3.org/2001/XMLSchema}string" maxOccurs="unbounded" minOccurs="0"/>
     *       </sequence>
     *     </restriction>
     *   </complexContent>
     * </complexType>
     * }</pre>
     * 
     * 
     */
    @XmlAccessorType(XmlAccessType.FIELD)
    @XmlType(name = "", propOrder = {
        "unidad"
    })
    public static class Unidades {

        protected List<String> unidad;

        /**
         * Gets the value of the unidad property.
         * 
         * <p>
         * This accessor method returns a reference to the live list,
         * not a snapshot. Therefore any modification you make to the
         * returned list will be present inside the Jakarta XML Binding object.
         * This is why there is not a {@code set} method for the unidad property.
         * 
         * <p>
         * For example, to add a new item, do as follows:
         * <pre>
         *    getUnidad().add(newItem);
         * </pre>
         * 
         * 
         * <p>
         * Objects of the following type(s) are allowed in the list
         * {@link String }
         * 
         * 
         * @return
         *     The value of the unidad property.
         */
        public List<String> getUnidad() {
            if (unidad == null) {
                unidad = new ArrayList<>();
            }
            return this.unidad;
        }

    }

}
