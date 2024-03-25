using Manonero.MessageBus.Kafka.Extensions;
using Microsoft.EntityFrameworkCore;
using ProductServices.BackgroundTasks;
using ProductServices.Core;
using ProductServices.Core.InMemory;
using ProductServices.Core.IServices;
using ProductServices.Core.Services;
using ProductServices.Extensions;
using ProductServices.Models;
using ProductServices.Settings;


var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

var appSetting = AppSetting.MapValue(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ModelContext>(options =>
{
    options.UseOracle(connection);
});

builder.Services.AddSingleton<ProductMemory>();

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IProductService, ProductService>();

builder.Services.AddKafkaConsumers(builder =>
{
    builder.AddConsumer<ProductConsumingTask>(appSetting.GetConsumerSetting("0"));
});

builder.Services.AddKafkaProducers(builder =>
{
    builder.AddProducer(appSetting.GetProducerSetting("1"));
});

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

app.LoadDataToMemory<ProductMemory, ModelContext>((data, context) =>
{
    new TableProductMemorySeedAsync().SeedAsync(data, context).Wait();
});

app.UseKafkaMessageBus(mess =>
{
    mess.RunConsumer("0");
});

app.Run();
