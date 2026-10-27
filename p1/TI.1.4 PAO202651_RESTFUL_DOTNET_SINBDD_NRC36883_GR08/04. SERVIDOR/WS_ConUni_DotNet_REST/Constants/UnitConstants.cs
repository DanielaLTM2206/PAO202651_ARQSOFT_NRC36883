namespace WS_ConUni_DotNet_REST.Constants
{
    public static class LongitudConstants
    {
        public const string MILLA = "Milla";
        public const string METRO = "Metro";
        public const string PULGADA = "Pulgada";
        public const string KILOMETRO = "Kilometro";
        public const string PIE = "Pie";
        public const string YARDA = "Yarda";

        // Factores directos a metros (Unidad base)
        public const double FACTOR_METRO = 1.0;
        public const double FACTOR_MILLA = 1609.344;
        public const double FACTOR_PULGADA = 0.0254;
        public const double FACTOR_KILOMETRO = 1000.0;
        public const double FACTOR_PIE = 0.3048;
        public const double FACTOR_YARDA = 0.9144;

        // Factores de compatibilidad directa
        public const double MILLA_A_METRO = 1609.344;
        public const double METRO_A_MILLA = 1.0 / 1609.344;
        public const double MILLA_A_PULGADA = 63360.0;
        public const double PULGADA_A_MILLA = 1.0 / 63360.0;
        public const double METRO_A_PULGADA = 39.3700787;
        public const double PULGADA_A_METRO = 0.0254;
    }

    public static class MasaConstants
    {
        public const string KILOGRAMO = "Kilogramo";
        public const string LIBRA = "Libra";
        public const string ONZA = "Onza";
        public const string GRAMO = "Gramo";

        // Factores directos a kilogramos (Unidad base)
        public const double FACTOR_KILOGRAMO = 1.0;
        public const double FACTOR_GRAMO = 0.001;
        public const double FACTOR_LIBRA = 0.45359237;
        public const double FACTOR_ONZA = 0.0283495231;

        // Factores de compatibilidad directa
        public const double KILOGRAMO_A_LIBRA = 2.20462262;
        public const double LIBRA_A_KILOGRAMO = 0.45359237;
        public const double KILOGRAMO_A_ONZA = 35.2739619;
        public const double ONZA_A_KILOGRAMO = 0.0283495231;
        public const double LIBRA_A_ONZA = 16.0;
        public const double ONZA_A_LIBRA = 0.0625;
    }

    public static class TemperaturaConstants
    {
        public const string CELSIUS = "Celsius";
        public const string FAHRENHEIT = "Fahrenheit";
        public const string KELVIN = "Kelvin";

        // Cero absoluto
        public const double CERO_ABSOLUTO_CELSIUS = -273.15;
        public const double CERO_ABSOLUTO_FAHRENHEIT = -459.67;
        public const double CERO_ABSOLUTO_KELVIN = 0.0;
    }
}
