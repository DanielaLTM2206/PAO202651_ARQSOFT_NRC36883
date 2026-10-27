using System;
using WS_ConUni_DotNet_REST.Constants;
using WS_ConUni_DotNet_REST.Models;
using WS_ConUni_DotNet_REST.Validators;

namespace WS_ConUni_DotNet_REST.Services
{
    public class TemperaturaService
    {
        public ConversionResultModel Convertir(string valorString, string origen, string destino)
        {
            var error = BaseValidator.ValidarStringTemperatura(valorString, origen, out double valor);
            if (error != null)
                return ConversionResultModel.Fallo(error);

            string o = origen.Trim().ToLowerInvariant();
            string d = destino.Trim().ToLowerInvariant();

            // Normalizar nombres
            string nomOrigen = NormalizarUnidad(o);
            string nomDestino = NormalizarUnidad(d);

            if (nomOrigen == "Desconocida")
            {
                return ConversionResultModel.Fallo(new ConversionErrorModel(
                    ErrorConstants.ERROR_CONVERSION_TEMPERATURA,
                    $"Unidad de origen no soportada: {origen}",
                    ErrorConstants.TIPO_CONVERSION,
                    valor,
                    origen));
            }

            if (nomDestino == "Desconocida")
            {
                return ConversionResultModel.Fallo(new ConversionErrorModel(
                    ErrorConstants.ERROR_CONVERSION_TEMPERATURA,
                    $"Unidad de destino no soportada: {destino}",
                    ErrorConstants.TIPO_CONVERSION,
                    valor,
                    destino));
            }

            if (nomOrigen == nomDestino)
            {
                var idModel = new UnidadConversionModel(valor, valor, nomOrigen, nomDestino, "Temperatura", 1.0);
                return ConversionResultModel.Exito(idModel);
            }

            double resultado = 0;
            double factor = 1.0;

            if (nomOrigen == TemperaturaConstants.CELSIUS && nomDestino == TemperaturaConstants.FAHRENHEIT)
            {
                resultado = (valor * 9.0 / 5.0) + 32.0;
                factor = 1.8;
            }
            else if (nomOrigen == TemperaturaConstants.FAHRENHEIT && nomDestino == TemperaturaConstants.CELSIUS)
            {
                resultado = (valor - 32.0) * 5.0 / 9.0;
                factor = 5.0 / 9.0;
            }
            else if (nomOrigen == TemperaturaConstants.CELSIUS && nomDestino == TemperaturaConstants.KELVIN)
            {
                resultado = valor + 273.15;
                factor = 1.0;
            }
            else if (nomOrigen == TemperaturaConstants.KELVIN && nomDestino == TemperaturaConstants.CELSIUS)
            {
                resultado = valor - 273.15;
                factor = 1.0;
            }
            else if (nomOrigen == TemperaturaConstants.FAHRENHEIT && nomDestino == TemperaturaConstants.KELVIN)
            {
                resultado = ((valor - 32.0) * 5.0 / 9.0) + 273.15;
                factor = 5.0 / 9.0;
            }
            else if (nomOrigen == TemperaturaConstants.KELVIN && nomDestino == TemperaturaConstants.FAHRENHEIT)
            {
                resultado = ((valor - 273.15) * 9.0 / 5.0) + 32.0;
                factor = 1.8;
            }

            var modelo = new UnidadConversionModel(
                valor,
                resultado,
                nomOrigen,
                nomDestino,
                "Temperatura",
                factor);

            return ConversionResultModel.Exito(modelo);
        }

        private static string NormalizarUnidad(string u)
        {
            if (u.StartsWith("cel") || u == "c" || u == "°c") return TemperaturaConstants.CELSIUS;
            if (u.StartsWith("fah") || u == "f" || u == "°f") return TemperaturaConstants.FAHRENHEIT;
            if (u.StartsWith("kel") || u == "k") return TemperaturaConstants.KELVIN;
            return "Desconocida";
        }

        // Métodos de conveniencia directa
        public ConversionResultModel ConvertirCelsiusAFahrenheit(string v) => Convertir(v, TemperaturaConstants.CELSIUS, TemperaturaConstants.FAHRENHEIT);
        public ConversionResultModel ConvertirFahrenheitACelsius(string v) => Convertir(v, TemperaturaConstants.FAHRENHEIT, TemperaturaConstants.CELSIUS);
        public ConversionResultModel ConvertirCelsiusAKelvin(string v) => Convertir(v, TemperaturaConstants.CELSIUS, TemperaturaConstants.KELVIN);
        public ConversionResultModel ConvertirKelvinACelsius(string v) => Convertir(v, TemperaturaConstants.KELVIN, TemperaturaConstants.CELSIUS);
    }
}
