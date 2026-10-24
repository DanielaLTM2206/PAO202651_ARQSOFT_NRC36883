package ec.edu.monster.clienteescritorio.vista;

import ec.edu.monster.clienteescritorio.controlador.ControladorConversiones;
import ec.edu.monster.clientesws.RegistroHistorialDTO;
import ec.edu.monster.clientesws.RespuestaConversion;
import java.awt.Color;
import java.awt.Component;
import java.awt.Font;
import java.awt.Image;
import java.awt.event.ActionEvent;
import java.util.List;
import javax.swing.BorderFactory;
import javax.swing.DefaultComboBoxModel;
import javax.swing.DefaultListCellRenderer;
import javax.swing.ImageIcon;
import javax.swing.JComboBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTable;
import javax.swing.JTextField;
import javax.swing.JPasswordField;
import javax.swing.JOptionPane;
import javax.swing.SwingConstants;
import javax.swing.SwingUtilities;
import javax.swing.table.DefaultTableCellRenderer;
import javax.swing.table.DefaultTableModel;

/**
 * Pantalla de Conversiones con Sullivan de Monsters Inc y Respuesta Estructurada (RF-06).
 * Cuenta con botones ModernButton de alto contraste y soporte multiplataforma completo.
 * 
 * @author GR08 - Arquitectura de Software
 */
public class InterfazConversiones extends JFrame {

    private final ControladorConversiones controlador;

    private JComboBox<String> cbxCategoria;
    private JTextField txtValor;
    private JComboBox<String> cbxOrigen;
    private JComboBox<String> cbxDestino;
    private ModernButton btnConvertir;
    private ModernButton btnCerrarSesion;

    // Tarjeta de Respuesta Estructurada (RF-06 / CU-07)
    private JLabel lblEstadoBadge;
    private JLabel lblCodigoBadge;
    private JLabel lblResultadoNumero;
    private JLabel lblMensajeServidor;

    // Panel de Historial Global Compartido
    private DefaultTableModel modeloHistorial;
    private JTable tablaHistorial;

    public InterfazConversiones(ControladorConversiones controlador) {
        this.controlador = controlador;
        initComponents();
        cargarUnidadesSegunCategoria();
        cargarHistorialEnTabla();
    }

    private void initComponents() {
        setTitle("Monster Inc - Panel de Conversiones SOAP GR08");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setResizable(false);
        setSize(1140, 530);
        setLocationRelativeTo(null);

        JPanel mainPanel = new JPanel();
        mainPanel.setLayout(null);
        mainPanel.setBackground(new Color(11, 19, 37)); // Azul medianoche profundo

        // Encabezado
        JLabel lblTitulo = new JLabel("Sistema de Conversión de Unidades");
        lblTitulo.setFont(new Font("Segoe UI", Font.BOLD, 22));
        lblTitulo.setForeground(Color.WHITE);
        lblTitulo.setBounds(30, 18, 450, 28);
        mainPanel.add(lblTitulo);

        JLabel lblSub = new JLabel("Consumo de Web Service SOAP (JAX-WS / Payara Server) - GR08");
        lblSub.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        lblSub.setForeground(new Color(148, 163, 184));
        lblSub.setBounds(30, 48, 450, 20);
        mainPanel.add(lblSub);

        // 1. Categoría Física
        JLabel lblCat = new JLabel("Categoría Física:");
        lblCat.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblCat.setForeground(new Color(226, 232, 240));
        lblCat.setBounds(30, 80, 190, 20);
        mainPanel.add(lblCat);

        cbxCategoria = new JComboBox<>(new String[]{"Masa", "Longitud", "Temperatura"});
        estilizarComboBox(cbxCategoria);
        cbxCategoria.setBounds(30, 105, 195, 34);
        cbxCategoria.addActionListener(e -> cargarUnidadesSegunCategoria());
        mainPanel.add(cbxCategoria);

        // 2. Valor Numérico
        JLabel lblVal = new JLabel("Valor Numérico:");
        lblVal.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblVal.setForeground(new Color(226, 232, 240));
        lblVal.setBounds(245, 80, 205, 20);
        mainPanel.add(lblVal);

        txtValor = new JTextField("73");
        txtValor.setFont(new Font("Segoe UI", Font.BOLD, 14));
        txtValor.setBackground(new Color(30, 41, 59));
        txtValor.setForeground(Color.WHITE);
        txtValor.setCaretColor(Color.WHITE);
        txtValor.setBorder(BorderFactory.createCompoundBorder(
                BorderFactory.createLineBorder(new Color(71, 85, 105), 1),
                BorderFactory.createEmptyBorder(4, 10, 4, 10)
        ));
        txtValor.setBounds(245, 105, 205, 34);
        mainPanel.add(txtValor);

        // 3. Unidad Origen
        JLabel lblOrig = new JLabel("Unidad Origen:");
        lblOrig.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblOrig.setForeground(new Color(226, 232, 240));
        lblOrig.setBounds(30, 152, 175, 20);
        mainPanel.add(lblOrig);

        cbxOrigen = new JComboBox<>();
        estilizarComboBox(cbxOrigen);
        cbxOrigen.setBounds(30, 177, 175, 34);
        mainPanel.add(cbxOrigen);

        // Flecha indicadora central
        JLabel lblFlecha = new JLabel();
        lblFlecha.setHorizontalAlignment(SwingConstants.CENTER);
        try {
            java.net.URL arrowUrl = getClass().getResource("/ec/edu/monster/clienteescritorio/vista/flecha40px.png");
            if (arrowUrl != null) {
                ImageIcon rawIcon = new ImageIcon(arrowUrl);
                Image img = rawIcon.getImage().getScaledInstance(28, 28, Image.SCALE_SMOOTH);
                lblFlecha.setIcon(new ImageIcon(img));
            } else {
                lblFlecha.setText("→");
                lblFlecha.setFont(new Font("Segoe UI", Font.BOLD, 22));
                lblFlecha.setForeground(new Color(99, 102, 241));
            }
        } catch (Exception ex) {
            lblFlecha.setText("→");
            lblFlecha.setFont(new Font("Segoe UI", Font.BOLD, 22));
            lblFlecha.setForeground(new Color(99, 102, 241));
        }
        lblFlecha.setBounds(212, 177, 32, 34);
        mainPanel.add(lblFlecha);

        // 4. Unidad Destino
        JLabel lblDest = new JLabel("Unidad Destino:");
        lblDest.setFont(new Font("Segoe UI", Font.BOLD, 12));
        lblDest.setForeground(new Color(226, 232, 240));
        lblDest.setBounds(250, 152, 200, 20);
        mainPanel.add(lblDest);

        cbxDestino = new JComboBox<>();
        estilizarComboBox(cbxDestino);
        cbxDestino.setBounds(250, 177, 200, 34);
        mainPanel.add(cbxDestino);

        // 5. Botón Convertir con ModernButton (Verde Esmeralda con texto Blanco Antialiased)
        btnConvertir = new ModernButton("CONVERTIR AHORA", new Color(16, 185, 129), Color.WHITE, 8);
        btnConvertir.setFont(new Font("Segoe UI", Font.BOLD, 14));
        btnConvertir.setBounds(30, 225, 420, 42);
        btnConvertir.addActionListener(this::accionConvertir);
        mainPanel.add(btnConvertir);

        // 6. Tarjeta de Respuesta Estructurada (RF-06 / CU-07)
        JPanel cardResultado = new JPanel();
        cardResultado.setLayout(null);
        cardResultado.setBackground(new Color(24, 33, 51));
        cardResultado.setBorder(BorderFactory.createLineBorder(new Color(51, 65, 85), 1));
        cardResultado.setBounds(30, 280, 420, 175);

        JLabel lblResTitle = new JLabel("RESPUESTA ESTRUCTURADA DEL SERVIDOR (RF-06)");
        lblResTitle.setFont(new Font("Segoe UI", Font.BOLD, 11));
        lblResTitle.setForeground(new Color(148, 163, 184));
        lblResTitle.setBounds(15, 10, 390, 16);
        cardResultado.add(lblResTitle);

        lblEstadoBadge = new JLabel("ESTADO: ESPERANDO");
        lblEstadoBadge.setFont(new Font("Segoe UI", Font.BOLD, 11));
        lblEstadoBadge.setForeground(new Color(148, 163, 184));
        lblEstadoBadge.setBounds(15, 32, 170, 20);
        cardResultado.add(lblEstadoBadge);

        lblCodigoBadge = new JLabel("CÓDIGO: ---");
        lblCodigoBadge.setFont(new Font("Segoe UI", Font.BOLD, 11));
        lblCodigoBadge.setForeground(new Color(148, 163, 184));
        lblCodigoBadge.setBounds(190, 32, 150, 20);
        cardResultado.add(lblCodigoBadge);

        lblResultadoNumero = new JLabel("---");
        lblResultadoNumero.setFont(new Font("Segoe UI", Font.BOLD, 26));
        lblResultadoNumero.setForeground(new Color(52, 211, 153)); // Verde menta brillante
        lblResultadoNumero.setBounds(15, 58, 390, 40);
        cardResultado.add(lblResultadoNumero);

        lblMensajeServidor = new JLabel("Seleccione unidades y presione Convertir.");
        lblMensajeServidor.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        lblMensajeServidor.setForeground(new Color(203, 213, 225));
        lblMensajeServidor.setBounds(15, 105, 390, 56);
        cardResultado.add(lblMensajeServidor);

        mainPanel.add(cardResultado);

        // 7. Imagen de Sullivan de Monsters Inc (Requerimiento institucional GR08)
        JLabel lblSulley = new JLabel();
        lblSulley.setHorizontalAlignment(SwingConstants.CENTER);
        try {
            java.net.URL imgUrl = getClass().getResource("/ec/edu/monster/clienteescritorio/vista/monsterlogin.jpg");
            if (imgUrl != null) {
                ImageIcon rawIcon = new ImageIcon(imgUrl);
                Image img = rawIcon.getImage().getScaledInstance(260, 345, Image.SCALE_SMOOTH);
                lblSulley.setIcon(new ImageIcon(img));
            }
        } catch (Exception ex) {
            System.err.println("No se pudo cargar la imagen de Sullivan: " + ex.getMessage());
        }
        lblSulley.setBounds(465, 45, 235, 315);
        mainPanel.add(lblSulley);

        // 8. Botón Cambiar Contraseña (RF-08 / DS_07 - Sincronización Global)
        ModernButton btnCambiarClave = new ModernButton("🔑 Cambiar Contraseña", new Color(37, 99, 235), Color.WHITE, 8);
        btnCambiarClave.setFont(new Font("Segoe UI", Font.BOLD, 12));
        btnCambiarClave.setBounds(465, 372, 235, 38);
        btnCambiarClave.addActionListener(e -> mostrarDialogoCambiarClave());
        mainPanel.add(btnCambiarClave);

        // 9. Botón Cerrar Sesión con ModernButton (Gris Pizarra elegante con texto Blanco)
        btnCerrarSesion = new ModernButton("Cerrar Sesión", new Color(51, 65, 85), Color.WHITE, 8);
        btnCerrarSesion.setFont(new Font("Segoe UI", Font.BOLD, 12));
        btnCerrarSesion.setBounds(465, 418, 235, 38);
        btnCerrarSesion.addActionListener(e -> {
            controlador.cerrarSesion();
            new LoginFrame(controlador).setVisible(true);
            dispose();
        });
        mainPanel.add(btnCerrarSesion);

        // 9. Panel de Historial Global Compartido (Sincronizado multi-canal)
        JPanel panelHistorial = new JPanel();
        panelHistorial.setLayout(null);
        panelHistorial.setBackground(new Color(24, 33, 51));
        panelHistorial.setBorder(BorderFactory.createCompoundBorder(
                BorderFactory.createLineBorder(new Color(51, 65, 85), 1),
                BorderFactory.createEmptyBorder(0, 0, 0, 0)
        ));
        panelHistorial.setBounds(705, 20, 405, 455);

        JLabel lblHistTitle = new JLabel("HISTORIAL COMPARTIDO");
        lblHistTitle.setFont(new Font("Segoe UI", Font.BOLD, 13));
        lblHistTitle.setForeground(new Color(56, 189, 248)); // Celeste
        lblHistTitle.setBounds(15, 12, 230, 20);
        panelHistorial.add(lblHistTitle);

        JLabel lblHistSub = new JLabel("Web • Móvil • Escritorio • Consola");
        lblHistSub.setFont(new Font("Segoe UI", Font.PLAIN, 10));
        lblHistSub.setForeground(new Color(148, 163, 184));
        lblHistSub.setBounds(15, 32, 230, 16);
        panelHistorial.add(lblHistSub);

        ModernButton btnRefrescar = new ModernButton("Refrescar", new Color(30, 41, 59), new Color(56, 189, 248), 6);
        btnRefrescar.setFont(new Font("Segoe UI", Font.BOLD, 11));
        btnRefrescar.setBounds(295, 12, 98, 30);
        btnRefrescar.addActionListener(e -> cargarHistorialEnTabla());
        panelHistorial.add(btnRefrescar);

        String[] columnas = {"#", "Hora", "Cat", "Conversión"};
        modeloHistorial = new DefaultTableModel(columnas, 0) {
            @Override
            public boolean isCellEditable(int row, int column) {
                return false;
            }
        };

        tablaHistorial = new JTable(modeloHistorial);
        tablaHistorial.setFont(new Font("Segoe UI", Font.PLAIN, 11));
        tablaHistorial.setRowHeight(28);
        tablaHistorial.setBackground(new Color(15, 23, 42));
        tablaHistorial.setForeground(new Color(241, 245, 249));
        tablaHistorial.setGridColor(new Color(30, 41, 59));
        tablaHistorial.setShowGrid(true);
        tablaHistorial.setSelectionBackground(new Color(30, 58, 138));
        tablaHistorial.setSelectionForeground(Color.WHITE);

        tablaHistorial.getTableHeader().setFont(new Font("Segoe UI", Font.BOLD, 11));
        tablaHistorial.getTableHeader().setBackground(new Color(30, 41, 59));
        tablaHistorial.getTableHeader().setForeground(new Color(56, 189, 248));
        tablaHistorial.getTableHeader().setPreferredSize(new java.awt.Dimension(0, 28));

        // Renderizador con estilo moderno, badges por categoría y zebra striping
        DefaultTableCellRenderer cellRenderer = new DefaultTableCellRenderer() {
            @Override
            public Component getTableCellRendererComponent(JTable table, Object value,
                    boolean isSelected, boolean hasFocus, int row, int column) {
                JLabel c = (JLabel) super.getTableCellRendererComponent(table, value, isSelected, hasFocus, row, column);
                c.setBorder(BorderFactory.createEmptyBorder(2, 6, 2, 6));

                if (isSelected) {
                    c.setBackground(new Color(30, 58, 138));
                    c.setForeground(Color.WHITE);
                } else {
                    c.setBackground(row % 2 == 0 ? new Color(15, 23, 42) : new Color(24, 33, 51));
                    c.setForeground(new Color(241, 245, 249));
                }

                String valStr = value != null ? value.toString() : "";
                if (column == 0) {
                    c.setHorizontalAlignment(SwingConstants.CENTER);
                    c.setFont(new Font("Segoe UI", Font.BOLD, 11));
                    if (!isSelected) c.setForeground(new Color(148, 163, 184));
                } else if (column == 1) {
                    c.setHorizontalAlignment(SwingConstants.CENTER);
                    c.setFont(new Font("Segoe UI", Font.PLAIN, 11));
                    if (!isSelected) c.setForeground(new Color(56, 189, 248));
                } else if (column == 2) {
                    c.setHorizontalAlignment(SwingConstants.CENTER);
                    c.setFont(new Font("Segoe UI", Font.BOLD, 10));
                    if ("Masa".equalsIgnoreCase(valStr)) {
                        c.setText("<html><b style='color:#34d399;'>MASA</b></html>");
                    } else if ("Longitud".equalsIgnoreCase(valStr)) {
                        c.setText("<html><b style='color:#60a5fa;'>LONGITUD</b></html>");
                    } else if ("Temperatura".equalsIgnoreCase(valStr)) {
                        c.setText("<html><b style='color:#fbbf24;'>TEMPERATURA</b></html>");
                    }
                } else if (column == 3) {
                    c.setHorizontalAlignment(SwingConstants.LEFT);
                    c.setFont(new Font("Segoe UI", Font.PLAIN, 11));
                }
                return c;
            }
        };

        for (int i = 0; i < 4; i++) {
            tablaHistorial.getColumnModel().getColumn(i).setCellRenderer(cellRenderer);
        }

        tablaHistorial.getColumnModel().getColumn(0).setPreferredWidth(30);
        tablaHistorial.getColumnModel().getColumn(1).setPreferredWidth(52);
        tablaHistorial.getColumnModel().getColumn(2).setPreferredWidth(86);
        tablaHistorial.getColumnModel().getColumn(3).setPreferredWidth(213);

        JScrollPane scrollHist = new JScrollPane(tablaHistorial);
        scrollHist.setBounds(12, 55, 381, 388);
        scrollHist.getViewport().setBackground(new Color(15, 23, 42));
        scrollHist.setBorder(BorderFactory.createLineBorder(new Color(51, 65, 85), 1));
        panelHistorial.add(scrollHist);

        mainPanel.add(panelHistorial);

        getContentPane().add(mainPanel);
    }

    /**
     * Aplica estilo de alto contraste al JComboBox para garantizar que el texto
     * sea siempre 100% visible tanto en estado cerrado como en el menú desplegable.
     */
    private void estilizarComboBox(JComboBox<String> cbx) {
        cbx.setFont(new Font("Segoe UI", Font.BOLD, 13));
        cbx.setBackground(Color.WHITE);
        cbx.setForeground(new Color(15, 23, 42)); // Texto oscuro de máxima legibilidad
        cbx.setOpaque(true);
        cbx.setRenderer(new DefaultListCellRenderer() {
            @Override
            public Component getListCellRendererComponent(JList<?> list, Object value, int index, boolean isSelected, boolean cellHasFocus) {
                JLabel l = (JLabel) super.getListCellRendererComponent(list, value, index, isSelected, cellHasFocus);
                l.setBorder(BorderFactory.createEmptyBorder(6, 10, 6, 10));
                l.setFont(new Font("Segoe UI", isSelected ? Font.BOLD : Font.PLAIN, 13));
                if (isSelected) {
                    l.setBackground(new Color(37, 99, 235)); // Azul real de selección
                    l.setForeground(Color.WHITE);
                } else {
                    l.setBackground(Color.WHITE);
                    l.setForeground(new Color(15, 23, 42)); // Azul oscuro pizarra
                }
                return l;
            }
        });
    }

    private void cargarUnidadesSegunCategoria() {
        String seleccion = (String) cbxCategoria.getSelectedItem();
        String[] unidades;
        if ("Longitud".equalsIgnoreCase(seleccion)) {
            unidades = new String[]{"centimetros", "metros", "pies", "yardas", "millas"};
        } else if ("Masa".equalsIgnoreCase(seleccion)) {
            unidades = new String[]{"kilogramos", "libras", "gramos", "onzas", "quintales"};
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
            lblCodigoBadge.setForeground(new Color(239, 68, 68));
            lblResultadoNumero.setText("Valor Inválido");
            lblResultadoNumero.setForeground(new Color(239, 68, 68));
            lblMensajeServidor.setText("<html>Por favor ingrese un número válido.</html>");
            return;
        }

        btnConvertir.setEnabled(false);
        lblMensajeServidor.setText("Invocando servicio SOAP del GR08...");

        SwingUtilities.invokeLater(() -> {
            try {
                RespuestaConversion res = controlador.convertir(valor, origen, destino);

                if ("OK".equalsIgnoreCase(res.getEstado())) {
                    lblEstadoBadge.setText("ESTADO: OK");
                    lblEstadoBadge.setForeground(new Color(16, 185, 129));
                    lblCodigoBadge.setText("CÓDIGO: " + res.getCodigo());
                    lblCodigoBadge.setForeground(new Color(16, 185, 129));
                    
                    double resultado = res.getResultado() != null ? res.getResultado() : 0.0;
                    lblResultadoNumero.setText(String.format(java.util.Locale.US, "%.4f %s", resultado, destino));
                    lblResultadoNumero.setForeground(new Color(52, 211, 153));
                    lblMensajeServidor.setText("<html>" + res.getMensaje() + " (" + valor + " " + origen + " &rarr; " + destino + ")</html>");
                    
                    // Actualizar historial central inmediatamente
                    cargarHistorialEnTabla();
                } else {
                    lblEstadoBadge.setText("ESTADO: ERROR");
                    lblEstadoBadge.setForeground(new Color(239, 68, 68));
                    lblCodigoBadge.setText("CÓDIGO: " + (res.getCodigo() != null ? res.getCodigo() : "ERR"));
                    lblCodigoBadge.setForeground(new Color(239, 68, 68));
                    lblResultadoNumero.setText("Error");
                    lblResultadoNumero.setForeground(new Color(239, 68, 68));
                    lblMensajeServidor.setText("<html>" + (res.getMensaje() != null ? res.getMensaje() : "Error en conversión") + "</html>");
                }
            } catch (Exception ex) {
                lblEstadoBadge.setText("ESTADO: ERROR");
                lblEstadoBadge.setForeground(new Color(239, 68, 68));
                lblCodigoBadge.setText("CÓDIGO: EXC-001");
                lblCodigoBadge.setForeground(new Color(239, 68, 68));
                lblResultadoNumero.setText("Error");
                lblResultadoNumero.setForeground(new Color(239, 68, 68));
                lblMensajeServidor.setText("<html>" + ex.getMessage() + "</html>");
            } finally {
                btnConvertir.setEnabled(true);
            }
        });
    }

    /**
     * Carga el historial global de conversiones de manera asíncrona para no congelar la UI Swing.
     */
    public void cargarHistorialEnTabla() {
        new Thread(() -> {
            List<RegistroHistorialDTO> items = controlador.obtenerHistorial();
            SwingUtilities.invokeLater(() -> {
                if (modeloHistorial == null) return;
                modeloHistorial.setRowCount(0);
                if (items == null || items.isEmpty()) {
                    modeloHistorial.addRow(new Object[]{"-", "--:--", "-", "Sin conversiones registradas"});
                    return;
                }
                for (RegistroHistorialDTO r : items) {
                    String hora = r.getFechaHora() != null && r.getFechaHora().length() >= 16 
                            ? r.getFechaHora().substring(11, 16) 
                            : r.getFechaHora();
                    String conv = String.format(java.util.Locale.US, 
                            "<html><span style='color:#cbd5e1;'>%.1f %s</span> <span style='color:#38bdf8; font-weight:bold;'>&rarr;</span> <span style='color:#4ade80; font-weight:bold;'>%.2f %s</span></html>", 
                            r.getValorOriginal(), r.getUnidadOrigen(), r.getResultado(), r.getUnidadDestino());
                    modeloHistorial.addRow(new Object[]{r.getId(), hora, r.getCategoria(), conv});
                }
            });
        }).start();
    }

    private void mostrarDialogoCambiarClave() {
        JPasswordField txtActual = new JPasswordField();
        JPasswordField txtNueva = new JPasswordField();
        JPasswordField txtConfirmar = new JPasswordField();

        JPanel panel = new JPanel(new java.awt.GridLayout(0, 1, 5, 5));
        panel.add(new JLabel("Contraseña Actual:"));
        panel.add(txtActual);
        panel.add(new JLabel("Nueva Contraseña:"));
        panel.add(txtNueva);
        panel.add(new JLabel("Confirmar Nueva Contraseña:"));
        panel.add(txtConfirmar);

        int result = JOptionPane.showConfirmDialog(this, panel,
                "Cambiar Contraseña - Sincronización Global",
                JOptionPane.OK_CANCEL_OPTION, JOptionPane.PLAIN_MESSAGE);

        if (result == JOptionPane.OK_OPTION) {
            String actual = new String(txtActual.getPassword()).trim();
            String nueva = new String(txtNueva.getPassword()).trim();
            String confirmar = new String(txtConfirmar.getPassword()).trim();

            if (actual.isEmpty() || nueva.isEmpty()) {
                JOptionPane.showMessageDialog(this, "Debe ingresar tanto la contraseña actual como la nueva.", "Campos Requeridos", JOptionPane.WARNING_MESSAGE);
                return;
            }
            if (!nueva.equals(confirmar)) {
                JOptionPane.showMessageDialog(this, "La nueva contraseña y la confirmación no coinciden.", "Error de Validación", JOptionPane.ERROR_MESSAGE);
                return;
            }

            var resp = controlador.cambiarContrasenia(actual, nueva);
            if ("OK".equalsIgnoreCase(resp.getEstado())) {
                JOptionPane.showMessageDialog(this,
                        resp.getMensaje() + "\n\nSincronizado en memoria para todos los canales (Web, Móvil, Escritorio, Consola).",
                        "Contraseña Actualizada", JOptionPane.INFORMATION_MESSAGE);
            } else {
                JOptionPane.showMessageDialog(this,
                        "[" + resp.getCodigo() + "] " + resp.getMensaje(),
                        "Error al Cambiar Contraseña", JOptionPane.ERROR_MESSAGE);
            }
        }
    }

    public static void main(String args[]) {
        try {
            javax.swing.UIManager.setLookAndFeel(javax.swing.UIManager.getSystemLookAndFeelClassName());
        } catch (Exception ignored) {
        }

        SwingUtilities.invokeLater(() -> {
            ControladorConversiones ctrl = new ControladorConversiones();
            new InterfazConversiones(ctrl).setVisible(true);
        });
    }
}
