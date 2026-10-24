/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.clienteconsola.vista;

/**
 *
 * @author joanc
 */
public enum OpcionMenu {
        CONVERTIR_LONGITUD("1", "Convertir Longitud"),
        CONVERTIR_MASA("2", "Convertir Masa"),
        CONVERTIR_TEMPERATURA("3", "Convertir Temperatura"),
        VER_HISTORIAL("4", "Ver Historial Global de Conversiones (Multi-canal)"),
        SALIR("5", "Salir");

        private final String numero;
        private final String descripcion;

        OpcionMenu(String numero, String descripcion) {
            this.numero = numero;
            this.descripcion = descripcion;
        }

        public String getNumero() { return numero; }
        public String getDescripcion() { return descripcion; }
    }
