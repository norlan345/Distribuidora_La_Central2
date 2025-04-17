using Distribuidora_La_Central.Web.Components;
using Distribuidora_La_Central.Shared.Services;
using Distribuidora_La_Central.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configuraci�n para API y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();  // �Esto faltaba!
builder.Services.AddSwaggerGen();

// Add device-specific services
builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7263") });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())  // Corregido: quitamos el !
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();  // Mapeo de API controllers

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Distribuidora_La_Central.Shared._Imports).Assembly);

app.Run();