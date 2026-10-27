# Cliente de Escritorio WPF - ConUni Monsters Inc (GR08)

Cliente de escritorio nativo Windows desarrollado en **C# con .NET 8 y Windows Presentation Foundation (WPF)** para el sistema multi-canal de conversión de unidades RESTful.

---

## 1. Características Implementadas

- **Autenticación Centralizada REST:**
  - Login interactivo consumiendo `POST /api/auth/login` del servidor backend.
  - Almacenamiento seguro del Bearer Token JWT en memoria para todas las peticiones posteriores.
  - Credenciales iniciales: Usuario: `MONSTER`, Contraseña: `MONSTER9`.
  - Diálogo modal para **Cambio de Contraseña** consumiendo `POST /api/auth/cambiar-clave`.
- **Servicios de Conversión (3 Paneles):**
  - **Longitud:** Millas, Metros, Pulgadas, Kilómetros, Pies, Yardas.
  - **Masa:** Kilogramos, Libras, Onzas, Gramos.
  - **Temperatura:** Celsius, Fahrenheit, Kelvin.
- **Validaciones Rigurosas:**
  - Validación de campos requeridos y formatos numéricos.
  - **Regla VAL_007:** Rechazo de valores que superen 15 dígitos o el rango operativo de 1 billón con mensaje de error explícito y controlado.
- **Historial Sincronizado en Memoria (RF-06 / RF-07):**
  - DataGrid conectado a `GET /api/historial`.
  - Actualización automática tras cada conversión exitosa.
  - Botón de refresco manual y botón para limpieza global del historial (`DELETE /api/historial`).
- **Diseño Visual:**
  - Paleta institucional oscura (`#0F172A`, `#151B28`, `#3B82F6`, `#334155`).
  - Sin emojis (estrictamente texto técnico, badges e iconos corporativos).
  - Avatar de Sulley / Monsters Inc incrustado como recurso ensamblado.

---

## 2. Requisitos de Ejecución

1. .NET 8 SDK instalado en Windows.
2. Servidor RESTful activo en `http://localhost:5000`.

---

## 3. Modo de Ejecución

### Opción A (Directa con script):
Hacer doble clic en:
```
04. CLIESC\ejecutar_cliente_escritorio.bat
```

### Opción B (Línea de comandos):
```powershell
cd "04. CLIESC\ConUni_CliEsc_GR08"
dotnet run
```
