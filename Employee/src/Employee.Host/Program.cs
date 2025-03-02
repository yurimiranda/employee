using Carter;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureHost();
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerService();
}

app.UseExceptionHandler(Log.Logger);
app.UseLogs();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();

await app.RunAsync();