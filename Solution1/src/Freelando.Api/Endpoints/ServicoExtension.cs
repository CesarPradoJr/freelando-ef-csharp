using Freelando.Api.Converters;
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
    }
}
