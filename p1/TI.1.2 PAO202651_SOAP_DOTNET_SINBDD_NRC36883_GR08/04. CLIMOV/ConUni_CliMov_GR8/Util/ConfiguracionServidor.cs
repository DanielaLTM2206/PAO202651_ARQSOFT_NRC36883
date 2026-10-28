namespace ec.edu.monster.Util;

/// <summary>
/// Dirección del servicio SOAP WSConversorUnidades. Se guarda en las preferencias del teléfono para que el
/// usuario pueda cambiarla (por ejemplo, para usar la IP de la PC desde un teléfono real).
/// </summary>
public static class ConfiguracionServidor
{
    private const string ClavePreferencia = "UrlServidorSoap";
    private const string Ruta = ":5080/WSConversorUnidades.svc";

    // En el emulador de Android, 10.0.2.2 apunta al localhost de la PC donde corre WS_ConUni_Dotnet_SOAP
    public static string UrlPorDefecto => DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2" + Ruta
            : "http://localhost" + Ruta;

    public static string UrlActual
    {
        get => Preferences.Default.Get(ClavePreferencia, UrlPorDefecto);
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Preferences.Default.Remove(ClavePreferencia);
            }
            else
            {
                Preferences.Default.Set(ClavePreferencia, value.Trim().TrimEnd('/'));
            }
        }
    }

    public static bool EsUrlValida(string? url) => DireccionServidor.EsUrlValida(url);
}
