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
    private Button btnCambiarClave;
    private Button btnRefrescarHistorial;

    private TextView tvEstado;
    private TextView tvCodigo;
    private TextView tvMensaje;
    private TextView tvHistorialVacio;
    private android.widget.LinearLayout llHistorialItems;

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
        cargarHistorial();
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
        btnCambiarClave = findViewById(R.id.btnCambiarClave);
        btnRefrescarHistorial = findViewById(R.id.btnRefrescarHistorial);

        tvEstado = findViewById(R.id.tvEstado);
        tvCodigo = findViewById(R.id.tvCodigo);
        tvMensaje = findViewById(R.id.tvMensaje);
        tvHistorialVacio = findViewById(R.id.tvHistorialVacio);
        llHistorialItems = findViewById(R.id.llHistorialItems);
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
        if (btnCambiarClave != null) {
            btnCambiarClave.setOnClickListener(v -> showChangePasswordDialog());
        }
        btnRefrescarHistorial.setOnClickListener(v -> cargarHistorial());
    }

    private void showChangePasswordDialog() {
        android.widget.LinearLayout layout = new android.widget.LinearLayout(this);
        layout.setOrientation(android.widget.LinearLayout.VERTICAL);
        layout.setPadding(50, 40, 50, 10);

        final android.widget.EditText etActual = new android.widget.EditText(this);
        etActual.setHint("Contraseña actual");
        etActual.setInputType(android.text.InputType.TYPE_CLASS_TEXT | android.text.InputType.TYPE_TEXT_VARIATION_PASSWORD);
        layout.addView(etActual);

        final android.widget.EditText etNueva = new android.widget.EditText(this);
        etNueva.setHint("Nueva contraseña");
        etNueva.setInputType(android.text.InputType.TYPE_CLASS_TEXT | android.text.InputType.TYPE_TEXT_VARIATION_PASSWORD);
        layout.addView(etNueva);

        final android.widget.EditText etConfirmar = new android.widget.EditText(this);
        etConfirmar.setHint("Confirmar nueva contraseña");
        etConfirmar.setInputType(android.text.InputType.TYPE_CLASS_TEXT | android.text.InputType.TYPE_TEXT_VARIATION_PASSWORD);
        layout.addView(etConfirmar);

        new androidx.appcompat.app.AlertDialog.Builder(this)
                .setTitle("🔑 Cambiar Contraseña")
                .setMessage("Se sincronizará en todos los clientes (Web, Móvil, Escritorio, Consola).")
                .setView(layout)
                .setPositiveButton("Actualizar", (dialog, which) -> {
                    String actual = etActual.getText().toString().trim();
                    String nueva = etNueva.getText().toString().trim();
                    String confirmar = etConfirmar.getText().toString().trim();

                    if (actual.isEmpty() || nueva.isEmpty()) {
                        showToast("Debe ingresar la contraseña actual y la nueva.");
                        return;
                    }
                    if (!nueva.equals(confirmar)) {
                        showToast("La confirmación no coincide.");
                        return;
                    }

                    setBusy(true);
                    conversionController.cambiarContrasenia(baseUrl, actual, nueva, currentToken, res -> runOnUiThread(() -> {
                        setBusy(false);
                        if (res.isSuccess()) {
                            new androidx.appcompat.app.AlertDialog.Builder(this)
                                    .setTitle("Éxito")
                                    .setMessage(res.getErrorMessage() != null ? res.getErrorMessage() : "Contraseña actualizada correctamente en memoria.")
                                    .setPositiveButton("OK", null)
                                    .show();
                        } else {
                            new androidx.appcompat.app.AlertDialog.Builder(this)
                                    .setTitle("Error [" + res.getCodigo() + "]")
                                    .setMessage(res.getErrorMessage())
                                    .setPositiveButton("OK", null)
                                    .show();
                        }
                    }));
                })
                .setNegativeButton("Cancelar", null)
                .show();
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
            
            // Actualizar historial central inmediatamente
            cargarHistorial();
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

    private void cargarHistorial() {
        if (TextUtils.isEmpty(currentToken)) return;
        tvHistorialVacio.setVisibility(android.view.View.VISIBLE);
        tvHistorialVacio.setText("Sincronizando historial central...");

        conversionController.cargarHistorial(baseUrl, currentToken, (items, error) -> runOnUiThread(() -> {
            if (llHistorialItems == null) return;
            llHistorialItems.removeAllViews();
            if (error != null) {
                tvHistorialVacio.setVisibility(android.view.View.VISIBLE);
                tvHistorialVacio.setText("No se pudo cargar el historial: " + error);
                return;
            }
            if (items == null || items.isEmpty()) {
                tvHistorialVacio.setVisibility(android.view.View.VISIBLE);
                tvHistorialVacio.setText("Sin conversiones registradas aún.");
                return;
            }

            tvHistorialVacio.setVisibility(android.view.View.GONE);
            for (ec.edu.monster.conuniclientemovil.model.RegistroHistorial item : items) {
                android.widget.LinearLayout card = new android.widget.LinearLayout(ConversionActivity.this);
                card.setOrientation(android.widget.LinearLayout.VERTICAL);
                card.setBackgroundColor(android.graphics.Color.parseColor("#182234"));
                card.setPadding(24, 18, 24, 18);
                android.widget.LinearLayout.LayoutParams cardParams = new android.widget.LinearLayout.LayoutParams(
                        android.widget.LinearLayout.LayoutParams.MATCH_PARENT,
                        android.widget.LinearLayout.LayoutParams.WRAP_CONTENT);
                cardParams.setMargins(0, 0, 0, 14);
                card.setLayoutParams(cardParams);

                android.widget.LinearLayout topRow = new android.widget.LinearLayout(ConversionActivity.this);
                topRow.setOrientation(android.widget.LinearLayout.HORIZONTAL);
                topRow.setGravity(android.view.Gravity.CENTER_VERTICAL);

                // # ID
                TextView tvId = new TextView(ConversionActivity.this);
                tvId.setText("#" + item.getId());
                tvId.setTextColor(android.graphics.Color.parseColor("#94A3B8"));
                tvId.setTextSize(11f);
                tvId.setTypeface(null, android.graphics.Typeface.BOLD);
                tvId.setPadding(0, 0, 14, 0);

                // Badge de Categoría
                TextView tvCat = new TextView(ConversionActivity.this);
                String cat = item.getCategoria() != null ? item.getCategoria().toUpperCase() : "GENERAL";
                tvCat.setText(cat);
                int catColor = android.graphics.Color.parseColor("#38BDF8"); // Celeste por defecto
                if ("MASA".equalsIgnoreCase(cat)) {
                    catColor = android.graphics.Color.parseColor("#34D399"); // Verde esmeralda
                } else if ("TEMPERATURA".equalsIgnoreCase(cat)) {
                    catColor = android.graphics.Color.parseColor("#FBBF24"); // Ámbar dorado
                }
                tvCat.setTextColor(catColor);
                tvCat.setTextSize(11f);
                tvCat.setTypeface(null, android.graphics.Typeface.BOLD);

                // Hora
                TextView tvFecha = new TextView(ConversionActivity.this);
                String hora = item.getFechaHora() != null && item.getFechaHora().length() >= 16
                        ? item.getFechaHora().substring(11, 16)
                        : (item.getFechaHora() != null ? item.getFechaHora() : "");
                tvFecha.setText(" • " + hora);
                tvFecha.setTextColor(android.graphics.Color.parseColor("#64748B"));
                tvFecha.setTextSize(11f);

                topRow.addView(tvId);
                topRow.addView(tvCat);
                topRow.addView(tvFecha);

                // Conversión con valor resultante en verde brillante
                TextView tvConv = new TextView(ConversionActivity.this);
                String convText = String.format(java.util.Locale.US, "%.1f %s  →  %.2f %s",
                        item.getValorOriginal(), item.getUnidadOrigen(), item.getResultado(), item.getUnidadDestino());
                android.text.SpannableString spannable = new android.text.SpannableString(convText);
                int arrowIdx = convText.indexOf("→");
                if (arrowIdx != -1 && arrowIdx + 2 < convText.length()) {
                    spannable.setSpan(new android.text.style.ForegroundColorSpan(android.graphics.Color.parseColor("#4ADE80")),
                            arrowIdx + 2, convText.length(), android.text.Spanned.SPAN_EXCLUSIVE_EXCLUSIVE);
                    spannable.setSpan(new android.text.style.StyleSpan(android.graphics.Typeface.BOLD),
                            arrowIdx + 2, convText.length(), android.text.Spanned.SPAN_EXCLUSIVE_EXCLUSIVE);
                }
                tvConv.setText(spannable);
                tvConv.setTextColor(android.graphics.Color.parseColor("#CBD5E1"));
                tvConv.setTextSize(13f);
                tvConv.setPadding(0, 8, 0, 0);

                card.addView(topRow);
                card.addView(tvConv);

                llHistorialItems.addView(card);
            }
        }));
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
