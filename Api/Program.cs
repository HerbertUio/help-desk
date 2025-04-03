using Api.Endpoints;
using Application.Validators;
using FluentValidation;
using Infrastructure.Ioc.Di;

var builder = WebApplication.CreateBuilder(args);

// --- Registro de Dependencias ---
builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>(ServiceLifetime.Scoped);
// --- Fin Registro ---

// --- Servicios API ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( /* ... Opciones Swagger ... */);
// --- Fin Servicios API ---

// --- Autenticación / Autorización ---
// TODO: Configurar JWT aquí si es necesario para otros endpoints
// builder.Services.AddAuthentication(...).AddJwtBearer(...);
// builder.Services.AddAuthorization();
// --- Fin Auth ---

var app = builder.Build();

// --- Pipeline HTTP ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
// app.UseAuthentication(); // Descomentar si configuras JWT
// app.UseAuthorization(); // Descomentar si configuras JWT
// --- Fin Pipeline ---


// --- Mapeo de Endpoints ---
// Aquí es donde se usan los métodos de extensión de las clases estáticas CORRECTAMENTE
app.MapAuthEndpoints();
app.MapUserEndpoints();
// --- Fin Mapeo ---

app.Run();