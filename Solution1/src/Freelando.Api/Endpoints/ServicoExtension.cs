using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ServicoExtensions
{
    public static void AddEndPointServico(this WebApplication app)
    {
        app.MapGet("/servicos", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext context) =>
        {
            var servico = converter.EntityListToResponseList(context.Servicos.ToList());
            return Results.Ok(await Task.FromResult(servico));
        }).WithTags("Servico").WithOpenApi();

        app.MapPost("/servico", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext context, ServicoRequest servicoRequest) =>
        {
            var servico = converter.RequestToEntity(servicoRequest);
            await context.Servicos.AddAsync(servico);
            await context.SaveChangesAsync();

            return Results.Created($"/servico/{servico.Id}", servico);
        }).WithTags("Servico").WithOpenApi();

        app.MapPut("/servico/{id}", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext context, Guid id, ServicoRequest servicoRequest) =>
        {
            var servico = await context.Servicos.FindAsync(id);
            if(servico is null)
            {
                return Results.NotFound();
            }
            var servicoAtualizado = converter.RequestToEntity(servicoRequest);
            servico.Descricao = servicoAtualizado.Descricao;
            servico.Titulo =servicoAtualizado.Titulo;
            await context.SaveChangesAsync();

            return Results.Ok(await Task.FromResult(servico));
        }).WithTags("Servico").WithOpenApi();

        app.MapDelete("/servico/{id}", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext context, Guid id) =>
        {
            var servico = await context.Servicos.FindAsync(id);
            if(servico is null)
            {
                return Results.NotFound();
            }
            context.Servicos.Remove(servico);
            await context.SaveChangesAsync();

            return Results.NoContent();
        }).WithTags("Servico").WithOpenApi();
    }
}
