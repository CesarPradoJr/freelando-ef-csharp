
using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Freelando.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class EspecialidadeExtension
{
    public static void AddEndPointEspecialidades(this WebApplication app)
    {
        app.MapGet("/especialidades", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext context) =>
        {
            var especialidades = converter.EntityListToResponseList(context.Especialidades.ToList());
            return Results.Ok(especialidades);
        }).WithTags("Especialidade").WithOpenApi();

        app.MapPost("/especialidade", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext context, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = converter.RequestToEntity(especialidadeRequest);

            await context.Especialidades.AddAsync(especialidade);
            await context.SaveChangesAsync();

            return Results.Created($"/especialidade/{especialidade.Id}", especialidade);
        }).WithTags("Especialidade").WithOpenApi();

        app.MapPut("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext context, Guid id, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = await context.Especialidades.FindAsync(id);
            if (especialidade is null)
            {
                return Results.NotFound();
            }
            var especialidadeAtualizada = converter.RequestToEntity(especialidadeRequest);
            especialidade.Descricao = especialidadeAtualizada.Descricao;
            especialidade.Projetos = especialidadeAtualizada.Projetos;
            await context.SaveChangesAsync();

            return Results.Ok(especialidade);
        }).WithTags("Especialidade").WithOpenApi();

        app.MapDelete("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext context, Guid id) =>
        {
            var especialidade = await context.Especialidades.FindAsync(id);
            if (especialidade is null)
            {
                return Results.NotFound();
            }

            context.Especialidades.Remove(especialidade);
            await context.SaveChangesAsync();

            return Results.NoContent();
        }).WithTags("Especialidade").WithOpenApi();
    }
}
