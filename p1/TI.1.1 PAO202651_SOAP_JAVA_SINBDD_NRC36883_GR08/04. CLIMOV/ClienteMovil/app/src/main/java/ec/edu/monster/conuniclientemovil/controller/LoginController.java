package ec.edu.monster.conuniclientemovil.controller;

import ec.edu.monster.conuniclientemovil.model.LoginResult;
import ec.edu.monster.conuniclientemovil.model.SoapClient;

import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class LoginController {

    public interface LoginCallback {
        void onResult(LoginResult result);
    }

    private final SoapClient soapClient;
    private final ExecutorService executorService;

    public LoginController(SoapClient soapClient) {
        this(soapClient, Executors.newSingleThreadExecutor());
    }

    public LoginController(SoapClient soapClient, ExecutorService executorService) {
        this.soapClient = soapClient;
        this.executorService = executorService;
    }

    public void login(String baseUrl, String usuario, String clave, LoginCallback callback) {
        executorService.execute(() -> {
            try {
                LoginResult result = soapClient.login(baseUrl, usuario, clave).get();
                callback.onResult(result);
            } catch (Exception exception) {
                callback.onResult(LoginResult.error(extractMessage(exception)));
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
            return "Error de conexión al servidor";
        }

        return message;
    }
}
