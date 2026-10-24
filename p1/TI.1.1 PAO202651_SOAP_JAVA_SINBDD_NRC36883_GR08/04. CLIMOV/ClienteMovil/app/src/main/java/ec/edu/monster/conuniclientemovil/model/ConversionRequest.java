package ec.edu.monster.conuniclientemovil.model;

/**
 * Modelo que encapsula los datos necesarios para una solicitud de conversión.
 */
public class ConversionRequest {

    private final ServiceType serviceType;
    private final double value;
    private final String originUnit;
    private final String destinationUnit;
    private final String token;

    public ConversionRequest(ServiceType serviceType, double value,
                             String originUnit, String destinationUnit, String token) {
        this.serviceType = serviceType;
        this.value = value;
        this.originUnit = originUnit;
        this.destinationUnit = destinationUnit;
        this.token = token;
    }

    public ServiceType getServiceType() {
        return serviceType;
    }

    public double getValue() {
        return value;
    }

    public String getOriginUnit() {
        return originUnit;
    }

    public String getDestinationUnit() {
        return destinationUnit;
    }

    public String getToken() {
        return token;
    }
}
