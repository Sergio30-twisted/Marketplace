using OrderService.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<OrderValidator>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<PaymentOrchestrator>();
builder.Services.AddScoped<OrderStateManager>();
builder.Services.AddScoped<EventPublisher>();
builder.Services.AddScoped<OrderProcessor>(); 
builder.Services.AddHttpClient<InventoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();