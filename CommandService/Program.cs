using CommandService.AsyncDataServices;
using CommandService.Data;
using CommandService.EventProcessing;
using Microsoft.EntityFrameworkCore;
using PlatService.AsyncDataServices.MessageBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICommandRepo, CommandRepo>(); // whenever a component in the application requires an instance of ICommandRepo, the dependency injection container will provide a CommandRepo instance
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());  // automapper
builder.Services.AddControllers(); // add our controllers to our services injector
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemory")); // db
builder.Services.AddSingleton<IEventProcessor, EventProcessor>(); 
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>(); // whenever IMessageBusClient is required, a singleton instance of MessageBusClient will be wired
builder.Services.AddHostedService<MessageBusSubscriber>(); // rabbitMq listener 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.MapControllers(); // maps our controllers to the http req

app.Run();

