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

// 2. Necesitamos HttpClient para que el Gateway pueda "llamar" al Catálogo
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddHostedService<EventBusConsumer>();

var app = builder.Build();
app.MapHub<NotificacionesHub>("/hub/notificaciones");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowVite");
app.UseAuthorization();
app.MapControllers();

app.Run();