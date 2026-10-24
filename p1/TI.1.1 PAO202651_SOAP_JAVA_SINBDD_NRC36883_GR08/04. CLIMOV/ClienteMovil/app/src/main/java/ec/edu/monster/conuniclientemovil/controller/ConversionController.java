package ec.edu.monster.conuniclientemovil.controller;

import ec.edu.monster.conuniclientemovil.model.ConversionRequest;
import ec.edu.monster.conuniclientemovil.model.ConversionResult;
import ec.edu.monster.conuniclientemovil.model.RegistroHistorial;
import ec.edu.monster.conuniclientemovil.model.SoapClient;

import java.util.Collections;
import java.util.List;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class ConversionController {

    public interface ConversionCallback {
        void onResult(ConversionResult result);
    }

    public interface HistorialCallback {
        void onHistorial(List<RegistroHistorial> items, String error);
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

    public void cargarHistorial(String baseUrl, String token, HistorialCallback callback) {
        executorService.execute(() -> {
            try {
                List<RegistroHistorial> items = soapClient.consultarHistorial(baseUrl, token).get();
                callback.onHistorial(items != null ? items : Collections.emptyList(), null);
            } catch (Throwable exception) {
                android.util.Log.e("ConUniHistorial", "Error consultando historial", exception);
                Throwable cause = exception;
                if (exception instanceof ExecutionException && exception.getCause() != null) {
                    cause = exception.getCause();
                }
                callback.onHistorial(Collections.emptyList(),
                        cause.getClass().getSimpleName() + ": " + cause.getMessage());
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
