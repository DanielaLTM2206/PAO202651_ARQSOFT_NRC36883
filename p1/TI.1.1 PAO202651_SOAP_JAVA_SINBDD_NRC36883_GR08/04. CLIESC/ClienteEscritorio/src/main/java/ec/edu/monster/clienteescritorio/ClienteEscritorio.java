package ec.edu.monster.clienteescritorio;

import ec.edu.monster.clienteescritorio.controlador.ControladorConversiones;
import ec.edu.monster.clienteescritorio.vista.LoginFrame;
import javax.swing.SwingUtilities;
import javax.swing.UIManager;

/**
 * Punto de entrada para el Cliente de Escritorio Swing (GR08).
 * 
 * @author GR08 - Arquitectura de Software
 */
public class ClienteEscritorio {

    public static void main(String[] args) {
        // Look and Feel nativo
        try {
            UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
        } catch (Exception ignored) {
        }

        SwingUtilities.invokeLater(() -> {
            ControladorConversiones controlador = new ControladorConversiones();
            LoginFrame login = new LoginFrame(controlador);
            login.setVisible(true);
        });
    }
}
