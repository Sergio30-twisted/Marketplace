using Auditoria_y_Trazabilidad.Application.Interface;
using Auditoria_y_Trazabilidad.Application.Service;
using Auditoria_y_Trazabilidad.Data;
using Auditoria_y_Trazabilidad.Data.Interface;
using Auditoria_y_Trazabilidad.Infrastructure.Interface;
using Auditoria_y_Trazabilidad.Infrastructure.Repository;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    });

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(
                new System.Text.Json.Serialization.JsonStringEnumConverter());
        });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new()
        {
            Title = "Microservicio Auditoría y Trazabilidad",
            Version = "v1",
            Description = "Receptor de logs, gestión de auditoría, trazabilidad distribuida y verificación de integridad."
        });
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
    });

    builder.Services.AddHttpContextAccessor();

    // ── Infraestructura ──────────────────────────────────────────────────────
    builder.Services.AddSingleton<IAuditoria_DBContext, Auditoria_DBContext>();
    builder.Services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();

    // ── Aplicación ───────────────────────────────────────────────────────────
    builder.Services.AddScoped<IEnriquecedorDeContextoService, EnriquecedorDeContextoService>();
    builder.Services.AddScoped<IVerificadorDeIntegridadService, VerificadorDeIntegridadService>();
    builder.Services.AddScoped<IReceptorDeLogsService, ReceptorDeLogsService>();
    builder.Services.AddScoped<IGestorDeAuditoriaService, GestorDeAuditoriaService>();
    builder.Services.AddScoped<IConsultorDeTrazabilidadService, ConsultorDeTrazabilidadService>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowAll");
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("=== ERROR AL INICIAR MICROSERVICIO DE AUDITORÍA ===");
    Console.WriteLine(ex.ToString());
    Console.ReadKey();
}
