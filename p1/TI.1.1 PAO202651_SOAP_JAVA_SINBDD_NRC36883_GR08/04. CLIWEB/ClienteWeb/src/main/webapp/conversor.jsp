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
            max-width: 980px;
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
        .btn-change-pass {
            display: block;
            width: 100%;
            text-align: center;
            background-color: #2563eb;
            color: #ffffff;
            border: none;
            padding: 10px;
            border-radius: 6px;
            font-weight: 600;
            font-size: 0.85rem;
            cursor: pointer;
            transition: background-color 0.2s;
            margin-bottom: 8px;
        }
        .btn-change-pass:hover {
            background-color: #1d4ed8;
        }
        /* Modal Cambiar Contraseña */
        .modal-backdrop {
            display: none;
            position: fixed;
            top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(0, 0, 0, 0.7);
            backdrop-filter: blur(4px);
            z-index: 1000;
            justify-content: center;
            align-items: center;
        }
        .modal-content {
            background-color: #1e293b;
            border: 1px solid #475569;
            border-radius: 12px;
            width: 90%;
            max-width: 440px;
            box-shadow: 0 25px 50px rgba(0,0,0,0.6);
            overflow: hidden;
            animation: modalFadeIn 0.2s ease-out;
        }
        @keyframes modalFadeIn {
            from { opacity: 0; transform: scale(0.95); }
            to { opacity: 1; transform: scale(1); }
        }
        .modal-header {
            padding: 18px 24px;
            background-color: #0f172a;
            border-bottom: 1px solid #334155;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .modal-header h3 {
            font-size: 1.1rem;
            color: #38bdf8;
            margin: 0;
            display: flex;
            align-items: center;
            gap: 8px;
        }
        .modal-close {
            background: none;
            border: none;
            color: #94a3b8;
            font-size: 1.3rem;
            cursor: pointer;
        }
        .modal-close:hover { color: #fff; }
        .modal-body {
            padding: 24px;
        }
        .modal-body .form-group {
            margin-bottom: 16px;
        }
        .modal-body label {
            display: block;
            margin-bottom: 6px;
            font-size: 0.85rem;
            color: #cbd5e1;
            font-weight: 600;
        }
        .modal-body input[type="password"] {
            width: 100%;
            padding: 10px 12px;
            background-color: #0f172a;
            border: 1px solid #334155;
            border-radius: 6px;
            color: #fff;
            font-size: 0.95rem;
        }
        .modal-body input[type="password"]:focus {
            border-color: #38bdf8;
            outline: none;
        }
        .modal-feedback {
            display: none;
            padding: 10px 12px;
            border-radius: 6px;
            font-size: 0.82rem;
            margin-bottom: 16px;
            line-height: 1.4;
        }
        .modal-feedback.success {
            background-color: rgba(16, 185, 129, 0.15);
            border: 1px solid #10b981;
            color: #34d399;
            display: block;
        }
        .modal-feedback.error {
            background-color: rgba(239, 68, 68, 0.15);
            border: 1px solid #ef4444;
            color: #f87171;
            display: block;
        }
        .modal-footer {
            padding: 14px 24px;
            background-color: #0f172a;
            border-top: 1px solid #334155;
            display: flex;
            justify-content: flex-end;
            gap: 12px;
        }
        .btn-cancel {
            background-color: #334155;
            color: #e2e8f0;
            border: none;
            padding: 9px 16px;
            border-radius: 6px;
            cursor: pointer;
            font-weight: 600;
            font-size: 0.85rem;
        }
        .btn-save-pass {
            background-color: #2563eb;
            color: #fff;
            border: none;
            padding: 9px 18px;
            border-radius: 6px;
            cursor: pointer;
            font-weight: 700;
            font-size: 0.85rem;
        }
        .btn-save-pass:hover { background-color: #1d4ed8; }
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
        /* Tarjeta de Resultado de la Conversión */
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
        .history-card {
            margin-top: 25px;
            background-color: #0b1325;
            border: 1px solid #334155;
            border-radius: 8px;
            padding: 18px 20px;
        }
        .history-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 14px;
        }
        .history-title {
            font-size: 0.85rem;
            font-weight: 700;
            color: #38bdf8;
            letter-spacing: 0.5px;
            text-transform: uppercase;
        }
        .btn-refresh {
            background-color: #1e293b;
            color: #f8fafc;
            border: 1px solid #475569;
            padding: 5px 12px;
            border-radius: 6px;
            font-size: 0.78rem;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s;
        }
        .btn-refresh:hover {
            background-color: #3b82f6;
            border-color: #3b82f6;
        }
        .table-container {
            max-height: 190px;
            overflow-y: auto;
            border: 1px solid #1e293b;
            border-radius: 6px;
        }
        .history-table {
            width: 100%;
            border-collapse: collapse;
            font-size: 0.82rem;
            text-align: left;
        }
        .history-table th {
            background-color: #151b28;
            color: #94a3b8;
            padding: 8px 10px;
            font-weight: 600;
            border-bottom: 1px solid #334155;
            position: sticky;
            top: 0;
        }
        .history-table td {
            padding: 8px 10px;
            border-bottom: 1px solid #1e293b;
            color: #e2e8f0;
        }
        .history-table tr:hover {
            background-color: rgba(30, 41, 59, 0.5);
        }
        .cat-pill {
            display: inline-block;
            padding: 2px 8px;
            border-radius: 12px;
            font-size: 0.72rem;
            font-weight: 600;
            background-color: #1e3a8a;
            color: #60a5fa;
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

            <!-- Tarjeta de Resultado de la Conversión -->
            <div class="response-card">
                <div class="response-header">
                    <span class="response-title">Resultado de la Conversión</span>
                    <div class="badges-container">
                        <span id="badgeEstado" class="status-badge badge-waiting">ESPERANDO</span>
                    </div>
                </div>
                <div id="displayResultado" class="result-display">---</div>
                <div id="displayMensaje" class="server-message">Selecciona las unidades, ingresa un valor y presiona "Convertir Ahora".</div>
            </div>

            <!-- Historial Global Compartido (Multi-canal) -->
            <div class="history-card">
                <div class="history-header">
                    <div>
                        <span class="history-title">Historial Global Compartido</span>
                        <div style="font-size: 0.72rem; color: #94a3b8; margin-top: 2px;">Sincronizado entre Web, Móvil, Escritorio y Consola</div>
                    </div>
                    <button type="button" class="btn-refresh" onclick="cargarHistorial()" title="Recargar historial">🔄 Actualizar</button>
                </div>
                <div class="table-container">
                    <table class="history-table" id="tablaHistorial">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Fecha / Hora</th>
                                <th>Categoría</th>
                                <th>Conversión Realizada</th>
                            </tr>
                        </thead>
                        <tbody id="tbodyHistorial">
                            <tr>
                                <td colspan="4" style="text-align: center; color: #94a3b8; padding: 12px;">Cargando historial central...</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
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
            <button type="button" class="btn-change-pass" onclick="abrirModalClave()">🔑 Cambiar Contraseña</button>
            <a href="LogoutServlet" class="btn-logout">Cerrar Sesión</a>
        </div>
    </div>

    <!-- Modal Cambiar Contraseña (Sincronización Multi-canal) -->
    <div id="modalClave" class="modal-backdrop">
        <div class="modal-content">
            <div class="modal-header">
                <h3>🔑 Cambiar Contraseña</h3>
                <button type="button" class="modal-close" onclick="cerrarModalClave()">&times;</button>
            </div>
            <form id="formCambiarClave" onsubmit="enviarCambioClave(event)">
                <div class="modal-body">
                    <p style="font-size: 0.78rem; color: #94a3b8; margin-bottom: 14px;">
                        Al cambiar tu contraseña, la actualización se sincroniza centralmente en el servidor SOAP en memoria para todos los clientes (Web, Móvil, Escritorio y Consola).
                    </p>
                    <div id="modalClaveFeedback" class="modal-feedback"></div>
                    <div class="form-group">
                        <label for="txtClaveActual">Contraseña Actual:</label>
                        <input type="password" id="txtClaveActual" name="txtClaveActual" required placeholder="Ingresa tu clave actual">
                    </div>
                    <div class="form-group">
                        <label for="txtClaveNueva">Nueva Contraseña:</label>
                        <input type="password" id="txtClaveNueva" name="txtClaveNueva" required placeholder="Mínimo 4 caracteres">
                    </div>
                    <div class="form-group">
                        <label for="txtClaveConfirmar">Confirmar Nueva Contraseña:</label>
                        <input type="password" id="txtClaveConfirmar" name="txtClaveConfirmar" required placeholder="Repite la nueva contraseña">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn-cancel" onclick="cerrarModalClave()">Cancelar</button>
                    <button type="submit" id="btnGuardarClave" class="btn-save-pass">Actualizar Contraseña</button>
                </div>
            </form>
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
                const displayResultado = document.getElementById("displayResultado");
                const displayMensaje = document.getElementById("displayMensaje");

                const valInput = document.getElementById("txtValor").value.trim();
                const categoria = document.getElementById("cmbTipo").value;
                const origen = document.getElementById("listaOrigen").value;
                const destino = document.getElementById("listaDestino").value;

                // Validar valor vacío
                if (!valInput) {
                    badgeEstado.className = "status-badge badge-error";
                    badgeEstado.innerText = "ESTADO: ERROR";
                    displayResultado.className = "result-display result-error";
                    displayResultado.innerText = "Dato Requerido";
                    displayMensaje.innerText = "Por favor ingresa un valor numérico para convertir.";
                    document.getElementById("txtValor").focus();
                    return;
                }

                // Validar número válido
                const valor = parseFloat(valInput);
                if (isNaN(valor)) {
                    badgeEstado.className = "status-badge badge-error";
                    badgeEstado.innerText = "ESTADO: ERROR";
                    displayResultado.className = "result-display result-error";
                    displayResultado.innerText = "Número Inválido";
                    displayMensaje.innerText = "El valor debe ser un número válido (ej. 100 o 3.14).";
                    document.getElementById("txtValor").focus();
                    return;
                }

                // Validar coherencia física
                if (categoria === "Longitud" && valor < 0) {
                    badgeEstado.className = "status-badge badge-error";
                    badgeEstado.innerText = "ESTADO: ERROR";
                    displayResultado.className = "result-display result-error";
                    displayResultado.innerText = "Valor No Válido";
                    displayMensaje.innerText = "La longitud no puede ser un valor negativo.";
                    document.getElementById("txtValor").focus();
                    return;
                }

                if (categoria === "Masa" && valor < 0) {
                    badgeEstado.className = "status-badge badge-error";
                    badgeEstado.innerText = "ESTADO: ERROR";
                    displayResultado.className = "result-display result-error";
                    displayResultado.innerText = "Valor No Válido";
                    displayMensaje.innerText = "La masa no puede ser un valor negativo.";
                    document.getElementById("txtValor").focus();
                    return;
                }

                if (categoria === "Temperatura") {
                    const oNorm = origen.toLowerCase();
                    if (oNorm === "kelvin" && valor < 0) {
                        badgeEstado.className = "status-badge badge-error";
                        badgeEstado.innerText = "ESTADO: ERROR";
                        displayResultado.className = "result-display result-error";
                        displayResultado.innerText = "Cero Absoluto";
                        displayMensaje.innerText = "La temperatura en Kelvin no puede ser menor a 0 K (cero absoluto).";
                        document.getElementById("txtValor").focus();
                        return;
                    }
                    if (oNorm === "celsius" && valor < -273.15) {
                        badgeEstado.className = "status-badge badge-error";
                        badgeEstado.innerText = "ESTADO: ERROR";
                        displayResultado.className = "result-display result-error";
                        displayResultado.innerText = "Cero Absoluto";
                        displayMensaje.innerText = "La temperatura en Celsius no puede ser menor a -273.15 °C (cero absoluto).";
                        document.getElementById("txtValor").focus();
                        return;
                    }
                    if (oNorm === "fahrenheit" && valor < -459.67) {
                        badgeEstado.className = "status-badge badge-error";
                        badgeEstado.innerText = "ESTADO: ERROR";
                        displayResultado.className = "result-display result-error";
                        displayResultado.innerText = "Cero Absoluto";
                        displayMensaje.innerText = "La temperatura en Fahrenheit no puede ser menor a -459.67 °F (cero absoluto).";
                        document.getElementById("txtValor").focus();
                        return;
                    }
                }

                // Mismas unidades
                if (origen === destino) {
                    badgeEstado.className = "status-badge badge-ok";
                    badgeEstado.innerText = "ESTADO: OK";
                    displayResultado.className = "result-display";
                    displayResultado.innerText = Number(valor).toFixed(4) + " " + destino;
                    displayMensaje.innerText = "Las unidades de origen y destino son iguales (" + origen + "). El valor se mantiene idéntico.";
                    return;
                }

                btn.disabled = true;
                badgeEstado.className = "status-badge badge-waiting";
                badgeEstado.innerText = "PROCESANDO";
                displayResultado.className = "result-display";
                displayResultado.innerText = "...";
                displayMensaje.innerText = "Consultando el servicio de conversión...";

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
                        const dest = document.getElementById("listaDestino").value;
                        displayResultado.className = "result-display";
                        displayResultado.innerText = Number(data.resultado).toFixed(4) + " " + dest;
                        displayMensaje.innerText = data.mensaje || "Conversión realizada con éxito.";
                        cargarHistorial();
                    } else {
                        badgeEstado.className = "status-badge badge-error";
                        badgeEstado.innerText = "ESTADO: ERROR";
                        displayResultado.className = "result-display result-error";
                        displayResultado.innerText = "Error";
                        if (data.codigo === "AUTH-001") {
                            displayMensaje.innerHTML = "Tu sesión ha expirado por inactividad o reinicio del servidor.<br><span style='color:#38bdf8; font-size:0.85rem;'>Haz clic en <strong>'Cerrar Sesión'</strong> e ingresa nuevamente.</span>";
                        } else {
                            displayMensaje.innerText = data.mensaje || "No se pudo realizar la conversión.";
                        }
                    }

                } catch (err) {
                    badgeEstado.className = "status-badge badge-error";
                    badgeEstado.innerText = "ESTADO: ERROR";
                    displayResultado.className = "result-display result-error";
                    displayResultado.innerText = "Sin Conexión";
                    displayMensaje.innerText = "No se pudo conectar con el servidor. Verifica que el servicio esté activo e inténtalo de nuevo.";
                } finally {
                    btn.disabled = false;
                }
            });

            // Cargar historial al iniciar la página
            cargarHistorial();
        };

        async function cargarHistorial() {
            const tbody = document.getElementById("tbodyHistorial");
            try {
                const res = await fetch('ConversorServlet', { method: 'GET' });
                if (res.status === 401) {
                    window.location.href = "index.jsp";
                    return;
                }
                const data = await res.json();
                if (data.estado === "OK" && data.item) {
                    if (data.item.length === 0) {
                        tbody.innerHTML = '<tr><td colspan="4" style="text-align:center; color:#94a3b8; padding:15px;">Sin conversiones registradas aún. ¡Realiza una en cualquier cliente!</td></tr>';
                        return;
                    }
                    let html = '';
                    data.item.forEach((r) => {
                        html += `<tr>
                            <td>\${r.id}</td>
                            <td style="color:#94a3b8; font-size:0.78rem;">\${r.fechaHora}</td>
                            <td><span class="cat-pill">\${r.categoria}</span></td>
                            <td><strong>\${Number(r.valorOriginal).toFixed(2)}</strong> \${r.unidadOrigen} <span style="color:#38bdf8; font-weight:bold; margin: 0 6px;">&rarr;</span> <strong style="color:#34d399;">\${Number(r.resultado).toFixed(4)}</strong> \${r.unidadDestino}</td>
                        </tr>`;
                    });
                    tbody.innerHTML = html;
                } else {
                    tbody.innerHTML = '<tr><td colspan="4" style="text-align:center; color:#94a3b8; padding:12px;">Sin registros en el historial.</td></tr>';
                }
            } catch (err) {
                tbody.innerHTML = '<tr><td colspan="4" style="text-align:center; color:#f87171; padding:12px;">No se pudo sincronizar el historial central.</td></tr>';
            }
        }

        function abrirModalClave() {
            const feedback = document.getElementById("modalClaveFeedback");
            feedback.className = "modal-feedback";
            feedback.innerText = "";
            document.getElementById("formCambiarClave").reset();
            document.getElementById("modalClave").style.display = "flex";
            document.getElementById("txtClaveActual").focus();
        }

        function cerrarModalClave() {
            document.getElementById("modalClave").style.display = "none";
        }

        async function enviarCambioClave(e) {
            e.preventDefault();
            const actual = document.getElementById("txtClaveActual").value.trim();
            const nueva = document.getElementById("txtClaveNueva").value.trim();
            const confirmar = document.getElementById("txtClaveConfirmar").value.trim();
            const feedback = document.getElementById("modalClaveFeedback");
            const btn = document.getElementById("btnGuardarClave");

            // Validar campos requeridos
            if (!actual || !nueva || !confirmar) {
                feedback.className = "modal-feedback error";
                feedback.innerText = "Por favor completa todos los campos de contraseña.";
                return;
            }

            // Validar longitud
            if (nueva.length < 4) {
                feedback.className = "modal-feedback error";
                feedback.innerText = "La nueva contraseña debe tener al menos 4 caracteres.";
                return;
            }

            // Validar nueva diferente a actual
            if (actual === nueva) {
                feedback.className = "modal-feedback error";
                feedback.innerText = "La nueva contraseña debe ser diferente a la contraseña actual.";
                return;
            }

            // Validar confirmación
            if (nueva !== confirmar) {
                feedback.className = "modal-feedback error";
                feedback.innerText = "La confirmación no coincide con la nueva contraseña.";
                return;
            }

            btn.disabled = true;
            btn.innerText = "Actualizando en servidor...";

            try {
                const params = new URLSearchParams();
                params.append("txtClaveActual", actual);
                params.append("txtClaveNueva", nueva);
                params.append("txtClaveConfirmar", confirmar);

                const res = await fetch("CambiarClaveServlet", {
                    method: "POST",
                    headers: { "Content-Type": "application/x-www-form-urlencoded" },
                    body: params.toString()
                });

                const data = await res.json();
                if (data.estado === "OK") {
                    feedback.className = "modal-feedback success";
                    feedback.innerHTML = '<strong>¡Éxito!</strong> ' + data.mensaje + '<br><span style="font-size:0.75rem;">Sincronizado inmediatamente en todos los clientes (Web, Consola, Escritorio y Móvil).</span>';
                    setTimeout(() => {
                        cerrarModalClave();
                    }, 2500);
                } else {
                    feedback.className = "modal-feedback error";
                    feedback.innerText = data.mensaje || "No se pudo actualizar la contraseña.";
                }
            } catch (err) {
                feedback.className = "modal-feedback error";
                feedback.innerText = "No se pudo conectar con el servidor. Inténtalo de nuevo más tarde.";
            } finally {
                btn.disabled = false;
                btn.innerText = "Actualizar Contraseña";
            }
        }
    </script>
</body>
</html>
