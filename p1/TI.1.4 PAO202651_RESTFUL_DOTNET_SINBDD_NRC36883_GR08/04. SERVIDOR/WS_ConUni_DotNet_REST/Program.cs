using Microsoft.OpenApi.Models;
using WS_ConUni_DotNet_REST.Controllers;
using WS_ConUni_DotNet_REST.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS para soportar Web (SPA/JSP/HTML), Móvil y Escritorio
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Controladores
builder.Services.AddControllers();

// Inyección de Dependencias (Servicios del Sistema)
builder.Services.AddSingleton<IServicioSeguridad, ServicioSeguridad>();
builder.Services.AddSingleton<ICatalogoService, CatalogoService>();
builder.Services.AddSingleton<IGestorHistorial, GestorHistorial>();

builder.Services.AddScoped<LongitudService>();
builder.Services.AddScoped<MasaService>();
builder.Services.AddScoped<TemperaturaService>();

// Inyección de controladores secundarios para ConversionController
builder.Services.AddScoped<LongitudController>();
builder.Services.AddScoped<MasaController>();
builder.Services.AddScoped<TemperaturaController>();

// Swagger / OpenAPI con soporte para Token de Autenticación
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API RESTful Conversor de Unidades - Monsters Inc (GR08)",
        Version = "v1",
        Description = "Servicio Web RESTful en C# .NET 8 sin base de datos (SINBDD) para conversión de unidades físicas, autenticación en memoria y sincronización de historial global.",
        Contact = new OpenApiContact
        {
            Name = "Grupo 08 - Arquitectura de Software",
            Email = "gr08@monster.edu.ec"
        }
    });

    // Definición de seguridad Bearer Token para pruebas en Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Encabezado de autorización REST. Ingrese 'Bearer' [espacio] y luego su token (ej: Bearer mst_...).",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Habilitar Swagger en desarrollo y producción para pruebas universitarias
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API ConUni REST v1 (GR08)");
    options.RoutePrefix = string.Empty; // Cargar Swagger directamente en la raíz (http://localhost:PUERTO/)
});

app.UseCors("AllowAll");

var webClientPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "..", "04. CLIWEB"));
if (Directory.Exists(webClientPath))
{
    app.UseFileServer(new FileServerOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(webClientPath),
        RequestPath = "/app",
        EnableDefaultFiles = true
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
