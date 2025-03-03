var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureHost();
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();
app.ConfigureApp();
await app.RunAsync();