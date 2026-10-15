<%@page contentType="text/html" pageEncoding="UTF-8"%>
<%
    String token = (String) session.getAttribute("token");
    String usuario = (String) session.getAttribute("usuario");
    if (token == null) {
        response.sendRedirect("index.jsp");
        return;
    }
%>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Panel de Conversión SOAP - Monsters Inc GR08</title>
    <style>
        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: radial-gradient(circle at top, #1e293b, #0f172a 80%);
            color: #f8fafc;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            padding: 25px 15px;
        }
        .main-container {
            background-color: #151b28;
            border: 1px solid #334155;
            border-radius: 12px;
            box-shadow: 0 20px 40px rgba(0, 0, 0, 0.6);
            max-width: 860px;
            width: 100%;
            overflow: hidden;
            display: flex;
            flex-direction: row;
        }
        .converter-section {
            flex: 1;
            padding: 35px 30px;
        }
        .side-profile {
            width: 250px;
            background-color: #0d121c;
            border-left: 1px solid #1e293b;
            padding: 30px 20px;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: space-between;
        }
        .sulley-card {
            text-align: center;
            width: 100%;
        }
        .sulley-card img {
            width: 100%;
            max-width: 200px;
            border-radius: 10px;
            border: 2px solid #3b82f6;
            box-shadow: 0 10px 20px rgba(0,0,0,0.5);
            margin-bottom: 15px;
        }
        .user-info {
            background-color: #1e293b;
            border: 1px solid #334155;
            border-radius: 8px;
            padding: 10px 14px;
            width: 100%;
            font-size: 0.85rem;
            color: #cbd5e1;
            margin-bottom: 15px;
        }
        .user-info strong {
            color: #38bdf8;
        }
        .btn-logout {
            display: block;
            width: 100%;
            text-align: center;
            background-color: #334155;
            color: #f8fafc;
            text-decoration: none;
            padding: 10px;
            border-radius: 6px;
            font-weight: 600;
            font-size: 0.85rem;
            transition: background-color 0.2s;
        }
        .btn-logout:hover {
            background-color: #ef4444;
        }
        h2 {
            font-size: 1.5rem;
            font-weight: 700;
            color: #ffffff;
            margin-bottom: 5px;
        }
        p.subtitle {
            font-size: 0.85rem;
            color: #94a3b8;
            margin-bottom: 25px;
        }
        .form-row {
            margin-bottom: 20px;
        }
        label {
            display: block;
            font-size: 0.85rem;
            font-weight: 600;
            color: #cbd5e1;
            margin-bottom: 6px;
        }
        select, input[type="number"] {
            width: 100%;
            background-color: #1e293b;
            border: 1px solid #475569;
            border-radius: 6px;
            color: #ffffff;
            padding: 10px 12px;
            font-size: 0.95rem;
            outline: none;
            transition: border-color 0.2s;
        }
        select:focus, input[type="number"]:focus {
            border-color: #3b82f6;
        }
        .columns-container {
            display: flex;
            align-items: flex-end;
            gap: 15px;
            margin-bottom: 25px;
        }
        .col {
            flex: 1;
        }
        .col-arrow {
            display: flex;
            align-items: center;
            justify-content: center;
            padding-bottom: 12px;
        }
        .col-arrow img {
            width: 36px;
            height: auto;
            filter: drop-shadow(0 2px 4px rgba(0,0,0,0.5));
        }
        button.btn-convert {
            width: 100%;
            background-color: #10b981;
            color: white;
            border: none;
            border-radius: 6px;
            padding: 14px;
            font-size: 1.05rem;
            font-weight: 700;
            cursor: pointer;
            transition: background-color 0.2s, transform 0.1s;
        }
        button.btn-convert:hover {
            background-color: #059669;
        }
        button.btn-convert:active {
            transform: scale(0.99);
        }
        button.btn-convert:disabled {
            background-color: #475569;
            cursor: not-allowed;
        }
        /* Tarjeta de Respuesta Estructurada (RF-06 / CU-07) */
        .response-card {
            background-color: #0f172a;
            border: 1px solid #334155;
            border-radius: 8px;
            padding: 20px;
            margin-top: 25px;
        }
        .response-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 12px;
        }
        .response-title {
            font-size: 0.75rem;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            color: #94a3b8;
        }
        .badges-container {
            display: flex;
            gap: 8px;
        }
        .status-badge {
            font-size: 0.75rem;
            font-weight: 700;
            padding: 3px 8px;
            border-radius: 4px;
            text-transform: uppercase;
        }
        .badge-waiting {
            background-color: #334155;
            color: #94a3b8;
        }
        .badge-ok {
            background-color: rgba(16, 185, 129, 0.2);
            color: #34d399;
            border: 1px solid #10b981;
        }
        .badge-error {
            background-color: rgba(239, 68, 68, 0.2);
            color: #f87171;
            border: 1px solid #ef4444;
        }
        .code-badge {
            background-color: #1e293b;
            color: #cbd5e1;
            border: 1px solid #475569;
            font-size: 0.75rem;
            font-weight: 700;
            padding: 3px 8px;
            border-radius: 4px;
        }
        .result-display {
            font-size: 1.8rem;
            font-weight: 700;
            color: #34d399;
            margin-bottom: 8px;
            min-height: 40px;
            display: flex;
            align-items: center;
        }
        .result-error {
            color: #f87171;
            font-size: 1.2rem;
        }
        .server-message {
            font-size: 0.85rem;
            color: #cbd5e1;
            line-height: 1.4;
        }
        @media (max-width: 768px) {
            .main-container {
                flex-direction: column-reverse;
            }
            .side-profile {
                width: 100%;
                border-left: none;
                border-bottom: 1px solid #1e293b;
                flex-direction: row;
                gap: 20px;
            }
            .sulley-card img {
                max-width: 120px;
            }
        }
    </style>
</head>
<body>
    <div class="main-container">
        <!-- Sección de Conversión -->
        <div class="converter-section">
            <h2>Panel de Conversiones SOAP</h2>
            <p class="subtitle">Consumo JAX-WS sobre Payara 7 &bull; Grupo GR08</p>

            <form id="formConversor">
                <!-- Selector de Categoría -->
                <div class="form-row">
                    <label for="cmbTipo">Categoría Física:</label>
                    <select name="cmbTipo" id="cmbTipo" onchange="actualizarListas()">
                        <option value="Longitud">Longitud</option>
                        <option value="Masa">Masa</option>
                        <option value="Temperatura">Temperatura</option>
                    </select>
                </div>

                <!-- Columnas Origen y Destino -->
                <div class="columns-container">
                    <div class="col">
                        <label for="listaOrigen">Unidad Origen:</label>
                        <select name="txtOrigen" id="listaOrigen" style="margin-bottom: 12px;"></select>
                        <label for="txtValor">Valor Numérico:</label>
                        <input type="number" step="any" name="txtValor" id="txtValor" value="100.0" required>
                    </div>

                    <div class="col-arrow">
                        <img src="Imagenes/flecha40px.png" alt="➜" title="Conversión">
                    </div>

                    <div class="col">
                        <label for="listaDestino">Unidad Destino:</label>
                        <select name="txtDestino" id="listaDestino" style="margin-bottom: 12px;"></select>
                        <label>&nbsp;</label>
                        <div style="height: 42px; display: flex; align-items: center; color: #94a3b8; font-size: 0.85rem;">
                            (Seleccione destino)
                        </div>
                    </div>
                </div>

                <button type="submit" id="btnConvertir" class="btn-convert">CONVERTIR AHORA</button>
            </form>

            <!-- Tarjeta de Respuesta Estructurada (RF-06 / CU-07) -->
            <div class="response-card">
                <div class="response-header">
                    <span class="response-title">Respuesta Estructurada del Servidor (RF-06)</span>
                    <div class="badges-container">
                        <span id="badgeEstado" class="status-badge badge-waiting">ESPERANDO</span>
                        <span id="badgeCodigo" class="code-badge">CÓDIGO: ---</span>
                    </div>
                </div>
                <div id="displayResultado" class="result-display">---</div>
                <div id="displayMensaje" class="server-message">Seleccione parámetros y presione "Convertir Ahora".</div>
            </div>
        </div>

        <!-- Sidebar con Sullivan de Monsters Inc (Requerimiento en todas las pantallas) -->
        <div class="side-profile">
            <div class="sulley-card">
                <img src="Imagenes/monsterlogin.jpg" alt="James P. Sullivan" title="James P. Sullivan - Monsters Inc">
                <div class="user-info">
                    Sesión activa: <strong><%= usuario %></strong><br>
                    <span style="font-size: 0.75rem; color: #94a3b8;">Token validado en memoria</span>
                </div>
            </div>
            <a href="LogoutServlet" class="btn-logout">Cerrar Sesión</a>
        </div>
    </div>

    <script>
        const dicUnidades = {
            "Longitud": ["centimetros", "metros", "pies", "yardas", "millas"],
            "Masa": ["gramos", "kilogramos", "onzas", "libras", "quintales"],
            "Temperatura": ["celsius", "fahrenheit", "kelvin", "newton", "reaumur"]
        };

        function actualizarListas() {
            const categoria = document.getElementById("cmbTipo").value;
            const listaOrigen = document.getElementById("listaOrigen");
            const listaDestino = document.getElementById("listaDestino");

            listaOrigen.innerHTML = "";
            listaDestino.innerHTML = "";

            dicUnidades[categoria].forEach(u => {
                listaOrigen.add(new Option(u, u));
                listaDestino.add(new Option(u, u));
            });

            if (listaDestino.options.length > 1) {
                listaDestino.selectedIndex = 1;
            }
        }

        window.onload = function() {
            actualizarListas();

            document.getElementById("formConversor").addEventListener("submit", async function(e) {
                e.preventDefault();

                const btn = document.getElementById("btnConvertir");
                const badgeEstado = document.getElementById("badgeEstado");
                const badgeCodigo = document.getElementById("badgeCodigo");
                const displayResultado = document.getElementById("displayResultado");
                const displayMensaje = document.getElementById("displayMensaje");

                btn.disabled = true;
                badgeEstado.className = "status-badge badge-waiting";
                badgeEstado.innerText = "PROCESANDO";
                badgeCodigo.innerText = "CÓDIGO: ---";
                displayResultado.className = "result-display";
                displayResultado.innerText = "...";
                displayMensaje.innerText = "Consultando Web Service SOAP GR08 en Payara 7...";

                const formData = new URLSearchParams(new FormData(this));

                try {
                    const response = await fetch('ConversorServlet', {
                        method: 'POST',
                        body: formData
                    });

                    if (response.status === 401) {
                        window.location.href = "index.jsp";
                        return;
                    }

                    const data = await response.json();

                    if (data.estado === "OK") {
                        badgeEstado.className = "status-badge badge-ok";
                        badgeEstado.innerText = "ESTADO: OK";
                        badgeCodigo.innerText = "CÓDIGO: " + data.codigo;
                        const dest = document.getElementById("listaDestino").value;
                        displayResultado.className = "result-display";
                        displayResultado.innerText = Number(data.resultado).toFixed(4) + " " + dest;
                        displayMensaje.innerText = data.mensaje;
                    } else {
                        badgeEstado.className = "status-badge badge-error";
                        badgeEstado.innerText = "ESTADO: ERROR";
                        badgeCodigo.innerText = "CÓDIGO: " + (data.codigo || "ERR");
                        displayResultado.className = "result-display result-error";
                        displayResultado.innerText = "Error en Conversión";
                        displayMensaje.innerText = data.mensaje || "Error desconocido devuelto por el Web Service.";
                    }

                } catch (err) {
                    badgeEstado.className = "status-badge badge-error";
                    badgeEstado.innerText = "ESTADO: ERROR";
                    badgeCodigo.innerText = "CÓDIGO: SYS-001";
                    displayResultado.className = "result-display result-error";
                    displayResultado.innerText = "Error de Conexión";
                    displayMensaje.innerText = "No se pudo conectar con el servidor: " + err.message;
                } finally {
                    btn.disabled = false;
                }
            });
        };
    </script>
</body>
</html>
