// Comportamiento de las páginas del cliente web ConUni GR08.
// Las validaciones definitivas se hacen en el servidor; aquí solo se mejora la experiencia de uso.

// Mismas reglas que ValidacionUtil: signo opcional, hasta 15 enteros y 10 decimales, punto o coma
const patronNumeroParcial = /^[+-]?\d{0,15}([.,]\d{0,10})?$/;

// Campos numéricos: bloquea letras y símbolos mientras se escribe o se pega texto
document.querySelectorAll("[data-solo-numeros]").forEach((campo) => {
    campo.addEventListener("beforeinput", (evento) => {
        if (evento.data === null || !evento.inputType.startsWith("insert")) {
            return;
        }
        const inicio = campo.selectionStart ?? campo.value.length;
        const fin = campo.selectionEnd ?? campo.value.length;
        const textoNuevo = campo.value.slice(0, inicio) + evento.data + campo.value.slice(fin);

        if (!patronNumeroParcial.test(textoNuevo)) {
            evento.preventDefault();
        }
    });
    campo.addEventListener("input", () => campo.classList.remove("campo-invalido"));
});

// Botón de ojo: muestra u oculta la contraseña de su campo
document.querySelectorAll("[data-alternar-contrasenia]").forEach((boton) => {
    boton.addEventListener("click", () => {
        const campo = document.getElementById(boton.dataset.alternarContrasenia);
        const mostrar = campo.type === "password";
        campo.type = mostrar ? "text" : "password";
        boton.classList.toggle("mostrando", mostrar);
        campo.focus();
    });
});

// Botón ⇄: intercambia las unidades de origen y destino
document.querySelectorAll("[data-intercambiar]").forEach((boton) => {
    boton.addEventListener("click", () => {
        const [origen, destino] = boton.dataset.intercambiar.split(",").map((id) => document.getElementById(id));
        [origen.value, destino.value] = [destino.value, origen.value];
        destino.classList.remove("campo-invalido");
    });
});

// Formularios que piden confirmación (cerrar sesión, limpiar historial).
// Usan el cuadro propio del sitio (_DialogoConfirmacion) en lugar del confirm() del navegador.
const dialogo = document.getElementById("dialogoConfirmacion");
if (dialogo) {
    const ventana = new bootstrap.Modal(dialogo);
    const botonAceptar = document.getElementById("dialogoAceptar");
    let formularioPendiente = null;

    document.querySelectorAll("form[data-confirmar]").forEach((formulario) => {
        formulario.addEventListener("submit", (evento) => {
            evento.preventDefault();
            formularioPendiente = formulario;
            document.getElementById("dialogoTitulo").textContent = formulario.dataset.confirmarTitulo ?? "¿Está seguro?";
            document.getElementById("dialogoMensaje").textContent = formulario.dataset.confirmar;
            botonAceptar.textContent = formulario.dataset.confirmarBoton ?? "Aceptar";
            ventana.show();
        });
    });

    // Enfoca "Cancelar" al abrir, así Enter no confirma por accidente
    dialogo.addEventListener("shown.bs.modal", () => dialogo.querySelector("[data-bs-dismiss]").focus());

    botonAceptar.addEventListener("click", () => {
        botonAceptar.disabled = true;
        formularioPendiente?.submit();   // submit() no vuelve a disparar el evento: no se pregunta dos veces
    });
}

// Evita enviar dos veces el mismo formulario y muestra que se está procesando
document.querySelectorAll("form[data-bloquear-envio]").forEach((formulario) => {
    formulario.addEventListener("submit", () => {
        const boton = formulario.querySelector("button[type='submit']");
        if (boton) {
            boton.disabled = true;
            boton.textContent = formulario.dataset.bloquearEnvio;
        }
        document.body.style.cursor = "progress";
    });
});
