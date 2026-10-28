// ==========================================================================
// CLIENTE WEB RESTFUL .NET 8 (SINBDD) - MONSTERS INC GR08
// ==========================================================================

// Detecta dinámicamente la IP del servidor RESTful (localhost o IP de red local)
const getApiBaseUrl = () => {
    const saved = localStorage.getItem("rest_server_host");
    if (saved) return `http://${saved}:5000/api`;
    const host = window.location.hostname || "localhost";
    return `http://${host}:5000/api`;
};
const API_BASE_URL = getApiBaseUrl();

const state = {
    token: sessionStorage.getItem("token") || null,
    usuario: sessionStorage.getItem("usuario") || "MONSTER"
};

// Mapas de conversión de los DropDowns de GR01
const UNIT_MAPS = {
    longitud: {
        millaMetro: { origen: "Milla", destino: "Metro" },
        metroMilla: { origen: "Metro", destino: "Milla" },
        millaPulgada: { origen: "Milla", destino: "Pulgada" },
        pulgadaMilla: { origen: "Pulgada", destino: "Milla" },
        metroPulgada: { origen: "Metro", destino: "Pulgada" },
        pulgadaMetro: { origen: "Pulgada", destino: "Metro" },
        kilometroMetro: { origen: "Kilometro", destino: "Metro" },
        metroKilometro: { origen: "Metro", destino: "Kilometro" },
        pieMetro: { origen: "Pie", destino: "Metro" },
        yardaMetro: { origen: "Yarda", destino: "Metro" }
    },
    masa: {
        kilogramoLibra: { origen: "Kilogramo", destino: "Libra" },
        libraKilogramo: { origen: "Libra", destino: "Kilogramo" },
        kilogramoOnza: { origen: "Kilogramo", destino: "Onza" },
        onzaKilogramo: { origen: "Onza", destino: "Kilogramo" },
        libraOnza: { origen: "Libra", destino: "Onza" },
        onzaLibra: { origen: "Onza", destino: "Libra" },
        gramoKilogramo: { origen: "Gramo", destino: "Kilogramo" },
        kilogramoGramo: { origen: "Kilogramo", destino: "Gramo" }
    },
    temperatura: {
        celsiusFahrenheit: { origen: "Celsius", destino: "Fahrenheit" },
        fahrenheitCelsius: { origen: "Fahrenheit", destino: "Celsius" },
        celsiusKelvin: { origen: "Celsius", destino: "Kelvin" },
        kelvinCelsius: { origen: "Kelvin", destino: "Celsius" },
        fahrenheitKelvin: { origen: "Fahrenheit", destino: "Kelvin" },
        kelvinFahrenheit: { origen: "Kelvin", destino: "Fahrenheit" }
    }
};

document.addEventListener("DOMContentLoaded", () => {
    initApp();
});

function initApp() {
    // Si ya existe token, verificar validez
    if (state.token) {
        verificarTokenValido();
    } else {
        mostrarVistaLogin();
    }

    setupEventListeners();
}

function setupEventListeners() {
    // Formulario de Login
    const loginForm = document.getElementById("loginForm");
    if (loginForm) {
        loginForm.addEventListener("submit", handleLogin);
    }

    // Botón Cerrar Sesión
    document.getElementById("btnCerrarSesion")?.addEventListener("click", handleLogout);

    // Formularios de Conversión
    document.getElementById("formLongitud")?.addEventListener("submit", (e) => handleConversion(e, "Longitud"));
    document.getElementById("formMasa")?.addEventListener("submit", (e) => handleConversion(e, "Masa"));
    document.getElementById("formTemperatura")?.addEventListener("submit", (e) => handleConversion(e, "Temperatura"));

    // Historial
    document.getElementById("btnRecargarHistorial")?.addEventListener("click", cargarHistorial);
    document.getElementById("btnLimpiarHistorial")?.addEventListener("click", limpiarHistorial);

    // Modal Cambiar Clave
    const modalClave = document.getElementById("modalCambiarClave");
    document.getElementById("btnAbrirModalClave")?.addEventListener("click", () => {
        modalClave.classList.add("active");
        ocultarAlertaClave();
        document.getElementById("formCambiarClave")?.reset();
    });

    document.getElementById("btnCerrarModalClave")?.addEventListener("click", () => {
        modalClave.classList.remove("active");
    });

    document.getElementById("formCambiarClave")?.addEventListener("submit", handleCambiarClave);
}

// ==========================================================================
// AUTENTICACIÓN RESTFUL (RF-01)
// ==========================================================================
async function handleLogin(e) {
    e.preventDefault();
    const alertBox = document.getElementById("loginAlert");
    const alertMsg = document.getElementById("loginAlertMsg");
    const btnLogin = document.getElementById("btnLogin");

    const usuario = document.getElementById("txtUsuario").value.trim();
    const clave = document.getElementById("txtClave").value;

    alertBox.style.display = "none";
    btnLogin.disabled = true;
    btnLogin.innerHTML = "<span>Autenticando...</span>";

    try {
        const response = await fetch(`${API_BASE_URL}/auth/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ usuario, clave })
        });

        const data = await response.json();

        if (response.ok && data.exitoso) {
            state.token = data.token;
            state.usuario = data.usuario;
            sessionStorage.setItem("token", data.token);
            sessionStorage.setItem("usuario", data.usuario);

            mostrarVistaDashboard();
            cargarHistorial();
        } else {
            alertMsg.textContent = data.mensaje || "Error al autenticar con el servidor REST.";
            alertBox.style.display = "flex";
        }
    } catch (err) {
        alertMsg.textContent = "No se pudo conectar con el servidor REST en " + API_BASE_URL;
        alertBox.style.display = "flex";
    } finally {
        btnLogin.disabled = false;
        btnLogin.innerHTML = "<span>INGRESAR AL SISTEMA</span>";
    }
}

async function verificarTokenValido() {
    try {
        const response = await fetch(`${API_BASE_URL}/auth/validar`, {
            headers: { "Authorization": `Bearer ${state.token}` }
        });

        if (response.ok) {
            mostrarVistaDashboard();
            cargarHistorial();
        } else {
            handleLogout();
        }
    } catch {
        // Si el servidor está apagado o no responde
        mostrarVistaDashboard();
    }
}

function handleLogout() {
    if (state.token) {
        fetch(`${API_BASE_URL}/auth/logout`, {
            method: "POST",
            headers: { "Authorization": `Bearer ${state.token}` }
        }).catch(() => {});
    }

    state.token = null;
    sessionStorage.removeItem("token");
    sessionStorage.removeItem("usuario");
    mostrarVistaLogin();
}

function mostrarVistaLogin() {
    document.getElementById("loginView").style.display = "flex";
    document.getElementById("dashboardView").style.display = "none";
}

function mostrarVistaDashboard() {
    document.getElementById("loginView").style.display = "none";
    document.getElementById("dashboardView").style.display = "block";

    document.getElementById("lblUsuarioActivo").textContent = state.usuario || "MONSTER";
    if (state.token) {
        document.getElementById("lblTokenPreview").textContent = state.token.substring(0, 16) + "...";
    }
}

// ==========================================================================
// CONVERSIONES RESTFUL (RF-05 / GR01 ADAPTACIÓN)
// ==========================================================================
async function handleConversion(e, categoria) {
    e.preventDefault();

    let ddlId = `ddl${categoria}`;
    let txtId = `txtValor${categoria}`;
    let resId = `res${categoria}`;

    const ddl = document.getElementById(ddlId);
    const txtInput = document.getElementById(txtId);
    const resContainer = document.getElementById(resId);

    const claveMapeo = ddl.value;
    const valor = txtInput.value.trim();

    // Validacion de longitud y desbordamiento en cliente
    const cleanDigits = valor.replace(/[-.]/g, '');
    if (cleanDigits.length > 15 || Math.abs(parseFloat(valor)) > 1e12) {
        mostrarResultado(resContainer, false, {
            codigoError: "VAL_007",
            mensaje: "El valor ingresado supera el limite maximo permitido (maximo 15 digitos o 1 billon).",
            detalles: "Por favor ingrese un valor numerico realista dentro de los rangos validos."
        });
        return;
    }

    const unidades = UNIT_MAPS[categoria.toLowerCase()][claveMapeo];
    if (!unidades) {
        mostrarResultado(resContainer, false, { mensaje: "Seleccion de conversion no valida" });
        return;
    }

    resContainer.style.display = "block";
    resContainer.className = "result-container";
    resContainer.innerHTML = "<p style='color: var(--text-secondary);'>Consultando servicio REST...</p>";

    try {
        const payload = {
            valor: valor,
            unidadOrigen: unidades.origen,
            unidadDestino: unidades.destino
        };

        const headers = { "Content-Type": "application/json" };
        if (state.token) {
            headers["Authorization"] = `Bearer ${state.token}`;
        }

        const response = await fetch(`${API_BASE_URL}/${categoria}/convertir`, {
            method: "POST",
            headers: headers,
            body: JSON.stringify(payload)
        });

        const data = await response.json();

        if (response.ok && data.exitoso && data.resultado) {
            mostrarResultado(resContainer, true, data.resultado);
            cargarHistorial(); // Refrescar historial sincronizado
        } else {
            const err = data.error || { mensaje: data.mensaje || "Error al procesar la conversion" };
            mostrarResultado(resContainer, false, err);
        }
    } catch (err) {
        mostrarResultado(resContainer, false, {
            mensaje: "Error de comunicacion con el servicio REST",
            detalles: err.message
        });
    }
}

function mostrarResultado(container, esExito, data) {
    container.style.display = "block";

    if (esExito) {
        container.className = "result-container result-success";
        container.innerHTML = `
            <div class="result-header">
                <span>Conversion Exitosa</span>
                <span style="font-family: var(--font-mono); font-size: 0.72rem;">${new Date().toLocaleTimeString()}</span>
            </div>
            <div style="font-size: 0.82rem; color: var(--text-secondary); margin-bottom: 2px;">
                ${data.valorOriginal} ${data.unidadOrigen} equivalen a:
            </div>
            <div class="result-value-big">
                ${data.valorConvertidoRedondeado} <span style="font-size: 1rem; color: #34d399;">${data.unidadDestino}</span>
            </div>
            <div class="result-meta">
                <span><strong>Valor Exacto:</strong> <code style="font-family: var(--font-mono); color: #fff;">${data.valorConvertidoExacto}</code></span>
                <span><strong>Factor Utilizado:</strong> ${data.factorConversion}</span>
            </div>
        `;
    } else {
        container.className = "result-container result-error";
        container.innerHTML = `
            <div class="result-header">
                <span>Error en la Conversion</span>
                <span>${data.codigoError || 'ERR'}</span>
            </div>
            <div style="font-weight: 600; color: #fca5a5; font-size: 0.88rem; margin-bottom: 4px;">
                ${data.mensaje}
            </div>
            ${data.detalles ? `<div style="font-size: 0.76rem; color: var(--text-muted); margin-top: 6px;">${data.detalles}</div>` : ''}
        `;
    }
}

// ==========================================================================
// HISTORIAL SINCRONIZADO (RF-06 / RF-07)
// ==========================================================================
async function cargarHistorial() {
    const tbody = document.getElementById("tblHistorialBody");
    const lblTotal = document.getElementById("lblTotalConversiones");

    try {
        const response = await fetch(`${API_BASE_URL}/historial`);
        const data = await response.json();

        if (response.ok && data.exitoso && Array.isArray(data.registros)) {
            lblTotal.textContent = data.registros.length;

            if (data.registros.length === 0) {
                tbody.innerHTML = `
                    <tr>
                        <td colspan="6" style="text-align: center; color: var(--text-muted); padding: 20px;">
                            No hay conversiones registradas aún en el servidor REST.
                        </td>
                    </tr>
                `;
                return;
            }

            tbody.innerHTML = data.registros.slice(0, 15).map(r => {
                const fecha = new Date(r.fecha).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', second: '2-digit' });
                return `
                    <tr>
                        <td style="color: var(--text-muted); font-size: 0.78rem;">${fecha}</td>
                        <td><span class="badge-tag badge-${obtenerClaseBadge(r.tipoConversion)}">${r.tipoConversion}</span></td>
                        <td><span class="val-mono">${r.valorOriginal}</span> ${r.unidadOrigen}</td>
                        <td style="color: #3b82f6;">→</td>
                        <td><span class="val-mono" style="color: #34d399;">${r.valorResultado}</span> ${r.unidadDestino}</td>
                        <td><code style="color: #cbd5e1; font-size: 0.76rem;">${r.usuario}</code></td>
                    </tr>
                `;
            }).join("");
        }
    } catch (err) {
        tbody.innerHTML = `
            <tr>
                <td colspan="6" style="text-align: center; color: #f87171; padding: 16px;">
                    Error al cargar el historial del servidor REST.
                </td>
            </tr>
        `;
    }
}

function obtenerClaseBadge(tipo) {
    if (!tipo) return "long";
    tipo = tipo.toLowerCase();
    if (tipo.includes("long")) return "long";
    if (tipo.includes("masa")) return "masa";
    return "temp";
}

async function limpiarHistorial() {
    if (!confirm("¿Está seguro de que desea limpiar el historial de conversiones en memoria?")) {
        return;
    }

    try {
        const response = await fetch(`${API_BASE_URL}/historial`, { method: "DELETE" });
        if (response.ok) {
            cargarHistorial();
        }
    } catch (err) {
        alert("Error al limpiar historial: " + err.message);
    }
}

// ==========================================================================
// CAMBIO DE CONTRASEÑA (RF-08)
// ==========================================================================
async function handleCambiarClave(e) {
    e.preventDefault();

    const claveActual = document.getElementById("txtClaveActual").value;
    const claveNueva = document.getElementById("txtClaveNueva").value;
    const claveConfirmar = document.getElementById("txtClaveConfirmar").value;

    const alertBox = document.getElementById("modalClaveAlert");
    const alertMsg = document.getElementById("modalClaveAlertMsg");

    if (claveNueva !== claveConfirmar) {
        alertMsg.textContent = "La nueva contraseña y su confirmación no coinciden.";
        alertBox.style.display = "flex";
        alertBox.className = "alert-box alert-error";
        return;
    }

    try {
        const response = await fetch(`${API_BASE_URL}/auth/cambiar-clave`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${state.token}`
            },
            body: JSON.stringify({ claveActual, claveNueva })
        });

        const data = await response.json();

        if (response.ok && data.exitoso) {
            alertMsg.textContent = "Contraseña cambiada exitosamente en el servidor REST.";
            alertBox.style.display = "flex";
            alertBox.className = "alert-box alert-success";
            setTimeout(() => {
                document.getElementById("modalCambiarClave").classList.remove("active");
            }, 1400);
        } else {
            alertMsg.textContent = data.mensaje || "Error al cambiar contraseña.";
            alertBox.style.display = "flex";
            alertBox.className = "alert-box alert-error";
        }
    } catch (err) {
        alertMsg.textContent = "Error al conectar con la API: " + err.message;
        alertBox.style.display = "flex";
        alertBox.className = "alert-box alert-error";
    }
}

function ocultarAlertaClave() {
    const alertBox = document.getElementById("modalClaveAlert");
    if (alertBox) alertBox.style.display = "none";
}
