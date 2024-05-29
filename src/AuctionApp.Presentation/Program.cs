using AuctionApp.Presentation.SignalR;
using Domain.Auth;
using Presentation;
using Presentation.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapSwagger().RequireAuthorization();
}

app.UseLogging();

app.UseExceptionHandling();

app.UseHttpsRedirection();

app.MapGroup("/Identity").WithTags("Identity").MapIdentityApi<User>();

app.MapHub<BidHub>("/BidsHub");

app.UseCors("ReactAppPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
