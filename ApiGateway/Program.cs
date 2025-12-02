using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Carrega o ocelot.json
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Adiciona Ocelot
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => Results.Json(new { message = "API Gateway (Ocelot) is running" }));

await app.UseOcelot();

app.Run();