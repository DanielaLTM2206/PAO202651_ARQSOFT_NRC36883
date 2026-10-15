package ec.edu.monster.clienteescritorio.vista;

import ec.edu.monster.clienteescritorio.controlador.ControladorConversiones;
import java.awt.Color;
import java.awt.Cursor;
import java.awt.Font;
import java.awt.Image;
import java.awt.event.ActionEvent;
import javax.swing.BorderFactory;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JPasswordField;
import javax.swing.JTextField;
import javax.swing.SwingConstants;
import javax.swing.SwingUtilities;

/**
 * Pantalla de Login de ClienteEscritorio con Sullivan de Monsters Inc.
 * 
 * @author GR08 - Arquitectura de Software
 */
public class LoginFrame extends JFrame {

    private final ControladorConversiones controlador;

    private JTextField txtUsuario;
    private JPasswordField txtClave;
    private JLabel lblNotificaciones;
    private JButton btnIniciarSesion;

    public LoginFrame(ControladorConversiones controlador) {
        this.controlador = controlador;
        initComponents();
    }

    private void initComponents() {
        setTitle("Monster Inc - Autenticación SOAP GR08");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setResizable(false);
        setSize(560, 380);
        setLocationRelativeTo(null);

        JPanel panel = new JPanel();
        panel.setLayout(null);
        panel.setBackground(new Color(15, 23, 42)); // Dark slate

        // Título
        JLabel lblTitulo = new JLabel("Conversor de Unidades");
        lblTitulo.setFont(new Font("Segoe UI", Font.BOLD, 18));
        lblTitulo.setForeground(Color.WHITE);
        lblTitulo.setBounds(30, 25, 260, 25);
        panel.add(lblTitulo);

        JLabel lblSub = new JLabel("Ingrese sus credenciales de acceso:");
        lblSub.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        lblSub.setForeground(new Color(148, 163, 184));
        lblSub.setBounds(30, 50, 260, 20);
        panel.add(lblSub);

        // Notificaciones / Error
        lblNotificaciones = new JLabel("");
        lblNotificaciones.setFont(new Font("Segoe UI", Font.BOLD, 11));
        lblNotificaciones.setForeground(new Color(239, 68, 68)); // Red
        lblNotificaciones.setBounds(30, 75, 260, 20);
        panel.add(lblNotificaciones);

        // Usuario
        JLabel lblUser = new JLabel("Usuario:");
        lblUser.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblUser.setForeground(new Color(226, 232, 240));
        lblUser.setBounds(30, 105, 80, 25);
        panel.add(lblUser);

        txtUsuario = new JTextField("monster");
        txtUsuario.setFont(new Font("Segoe UI", Font.PLAIN, 13));
        txtUsuario.setBackground(new Color(30, 41, 59));
        txtUsuario.setForeground(Color.WHITE);
        txtUsuario.setCaretColor(Color.WHITE);
        txtUsuario.setBorder(BorderFactory.createCompoundBorder(
                BorderFactory.createLineBorder(new Color(71, 85, 105)),
                BorderFactory.createEmptyBorder(4, 8, 4, 8)
        ));
        txtUsuario.setBounds(110, 105, 180, 30);
        panel.add(txtUsuario);

        // Clave
        JLabel lblPass = new JLabel("Clave:");
        lblPass.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblPass.setForeground(new Color(226, 232, 240));
        lblPass.setBounds(30, 150, 80, 25);
        panel.add(lblPass);

        txtClave = new JPasswordField("monster9");
        txtClave.setFont(new Font("Segoe UI", Font.PLAIN, 13));
        txtClave.setBackground(new Color(30, 41, 59));
        txtClave.setForeground(Color.WHITE);
        txtClave.setCaretColor(Color.WHITE);
        txtClave.setBorder(BorderFactory.createCompoundBorder(
                BorderFactory.createLineBorder(new Color(71, 85, 105)),
                BorderFactory.createEmptyBorder(4, 8, 4, 8)
        ));
        txtClave.setBounds(110, 150, 180, 30);
        panel.add(txtClave);

        // Botón Iniciar Sesión
        btnIniciarSesion = new JButton("INICIAR SESIÓN");
        btnIniciarSesion.setFont(new Font("Segoe UI", Font.BOLD, 13));
        btnIniciarSesion.setBackground(new Color(79, 70, 229)); // Indigo primary
        btnIniciarSesion.setForeground(Color.WHITE);
        btnIniciarSesion.setFocusPainted(false);
        btnIniciarSesion.setCursor(new Cursor(Cursor.HAND_CURSOR));
        btnIniciarSesion.setBorder(BorderFactory.createEmptyBorder());
        btnIniciarSesion.setBounds(30, 215, 260, 42);
        btnIniciarSesion.addActionListener(this::accionIniciarSesion);
        panel.add(btnIniciarSesion);

        // Imagen de Sullivan (Sulley de Monsters Inc)
        JLabel lblSulley = new JLabel();
        lblSulley.setHorizontalAlignment(SwingConstants.CENTER);
        try {
            java.net.URL imgUrl = getClass().getResource("/ec/edu/monster/clienteescritorio/vista/monsterlogin.jpg");
            if (imgUrl != null) {
                ImageIcon rawIcon = new ImageIcon(imgUrl);
                Image img = rawIcon.getImage().getScaledInstance(210, 280, Image.SCALE_SMOOTH);
                lblSulley.setIcon(new ImageIcon(img));
            }
        } catch (Exception ex) {
            System.err.println("No se pudo cargar la imagen de Sullivan: " + ex.getMessage());
        }
        lblSulley.setBounds(315, 25, 210, 280);
        panel.add(lblSulley);

        getContentPane().add(panel);
    }

    private void accionIniciarSesion(ActionEvent e) {
        String usuario = txtUsuario.getText().trim();
        String clave = new String(txtClave.getPassword()).trim();

        if (usuario.isEmpty() || clave.isEmpty()) {
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
}
