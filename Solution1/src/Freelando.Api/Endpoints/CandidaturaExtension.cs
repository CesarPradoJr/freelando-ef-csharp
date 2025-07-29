using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class CandidaturaExtension
{
    public static void AddEndPointCandidatura(this WebApplication app)
    {
        app.MapGet("/candidaturas", async ([FromServices] CandidaturaConverter converter, [FromServices] FreelandoContext context) =>
        {
            var candidatura = converter.EntityListToResponseList(context.Candidaturas.ToList());
            var entries = context.ChangeTracker.Entries();
            return Results.Ok(await Task.FromResult(candidatura));
        }).WithTags("Candidatura").WithOpenApi();

        app.MapPost("/candidatura", async ([FromServices] CandidaturaConverter converter, [FromServices] FreelandoContext context, CandidaturaRequest candidaturaRequest) =>
        {
            var candidatura = converter.RequestToEntity(candidaturaRequest);
            await context.Candidaturas.AddAsync(candidatura);
            await context.SaveChangesAsync();

            return Results.Created($"/candidatura/{candidatura.Id}", candidatura);
        }).WithTags("Candidatura").WithOpenApi();

        app.MapPut("/candidatura/{id}", async ([FromServices] CandidaturaConverter converter, [FromServices] FreelandoContext context, Guid id, CandidaturaRequest candidaturaRequest) =>
        {
            var candidatura = await context.Candidaturas.FindAsync(id);
            if (candidatura is null)
            {
                return Results.NotFound();
            }
            var candidaturaAtualizada = converter.RequestToEntity(candidaturaRequest);
            candidatura.Servico = candidaturaAtualizada.Servico;
            candidatura.ValorProposto = candidaturaAtualizada.ValorProposto;
            candidatura.DescricaoProposta = candidaturaAtualizada.DescricaoProposta;
            candidatura.DuracaoProposta = candidaturaAtualizada.DuracaoProposta;
            candidatura.Status = candidaturaAtualizada.Status;

            await context.SaveChangesAsync();
            
            return Results.Ok(candidatura);
        }).WithTags("Candidatura").WithOpenApi();

        app.MapDelete("/candidatura/{id}", async ([FromServices] CandidaturaConverter converter, [FromServices] FreelandoContext context, Guid id) =>
        {
            var candidatura = await context.Candidaturas.FindAsync(id);
            if (candidatura is null)
            {
                return Results.NotFound();
            }
            context.Candidaturas.Remove(candidatura);
            await context.SaveChangesAsync();

            return Results.NoContent();
        }).WithTags("Candidatura").WithOpenApi();
    }
}
