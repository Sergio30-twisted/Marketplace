using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. CONFIGURACIÓN DE SERVICIOS (CONTENEDOR DE DEPENDENCIAS)
// =========================================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🛠️ REGISTROS DE INYECCIÓN DE DEPENDENCIAS (Resuelve toda la cadena de activación)
builder.Services.AddScoped<Logisitica_Integrada.Services.ShipmentRepository>();
builder.Services.AddScoped<Logisitica_Integrada.Services.LogisticsNotifier>();
builder.Services.AddScoped<Logisitica_Integrada.Services.ShipmentManager>();

// Construcción de la aplicación (Ya no arrojará la excepción AggregateException)
var app = builder.Build();

// =========================================================================
// 2. PIPELINE DE PETICIONES HTTP (MIDDLEWARES)
// =========================================================================

// Configuración de Swagger visible en cualquier entorno (Esencial para Docker)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Logistica API v1");
    c.RoutePrefix = string.Empty; // Hace que la documentación sea la página de inicio del contenedor
});

// ⚠️ Recordatorio: UseHttpsRedirection() se mantiene comentado para evitar bucles infinitos con NGINX
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();