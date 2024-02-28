using CommandService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICommandRepo, CommandRepo>(); // whenever a component in the application requires an instance of ICommandRepo, the dependency injection container will provide a CommandRepo instance
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());  // automapper
builder.Services.AddControllers(); // add our controllers to our services injector
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemory")); // db


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

