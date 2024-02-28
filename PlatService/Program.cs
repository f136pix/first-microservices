using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;
using PlatService.AsyncDataServices.MessageBus;

var builder = WebApplication.CreateBuilder(args);

// managing dependence injection // auto-wiring
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services
    .AddHttpClient<ICommandDataClient, HttpCommandDataClient>(); // adding http client to the dependencies container
builder.Services.AddControllers(); // add our controllers to our services injector
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>(); // whenever IMessageBusClient is required, a singleton instance of MessageBusClient will be wired

var isDevelopment = builder.Environment.IsDevelopment();
if (isDevelopment)
{
    Console.WriteLine("--> Is Dev mode");
    Console.WriteLine("--> Local Mem DB");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemory"));
}
else
{
    Console.WriteLine("--> Is Production mode");
    Console.WriteLine("--> Sql Server DB");
    builder.Services.AddDbContext<AppDbContext>(
        opt => opt.UseSqlServer(
            "Server=mssql-clutsterip-srv,1433;Initial Catalog=platformsdb;User ID=sa;Password=pa55w0rd!;Trust Server Certificate=True"));
}

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
//app.UseHttpsRedirection();
app.MapControllers(); // maps our controllers to recieve htps req
PrepDb.PrepPopulation(app, isDevelopment);
app.Run();