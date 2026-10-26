using System.Text.Json.Serialization;

namespace ec.edu.monster.Cliente.Dto;

public class UnidadesCategoriaRespuesta
{
    [JsonPropertyName("Categoria")]
    public string Categoria { get; set; } = string.Empty;

    [JsonPropertyName("Unidades")]
    public List<string> Unidades { get; set; } = new();
}
