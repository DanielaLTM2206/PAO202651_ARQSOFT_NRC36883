using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using ec.edu.monster.Contratos;

namespace ec.edu.monster.Util;

/// <summary>
/// Direcciones con las que los clientes de otras computadoras de la red llegan a este servidor.
/// Se muestran al arrancar para saber qué IP escribir en "Configurar servidor" de cada cliente.
/// </summary>
public static class DireccionesRed
{
    /// <summary>
    /// IP de las tarjetas de red activas. Se prefieren las que tienen puerta de enlace (Wi-Fi, cable o el
    /// hotspot de un celular): así no aparecen los adaptadores virtuales (VirtualBox, Hyper-V), que no
    /// sirven para que otra computadora se conecte. Si ninguna tiene puerta de enlace, se muestran todas.
    /// </summary>
    public static IReadOnlyList<IPAddress> ObtenerIPv4Locales()
    {
        List<IPInterfaceProperties> interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(interfaz => interfaz.OperationalStatus == OperationalStatus.Up
                        && interfaz.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(interfaz => interfaz.GetIPProperties())
                .ToList();

        List<IPInterfaceProperties> conPuertaEnlace = interfaces
                .Where(propiedades => propiedades.GatewayAddresses.Any(puerta =>
                        puerta.Address.AddressFamily == AddressFamily.InterNetwork
                        && !puerta.Address.Equals(IPAddress.Any)))
                .ToList();

        return (conPuertaEnlace.Count > 0 ? conPuertaEnlace : interfaces)
                .SelectMany(propiedades => propiedades.UnicastAddresses)
                .Select(direccion => direccion.Address)
                .ToList();
    }

    /// <summary>
    /// Arma la URL del servicio para cada IPv4 de la red. Se descartan IPv6, loopback (127.x)
    /// y las direcciones automáticas 169.254.x, que no sirven para conectarse desde otra computadora.
    /// </summary>
    public static IReadOnlyList<string> CrearUrlsServicio(IEnumerable<IPAddress> direcciones, int puerto)
    {
        return direcciones
                .Where(EsUtilEnLaRed)
                .Select(direccion => $"http://{direccion}:{puerto}{ConstantesWS.Ruta}")
                .Distinct()
                .ToList();
    }

    /// <summary>
    /// Puerto de una dirección de escucha de Kestrel, por ejemplo http://[::]:5080 → 5080.
    /// </summary>
    public static int? ObtenerPuerto(string direccionEscucha)
    {
        int dosPuntos = direccionEscucha.LastIndexOf(':');
        string resto = dosPuntos >= 0 ? direccionEscucha[(dosPuntos + 1)..].TrimEnd('/') : string.Empty;
        return int.TryParse(resto, out int puerto) && puerto is > 0 and <= 65535 ? puerto : null;
    }

    private static bool EsUtilEnLaRed(IPAddress direccion)
    {
        if (direccion.AddressFamily != AddressFamily.InterNetwork || IPAddress.IsLoopback(direccion))
        {
            return false;
        }
        byte[] bytes = direccion.GetAddressBytes();
        return !(bytes[0] == 169 && bytes[1] == 254);
    }
}
