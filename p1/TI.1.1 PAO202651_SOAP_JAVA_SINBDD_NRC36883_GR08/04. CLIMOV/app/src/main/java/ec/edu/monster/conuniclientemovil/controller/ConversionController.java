package ec.edu.monster.conuniclientemovil.controller;

import ec.edu.monster.conuniclientemovil.model.ConversionRequest;
import ec.edu.monster.conuniclientemovil.model.ConversionResult;
import ec.edu.monster.conuniclientemovil.model.SoapClient;

import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class ConversionController {

    public interface ConversionCallback {
        void onResult(ConversionResult result);
    }

    private final SoapClient soapClient;
    private final ExecutorService executorService;

    public ConversionController(SoapClient soapClient) {
        this(soapClient, Executors.newSingleThreadExecutor());
    }

    public ConversionController(SoapClient soapClient, ExecutorService executorService) {
        this.soapClient = soapClient;
        this.executorService = executorService;
    }

    public void convert(ConversionRequest request, String baseUrl, ConversionCallback callback) {
        executorService.execute(() -> {
            try {
                ConversionResult result = soapClient.convertir(
                        baseUrl,
                        request.getServiceType(),
                        request.getValue(),
                        request.getOriginUnit(),
                        request.getDestinationUnit(),
                        request.getToken()
                ).get();

                callback.onResult(result);
            } catch (Exception exception) {
                callback.onResult(ConversionResult.error("SYS-001", extractMessage(exception)));
            }
        });
    }

    public void shutdown() {
        executorService.shutdown();
    }

    private String extractMessage(Exception exception) {
        Throwable cause = exception;
        if (exception instanceof ExecutionException && exception.getCause() != null) {
            cause = exception.getCause();
        }

        String message = cause.getMessage();
        if (message == null || message.trim().isEmpty()) {
            return "Error de comunicación SOAP";
        }

        return message;
    }
}
