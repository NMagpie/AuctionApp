using Application.Abstractions;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddSingleton<AuctionAppDbContext, AuctionAppDbContext>()
    .AddSingleton<IRepository, EntityRepository>()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IRepository).Assembly));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
