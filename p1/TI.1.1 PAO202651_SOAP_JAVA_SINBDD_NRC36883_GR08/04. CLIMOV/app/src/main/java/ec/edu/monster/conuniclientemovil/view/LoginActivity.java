package ec.edu.monster.conuniclientemovil.view;

import android.content.Intent;
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

    private static final String DEFAULT_BASE_URL = "http://10.0.2.2:8080/WS_ConUni_Java_SOAP/";

    /* ── Widgets ─────────────────────────────────── */
    private EditText etUsuario;
    private EditText etClave;
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

        String baseUrl = appPreferences.getBaseUrl(DEFAULT_BASE_URL);

        setBusy(true);

        loginController.login(baseUrl, usuario, clave, result ->
                runOnUiThread(() -> handleLoginResult(result, baseUrl))
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
}
