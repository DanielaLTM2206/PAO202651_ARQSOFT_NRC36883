package ec.edu.monster.conuniclientemovil.model;

import android.content.Context;
import android.content.SharedPreferences;
import android.text.TextUtils;

/**
 * Gestiona la persistencia local de preferencias de la aplicación.
 *
 * Forma parte de la capa Model: es la fuente de datos local.
 */
public class AppPreferences {

    private static final String PREFS_NAME = "conuni_prefs";
    private static final String KEY_BASE_URL = "base_url";
    private static final String KEY_TOKEN = "token";

    private final SharedPreferences sharedPreferences;

    public AppPreferences(Context context) {
        sharedPreferences = context.getApplicationContext()
                .getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE);
    }

    public String getBaseUrl(String defaultValue) {
        return sharedPreferences.getString(KEY_BASE_URL, defaultValue);
    }

    public void saveBaseUrl(String baseUrl) {
        if (TextUtils.isEmpty(baseUrl)) {
            return;
        }

        sharedPreferences.edit().putString(KEY_BASE_URL, baseUrl.trim()).apply();
    }

    public String getToken() {
        return sharedPreferences.getString(KEY_TOKEN, null);
    }

    public void saveToken(String token) {
        if (TextUtils.isEmpty(token)) {
            return;
        }

        sharedPreferences.edit().putString(KEY_TOKEN, token).apply();
    }

    public void clearToken() {
        sharedPreferences.edit().remove(KEY_TOKEN).apply();
    }
}
