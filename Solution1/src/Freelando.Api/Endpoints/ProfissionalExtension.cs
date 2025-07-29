using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ProfissionalExtension
{
    public static void AddEndPointProfissional(this WebApplication app)
    {
        app.MapGet("/profissionais", async ([FromServices] ProfissionalConverter converter, [FromServices] FreelandoContext context) =>
        {
            var profissional = converter.EntityListToResponseList(context.Profissionais.Include(e => e.Especialidades).ToList());
            var entries = context.ChangeTracker.Entries();
            return Results.Ok(await Task.FromResult(profissional));
        }).WithTags("Profissional").WithOpenApi();

        app.MapPost("/profissional", async ([FromServices] ProfissionalConverter converter, [FromServices] FreelandoContext context, ProfissionalRequest profissionalRequest) =>
        {
            var profissional = converter.RequestToEntity(profissionalRequest);
            await context.Profissionais.AddAsync(profissional);
            await context.SaveChangesAsync();

            return Results.Created($"/profissional/{profissional.Id}", profissional);
        }).WithTags("Profissional").WithOpenApi();

        app.MapPut("/profissional/{id}", async ([FromServices] ProfissionalConverter converter, [FromServices] FreelandoContext context, Guid id, ProfissionalRequest profissionalRequest) =>
        {
            var profissional = await context.Profissionais.FindAsync(id);
            if(profissional is null)
            {
                return Results.NotFound();
            }
            var profissionalAtualizado = converter.RequestToEntity(profissionalRequest);
            profissional.Nome = profissionalAtualizado.Nome;
            profissional.Email = profissionalAtualizado.Email;
            profissional.Telefone = profissionalAtualizado.Telefone;
            profissional.Cpf = profissionalAtualizado.Cpf;
            profissional.Especialidades = profissionalAtualizado.Especialidades;
            await context.SaveChangesAsync();
            
            return Results.Ok(profissional);
        }).WithTags("Profissional").WithOpenApi();

        app.MapDelete("/profissional/{id}", async ([FromServices] ProfissionalConverter converter, [FromServices] FreelandoContext context, Guid id) =>
        {
            var profissional = await context.Profissionais.FindAsync(id);
            if(profissional is null)
            {
                return Results.NotFound();
            }
            context.Profissionais.Remove(profissional);
            await context.SaveChangesAsync();

            return Results.NoContent();
        }).WithTags("Profissional").WithOpenApi();
    }
}
