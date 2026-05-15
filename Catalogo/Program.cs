using Catalogo.Application.Interface;
using Catalogo.Application.Service;
using Catalogo.Data;
using Catalogo.Infrastructure.Interface;
using Catalogo.Infrastructure.Messaging;
using Catalogo.Infrastructure.Repository;
using Catalogo.Services;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowVite", policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddGrpc();

    builder.Services.AddSingleton<IMarketPlace_DBContext, MarketPlace_DBContext>();
    builder.Services.AddScoped<IConsultaCatalogoRepository, ConsultaCatalogoRepository>();
    builder.Services.AddScoped<IConsultaDeCatalogoService, ConsultaDeCatalogoService>();
    builder.Services.AddScoped<ICreacionCatalogoRepository, CreacionCatalogoRepository>();
    builder.Services.AddScoped<IGestorDeCategoriaService, GestorDeCategoriaService>();
    builder.Services.AddScoped<IAdministracionDeCatalogosService, AdministracionDeCatalogosService>();
    builder.Services.AddHostedService<CatalogoConsumer>();

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors("AllowVite");
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.MapGrpcService<CatalogoGrpcService>();
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("=== ERROR AL INICIAR ===");
    Console.WriteLine(ex.ToString());
    Console.ReadKey();
}