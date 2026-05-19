using APIGateWay_MarketPlace.Hubs;
using APIGateWay_MarketPlace.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. CORS para Vite
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVite",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuramos SignalR de forma elástica para conexiones WebSocket directas
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddHostedService<EventBusConsumer>();

var app = builder.Build();

app.UseCors("AllowVite");

// Ajuste clave: Mapeamos los controladores antes del Hub para que las rutas HTTP 
// no intenten buscar un fallback seguro en HTTPS
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificacionesHub>("/hub/notificaciones");
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🛠️ FORZAR ÚNICAMENTE HTTP EN EL PUERTO 7013 (Elimina el puerto HTTPS 7003 de la memoria)
app.Urls.Add("http://*:7013");

app.Run();