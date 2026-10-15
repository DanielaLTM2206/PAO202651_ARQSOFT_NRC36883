package ec.edu.monster.clienteescritorio.vista;

import java.awt.Color;
import java.awt.Cursor;
import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.RenderingHints;
import javax.swing.JButton;

/**
 * Botón con estilo moderno, esquinas redondeadas y soporte robusto
 * para cualquier Look and Feel de Swing (incluyendo Windows L&F).
 * Evita el problema nativo de WindowsButtonUI donde se ignora el
 * color de fondo y el texto queda blanco sobre blanco.
 * 
 * @author GR08 - Arquitectura de Software
 */
public class ModernButton extends JButton {

    private Color normalColor;
    private Color hoverColor;
    private Color pressedColor;
    private int cornerRadius;

    public ModernButton(String text, Color bg, Color fg) {
        this(text, bg, fg, 8);
    }

    public ModernButton(String text, Color bg, Color fg, int cornerRadius) {
        super(text);
        this.normalColor = bg;
        this.hoverColor = calcularHover(bg);
        this.pressedColor = calcularPressed(bg);
        this.cornerRadius = cornerRadius;

        setForeground(fg);
        setOpaque(false);
        setContentAreaFilled(false);
        setBorderPainted(false);
        setFocusPainted(false);
        setCursor(new Cursor(Cursor.HAND_CURSOR));
    }

    private Color calcularHover(Color c) {
        return new Color(
            Math.min(255, c.getRed() + 28),
            Math.min(255, c.getGreen() + 28),
            Math.min(255, c.getBlue() + 28)
        );
    }

    private Color calcularPressed(Color c) {
        return new Color(
            Math.max(0, c.getRed() - 35),
            Math.max(0, c.getGreen() - 35),
            Math.max(0, c.getBlue() - 35)
        );
    }

    public void setButtonColors(Color bg, Color fg) {
        this.normalColor = bg;
        this.hoverColor = calcularHover(bg);
        this.pressedColor = calcularPressed(bg);
        setForeground(fg);
        repaint();
    }

    @Override
    protected void paintComponent(Graphics g) {
        Graphics2D g2 = (Graphics2D) g.create();
        g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
        g2.setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING, RenderingHints.VALUE_TEXT_ANTIALIAS_ON);

        if (!isEnabled()) {
            g2.setColor(new Color(100, 116, 139));
        } else if (getModel().isPressed()) {
            g2.setColor(pressedColor);
        } else if (getModel().isRollover()) {
            g2.setColor(hoverColor);
        } else {
            g2.setColor(normalColor);
        }

        g2.fillRoundRect(0, 0, getWidth(), getHeight(), cornerRadius, cornerRadius);

        // Texto centrado de alto contraste
        g2.setColor(isEnabled() ? getForeground() : new Color(203, 213, 225));
        g2.setFont(getFont());
        FontMetrics fm = g2.getFontMetrics();
        int x = (getWidth() - fm.stringWidth(getText())) / 2;
        int y = (getHeight() + fm.getAscent() - fm.getDescent()) / 2;
        g2.drawString(getText(), x, y);

        g2.dispose();
    }
}
