package ec.edu.monster.conuniclientemovil.view;

import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.text.TextUtils;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import ec.edu.monster.conuniclientemovil.R;
import ec.edu.monster.conuniclientemovil.controller.LoginController;
import ec.edu.monster.conuniclientemovil.model.AppPreferences;
import ec.edu.monster.conuniclientemovil.model.LoginResult;
import ec.edu.monster.conuniclientemovil.model.SoapClient;

/**
 * Pantalla de login — View del patrón MVC.
 *
 * Muestra el formulario de credenciales y la imagen del monstruo.
 * Delega la autenticación al LoginController.
 * Al obtener un token exitoso, navega a ConversionActivity.
 */
public class LoginActivity extends AppCompatActivity {

    public static final String EXTRA_TOKEN = "extra_token";
    public static final String EXTRA_BASE_URL = "extra_base_url";

    private static final String DEFAULT_BASE_URL_EMU = "http://10.0.2.2:8080/WS_ConUni_Java_SOAP/";
    private static final String DEFAULT_BASE_URL_REAL = "http://127.0.0.1:8080/WS_ConUni_Java_SOAP/";

    /* ── Widgets ─────────────────────────────────── */
    private EditText etUsuario;
    private EditText etClave;
    private EditText etServidorIp;
    private Button btnLogin;

    /* ── Controller & Model ──────────────────────── */
    private LoginController loginController;
    private AppPreferences appPreferences;
    private SoapClient soapClient;

    /* ══════════════════════════════════════════════════
       Ciclo de vida
       ══════════════════════════════════════════════════ */

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        initDependencies();
        bindViews();
        setupActions();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        loginController.shutdown();
        soapClient.shutdown();
    }

    /* ══════════════════════════════════════════════════
       Inicialización
       ══════════════════════════════════════════════════ */

    private void initDependencies() {
        appPreferences = new AppPreferences(this);
        soapClient = new SoapClient();
        loginController = new LoginController(soapClient);
    }

    private void bindViews() {
        etUsuario = findViewById(R.id.etUsuario);
        etClave = findViewById(R.id.etClave);
        etServidorIp = findViewById(R.id.etServidorIp);
        String saved = appPreferences.getBaseUrl("");
        if (!TextUtils.isEmpty(saved)) {
            String host = extractHost(saved);
            etServidorIp.setText(host);
        }
        btnLogin = findViewById(R.id.btnLogin);
    }

    private void setupActions() {
        btnLogin.setOnClickListener(v -> doLogin());
    }

    /* ══════════════════════════════════════════════════
       Acción de login → delegada al Controller
       ══════════════════════════════════════════════════ */

    private void doLogin() {
        String usuario = etUsuario.getText().toString().trim();
        String clave = etClave.getText().toString();

        if (TextUtils.isEmpty(usuario) || TextUtils.isEmpty(clave)) {
            Toast.makeText(this, "Usuario y clave son obligatorios.", Toast.LENGTH_SHORT).show();
            return;
        }

        String customHost = etServidorIp.getText().toString().trim();
        String baseUrl;
        if (!TextUtils.isEmpty(customHost)) {
            if (customHost.startsWith("http://") || customHost.startsWith("https://")) {
                baseUrl = customHost.endsWith("/") ? customHost : customHost + "/";
                if (!baseUrl.contains("WS_ConUni_Java_SOAP")) {
                    baseUrl += "WS_ConUni_Java_SOAP/";
                }
            } else if (customHost.contains(":")) {
                baseUrl = "http://" + customHost + "/WS_ConUni_Java_SOAP/";
            } else {
                baseUrl = "http://" + customHost + ":8080/WS_ConUni_Java_SOAP/";
            }
        } else {
            boolean isEmulator = Build.FINGERPRINT.startsWith("generic")
                    || Build.MODEL.contains("google_sdk")
                    || Build.MODEL.contains("Emulator")
                    || Build.HARDWARE.contains("goldfish")
                    || Build.HARDWARE.contains("ranchu");
            baseUrl = isEmulator ? DEFAULT_BASE_URL_EMU : DEFAULT_BASE_URL_REAL;
        }

        setBusy(true);
        final String effectiveBaseUrl = baseUrl;

        loginController.login(effectiveBaseUrl, usuario, clave, result ->
                runOnUiThread(() -> {
                    String actualUrl = soapClient.getResolvedBaseUrl() != null
                            ? soapClient.getResolvedBaseUrl()
                            : effectiveBaseUrl;
                    appPreferences.saveBaseUrl(actualUrl);
                    handleLoginResult(result, actualUrl);
                })
        );
    }

    /* ══════════════════════════════════════════════════
       Renderizado de resultado
       ══════════════════════════════════════════════════ */

    private void handleLoginResult(LoginResult result, String baseUrl) {
        setBusy(false);

        if (result.isSuccess()) {
            appPreferences.saveToken(result.getToken());
            navigateToConversion(result.getToken(), baseUrl);
        } else {
            appPreferences.clearToken();
            Toast.makeText(this,
                    "Error de login: " + result.getErrorMessage(),
                    Toast.LENGTH_LONG).show();
        }
    }

    /* ══════════════════════════════════════════════════
       Navegación
       ══════════════════════════════════════════════════ */

    private void navigateToConversion(String token, String baseUrl) {
        Intent intent = new Intent(this, ConversionActivity.class);
        intent.putExtra(EXTRA_TOKEN, token);
        intent.putExtra(EXTRA_BASE_URL, baseUrl);
        startActivity(intent);
        finish();
    }

    /* ══════════════════════════════════════════════════
       Helpers de UI
       ══════════════════════════════════════════════════ */

    private void setBusy(boolean busy) {
        etUsuario.setEnabled(!busy);
        etClave.setEnabled(!busy);
        btnLogin.setEnabled(!busy);
    }
    private String extractHost(String url) {
        if (url == null) return "";
        String s = url.replace("http://", "").replace("https://", "");
        int colon = s.indexOf(':');
        if (colon != -1) return s.substring(0, colon);
        int slash = s.indexOf('/');
        if (slash != -1) return s.substring(0, slash);
        return s;
    }

}
