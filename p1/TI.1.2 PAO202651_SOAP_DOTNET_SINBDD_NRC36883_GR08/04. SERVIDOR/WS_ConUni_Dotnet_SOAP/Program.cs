using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using ec.edu.monster.Contratos;
using ec.edu.monster.Excepciones;
using ec.edu.monster.Seguridad;
using ec.edu.monster.Servicios;
using ec.edu.monster.Util;

// Escucha en todas las interfaces de red (appsettings.json → Kestrel → http://*:5080):
// los clientes de esta PC usan localhost y los de otras computadoras usan la IP que se muestra al arrancar
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();

// La dirección del WSDL usa el host de la petición (localhost en la PC, 10.0.2.2 en el emulador)
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

// Comportamientos propios: validación del token (cabecera SOAP) y traducción de errores a SOAP Faults
builder.Services.AddSingleton<IServiceBehavior, InspectorAutenticacion>();
builder.Services.AddSingleton<IServiceBehavior, ManejadorErrores>();

var app = builder.Build();

app.UseServiceModel(servicios =>
{
    var enlace = new BasicHttpBinding { Namespace = ConstantesWS.Espacio };

    servicios.AddService<WSConversorUnidades>();
    servicios.AddServiceEndpoint<WSConversorUnidades, IWSConversorUnidades>(enlace, ConstantesWS.Ruta);

    // Publica el WSDL en http://localhost:5080/WSConversorUnidades.svc?wsdl
    var metadatos = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    metadatos.HttpGetEnabled = true;
});

app.Lifetime.ApplicationStarted.Register(() =>
{
    int puerto = app.Urls.Select(DireccionesRed.ObtenerPuerto).FirstOrDefault(p => p is not null) ?? 5080;
    IReadOnlyList<string> urls = DireccionesRed.CrearUrlsServicio(DireccionesRed.ObtenerIPv4Locales(), puerto);

    app.Logger.LogInformation("Clientes de esta computadora: http://localhost:{Puerto}{Ruta}", puerto, ConstantesWS.Ruta);
    foreach (string url in urls)
    {
        app.Logger.LogInformation("Clientes de otras computadoras de la red: {Url}", url);
    }
    if (urls.Count == 0)
    {
        app.Logger.LogWarning("No se encontró una IP de red: conecte la computadora al Wi-Fi o al cable de red.");
    }
});

app.Run();
