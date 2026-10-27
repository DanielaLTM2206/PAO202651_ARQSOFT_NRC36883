namespace WS_ConUni_DotNet_REST.Constants
{
    public static class ErrorConstants
    {
        // Códigos de error de validación
        public const string VALOR_NEGATIVO = "VAL_001";
        public const string VALOR_NAN = "VAL_002";
        public const string VALOR_INFINITO = "VAL_003";
        public const string TEMPERATURA_BAJO_CERO_ABSOLUTO = "VAL_004";
        public const string VALOR_VACIO = "VAL_005";
        public const string VALOR_NO_NUMERICO = "VAL_006";
        public const string VALOR_FUERA_DE_RANGO = "VAL_007";

        // Códigos de error de conversión
        public const string ERROR_CONVERSION_LONGITUD = "CONV_001";
        public const string ERROR_CONVERSION_MASA = "CONV_002";
        public const string ERROR_CONVERSION_TEMPERATURA = "CONV_003";

        // Códigos de autenticación y seguridad
        public const string AUTH_CREDENCIALES_INVALIDAS = "AUTH_001";
        public const string AUTH_TOKEN_INVALIDO = "AUTH_002";
        public const string AUTH_TOKEN_EXPIRADO = "AUTH_003";
        public const string AUTH_CAMPOS_REQUERIDOS = "AUTH_004";
        public const string AUTH_CLAVE_ACTUAL_INCORRECTA = "AUTH_005";
        public const string AUTH_CLAVE_NUEVA_INVALIDA = "AUTH_006";

        // Códigos de error del sistema
        public const string ERROR_INTERNO = "SYS_001";
        public const string ERROR_SERVICIO_NO_DISPONIBLE = "SYS_002";

        // Tipos de error
        public const string TIPO_VALIDACION = "Validacion";
        public const string TIPO_CONVERSION = "Conversion";
        public const string TIPO_SEGURIDAD = "Seguridad";
        public const string TIPO_SISTEMA = "Sistema";

        // Mensajes base
        public const string MSG_VALOR_NEGATIVO = "El valor no puede ser negativo";
        public const string MSG_VALOR_NAN = "El valor no es un número válido";
        public const string MSG_VALOR_INFINITO = "El valor no puede ser infinito";
        public const string MSG_TEMPERATURA_INVALIDA = "La temperatura está por debajo del cero absoluto";
        public const string MSG_ERROR_INTERNO = "Error interno del sistema";
        public const string MSG_VALOR_VACIO = "El valor no puede estar vacío";
        public const string MSG_VALOR_NO_NUMERICO = "El valor no es un número válido";
    }
}
