using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ContratoExtension
{
    public static void AddEndPointContrato(this WebApplication app)
    {
        app.MapGet("/contratos", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext context) =>
        {
            var contrato = converter.EntityListToResponseList(context.Contratos.ToList());
            var entries = context.ChangeTracker.Entries();
            return Results.Ok(await Task.FromResult(contrato));
        }).WithTags("Contrato").WithOpenApi();

        app.MapPost("/contrato", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext context, ContratoRequest contratoRequest) =>
        {
            var contrato = converter.RequestToEntity(contratoRequest);
            await context.Contratos.AddAsync(contrato);
            await context.AddRangeAsync();
            
            return Results.Created($"/contrato/{contrato.Id}", contrato);
        }).WithTags("Contrato").WithOpenApi();

        app.MapPut("/contrato/{id}", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext context, Guid id, ContratoRequest contratoRequest) =>
        {
            var contrato = await context.Contratos.FindAsync(id);
            if(contrato is null)
            {
                return Results.NotFound();
            }
            var contratoAtualizado = converter.RequestToEntity(contratoRequest);
            contrato.Valor = contratoAtualizado.Valor;
            await context.SaveChangesAsync();
            
            return Results.Ok(contrato);
        }).WithTags("Contrato").WithOpenApi();

        app.MapDelete("/contrato/{id}", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext context, Guid id) =>
        {
            var contrato = await context.Contratos.FindAsync(id);
            if(contrato is null)
            {
                return Results.NotFound();
            }
            context.Contratos.Remove(contrato);
            await context.SaveChangesAsync();

            return Results.NoContent();
        }).WithTags("Contrato").WithOpenApi();
    }
}
