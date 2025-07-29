using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ProjetoExtension
{
    public static void AddEndPointProjeto(this WebApplication app)
    {
        app.MapGet("/projetos", async ([FromServices] ProjetoConverter converter, [FromServices] FreelandoContext context) =>
        {
            var projetos = converter.EntityListToResponseList(context.Projetos.Include(p => p.Cliente).Include(p => p.Especialidades).ToList());
            return Results.Ok(await Task.FromResult(projetos));
        }).WithTags("Projeto").WithOpenApi();

        app.MapPost("/projeto", async ([FromServices] ProjetoConverter converter, [FromServices] FreelandoContext context, ProjetoRequest projetoRequest) =>
        {
            var projeto = converter.RequestToEntity(projetoRequest);
            await context.Projetos.AddAsync(projeto);
            await context.SaveChangesAsync();

            return Results.Created($"/projeto/{projeto.Id}", projeto);
        }).WithTags("Projeto").WithOpenApi();

        app.MapPut("/projeto/{id}", async ([FromServices] ProjetoConverter converter, [FromServices] FreelandoContext context, Guid id, ProjetoRequest projetoRequest) =>
        {
            var projeto = await context.Projetos.FindAsync(projetoRequest);
            if(projeto is null)
            {
                return Results.NotFound();
            }
            var projetoAtualizado = converter.RequestToEntity(projetoRequest);
            projeto.Descricao = projetoAtualizado.Descricao;
            projeto.Titulo = projetoAtualizado.Titulo;
            projeto.Status = projetoAtualizado.Status;
            await context.SaveChangesAsync();

            return Results.Ok(projeto);
        }).WithTags("Projeto").WithOpenApi();

        app.MapDelete("/projeto/{id}", async ([FromServices] ProjetoConverter converter, [FromServices] FreelandoContext context, Guid id) =>
        {
            var projeto = await context.Projetos.FindAsync(id);
            if(projeto is null)
            {
                return Results.NotFound();
            }
            context.Projetos.Remove(projeto);
            await context.SaveChangesAsync();

            return Results.NoContent();
        }).WithTags("Projeto").WithOpenApi();
    }
}
