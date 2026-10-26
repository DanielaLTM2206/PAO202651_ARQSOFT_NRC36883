namespace ec.edu.monster.Modelos.Utilidades;

public interface IConversor<T> where T : struct, Enum
{
    double Convertir(double valor, T unidadOrigen, T unidadDestino);
}
