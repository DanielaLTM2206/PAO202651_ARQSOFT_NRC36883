
package ec.edu.monster.clientesws;

import javax.xml.namespace.QName;
import jakarta.xml.bind.JAXBElement;
import jakarta.xml.bind.annotation.XmlElementDecl;
import jakarta.xml.bind.annotation.XmlRegistry;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the ec.edu.monster.clientesws package. 
 * <p>An ObjectFactory allows you to programatically 
 * construct new instances of the Java representation 
 * for XML content. The Java representation of XML 
 * content can consist of schema derived interfaces 
 * and classes representing the binding of schema 
 * type definitions, element declarations and model 
 * groups.  Factory methods for each of these are 
 * provided in this class.
 * 
 */
@XmlRegistry
public class ObjectFactory {

    private static final QName _CategoriaDTO_QNAME = new QName("http://monster.edu.ec/services", "CategoriaDTO");
    private static final QName _RespuestaAutenticacion_QNAME = new QName("http://monster.edu.ec/services", "RespuestaAutenticacion");
    private static final QName _RespuestaCatalogo_QNAME = new QName("http://monster.edu.ec/services", "RespuestaCatalogo");
    private static final QName _RespuestaConversion_QNAME = new QName("http://monster.edu.ec/services", "RespuestaConversion");
    private static final QName _Autenticar_QNAME = new QName("http://monster.edu.ec/services", "autenticar");
    private static final QName _AutenticarResponse_QNAME = new QName("http://monster.edu.ec/services", "autenticarResponse");
    private static final QName _ConsultarCatalogo_QNAME = new QName("http://monster.edu.ec/services", "consultarCatalogo");
    private static final QName _ConsultarCatalogoResponse_QNAME = new QName("http://monster.edu.ec/services", "consultarCatalogoResponse");
    private static final QName _ConsultarUnidadesPorCategoria_QNAME = new QName("http://monster.edu.ec/services", "consultarUnidadesPorCategoria");
    private static final QName _ConsultarUnidadesPorCategoriaResponse_QNAME = new QName("http://monster.edu.ec/services", "consultarUnidadesPorCategoriaResponse");
    private static final QName _Convertir_QNAME = new QName("http://monster.edu.ec/services", "convertir");
    private static final QName _ConvertirLongitud_QNAME = new QName("http://monster.edu.ec/services", "convertirLongitud");
    private static final QName _ConvertirLongitudResponse_QNAME = new QName("http://monster.edu.ec/services", "convertirLongitudResponse");
    private static final QName _ConvertirMasa_QNAME = new QName("http://monster.edu.ec/services", "convertirMasa");
    private static final QName _ConvertirMasaResponse_QNAME = new QName("http://monster.edu.ec/services", "convertirMasaResponse");
    private static final QName _ConvertirResponse_QNAME = new QName("http://monster.edu.ec/services", "convertirResponse");
    private static final QName _ConvertirTemperatura_QNAME = new QName("http://monster.edu.ec/services", "convertirTemperatura");
    private static final QName _ConvertirTemperaturaResponse_QNAME = new QName("http://monster.edu.ec/services", "convertirTemperaturaResponse");
    private static final QName _ConsultarHistorial_QNAME = new QName("http://monster.edu.ec/services", "consultarHistorial");
    private static final QName _ConsultarHistorialResponse_QNAME = new QName("http://monster.edu.ec/services", "consultarHistorialResponse");

    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: ec.edu.monster.clientesws
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link CategoriaDTO }
     * 
     * @return
     *     the new instance of {@link CategoriaDTO }
     */
    public CategoriaDTO createCategoriaDTO() {
        return new CategoriaDTO();
    }

    /**
     * Create an instance of {@link RespuestaAutenticacion }
     * 
     * @return
     *     the new instance of {@link RespuestaAutenticacion }
     */
    public RespuestaAutenticacion createRespuestaAutenticacion() {
        return new RespuestaAutenticacion();
    }

    /**
     * Create an instance of {@link RespuestaCatalogo }
     * 
     * @return
     *     the new instance of {@link RespuestaCatalogo }
     */
    public RespuestaCatalogo createRespuestaCatalogo() {
        return new RespuestaCatalogo();
    }

    /**
     * Create an instance of {@link RespuestaConversion }
     * 
     * @return
     *     the new instance of {@link RespuestaConversion }
     */
    public RespuestaConversion createRespuestaConversion() {
        return new RespuestaConversion();
    }

    /**
     * Create an instance of {@link Autenticar }
     * 
     * @return
     *     the new instance of {@link Autenticar }
     */
    public Autenticar createAutenticar() {
        return new Autenticar();
    }

    /**
     * Create an instance of {@link AutenticarResponse }
     * 
     * @return
     *     the new instance of {@link AutenticarResponse }
     */
    public AutenticarResponse createAutenticarResponse() {
        return new AutenticarResponse();
    }

    /**
     * Create an instance of {@link ConsultarCatalogo }
     * 
     * @return
     *     the new instance of {@link ConsultarCatalogo }
     */
    public ConsultarCatalogo createConsultarCatalogo() {
        return new ConsultarCatalogo();
    }

    /**
     * Create an instance of {@link ConsultarCatalogoResponse }
     * 
     * @return
     *     the new instance of {@link ConsultarCatalogoResponse }
     */
    public ConsultarCatalogoResponse createConsultarCatalogoResponse() {
        return new ConsultarCatalogoResponse();
    }

    /**
     * Create an instance of {@link ConsultarUnidadesPorCategoria }
     * 
     * @return
     *     the new instance of {@link ConsultarUnidadesPorCategoria }
     */
    public ConsultarUnidadesPorCategoria createConsultarUnidadesPorCategoria() {
        return new ConsultarUnidadesPorCategoria();
    }

    /**
     * Create an instance of {@link ConsultarUnidadesPorCategoriaResponse }
     * 
     * @return
     *     the new instance of {@link ConsultarUnidadesPorCategoriaResponse }
     */
    public ConsultarUnidadesPorCategoriaResponse createConsultarUnidadesPorCategoriaResponse() {
        return new ConsultarUnidadesPorCategoriaResponse();
    }

    /**
     * Create an instance of {@link Convertir }
     * 
     * @return
     *     the new instance of {@link Convertir }
     */
    public Convertir createConvertir() {
        return new Convertir();
    }

    /**
     * Create an instance of {@link ConvertirLongitud }
     * 
     * @return
     *     the new instance of {@link ConvertirLongitud }
     */
    public ConvertirLongitud createConvertirLongitud() {
        return new ConvertirLongitud();
    }

    /**
     * Create an instance of {@link ConvertirLongitudResponse }
     * 
     * @return
     *     the new instance of {@link ConvertirLongitudResponse }
     */
    public ConvertirLongitudResponse createConvertirLongitudResponse() {
        return new ConvertirLongitudResponse();
    }

    /**
     * Create an instance of {@link ConvertirMasa }
     * 
     * @return
     *     the new instance of {@link ConvertirMasa }
     */
    public ConvertirMasa createConvertirMasa() {
        return new ConvertirMasa();
    }

    /**
     * Create an instance of {@link ConvertirMasaResponse }
     * 
     * @return
     *     the new instance of {@link ConvertirMasaResponse }
     */
    public ConvertirMasaResponse createConvertirMasaResponse() {
        return new ConvertirMasaResponse();
    }

    /**
     * Create an instance of {@link ConvertirResponse }
     * 
     * @return
     *     the new instance of {@link ConvertirResponse }
     */
    public ConvertirResponse createConvertirResponse() {
        return new ConvertirResponse();
    }

    /**
     * Create an instance of {@link ConvertirTemperatura }
     * 
     * @return
     *     the new instance of {@link ConvertirTemperatura }
     */
    public ConvertirTemperatura createConvertirTemperatura() {
        return new ConvertirTemperatura();
    }

    /**
     * Create an instance of {@link ConvertirTemperaturaResponse }
     * 
     * @return
     *     the new instance of {@link ConvertirTemperaturaResponse }
     */
    public ConvertirTemperaturaResponse createConvertirTemperaturaResponse() {
        return new ConvertirTemperaturaResponse();
    }

    /**
     * Create an instance of {@link CategoriaDTO.Unidades }
     * 
     * @return
     *     the new instance of {@link CategoriaDTO.Unidades }
     */
    public CategoriaDTO.Unidades createCategoriaDTOUnidades() {
        return new CategoriaDTO.Unidades();
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link CategoriaDTO }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link CategoriaDTO }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "CategoriaDTO")
    public JAXBElement<CategoriaDTO> createCategoriaDTO(CategoriaDTO value) {
        return new JAXBElement<>(_CategoriaDTO_QNAME, CategoriaDTO.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link RespuestaAutenticacion }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link RespuestaAutenticacion }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "RespuestaAutenticacion")
    public JAXBElement<RespuestaAutenticacion> createRespuestaAutenticacion(RespuestaAutenticacion value) {
        return new JAXBElement<>(_RespuestaAutenticacion_QNAME, RespuestaAutenticacion.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link RespuestaCatalogo }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link RespuestaCatalogo }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "RespuestaCatalogo")
    public JAXBElement<RespuestaCatalogo> createRespuestaCatalogo(RespuestaCatalogo value) {
        return new JAXBElement<>(_RespuestaCatalogo_QNAME, RespuestaCatalogo.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link RespuestaConversion }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link RespuestaConversion }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "RespuestaConversion")
    public JAXBElement<RespuestaConversion> createRespuestaConversion(RespuestaConversion value) {
        return new JAXBElement<>(_RespuestaConversion_QNAME, RespuestaConversion.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Autenticar }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Autenticar }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "autenticar")
    public JAXBElement<Autenticar> createAutenticar(Autenticar value) {
        return new JAXBElement<>(_Autenticar_QNAME, Autenticar.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link AutenticarResponse }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link AutenticarResponse }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "autenticarResponse")
    public JAXBElement<AutenticarResponse> createAutenticarResponse(AutenticarResponse value) {
        return new JAXBElement<>(_AutenticarResponse_QNAME, AutenticarResponse.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ConsultarCatalogo }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ConsultarCatalogo }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "consultarCatalogo")
    public JAXBElement<ConsultarCatalogo> createConsultarCatalogo(ConsultarCatalogo value) {
        return new JAXBElement<>(_ConsultarCatalogo_QNAME, ConsultarCatalogo.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ConsultarCatalogoResponse }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ConsultarCatalogoResponse }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "consultarCatalogoResponse")
    public JAXBElement<ConsultarCatalogoResponse> createConsultarCatalogoResponse(ConsultarCatalogoResponse value) {
        return new JAXBElement<>(_ConsultarCatalogoResponse_QNAME, ConsultarCatalogoResponse.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ConsultarUnidadesPorCategoria }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ConsultarUnidadesPorCategoria }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "consultarUnidadesPorCategoria")
    public JAXBElement<ConsultarUnidadesPorCategoria> createConsultarUnidadesPorCategoria(ConsultarUnidadesPorCategoria value) {
        return new JAXBElement<>(_ConsultarUnidadesPorCategoria_QNAME, ConsultarUnidadesPorCategoria.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ConsultarUnidadesPorCategoriaResponse }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ConsultarUnidadesPorCategoriaResponse }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "consultarUnidadesPorCategoriaResponse")
    public JAXBElement<ConsultarUnidadesPorCategoriaResponse> createConsultarUnidadesPorCategoriaResponse(ConsultarUnidadesPorCategoriaResponse value) {
        return new JAXBElement<>(_ConsultarUnidadesPorCategoriaResponse_QNAME, ConsultarUnidadesPorCategoriaResponse.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Convertir }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Convertir }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "convertir")
    public JAXBElement<Convertir> createConvertir(Convertir value) {
        return new JAXBElement<>(_Convertir_QNAME, Convertir.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ConvertirLongitud }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ConvertirLongitud }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "convertirLongitud")
    public JAXBElement<ConvertirLongitud> createConvertirLongitud(ConvertirLongitud value) {
        return new JAXBElement<>(_ConvertirLongitud_QNAME, ConvertirLongitud.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ConvertirLongitudResponse }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ConvertirLongitudResponse }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "convertirLongitudResponse")
    public JAXBElement<ConvertirLongitudResponse> createConvertirLongitudResponse(ConvertirLongitudResponse value) {
        return new JAXBElement<>(_ConvertirLongitudResponse_QNAME, ConvertirLongitudResponse.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ConvertirMasa }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ConvertirMasa }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "convertirMasa")
    public JAXBElement<ConvertirMasa> createConvertirMasa(ConvertirMasa value) {
        return new JAXBElement<>(_ConvertirMasa_QNAME, ConvertirMasa.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ConvertirMasaResponse }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ConvertirMasaResponse }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "convertirMasaResponse")
    public JAXBElement<ConvertirMasaResponse> createConvertirMasaResponse(ConvertirMasaResponse value) {
        return new JAXBElement<>(_ConvertirMasaResponse_QNAME, ConvertirMasaResponse.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ConvertirResponse }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ConvertirResponse }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "convertirResponse")
    public JAXBElement<ConvertirResponse> createConvertirResponse(ConvertirResponse value) {
        return new JAXBElement<>(_ConvertirResponse_QNAME, ConvertirResponse.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ConvertirTemperatura }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ConvertirTemperatura }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "convertirTemperatura")
    public JAXBElement<ConvertirTemperatura> createConvertirTemperatura(ConvertirTemperatura value) {
        return new JAXBElement<>(_ConvertirTemperatura_QNAME, ConvertirTemperatura.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ConvertirTemperaturaResponse }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ConvertirTemperaturaResponse }{@code >}
     */
    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "convertirTemperaturaResponse")
    public JAXBElement<ConvertirTemperaturaResponse> createConvertirTemperaturaResponse(ConvertirTemperaturaResponse value) {
        return new JAXBElement<>(_ConvertirTemperaturaResponse_QNAME, ConvertirTemperaturaResponse.class, null, value);
    }

    public RegistroHistorialDTO createRegistroHistorialDTO() {
        return new RegistroHistorialDTO();
    }

    public RespuestaHistorial createRespuestaHistorial() {
        return new RespuestaHistorial();
    }

    public ConsultarHistorial createConsultarHistorial() {
        return new ConsultarHistorial();
    }

    public ConsultarHistorialResponse createConsultarHistorialResponse() {
        return new ConsultarHistorialResponse();
    }

    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "consultarHistorial")
    public JAXBElement<ConsultarHistorial> createConsultarHistorial(ConsultarHistorial value) {
        return new JAXBElement<>(_ConsultarHistorial_QNAME, ConsultarHistorial.class, null, value);
    }

    @XmlElementDecl(namespace = "http://monster.edu.ec/services", name = "consultarHistorialResponse")
    public JAXBElement<ConsultarHistorialResponse> createConsultarHistorialResponse(ConsultarHistorialResponse value) {
        return new JAXBElement<>(_ConsultarHistorialResponse_QNAME, ConsultarHistorialResponse.class, null, value);
    }

}
