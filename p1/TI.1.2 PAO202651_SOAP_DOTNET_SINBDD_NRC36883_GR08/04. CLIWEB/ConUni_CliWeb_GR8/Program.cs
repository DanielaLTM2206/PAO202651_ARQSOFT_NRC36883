using ec.edu.monster.Cliente;
using ec.edu.monster.Servicios;
using ec.edu.monster.Sesion;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Sesión del navegador: guarda el token y el usuario de cada persona (sin base de datos)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opciones =>
{
    opciones.IdleTimeout = TimeSpan.FromMinutes(30);
    // Nombre propio: la web RESTful (.ConUni.Sesion) también corre en localhost y no deben mezclarse
    opciones.Cookie.Name = ".ConUniSoap.Sesion";
    opciones.Cookie.HttpOnly = true;
    opciones.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AlmacenSesion>();
builder.Services.AddScoped<IAlmacenToken>(proveedor => proveedor.GetRequiredService<AlmacenSesion>());
builder.Services.AddScoped<ManejadorTokenSOAP>();

// Cliente del servicio SOAP .NET (la URL está en appsettings.json → ServidorSoap:Url)
builder.Services.AddScoped<IContratoWSConversor, ClienteWSConversor>();

// Lógica de la aplicación: validaciones y llamadas al servicio
builder.Services.AddScoped<ServicioCuenta>();
builder.Services.AddScoped<ServicioConversion>();

// Todos los formularios POST exigen el token antifalsificación
builder.Services.AddControllersWithViews(opciones =>
{
    opciones.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Cuenta/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

// La página inicial es el conversor; sin sesión, el filtro RequiereSesion redirige al inicio de sesión
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Conversor}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
