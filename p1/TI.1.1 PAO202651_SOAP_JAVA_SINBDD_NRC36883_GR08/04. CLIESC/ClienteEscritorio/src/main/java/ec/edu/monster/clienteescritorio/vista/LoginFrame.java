package ec.edu.monster.clienteescritorio.vista;

import ec.edu.monster.clienteescritorio.controlador.ControladorConversiones;
import java.awt.Color;
import java.awt.Font;
import java.awt.Image;
import java.awt.event.ActionEvent;
import javax.swing.BorderFactory;
import javax.swing.ImageIcon;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JPasswordField;
import javax.swing.JTextField;
import javax.swing.SwingConstants;
import javax.swing.SwingUtilities;

/**
 * Pantalla de Login de ClienteEscritorio con Sullivan de Monsters Inc.
 * Utiliza ModernButton para garantizar colores vivos y texto visible en cualquier SO.
 * 
 * @author GR08 - Arquitectura de Software
 */
public class LoginFrame extends JFrame {

    private final ControladorConversiones controlador;

    private JTextField txtUsuario;
    private JPasswordField txtClave;
    private JLabel lblNotificaciones;
    private ModernButton btnIniciarSesion;

    public LoginFrame(ControladorConversiones controlador) {
        this.controlador = controlador;
        initComponents();
    }

    private void initComponents() {
        setTitle("Monster Inc - Autenticación SOAP GR08");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setResizable(false);
        setSize(580, 400);
        setLocationRelativeTo(null);

        JPanel panel = new JPanel();
        panel.setLayout(null);
        panel.setBackground(new Color(15, 23, 42)); // Dark slate 900

        // Título
        JLabel lblTitulo = new JLabel("Conversor de Unidades");
        lblTitulo.setFont(new Font("Segoe UI", Font.BOLD, 20));
        lblTitulo.setForeground(Color.WHITE);
        lblTitulo.setBounds(30, 25, 280, 26);
        panel.add(lblTitulo);

        JLabel lblSub = new JLabel("Ingrese sus credenciales de acceso:");
        lblSub.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        lblSub.setForeground(new Color(148, 163, 184));
        lblSub.setBounds(30, 52, 280, 20);
        panel.add(lblSub);

        // Notificaciones / Error
        lblNotificaciones = new JLabel("");
        lblNotificaciones.setFont(new Font("Segoe UI", Font.BOLD, 11));
        lblNotificaciones.setForeground(new Color(239, 68, 68)); // Rojo
        lblNotificaciones.setBounds(30, 78, 280, 20);
        panel.add(lblNotificaciones);

        // Campo Usuario
        JLabel lblUser = new JLabel("Usuario:");
        lblUser.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblUser.setForeground(new Color(226, 232, 240));
        lblUser.setBounds(30, 110, 80, 25);
        panel.add(lblUser);

        txtUsuario = new JTextField("monster");
        txtUsuario.setFont(new Font("Segoe UI", Font.BOLD, 13));
        txtUsuario.setBackground(new Color(30, 41, 59));
        txtUsuario.setForeground(Color.WHITE);
        txtUsuario.setCaretColor(Color.WHITE);
        txtUsuario.setBorder(BorderFactory.createCompoundBorder(
                BorderFactory.createLineBorder(new Color(71, 85, 105), 1),
                BorderFactory.createEmptyBorder(4, 8, 4, 8)
        ));
        txtUsuario.setBounds(110, 108, 180, 32);
        panel.add(txtUsuario);

        // Campo Clave
        JLabel lblPass = new JLabel("Clave:");
        lblPass.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblPass.setForeground(new Color(226, 232, 240));
        lblPass.setBounds(30, 158, 80, 25);
        panel.add(lblPass);

        txtClave = new JPasswordField("monster9");
        txtClave.setFont(new Font("Segoe UI", Font.BOLD, 13));
        txtClave.setBackground(new Color(30, 41, 59));
        txtClave.setForeground(Color.WHITE);
        txtClave.setCaretColor(Color.WHITE);
        txtClave.setBorder(BorderFactory.createCompoundBorder(
                BorderFactory.createLineBorder(new Color(71, 85, 105), 1),
                BorderFactory.createEmptyBorder(4, 8, 4, 8)
        ));
        txtClave.setBounds(110, 156, 180, 32);
        panel.add(txtClave);

        // Botón Iniciar Sesión con ModernButton (Azul / Indigo vibrante con texto blanco)
        btnIniciarSesion = new ModernButton("INICIAR SESIÓN", new Color(79, 70, 229), Color.WHITE, 8);
        btnIniciarSesion.setFont(new Font("Segoe UI", Font.BOLD, 13));
        btnIniciarSesion.setBounds(30, 225, 260, 44);
        btnIniciarSesion.addActionListener(this::accionIniciarSesion);
        panel.add(btnIniciarSesion);

        // Imagen de Sullivan de Monsters Inc
        JLabel lblSulley = new JLabel();
        lblSulley.setHorizontalAlignment(SwingConstants.CENTER);
        try {
            java.net.URL imgUrl = getClass().getResource("/ec/edu/monster/clienteescritorio/vista/monsterlogin.jpg");
            if (imgUrl != null) {
                ImageIcon rawIcon = new ImageIcon(imgUrl);
                Image img = rawIcon.getImage().getScaledInstance(225, 305, Image.SCALE_SMOOTH);
                lblSulley.setIcon(new ImageIcon(img));
            }
        } catch (Exception ex) {
            System.err.println("No se pudo cargar la imagen de Sullivan: " + ex.getMessage());
        }
        lblSulley.setBounds(315, 25, 225, 305);
        panel.add(lblSulley);

        getContentPane().add(panel);
    }

    private void accionIniciarSesion(ActionEvent e) {
        String usuario = txtUsuario.getText().trim();
        String clave = new String(txtClave.getPassword()).trim();

        if (usuario.isEmpty() || clave.isEmpty()) {
            lblNotificaciones.setForeground(new Color(239, 68, 68));
            lblNotificaciones.setText("Complete usuario y clave.");
            return;
        }

        btnIniciarSesion.setEnabled(false);
        lblNotificaciones.setForeground(new Color(59, 130, 246));
        lblNotificaciones.setText("Autenticando con el servidor SOAP...");

        SwingUtilities.invokeLater(() -> {
            boolean exito = controlador.iniciarSesion(usuario, clave);
            if (exito) {
                lblNotificaciones.setText("");
                InterfazConversiones interfaz = new InterfazConversiones(controlador);
                interfaz.setVisible(true);
                dispose();
            } else {
                lblNotificaciones.setForeground(new Color(239, 68, 68));
                String err = controlador.getUltimoError();
                lblNotificaciones.setText(err != null ? err : "Credenciales incorrectas");
                btnIniciarSesion.setEnabled(true);
            }
        });
    }

    public static void main(String args[]) {
        try {
            javax.swing.UIManager.setLookAndFeel(javax.swing.UIManager.getSystemLookAndFeelClassName());
        } catch (Exception ignored) {
        }

        SwingUtilities.invokeLater(() -> new LoginFrame(new ControladorConversiones()).setVisible(true));
    }
}
