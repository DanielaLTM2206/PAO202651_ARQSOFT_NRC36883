package ec.edu.monster.controladores.filtros;

import jakarta.ws.rs.NameBinding;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/**
 * Marca los recursos o métodos que exigen un token Bearer válido.
 * Solo esos pasan por {@link FiltroAutenticacion}; el login no la lleva.
 */
@NameBinding
@Retention(RetentionPolicy.RUNTIME)
@Target({ElementType.TYPE, ElementType.METHOD})
public @interface Asegurado {
}
