using BoardGameShelf.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGroup("/health").MapHealthEndpoints();

app.MapGet("/", () => "Board Game Shelf");

app.Run();