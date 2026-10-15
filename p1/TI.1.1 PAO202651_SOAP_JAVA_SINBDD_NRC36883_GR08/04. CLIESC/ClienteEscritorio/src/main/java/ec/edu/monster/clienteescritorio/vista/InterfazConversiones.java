package ec.edu.monster.clienteescritorio.vista;

import ec.edu.monster.clientesws.CategoriaDTO;
import ec.edu.monster.clientesws.RespuestaConversion;
import ec.edu.monster.clienteescritorio.controlador.ControladorConversiones;
import java.awt.Color;
import java.awt.Cursor;
import java.awt.Font;
import java.awt.Image;
import java.awt.event.ActionEvent;
import java.util.Arrays;
import java.util.List;
import javax.swing.BorderFactory;
import javax.swing.DefaultComboBoxModel;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.SwingConstants;
import javax.swing.SwingUtilities;

/**
 * Pantalla de Conversiones con Sullivan de Monsters Inc y Respuesta Estructurada.
 * 
 * @author GR08 - Arquitectura de Software
 */
public class InterfazConversiones extends JFrame {

    private final ControladorConversiones controlador;

    private JComboBox<String> cbxCategoria;
    private JComboBox<String> cbxOrigen;
    private JComboBox<String> cbxDestino;
    private JTextField txtValor;
    private JLabel lblResultadoNumero;
    private JLabel lblEstadoBadge;
    private JLabel lblCodigoBadge;
    private JLabel lblMensajeServidor;
    private JButton btnConvertir;

    public InterfazConversiones(ControladorConversiones controlador) {
        this.controlador = controlador;
        initComponents();
        cargarUnidadesSegunCategoria();
    }

    private void initComponents() {
        setTitle("Monster Inc - Panel de Conversiones SOAP GR08");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setResizable(false);
        setSize(780, 520);
        setLocationRelativeTo(null);

        JPanel mainPanel = new JPanel();
        mainPanel.setLayout(null);
        mainPanel.setBackground(new Color(15, 23, 42)); // Dark Slate

        // Encabezado
        JLabel lblTitulo = new JLabel("Sistema de Conversión de Unidades");
        lblTitulo.setFont(new Font("Segoe UI", Font.BOLD, 20));
        lblTitulo.setForeground(Color.WHITE);
        lblTitulo.setBounds(30, 20, 450, 28);
        mainPanel.add(lblTitulo);

        JLabel lblSub = new JLabel("Consumo de Web Service SOAP (JAX-WS / Payara Server) - GR08");
        lblSub.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        lblSub.setForeground(new Color(148, 163, 184));
        lblSub.setBounds(30, 48, 450, 18);
        mainPanel.add(lblSub);

        // 1. Selector de Categoría
        JLabel lblCat = new JLabel("Categoría Física:");
        lblCat.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblCat.setForeground(new Color(226, 232, 240));
        lblCat.setBounds(30, 85, 120, 20);
        mainPanel.add(lblCat);

        cbxCategoria = new JComboBox<>(new String[]{"Longitud", "Masa", "Temperatura"});
        cbxCategoria.setFont(new Font("Segoe UI", Font.BOLD, 13));
        cbxCategoria.setBackground(new Color(30, 41, 59));
        cbxCategoria.setForeground(Color.WHITE);
        cbxCategoria.setBounds(30, 110, 220, 32);
        cbxCategoria.addActionListener(e -> cargarUnidadesSegunCategoria());
        mainPanel.add(cbxCategoria);

        // 2. Valor a convertir
        JLabel lblVal = new JLabel("Valor Numérico:");
        lblVal.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblVal.setForeground(new Color(226, 232, 240));
        lblVal.setBounds(270, 85, 120, 20);
        mainPanel.add(lblVal);

        txtValor = new JTextField("100.0");
        txtValor.setFont(new Font("Segoe UI", Font.BOLD, 14));
        txtValor.setBackground(new Color(30, 41, 59));
        txtValor.setForeground(Color.WHITE);
        txtValor.setCaretColor(Color.WHITE);
        txtValor.setBorder(BorderFactory.createCompoundBorder(
                BorderFactory.createLineBorder(new Color(71, 85, 105)),
                BorderFactory.createEmptyBorder(4, 8, 4, 8)
        ));
        txtValor.setBounds(270, 110, 210, 32);
        mainPanel.add(txtValor);

        // 3. Unidad Origen
        JLabel lblOrig = new JLabel("Unidad Origen:");
        lblOrig.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblOrig.setForeground(new Color(226, 232, 240));
        lblOrig.setBounds(30, 160, 120, 20);
        mainPanel.add(lblOrig);

        cbxOrigen = new JComboBox<>();
        cbxOrigen.setFont(new Font("Segoe UI", Font.PLAIN, 13));
        cbxOrigen.setBackground(new Color(30, 41, 59));
        cbxOrigen.setForeground(Color.WHITE);
        cbxOrigen.setBounds(30, 185, 200, 32);
        mainPanel.add(cbxOrigen);

        // Flecha indicadora
        JLabel lblFlecha = new JLabel("➜");
        lblFlecha.setFont(new Font("Segoe UI", Font.BOLD, 22));
        lblFlecha.setForeground(new Color(99, 102, 241));
        lblFlecha.setHorizontalAlignment(SwingConstants.CENTER);
        lblFlecha.setBounds(235, 185, 40, 32);
        mainPanel.add(lblFlecha);

        // 4. Unidad Destino
        JLabel lblDest = new JLabel("Unidad Destino:");
        lblDest.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblDest.setForeground(new Color(226, 232, 240));
        lblDest.setBounds(280, 160, 120, 20);
        mainPanel.add(lblDest);

        cbxDestino = new JComboBox<>();
        cbxDestino.setFont(new Font("Segoe UI", Font.PLAIN, 13));
        cbxDestino.setBackground(new Color(30, 41, 59));
        cbxDestino.setForeground(Color.WHITE);
        cbxDestino.setBounds(280, 185, 200, 32);
        mainPanel.add(cbxDestino);

        // 5. Botón Convertir
        btnConvertir = new JButton("CONVERTIR AHORA");
        btnConvertir.setFont(new Font("Segoe UI", Font.BOLD, 14));
        btnConvertir.setBackground(new Color(16, 185, 129)); // Emerald Green
        btnConvertir.setForeground(Color.WHITE);
        btnConvertir.setFocusPainted(false);
        btnConvertir.setCursor(new Cursor(Cursor.HAND_CURSOR));
        btnConvertir.setBorder(BorderFactory.createEmptyBorder());
        btnConvertir.setBounds(30, 235, 450, 42);
        btnConvertir.addActionListener(this::accionConvertir);
        mainPanel.add(btnConvertir);

        // 6. Tarjeta de Respuesta Estructurada (RF-06 / CU-07)
        JPanel cardResultado = new JPanel();
        cardResultado.setLayout(null);
        cardResultado.setBackground(new Color(24, 33, 51));
        cardResultado.setBorder(BorderFactory.createLineBorder(new Color(51, 65, 85), 1));
        cardResultado.setBounds(30, 295, 450, 160);

        JLabel lblResTitle = new JLabel("RESPUESTA ESTRUCTURADA DEL SERVIDOR (RF-06)");
        lblResTitle.setFont(new Font("Segoe UI", Font.BOLD, 11));
        lblResTitle.setForeground(new Color(148, 163, 184));
        lblResTitle.setBounds(15, 10, 350, 16);
        cardResultado.add(lblResTitle);

        lblEstadoBadge = new JLabel("ESTADO: ESPERANDO");
        lblEstadoBadge.setFont(new Font("Segoe UI", Font.BOLD, 11));
        lblEstadoBadge.setForeground(new Color(148, 163, 184));
        lblEstadoBadge.setBounds(15, 32, 160, 20);
        cardResultado.add(lblEstadoBadge);

        lblCodigoBadge = new JLabel("CÓDIGO: ---");
        lblCodigoBadge.setFont(new Font("Segoe UI", Font.BOLD, 11));
        lblCodigoBadge.setForeground(new Color(148, 163, 184));
        lblCodigoBadge.setBounds(180, 32, 150, 20);
        cardResultado.add(lblCodigoBadge);

        lblResultadoNumero = new JLabel("---");
        lblResultadoNumero.setFont(new Font("Segoe UI", Font.BOLD, 26));
        lblResultadoNumero.setForeground(new Color(52, 211, 153)); // Green text
        lblResultadoNumero.setBounds(15, 58, 420, 38);
        cardResultado.add(lblResultadoNumero);

        lblMensajeServidor = new JLabel("Seleccione unidades y presione Convertir.");
        lblMensajeServidor.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        lblMensajeServidor.setForeground(new Color(203, 213, 225));
        lblMensajeServidor.setBounds(15, 105, 420, 42);
        cardResultado.add(lblMensajeServidor);

        mainPanel.add(cardResultado);

        // 7. Imagen de Sullivan de Monsters Inc en esta pantalla (Requerimiento del usuario)
        JLabel lblSulley = new JLabel();
        lblSulley.setHorizontalAlignment(SwingConstants.CENTER);
        try {
            java.net.URL imgUrl = getClass().getResource("/ec/edu/monster/clienteescritorio/vista/monsterlogin.jpg");
            if (imgUrl != null) {
                ImageIcon rawIcon = new ImageIcon(imgUrl);
                Image img = rawIcon.getImage().getScaledInstance(230, 330, Image.SCALE_SMOOTH);
                lblSulley.setIcon(new ImageIcon(img));
            }
        } catch (Exception ex) {
            System.err.println("No se pudo cargar la imagen de Sullivan: " + ex.getMessage());
        }
        lblSulley.setBounds(505, 75, 230, 330);
        mainPanel.add(lblSulley);

        // Botón Cerrar Sesión
        JButton btnCerrarSesion = new JButton("Cerrar Sesión");
        btnCerrarSesion.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        btnCerrarSesion.setBackground(new Color(51, 65, 85));
        btnCerrarSesion.setForeground(new Color(226, 232, 240));
        btnCerrarSesion.setFocusPainted(false);
        btnCerrarSesion.setCursor(new Cursor(Cursor.HAND_CURSOR));
        btnCerrarSesion.setBounds(505, 418, 230, 36);
        btnCerrarSesion.addActionListener(e -> {
            controlador.cerrarSesion();
            new LoginFrame(controlador).setVisible(true);
            dispose();
        });
        mainPanel.add(btnCerrarSesion);

        getContentPane().add(mainPanel);
    }

    private void cargarUnidadesSegunCategoria() {
        String seleccion = (String) cbxCategoria.getSelectedItem();
        String[] unidades;
        if ("Longitud".equalsIgnoreCase(seleccion)) {
            unidades = new String[]{"centimetros", "metros", "pies", "yardas", "millas"};
        } else if ("Masa".equalsIgnoreCase(seleccion)) {
            unidades = new String[]{"gramos", "kilogramos", "onzas", "libras", "quintales"};
        } else {
            unidades = new String[]{"celsius", "fahrenheit", "kelvin", "newton", "reaumur"};
        }
        cbxOrigen.setModel(new DefaultComboBoxModel<>(unidades));
        cbxDestino.setModel(new DefaultComboBoxModel<>(unidades));
        if (unidades.length > 1) {
            cbxDestino.setSelectedIndex(1);
        }
    }

    private void accionConvertir(ActionEvent e) {
        String valStr = txtValor.getText().trim();
        String origen = (String) cbxOrigen.getSelectedItem();
        String destino = (String) cbxDestino.getSelectedItem();

        double valor;
        try {
            valor = Double.parseDouble(valStr);
        } catch (NumberFormatException ex) {
            lblEstadoBadge.setText("ESTADO: ERROR");
            lblEstadoBadge.setForeground(new Color(239, 68, 68));
            lblCodigoBadge.setText("CÓDIGO: PARAM-001");
            lblResultadoNumero.setText("Valor Inválido");
            lblResultadoNumero.setForeground(new Color(239, 68, 68));
            lblMensajeServidor.setText("Por favor ingrese un número válido.");
            return;
        }

        btnConvertir.setEnabled(false);
        lblMensajeServidor.setText("Invocando servicio SOAP del GR08...");

        SwingUtilities.invokeLater(() -> {
            RespuestaConversion res = controlador.convertir(valor, origen, destino);

            if ("OK".equalsIgnoreCase(res.getEstado())) {
                lblEstadoBadge.setText("ESTADO: OK");
                lblEstadoBadge.setForeground(new Color(16, 185, 129));
                lblCodigoBadge.setText("CÓDIGO: " + res.getCodigo());
                lblResultadoNumero.setText(String.format("%.4f %s", res.getResultado(), destino));
                lblResultadoNumero.setForeground(new Color(52, 211, 153));
                lblMensajeServidor.setText("<html>" + res.getMensaje() + " (" + valor + " " + origen + " ➜ " + destino + ")</html>");
            } else {
                lblEstadoBadge.setText("ESTADO: ERROR");
                lblEstadoBadge.setForeground(new Color(239, 68, 68));
                lblCodigoBadge.setText("CÓDIGO: " + (res.getCodigo() != null ? res.getCodigo() : "ERR"));
                lblResultadoNumero.setText("Error");
                lblResultadoNumero.setForeground(new Color(239, 68, 68));
                lblMensajeServidor.setText("<html>" + (res.getMensaje() != null ? res.getMensaje() : "Error en conversión") + "</html>");
            }
            btnConvertir.setEnabled(true);
        });
    }
}
