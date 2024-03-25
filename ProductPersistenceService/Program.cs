using Manonero.MessageBus.Kafka.Extensions;
using Microsoft.EntityFrameworkCore;
using ProductPersistenceServices;
using ProductPersistenceServices.BackgroundTasks;
using ProductPersistenceServices.Core.IServices;
using ProductPersistenceServices.Models;
using ProductPersistenceServices.Settings;
using ProductPersistenceServices.Core.Services;


var builder = Host.CreateApplicationBuilder(args);

var appSetting = AppSetting.MapValue(builder.Configuration);

builder.Services.AddHostedService<Worker>();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"));
//});

var optionsBuilder = new DbContextOptionsBuilder<ModelContext>();
optionsBuilder.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton<ModelContext>(new ModelContext(optionsBuilder.Options));

builder.Services.AddSingleton<IProductPersistenceService, ProductPersistenceService>();

builder.Services.AddKafkaConsumers(builder =>
{
    builder.AddConsumer<ProductPersistenceConsuming>(appSetting.GetConsumerSetting("1"));
});

var host = builder.Build();

host.UseKafkaMessageBus(mess =>
{
    mess.RunConsumer("1");
});

host.Run();
