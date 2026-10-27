using System;
using System.Collections.Generic;
using WS_ConUni_DotNet_REST.Constants;
using WS_ConUni_DotNet_REST.Models;
using WS_ConUni_DotNet_REST.Validators;

namespace WS_ConUni_DotNet_REST.Services
{
    public class LongitudService
    {
        private static readonly Dictionary<string, double> _factoresAMetro = new(StringComparer.OrdinalIgnoreCase)
        {
            ["metro"] = LongitudConstants.FACTOR_METRO,
            ["metros"] = LongitudConstants.FACTOR_METRO,
            ["m"] = LongitudConstants.FACTOR_METRO,

            ["milla"] = LongitudConstants.FACTOR_MILLA,
            ["millas"] = LongitudConstants.FACTOR_MILLA,
            ["mi"] = LongitudConstants.FACTOR_MILLA,

            ["pulgada"] = LongitudConstants.FACTOR_PULGADA,
            ["pulgadas"] = LongitudConstants.FACTOR_PULGADA,
            ["in"] = LongitudConstants.FACTOR_PULGADA,

            ["kilometro"] = LongitudConstants.FACTOR_KILOMETRO,
            ["kilometros"] = LongitudConstants.FACTOR_KILOMETRO,
            ["km"] = LongitudConstants.FACTOR_KILOMETRO,

            ["pie"] = LongitudConstants.FACTOR_PIE,
            ["pies"] = LongitudConstants.FACTOR_PIE,
            ["ft"] = LongitudConstants.FACTOR_PIE,

            ["yarda"] = LongitudConstants.FACTOR_YARDA,
            ["yardas"] = LongitudConstants.FACTOR_YARDA,
            ["yd"] = LongitudConstants.FACTOR_YARDA
        };

        public ConversionResultModel Convertir(string valorString, string origen, string destino)
        {
            var error = BaseValidator.ValidarStringPositivo(valorString, origen, out double valor);
            if (error != null)
                return ConversionResultModel.Fallo(error);

            if (!_factoresAMetro.TryGetValue(origen.Trim(), out double factorOrigen))
            {
                return ConversionResultModel.Fallo(new ConversionErrorModel(
                    ErrorConstants.ERROR_CONVERSION_LONGITUD,
                    $"Unidad de origen no soportada: {origen}",
                    ErrorConstants.TIPO_CONVERSION,
                    valor,
                    origen));
            }

            if (!_factoresAMetro.TryGetValue(destino.Trim(), out double factorDestino))
            {
                return ConversionResultModel.Fallo(new ConversionErrorModel(
                    ErrorConstants.ERROR_CONVERSION_LONGITUD,
                    $"Unidad de destino no soportada: {destino}",
                    ErrorConstants.TIPO_CONVERSION,
                    valor,
                    destino));
            }

            // Convertir primero a metros y luego a la unidad destino
            double valorEnMetros = valor * factorOrigen;
            double resultadoFinal = valorEnMetros / factorDestino;
            double factorConversion = factorOrigen / factorDestino;

            var modelo = new UnidadConversionModel(
                valor,
                resultadoFinal,
                origen,
                destino,
                "Longitud",
                factorConversion);

            return ConversionResultModel.Exito(modelo);
        }

        // Métodos de compatibilidad directa con GR01
        public ConversionResultModel ConvertirMillaAMetro(string valor) => Convertir(valor, LongitudConstants.MILLA, LongitudConstants.METRO);
        public ConversionResultModel ConvertirMetroAMilla(string valor) => Convertir(valor, LongitudConstants.METRO, LongitudConstants.MILLA);
        public ConversionResultModel ConvertirMillaAPulgada(string valor) => Convertir(valor, LongitudConstants.MILLA, LongitudConstants.PULGADA);
        public ConversionResultModel ConvertirPulgadaAMilla(string valor) => Convertir(valor, LongitudConstants.PULGADA, LongitudConstants.MILLA);
        public ConversionResultModel ConvertirMetroAPulgada(string valor) => Convertir(valor, LongitudConstants.METRO, LongitudConstants.PULGADA);
        public ConversionResultModel ConvertirPulgadaAMetro(string valor) => Convertir(valor, LongitudConstants.PULGADA, LongitudConstants.METRO);
    }
}
