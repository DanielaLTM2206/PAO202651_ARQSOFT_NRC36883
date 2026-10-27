using System;
using System.Collections.Generic;
using WS_ConUni_DotNet_REST.Constants;
using WS_ConUni_DotNet_REST.Models;
using WS_ConUni_DotNet_REST.Validators;

namespace WS_ConUni_DotNet_REST.Services
{
    public class MasaService
    {
        private static readonly Dictionary<string, double> _factoresAKilogramo = new(StringComparer.OrdinalIgnoreCase)
        {
            ["kilogramo"] = MasaConstants.FACTOR_KILOGRAMO,
            ["kilogramos"] = MasaConstants.FACTOR_KILOGRAMO,
            ["kg"] = MasaConstants.FACTOR_KILOGRAMO,

            ["libra"] = MasaConstants.FACTOR_LIBRA,
            ["libras"] = MasaConstants.FACTOR_LIBRA,
            ["lb"] = MasaConstants.FACTOR_LIBRA,

            ["onza"] = MasaConstants.FACTOR_ONZA,
            ["onzas"] = MasaConstants.FACTOR_ONZA,
            ["oz"] = MasaConstants.FACTOR_ONZA,

            ["gramo"] = MasaConstants.FACTOR_GRAMO,
            ["gramos"] = MasaConstants.FACTOR_GRAMO,
            ["g"] = MasaConstants.FACTOR_GRAMO
        };

        public ConversionResultModel Convertir(string valorString, string origen, string destino)
        {
            var error = BaseValidator.ValidarStringPositivo(valorString, origen, out double valor);
            if (error != null)
                return ConversionResultModel.Fallo(error);

            if (!_factoresAKilogramo.TryGetValue(origen.Trim(), out double factorOrigen))
            {
                return ConversionResultModel.Fallo(new ConversionErrorModel(
                    ErrorConstants.ERROR_CONVERSION_MASA,
                    $"Unidad de origen no soportada: {origen}",
                    ErrorConstants.TIPO_CONVERSION,
                    valor,
                    origen));
            }

            if (!_factoresAKilogramo.TryGetValue(destino.Trim(), out double factorDestino))
            {
                return ConversionResultModel.Fallo(new ConversionErrorModel(
                    ErrorConstants.ERROR_CONVERSION_MASA,
                    $"Unidad de destino no soportada: {destino}",
                    ErrorConstants.TIPO_CONVERSION,
                    valor,
                    destino));
            }

            double valorEnKg = valor * factorOrigen;
            double resultadoFinal = valorEnKg / factorDestino;
            double factorConversion = factorOrigen / factorDestino;

            var modelo = new UnidadConversionModel(
                valor,
                resultadoFinal,
                origen,
                destino,
                "Masa",
                factorConversion);

            return ConversionResultModel.Exito(modelo);
        }

        // Métodos de compatibilidad directa con GR01
        public ConversionResultModel ConvertirKilogramoALibra(string valor) => Convertir(valor, MasaConstants.KILOGRAMO, MasaConstants.LIBRA);
        public ConversionResultModel ConvertirLibraAKilogramo(string valor) => Convertir(valor, MasaConstants.LIBRA, MasaConstants.KILOGRAMO);
        public ConversionResultModel ConvertirKilogramoAOnza(string valor) => Convertir(valor, MasaConstants.KILOGRAMO, MasaConstants.ONZA);
        public ConversionResultModel ConvertirOnzaAKilogramo(string valor) => Convertir(valor, MasaConstants.ONZA, MasaConstants.KILOGRAMO);
        public ConversionResultModel ConvertirLibraAOnza(string valor) => Convertir(valor, MasaConstants.LIBRA, MasaConstants.ONZA);
        public ConversionResultModel ConvertirOnzaALibra(string valor) => Convertir(valor, MasaConstants.ONZA, MasaConstants.LIBRA);
    }
}
