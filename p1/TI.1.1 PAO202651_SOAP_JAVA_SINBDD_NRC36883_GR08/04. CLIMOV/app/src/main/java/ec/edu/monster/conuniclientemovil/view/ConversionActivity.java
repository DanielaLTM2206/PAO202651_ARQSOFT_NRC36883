package ec.edu.monster.conuniclientemovil.view;

import android.content.Intent;
import android.os.Bundle;
import android.text.TextUtils;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;

import ec.edu.monster.conuniclientemovil.R;
import ec.edu.monster.conuniclientemovil.controller.ConversionController;
import ec.edu.monster.conuniclientemovil.model.AppPreferences;
import ec.edu.monster.conuniclientemovil.model.ConversionRequest;
import ec.edu.monster.conuniclientemovil.model.ConversionResult;
import ec.edu.monster.conuniclientemovil.model.ServiceType;
import ec.edu.monster.conuniclientemovil.model.SoapClient;

import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

/**
 * Pantalla de conversión de unidades con Sullivan de Monsters Inc y Respuesta Estructurada (RF-06).
 *
 * @author GR08 - Arquitectura de Software
 */
public class ConversionActivity extends AppCompatActivity {

    /* ── Widgets ────────────────────────────────────────── */
    private EditText etValor;
    private EditText etDestinoPreview;

    private Spinner spServicio;
    private Spinner spUnidadOrigen;
    private Spinner spUnidadDestino;

    private Button btnConvertir;
    private Button btnLogout;

    private TextView tvEstado;
    private TextView tvCodigo;
    private TextView tvMensaje;

    /* ── Controller & Model ─────────────────────────────── */
    private ConversionController conversionController;
    private AppPreferences appPreferences;
    private SoapClient soapClient;

    private String currentToken;
    private String baseUrl;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_conversion);

        extractIntentData();
        initDependencies();
        bindViews();
        setupServiceSpinner();
        setInitialValues();
        setupActions();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        conversionController.shutdown();
        soapClient.shutdown();
    }

    private void extractIntentData() {
        Intent intent = getIntent();
        currentToken = intent.getStringExtra(LoginActivity.EXTRA_TOKEN);
        baseUrl = intent.getStringExtra(LoginActivity.EXTRA_BASE_URL);
    }

    private void initDependencies() {
        appPreferences = new AppPreferences(this);
        soapClient = new SoapClient();
        conversionController = new ConversionController(soapClient);
    }

    private void bindViews() {
        etValor = findViewById(R.id.etValor);
        etDestinoPreview = findViewById(R.id.etDestinoPreview);

        spServicio = findViewById(R.id.spServicio);
        spUnidadOrigen = findViewById(R.id.spUnidadOrigen);
        spUnidadDestino = findViewById(R.id.spUnidadDestino);

        btnConvertir = findViewById(R.id.btnConvertir);
        btnLogout = findViewById(R.id.btnLogout);

        tvEstado = findViewById(R.id.tvEstado);
        tvCodigo = findViewById(R.id.tvCodigo);
        tvMensaje = findViewById(R.id.tvMensaje);
    }

    private void setupServiceSpinner() {
        List<String> serviceNames = new ArrayList<>();
        for (ServiceType type : ServiceType.values()) {
            serviceNames.add(type.getDisplayName());
        }

        ArrayAdapter<String> serviceAdapter = new ArrayAdapter<>(
                this,
                R.layout.item_spinner_value,
                serviceNames
        );
        serviceAdapter.setDropDownViewResource(R.layout.item_spinner_dropdown_value);
        spServicio.setAdapter(serviceAdapter);

        spServicio.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, android.view.View view,
                                       int position, long id) {
                ServiceType selected = ServiceType.values()[position];
                updateUnitsForService(selected);
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                updateUnitsForService(ServiceType.LONGITUD);
            }
        });

        updateUnitsForService(ServiceType.LONGITUD);
    }

    private void updateUnitsForService(ServiceType serviceType) {
        List<String> units = serviceType.getUnits();

        ArrayAdapter<String> originAdapter = new ArrayAdapter<>(
                this, R.layout.item_spinner_value, units);
        originAdapter.setDropDownViewResource(R.layout.item_spinner_dropdown_value);
        spUnidadOrigen.setAdapter(originAdapter);

        ArrayAdapter<String> destinationAdapter = new ArrayAdapter<>(
                this, R.layout.item_spinner_value, units);
        destinationAdapter.setDropDownViewResource(R.layout.item_spinner_dropdown_value);
        spUnidadDestino.setAdapter(destinationAdapter);

        if (units.size() > 1) {
            spUnidadDestino.setSelection(1);
        }
    }

    private void setInitialValues() {
        tvEstado.setText("ESTADO: ESPERANDO");
        tvEstado.setTextColor(ContextCompat.getColor(this, android.R.color.darker_gray));
        tvCodigo.setText("CÓDIGO: ---");
        tvMensaje.setText("Seleccione unidades y presione Convertir.");
    }

    private void setupActions() {
        btnConvertir.setOnClickListener(v -> doConvert());
        btnLogout.setOnClickListener(v -> doLogout());
    }

    private void doConvert() {
        String valueText = etValor.getText().toString().trim();
        String selectedServiceName = (String) spServicio.getSelectedItem();
        String origin = (String) spUnidadOrigen.getSelectedItem();
        String destination = (String) spUnidadDestino.getSelectedItem();

        if (TextUtils.isEmpty(valueText)
                || TextUtils.isEmpty(selectedServiceName)
                || TextUtils.isEmpty(origin)
                || TextUtils.isEmpty(destination)) {
            showToast("Complete todos los campos de conversión.");
            return;
        }

        if (origin.equals(destination)) {
            showToast("Seleccione unidades diferentes para convertir.");
            return;
        }

        if (TextUtils.isEmpty(currentToken)) {
            showToast("Token inválido o expirado. Inicie sesión nuevamente.");
            doLogout();
            return;
        }

        final double inputValue;
        try {
            inputValue = Double.parseDouble(valueText.replace(',', '.'));
        } catch (NumberFormatException ex) {
            showToast("El valor numérico no es válido.");
            return;
        }

        ServiceType serviceType = ServiceType.fromDisplayName(selectedServiceName);
        ConversionRequest request = new ConversionRequest(
                serviceType, inputValue, origin, destination, currentToken);

        setBusy(true);
        tvEstado.setText("ESTADO: PROCESANDO");
        tvEstado.setTextColor(ContextCompat.getColor(this, android.R.color.holo_blue_light));
        tvCodigo.setText("CÓDIGO: ---");
        tvMensaje.setText("Consultando Web Service SOAP GR08...");

        conversionController.convert(request, baseUrl, result ->
                runOnUiThread(() -> handleConversionResult(result))
        );
    }

    private void handleConversionResult(ConversionResult result) {
        setBusy(false);

        if (result.isSuccess()) {
            if (result.getResultado() != null) {
                etDestinoPreview.setText(String.format(Locale.US, "%.4f", result.getResultado()));
            } else {
                etDestinoPreview.setText("OK");
            }
            tvEstado.setText("ESTADO: OK");
            tvEstado.setTextColor(ContextCompat.getColor(this, android.R.color.holo_green_light));
            tvCodigo.setText("CÓDIGO: " + (result.getCodigo() != null ? result.getCodigo() : "200"));
            tvMensaje.setText(result.getMensaje() != null ? result.getMensaje() : "Conversión realizada con éxito.");
            showToast("Conversión exitosa.");
        } else {
            etDestinoPreview.setText("Error");
            tvEstado.setText("ESTADO: ERROR");
            tvEstado.setTextColor(ContextCompat.getColor(this, android.R.color.holo_red_light));
            tvCodigo.setText("CÓDIGO: " + (result.getCodigo() != null ? result.getCodigo() : "ERR"));
            tvMensaje.setText(result.getMensaje() != null ? result.getMensaje() : "Error en conversión.");

            if (result.isTokenError()) {
                showToast("Sesión expirada. Inicie sesión nuevamente.");
                doLogout();
                return;
            }
            showToast("Error en conversión: " + result.getMensaje());
        }
    }

    private void doLogout() {
        currentToken = null;
        appPreferences.clearToken();
        soapClient.clearSessionToken();

        Intent intent = new Intent(this, LoginActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        startActivity(intent);
        finish();
    }

    private void showToast(String message) {
        Toast.makeText(this, message, Toast.LENGTH_SHORT).show();
    }

    private void setBusy(boolean busy) {
        etValor.setEnabled(!busy);
        spServicio.setEnabled(!busy);
        spUnidadOrigen.setEnabled(!busy);
        spUnidadDestino.setEnabled(!busy);
        btnConvertir.setEnabled(!busy);
        btnLogout.setEnabled(!busy);
    }
}
