TI.1.4. PAO202651 - SISTEMA DE CONVERSION DE UNIDADES (RESTFUL .NET 8 / ASP.NET CORE)
NRC: 36883 - GRUPO 08

================================================================================
NOTA DE ARQUITECTURA: SOLUCION SIN BASE DE DATOS (SINBDD)
================================================================================
Este proyecto ha sido disenado y construido bajo el requerimiento arquitectonico
estricto de operacion SIN BASE DE DATOS (SINBDD).

Razones por las cuales NO existe un Diagrama de Clases de Entidad / Base de Datos:
1. Inexistencia de Persistencia Relacional:
   El sistema no utiliza ningun motor de base de datos (PostgreSQL, SQL Server,
   MySQL, Oracle ni SQLite) ni frameworks de mapeo objeto-relacional (Entity Framework,
   Dapper o Hibernate).
2. Almacenamiento Volatil y Seguro en Memoria Central (.NET 8):
   - Catalogo de Unidades: Mapeado estaticamente en memoria dentro de las clases de
     dominio del Web API.
   - Autenticacion y Credenciales Multi-canal: La credencial activa ('_currentPassword')
     y las sesiones se administran mediante un diccionario concurrente hilo-seguro
     ConcurrentDictionary<string, UserSession> en el servicio AuthService.
   - Historial Global Compartido: Todas las conversiones exitosas se encolan
     dinamicamente en una estructura hilo-segura ConcurrentQueue<ConversionRecord>.
3. Sincronizacion Inmediata entre Clientes:
   Cualquier cambio de contrasena efectuado desde la aplicacion Web SPA, la consola CLI,
   el cliente de escritorio WPF o el cliente movil .NET MAUI afecta de manera atomica
   e instantanea a la instancia en memoria del servidor Kestrel.

Credenciales del Sistema:
- Usuario: monster
- Contrasena: monster9
