using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;

namespace Freelando.Api.Endpoints;

public static class ClienteExtension
{
    public static void AddEndPointClientes(this WebApplication app)
    {
        app.MapGet("/clientes", async ([FromServices] ClienteConverter converter, [FromServices] FreelandoContext context) =>
        {
            var clientes = converter.EntityListToResponseList(context.Clientes.ToList());
            var entries = context.ChangeTracker.Entries();
            return Results.Ok(await Task.FromResult(clientes));
        }).WithTags("Cliente").WithOpenApi();

        app.MapPost("/cliente", async ([FromServices] ClienteConverter converter, [FromServices] FreelandoContext context, ClienteRequest clienteRequest) =>
        {
            var cliente = converter.RequestToEntity(clienteRequest);
            await context.Clientes.AddAsync(cliente);
            await context.SaveChangesAsync();

            return Results.Created($"/cliente/{cliente.Id}", cliente);
        }).WithTags("Cliente").WithOpenApi();

        app.MapPut("/clientes/{id}", async ([FromServices] ClienteConverter converter, [FromServices] FreelandoContext context, Guid id, ClienteRequest clienteRequest) =>
        {
            var cliente = await context.Clientes.FindAsync(id);
            if(cliente is null)
            {
                return Results.NotFound();
            }
            var clienteAtualizado = converter.RequestToEntity(clienteRequest);
            cliente.Nome = clienteAtualizado.Nome;
            cliente.Email = clienteAtualizado.Email;
            cliente.Telefone = clienteAtualizado.Telefone;
            cliente.Cpf = clienteAtualizado.Cpf;

            await context.SaveChangesAsync();

            return Results.Ok(cliente);
        }).WithTags("Cliente").WithOpenApi();

        app.MapDelete("/cliente/{id}", async ([FromServices] ClienteConverter converter, [FromServices] FreelandoContext context, Guid id) =>
        {
            var cliente = await context.Clientes.FindAsync(id);
            if ( cliente is null)
            {
                return Results.NotFound();
            }
            context.Clientes.Remove(cliente);
            await context.SaveChangesAsync();

            return Results.NoContent();
        }).WithTags("Cliente").WithOpenApi();
    }
}
