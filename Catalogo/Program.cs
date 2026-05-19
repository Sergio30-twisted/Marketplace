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

    // Elimina toda la configuración de CORS
    // builder.Services.AddCors(...);

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddGrpc();
    builder.WebHost.UseUrls("http://0.0.0.0:5110", "http://0.0.0.0:5112");

    builder.Services.AddSingleton<IMarketPlace_DBContext, MarketPlace_DBContext>();
    builder.Services.AddScoped<IConsultaCatalogoRepository, ConsultaCatalogoRepository>();
    builder.Services.AddScoped<IConsultaDeCatalogoService, ConsultaDeCatalogoService>();
    builder.Services.AddScoped<ICreacionCatalogoRepository, CreacionCatalogoRepository>();
    builder.Services.AddScoped<IGestorDeCategoriaService, GestorDeCategoriaService>();
    builder.Services.AddScoped<IAdministracionDeCatalogosService, AdministracionDeCatalogosService>();
    builder.Services.AddHostedService<CrearCatalogoConsumer>();

    var app = builder.Build();

    app.MapGrpcService<CatalogoGrpcService>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    // Elimina app.UseCors("AllowVite");
    //app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("=== ERROR AL INICIAR ===");
    Console.WriteLine(ex.ToString());
    Console.ReadKey();
}
