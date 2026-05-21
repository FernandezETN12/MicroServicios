var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Simulamos una base de datos de usuarios con su saldo
var usuarios = new Dictionary<int, decimal>
{
    { 1, 1500.00m }, // El usuario 1 tiene $1500
    { 2, 200.00m }   // El usuario 2 tiene $200
};

// Endpoint para consultar el saldo de un usuario
app.MapGet("/api/usuarios/{id}/saldo", (int id) =>
{
    if (usuarios.TryGetValue(id, out var saldo))
    {
        return Results.Ok(new { UsuarioId = id, Saldo = saldo });
    }
    return Results.NotFound(new { Mensaje = "Usuario no encontrado" });
});

app.MapPost("/api/usuarios/{id}/debitar", (int id, decimal monto) =>
{
    if (usuarios.TryGetValue(id, out var saldo))
    {
        if (saldo >= monto)
        {
            usuarios[id] -= monto; // Debitamos el monto del saldo
            return Results.Ok(new { UsuarioId = id, SaldoActualizado = usuarios[id] });
        }
        return Results.BadRequest(new { Mensaje = "Saldo insuficiente" });
    }
    return Results.NotFound(new { Mensaje = "Usuario no encontrado" });
});

app.Run("http://localhost:5001");

// Modelos locales para mapear los JSON
public record DebitoRequest(decimal Monto);