using Catologo.API.Application.Service;
using Catologo.API.Data.Interface;
using Catologo.API.Data;
using Catologo.API.Infrastructure.Interface;
using Catologo.API.Infrastructure.Repository;
using Catologo.API.Application.Interface.Catalogo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMarketPlace_DBContext, MarketPlace_DBContext>();

//CONEXIÓN SERVICE PRODUCTO
builder.Services.AddScoped<IConsultaDeCatalogoService, ConsultaDeCatalogoService>();

//CONEXIÓN REPOSITORY PRODUCTO
builder.Services.AddScoped<IProductoRepository, ProductRepository>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
