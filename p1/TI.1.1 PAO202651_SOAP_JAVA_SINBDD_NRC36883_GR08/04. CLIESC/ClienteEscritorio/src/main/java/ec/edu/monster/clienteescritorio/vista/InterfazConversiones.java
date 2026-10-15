package ec.edu.monster.clienteescritorio.vista;

import ec.edu.monster.clienteescritorio.controlador.ControladorConversiones;
import ec.edu.monster.clienteescritorio.modelo.CatalogoUnidades;
import java.util.Arrays;
import javax.swing.DefaultComboBoxModel;

/**
 * Pantalla de Conversiones con Sullivan de Monsters Inc (GR08).
 * Compatible con NetBeans GUI Builder (.form).
 * 
 * @author GR08 - Arquitectura de Software
 */
public class InterfazConversiones extends javax.swing.JFrame {
    
    private final ControladorConversiones controlador;
    private static final java.util.logging.Logger logger = java.util.logging.Logger.getLogger(InterfazConversiones.class.getName());

    public InterfazConversiones(ControladorConversiones controladorAuth) {
        initComponents();
        this.controlador = controladorAuth;
        setLocationRelativeTo(null);
        actualizarCombosUnidades();
    }

    private DefaultComboBoxModel<String> crearModeloString(Enum<?>[] valores) {
        String[] nombres = Arrays.stream(valores)
                                 .map(Enum::name)
                                 .toArray(String[]::new);
        return new DefaultComboBoxModel<>(nombres);
    }
    
    private void actualizarCombosUnidades() {
        String tipoSeleccionado = cbxTipoConversion.getSelectedItem().toString();

        switch (tipoSeleccionado) {
            case "Longitud" -> {
                cbxOrigen.setModel(crearModeloString(CatalogoUnidades.Longitud.values()));
                cbxDestino.setModel(crearModeloString(CatalogoUnidades.Longitud.values()));
            }
            case "Masa" -> {
                cbxOrigen.setModel(crearModeloString(CatalogoUnidades.Masa.values()));
                cbxDestino.setModel(crearModeloString(CatalogoUnidades.Masa.values()));
            }
            case "Temperatura" -> {
                cbxOrigen.setModel(crearModeloString(CatalogoUnidades.Temperatura.values()));
                cbxDestino.setModel(crearModeloString(CatalogoUnidades.Temperatura.values()));
            }
        }
    }
    
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jPanel1 = new javax.swing.JPanel();
        cbxOrigen = new javax.swing.JComboBox<>();
        cbxDestino = new javax.swing.JComboBox<>();
        txtTitulo = new javax.swing.JLabel();
        txtValor = new javax.swing.JTextField();
        txtResultado = new javax.swing.JTextField();
        btnConvertir = new javax.swing.JButton();
        cbxTipoConversion = new javax.swing.JComboBox<>();
        lblOrigen = new javax.swing.JLabel();
        lblFlecha = new javax.swing.JLabel();
        lblDestino = new javax.swing.JLabel();
        icono = new javax.swing.JLabel();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        setTitle("Monster Inc - Panel de Conversiones GR08");
        setBackground(new java.awt.Color(13, 13, 13));

        jPanel1.setBackground(new java.awt.Color(13, 13, 13));

        cbxOrigen.setBackground(new java.awt.Color(13, 13, 13));
        cbxOrigen.setFont(new java.awt.Font("SansSerif", 1, 12)); // NOI18N
        cbxOrigen.setForeground(new java.awt.Color(242, 242, 242));
        cbxOrigen.setModel(new javax.swing.DefaultComboBoxModel<>(new String[] { "Item 1", "Item 2", "Item 3", "Item 4" }));

        cbxDestino.setBackground(new java.awt.Color(13, 13, 13));
        cbxDestino.setFont(new java.awt.Font("SansSerif", 1, 12)); // NOI18N
        cbxDestino.setForeground(new java.awt.Color(242, 242, 242));
        cbxDestino.setModel(new javax.swing.DefaultComboBoxModel<>(new String[] { "Item 1", "Item 2", "Item 3", "Item 4" }));

        txtTitulo.setFont(new java.awt.Font("SansSerif", 1, 16)); // NOI18N
        txtTitulo.setForeground(new java.awt.Color(242, 242, 242));
        txtTitulo.setText("Elija el tipo de unidad para la conversión");

        txtValor.setFont(new java.awt.Font("SansSerif", 0, 14)); // NOI18N
        txtValor.setHorizontalAlignment(javax.swing.JTextField.TRAILING);

        txtResultado.setEditable(false);
        txtResultado.setBackground(new java.awt.Color(255, 255, 255));
        txtResultado.setFont(new java.awt.Font("SansSerif", 0, 14)); // NOI18N
        txtResultado.setDisabledTextColor(new java.awt.Color(0, 0, 0));
        txtResultado.setEnabled(false);

        btnConvertir.setBackground(new java.awt.Color(82, 122, 242));
        btnConvertir.setFont(new java.awt.Font("SansSerif", 1, 14)); // NOI18N
        btnConvertir.setForeground(new java.awt.Color(242, 242, 242));
        btnConvertir.setText("CONVERTIR");
        btnConvertir.setBorder(new javax.swing.border.SoftBevelBorder(javax.swing.border.BevelBorder.RAISED));
        btnConvertir.addActionListener(this::btnConvertirActionPerformed);

        cbxTipoConversion.setBackground(new java.awt.Color(13, 13, 13));
        cbxTipoConversion.setFont(new java.awt.Font("SansSerif", 1, 14)); // NOI18N
        cbxTipoConversion.setForeground(new java.awt.Color(242, 242, 242));
        cbxTipoConversion.setModel(new javax.swing.DefaultComboBoxModel<>(new String[] { "Longitud", "Masa", "Temperatura" }));
        cbxTipoConversion.addActionListener(this::cbxTipoConversionActionPerformed);

        lblOrigen.setFont(new java.awt.Font("SansSerif", 1, 14)); // NOI18N
        lblOrigen.setForeground(new java.awt.Color(242, 242, 242));
        lblOrigen.setText("Unidad de origen");

        lblFlecha.setIcon(new javax.swing.ImageIcon(getClass().getResource("/ec/edu/monster/clienteescritorio/vista/flecha40px.png"))); // NOI18N

        lblDestino.setFont(new java.awt.Font("SansSerif", 1, 14)); // NOI18N
        lblDestino.setForeground(new java.awt.Color(242, 242, 242));
        lblDestino.setText("Unidad de destino");

        icono.setHorizontalAlignment(javax.swing.SwingConstants.CENTER);
        icono.setIcon(new javax.swing.ImageIcon(getClass().getResource("/ec/edu/monster/clienteescritorio/vista/monsterlogin.jpg"))); // NOI18N

        javax.swing.GroupLayout jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
        jPanel1.setLayout(jPanel1Layout);
        jPanel1Layout.setHorizontalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addGap(24, 24, 24)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                    .addComponent(btnConvertir, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                            .addComponent(cbxOrigen, 0, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                            .addComponent(lblOrigen, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                            .addComponent(txtValor, javax.swing.GroupLayout.PREFERRED_SIZE, 138, javax.swing.GroupLayout.PREFERRED_SIZE))
                        .addGap(18, 18, 18)
                        .addComponent(lblFlecha)
                        .addGap(18, 18, 18)
                        .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                            .addComponent(lblDestino)
                            .addComponent(txtResultado)
                            .addComponent(cbxDestino, javax.swing.GroupLayout.PREFERRED_SIZE, 138, javax.swing.GroupLayout.PREFERRED_SIZE)))
                    .addComponent(txtTitulo)
                    .addComponent(cbxTipoConversion, javax.swing.GroupLayout.PREFERRED_SIZE, 256, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(18, 18, 18)
                .addComponent(icono, javax.swing.GroupLayout.PREFERRED_SIZE, 226, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addContainerGap(24, Short.MAX_VALUE))
        );
        jPanel1Layout.setVerticalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addGap(16, 16, 16)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                    .addComponent(icono, javax.swing.GroupLayout.PREFERRED_SIZE, 0, Short.MAX_VALUE)
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addComponent(txtTitulo)
                        .addGap(18, 18, 18)
                        .addComponent(cbxTipoConversion, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGap(53, 53, 53)
                        .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addGroup(jPanel1Layout.createSequentialGroup()
                                .addComponent(lblOrigen)
                                .addGap(18, 18, 18)
                                .addComponent(cbxOrigen, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                                .addGap(18, 18, 18)
                                .addComponent(txtValor, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                            .addGroup(jPanel1Layout.createSequentialGroup()
                                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING)
                                    .addGroup(jPanel1Layout.createSequentialGroup()
                                        .addComponent(lblDestino)
                                        .addGap(18, 18, 18)
                                        .addComponent(cbxDestino, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                                    .addComponent(lblFlecha, javax.swing.GroupLayout.PREFERRED_SIZE, 46, javax.swing.GroupLayout.PREFERRED_SIZE))
                                .addGap(18, 18, 18)
                                .addComponent(txtResultado, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)))
                        .addGap(26, 26, 26)
                        .addComponent(btnConvertir, javax.swing.GroupLayout.PREFERRED_SIZE, 44, javax.swing.GroupLayout.PREFERRED_SIZE)))
                .addContainerGap(30, Short.MAX_VALUE))
        );

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jPanel1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jPanel1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void btnConvertirActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnConvertirActionPerformed
        try {
            double valor = Double.parseDouble(txtValor.getText());
            String origen = cbxOrigen.getSelectedItem().toString().toLowerCase();
            String destino = cbxDestino.getSelectedItem().toString().toLowerCase();
            String tipo = cbxTipoConversion.getSelectedItem().toString();
            
            double resultado = 0;

            switch (tipo) {
                case "Longitud" -> resultado = controlador.convertirLongitud(valor, origen, destino);
                case "Masa" -> resultado = controlador.convertirMasa(valor, origen, destino);
                case "Temperatura" -> resultado = controlador.convertirTemperatura(valor, origen, destino);
            }
            
            txtResultado.setText(String.format(java.util.Locale.US, "%.4f", resultado));
            
        } catch (NumberFormatException e) {
            javax.swing.JOptionPane.showMessageDialog(this, "Ingresa un número válido.", "Error", javax.swing.JOptionPane.ERROR_MESSAGE);
        } catch (Exception e) {
            javax.swing.JOptionPane.showMessageDialog(this, "Error: " + e.getMessage(), "Error", javax.swing.JOptionPane.ERROR_MESSAGE);
        }
    }//GEN-LAST:event_btnConvertirActionPerformed

    private void cbxTipoConversionActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_cbxTipoConversionActionPerformed
        actualizarCombosUnidades();
    }//GEN-LAST:event_cbxTipoConversionActionPerformed

    public static void main(String args[]) {
        try {
            for (javax.swing.UIManager.LookAndFeelInfo info : javax.swing.UIManager.getInstalledLookAndFeels()) {
                if ("Nimbus".equals(info.getName())) {
                    javax.swing.UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (ReflectiveOperationException | javax.swing.UnsupportedLookAndFeelException ex) {
            logger.log(java.util.logging.Level.SEVERE, null, ex);
        }

        java.awt.EventQueue.invokeLater(() -> new InterfazConversiones(new ControladorConversiones()).setVisible(true));
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton btnConvertir;
    private javax.swing.JComboBox<String> cbxDestino;
    private javax.swing.JComboBox<String> cbxOrigen;
    private javax.swing.JComboBox<String> cbxTipoConversion;
    private javax.swing.JLabel icono;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JLabel lblDestino;
    private javax.swing.JLabel lblFlecha;
    private javax.swing.JLabel lblOrigen;
    private javax.swing.JTextField txtResultado;
    private javax.swing.JLabel txtTitulo;
    private javax.swing.JTextField txtValor;
    // End of variables declaration//GEN-END:variables
}
