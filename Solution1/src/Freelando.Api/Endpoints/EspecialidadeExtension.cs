
using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Freelando.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        app.MapGet("/especialidades/{letraInicial}", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext context, string letraInicial) =>
        {
            Expression<Func<Especialidade, bool>> filtroExpression = null;

            if (letraInicial.Length == 1 && char.IsUpper(letraInicial[0]))
            {
                filtroExpression = especialidade => especialidade.Descricao.StartsWith(letraInicial);
            }

            IQueryable<Especialidade> especialidades = context.Especialidades;
            if (filtroExpression != null)
            {
                especialidades = especialidades.Where(filtroExpression);
            }

            return await especialidades.ToListAsync();
        }).WithTags("Especialidade").WithOpenApi();

        app.MapPost("/especialidade", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext context, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = converter.RequestToEntity(especialidadeRequest);

            Func<Especialidade, bool> validarDescricao = especialidade => !string.IsNullOrEmpty(especialidade.Descricao) && char.IsUpper(especialidade.Descricao[0]);

            if (!validarDescricao(especialidade))
            {
                return Results.BadRequest("A descrição não pode estar em branco e deve começar com letra maiúscular!");
            }

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
