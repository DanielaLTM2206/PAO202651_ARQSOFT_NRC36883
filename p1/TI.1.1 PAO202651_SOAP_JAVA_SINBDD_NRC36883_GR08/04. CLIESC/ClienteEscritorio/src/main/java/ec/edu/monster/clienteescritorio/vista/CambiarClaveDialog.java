package ec.edu.monster.clienteescritorio.vista;

import ec.edu.monster.clienteescritorio.controlador.ControladorConversiones;
import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.Frame;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.GridLayout;
import java.awt.Insets;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import javax.swing.BorderFactory;
import javax.swing.JDialog;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JPasswordField;
import javax.swing.SwingConstants;
import javax.swing.SwingUtilities;
import javax.swing.Timer;

/**
 * Diálogo modal estilizado para el cambio de contraseña corporativa.
 * Diseño moderno oscuro (Dark Slate) alineado con Monsters Inc. GR08.
 * 
 * @author GR08 - Arquitectura de Software
 */
public class CambiarClaveDialog extends JDialog {

    private final ControladorConversiones controlador;

    private JPasswordField txtActual;
    private JPasswordField txtNueva;
    private JPasswordField txtConfirmar;
    private JLabel lblMensaje;
    private ModernButton btnGuardar;
    private ModernButton btnCancelar;

    public CambiarClaveDialog(Frame parent, ControladorConversiones controlador) {
        super(parent, "Cambiar Contraseña Corporativa", true);
        this.controlador = controlador;
        initComponents();
    }

    private void initComponents() {
        setSize(440, 480);
        setResizable(false);
        setLocationRelativeTo(getParent());
        getContentPane().setBackground(new Color(15, 23, 42)); // Slate 900
        setLayout(new BorderLayout());

        // Contenedor principal con tarjeta interna
        JPanel cardPanel = new JPanel();
        cardPanel.setBackground(new Color(21, 27, 40)); // Slate 850
        cardPanel.setBorder(BorderFactory.createCompoundBorder(
                BorderFactory.createEmptyBorder(16, 16, 16, 16),
                BorderFactory.createCompoundBorder(
                        BorderFactory.createLineBorder(new Color(51, 65, 85), 1, true),
                        BorderFactory.createEmptyBorder(20, 22, 20, 22)
                )
        ));
        cardPanel.setLayout(new GridBagLayout());
        GridBagConstraints gbc = new GridBagConstraints();
        gbc.insets = new Insets(0, 0, 0, 0);
        gbc.fill = GridBagConstraints.HORIZONTAL;
        gbc.weightx = 1.0;

        // 1. Encabezado
        JLabel lblTitulo = new JLabel("🔐 Cambiar Contraseña");
        lblTitulo.setFont(new Font("Segoe UI", Font.BOLD, 19));
        lblTitulo.setForeground(Color.WHITE);
        gbc.gridx = 0;
        gbc.gridy = 0;
        gbc.insets = new Insets(0, 0, 4, 0);
        cardPanel.add(lblTitulo, gbc);

        JLabel lblSubtitulo = new JLabel("Sincronización central e inmediata en el servidor SOAP.");
        lblSubtitulo.setFont(new Font("Segoe UI", Font.PLAIN, 11));
        lblSubtitulo.setForeground(new Color(148, 163, 184));
        gbc.gridy = 1;
        gbc.insets = new Insets(0, 0, 16, 0);
        cardPanel.add(lblSubtitulo, gbc);

        // 2. Banner de Notificaciones / Errores
        lblMensaje = new JLabel("", SwingConstants.CENTER);
        lblMensaje.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblMensaje.setOpaque(true);
        lblMensaje.setBackground(new Color(21, 27, 40));
        lblMensaje.setForeground(new Color(239, 68, 68));
        lblMensaje.setVisible(false);
        lblMensaje.setBorder(BorderFactory.createEmptyBorder(6, 8, 6, 8));
        gbc.gridy = 2;
        gbc.insets = new Insets(0, 0, 12, 0);
        cardPanel.add(lblMensaje, gbc);

        // 3. Campo: Contraseña Actual
        JLabel lblActual = new JLabel("Contraseña Actual:");
        lblActual.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblActual.setForeground(new Color(226, 232, 240));
        gbc.gridy = 3;
        gbc.insets = new Insets(0, 0, 4, 0);
        cardPanel.add(lblActual, gbc);

        txtActual = crearCampoPassword();
        gbc.gridy = 4;
        gbc.insets = new Insets(0, 0, 12, 0);
        cardPanel.add(txtActual, gbc);

        // 4. Campo: Nueva Contraseña
        JLabel lblNueva = new JLabel("Nueva Contraseña (mínimo 4 caracteres):");
        lblNueva.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblNueva.setForeground(new Color(226, 232, 240));
        gbc.gridy = 5;
        gbc.insets = new Insets(0, 0, 4, 0);
        cardPanel.add(lblNueva, gbc);

        txtNueva = crearCampoPassword();
        gbc.gridy = 6;
        gbc.insets = new Insets(0, 0, 12, 0);
        cardPanel.add(txtNueva, gbc);

        // 5. Campo: Confirmar Contraseña
        JLabel lblConfirmar = new JLabel("Confirmar Nueva Contraseña:");
        lblConfirmar.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblConfirmar.setForeground(new Color(226, 232, 240));
        gbc.gridy = 7;
        gbc.insets = new Insets(0, 0, 4, 0);
        cardPanel.add(lblConfirmar, gbc);

        txtConfirmar = crearCampoPassword();
        gbc.gridy = 8;
        gbc.insets = new Insets(0, 0, 20, 0);
        cardPanel.add(txtConfirmar, gbc);

        // 6. Botones de Acción
        JPanel buttonPanel = new JPanel(new GridLayout(1, 2, 12, 0));
        buttonPanel.setOpaque(false);

        btnCancelar = new ModernButton("Cancelar", new Color(51, 65, 85), Color.WHITE, 8);
        btnCancelar.setFont(new Font("Segoe UI", Font.BOLD, 13));
        btnCancelar.setPreferredSize(new Dimension(140, 38));
        btnCancelar.addActionListener(e -> dispose());
        buttonPanel.add(btnCancelar);

        btnGuardar = new ModernButton("Guardar Clave", new Color(37, 99, 235), Color.WHITE, 8);
        btnGuardar.setFont(new Font("Segoe UI", Font.BOLD, 13));
        btnGuardar.setPreferredSize(new Dimension(140, 38));
        btnGuardar.addActionListener(e -> ejecutarCambioClave());
        buttonPanel.add(btnGuardar);

        gbc.gridy = 9;
        gbc.insets = new Insets(0, 0, 0, 0);
        cardPanel.add(buttonPanel, gbc);

        add(cardPanel, BorderLayout.CENTER);

        // Atajo Enter para guardar y Esc para cancelar
        KeyAdapter enterKey = new KeyAdapter() {
            @Override
            public void keyPressed(KeyEvent e) {
                if (e.getKeyCode() == KeyEvent.VK_ENTER) {
                    ejecutarCambioClave();
                } else if (e.getKeyCode() == KeyEvent.VK_ESCAPE) {
                    dispose();
                }
            }
        };
        txtActual.addKeyListener(enterKey);
        txtNueva.addKeyListener(enterKey);
        txtConfirmar.addKeyListener(enterKey);
    }

    private JPasswordField crearCampoPassword() {
        JPasswordField field = new JPasswordField();
        field.setPreferredSize(new Dimension(320, 36));
        field.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        field.setBackground(new Color(13, 18, 28));
        field.setForeground(Color.WHITE);
        field.setCaretColor(Color.WHITE);
        field.setEchoChar('•');
        field.setBorder(BorderFactory.createCompoundBorder(
                BorderFactory.createLineBorder(new Color(51, 65, 85), 1, true),
                BorderFactory.createEmptyBorder(6, 10, 6, 10)
        ));
        return field;
    }

    private void mostrarMensaje(String texto, boolean esError) {
        lblMensaje.setText(texto);
        if (esError) {
            lblMensaje.setBackground(new Color(69, 10, 10)); // Rojo oscuro suave
            lblMensaje.setForeground(new Color(248, 113, 113)); // Rojo claro
            lblMensaje.setBorder(BorderFactory.createCompoundBorder(
                    BorderFactory.createLineBorder(new Color(185, 28, 28), 1, true),
                    BorderFactory.createEmptyBorder(6, 10, 6, 10)
            ));
        } else {
            lblMensaje.setBackground(new Color(6, 78, 59)); // Verde oscuro suave
            lblMensaje.setForeground(new Color(52, 211, 153)); // Verde menta
            lblMensaje.setBorder(BorderFactory.createCompoundBorder(
                    BorderFactory.createLineBorder(new Color(5, 150, 105), 1, true),
                    BorderFactory.createEmptyBorder(6, 10, 6, 10)
            ));
        }
        lblMensaje.setVisible(true);
        revalidate();
        repaint();
    }

    private void ejecutarCambioClave() {
        String actual = new String(txtActual.getPassword()).trim();
        String nueva = new String(txtNueva.getPassword()).trim();
        String confirmar = new String(txtConfirmar.getPassword()).trim();

        if (actual.isEmpty() || nueva.isEmpty() || confirmar.isEmpty()) {
            mostrarMensaje("Por favor completa todos los campos requeridos.", true);
            return;
        }

        if (nueva.length() < 4) {
            mostrarMensaje("La nueva contraseña debe tener al menos 4 caracteres.", true);
            return;
        }

        if (actual.equals(nueva)) {
            mostrarMensaje("La nueva contraseña debe ser diferente a la actual.", true);
            return;
        }

        if (!nueva.equals(confirmar)) {
            mostrarMensaje("La confirmación no coincide con la nueva contraseña.", true);
            return;
        }

        btnGuardar.setEnabled(false);
        btnCancelar.setEnabled(false);
        mostrarMensaje("Actualizando contraseña en el servidor...", false);

        new Thread(() -> {
            try {
                var resp = controlador.cambiarContrasenia(actual, nueva);
                SwingUtilities.invokeLater(() -> {
                    if ("OK".equalsIgnoreCase(resp.getEstado())) {
                        mostrarMensaje("✓ " + (resp.getMensaje() != null ? resp.getMensaje() : "Contraseña actualizada exitosamente."), false);
                        
                        // Cerrar automáticamente tras 1.5 segundos de confirmación
                        Timer timer = new Timer(1500, evt -> dispose());
                        timer.setRepeats(false);
                        timer.start();
                    } else {
                        btnGuardar.setEnabled(true);
                        btnCancelar.setEnabled(true);
                        mostrarMensaje(resp.getMensaje() != null ? resp.getMensaje() : "Error al actualizar la contraseña.", true);
                    }
                });
            } catch (Exception ex) {
                SwingUtilities.invokeLater(() -> {
                    btnGuardar.setEnabled(true);
                    btnCancelar.setEnabled(true);
                    mostrarMensaje("Error de comunicación: " + ex.getMessage(), true);
                });
            }
        }).start();
    }
}
