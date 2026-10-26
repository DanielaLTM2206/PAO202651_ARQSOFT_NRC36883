package ec.edu.monster.servicios;

import ec.edu.monster.excepciones.ExcepcionValidacion;
import ec.edu.monster.modelos.utilidades.IConversor;

public class ServicioConversor<T> {

    private final IConversor<T> conversor;

    public ServicioConversor(IConversor<T> conversor) {
        this.conversor = conversor;
    }

    public double convertir(double valor, T origen, T destino) {
        if (conversor == null) {
            throw new IllegalStateException("No se ha inyectado un conversor");
        }
        if (!Double.isFinite(valor)) {
            throw new ExcepcionValidacion("El valor debe ser un número finito");
        }

        double resultado = conversor.convertir(valor, origen, destino);

        if (!Double.isFinite(resultado)) {
            throw new ExcepcionValidacion("El resultado excede el rango numérico soportado");
        }
        return resultado;
    }
}
