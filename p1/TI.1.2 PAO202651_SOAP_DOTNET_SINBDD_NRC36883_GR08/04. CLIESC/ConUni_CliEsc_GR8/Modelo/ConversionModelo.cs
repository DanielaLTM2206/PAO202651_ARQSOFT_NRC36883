namespace ec.edu.monster.Modelo;

public class ConversionModelo
{
    public TipoConversion? TipoConversion { get; set; }
    public double Valor { get; set; }
    public string? UnidadInicial { get; set; }
    public string? UnidadFinal { get; set; }

    public ConversionModelo()
    {
    }

    public ConversionModelo(TipoConversion? tipoConversion, double valor, string? unidadInicial, string? unidadFinal)
    {
        TipoConversion = tipoConversion;
        Valor = valor;
        UnidadInicial = unidadInicial;
        UnidadFinal = unidadFinal;
    }
}
