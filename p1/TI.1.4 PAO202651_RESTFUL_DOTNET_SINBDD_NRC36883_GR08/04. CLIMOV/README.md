# Cliente Móvil .NET MAUI - ConUni Monsters Inc (GR08)

Cliente móvil multi-plataforma desarrollado en **C# con .NET MAUI 8 (Android / Windows)** para el sistema multi-canal de conversión de unidades RESTful.

---

## 1. Características Implementadas

- **Autenticación Centralizada REST con JWT:**
  - Login interactivo consumiendo `POST /api/auth/login` del servidor backend.
  - Almacenamiento seguro del Bearer Token JWT en el cliente HTTP para todas las peticiones posteriores.
  - Credenciales iniciales: Usuario: `MONSTER`, Contraseña: `MONSTER9`.
  - Diálogo modal para **Cambio de Contraseña** consumiendo `POST /api/auth/cambiar-clave`.
- **Servicios de Conversión:**
  - **Longitud:** Millas, Metros, Pulgadas, Kilómetros, Pies, Yardas.
  - **Masa:** Kilogramos, Libras, Onzas, Gramos.
  - **Temperatura:** Celsius, Fahrenheit, Kelvin.
- **Validaciones Rigurosas:**
  - Validación de campos requeridos y formatos numéricos.
  - **Regla VAL_007:** Rechazo de valores que superen 15 dígitos o el rango operativo de 1 billón con tarjeta de error controlada.
- **Historial Sincronizado en Memoria (RF-06 / RF-07):**
  - `CollectionView` conectado a `GET /api/historial`.
  - Actualización automática tras cada conversión exitosa.
  - Botón de refresco manual y botón para limpieza global del historial (`DELETE /api/historial`).
- **Diseño Visual:**
  - Paleta institucional oscura (`#0F172A`, `#151B28`, `#1E293B`, `#2563EB`, `#38BDF8`).
  - Sin emojis (estrictamente texto técnico, badges e iconos corporativos).
  - Avatar de Sullivan / Monsters Inc incrustado como recurso ensamblado.
- **Configuración de Red Móvil:**
  - Campo configurable de Host/IP en la pantalla de inicio de sesión.
  - Para el **Emulador Android**, la dirección IP por defecto es `10.0.2.2` (apunta automáticamente al `localhost:5000` de Windows).
  - Para dispositivos físicos en la misma red Wi-Fi, se puede colocar la IP de la máquina (ej: `192.168.1.X`).

---

## 2. Cómo Abrir y Ejecutar en Visual Studio ("el morado")

1. Asegúrate de tener activa la carga de trabajo **"Desarrollo con .NET MAUI"** en tu instalador de Visual Studio 2022.
2. Abre la solución haciendo doble clic directo en:
   ```
   04. CLIMOV\ConUni_CliMov_GR08.sln
   ```
3. En la barra superior de Visual Studio, selecciona el dispositivo de destino:
   - **Emulador Android** (ej: *Pixel 5 - API 34* o similar).
   - O **Equipo local (Windows)** para ejecutarlo directamente en modo ventana.
4. Presiona **F5** (o el botón verde de Iniciar ▶️).
5. En la pantalla de login:
   - **Servidor API:** `10.0.2.2` (si usas emulador Android) o `localhost` (si ejecutas en Windows).
   - **Usuario:** `MONSTER`
   - **Contraseña:** `MONSTER9`
