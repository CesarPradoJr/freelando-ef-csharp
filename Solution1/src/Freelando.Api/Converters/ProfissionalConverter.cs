using Freelando.Api.Requests;
using Freelando.Api.Responses;
using Freelando.Modelo;

namespace Freelando.Api.Converters;

public class ProfissionalConverter
{
    private EspecialidadeConverter? _especialidadeConverter;
    private ContratoConverter? _contratoConverter;
    public ProfissionalResponse EntityToResponse(Profissional? profissional)
    {
        _especialidadeConverter = new EspecialidadeConverter();
        _contratoConverter = new ContratoConverter();
        if (profissional == null)
        {
            return new ProfissionalResponse(Guid.Empty, null, null, null, null, new List<EspecialidadeResponse>());
        }


        return new ProfissionalResponse(profissional.Id, profissional.Nome, profissional.Cpf, profissional.Email, profissional.Telefone, _especialidadeConverter.EntityListToResponseList(profissional.Especialidades));
    }

    public Profissional RequestToEntity(ProfissionalRequest? profissionalRequest)
    {
        if (profissionalRequest == null)
        {
            return new Profissional(Guid.Empty, null, null, null, null, new List<Especialidade>(), new List<Contrato>());
        }

        return new Profissional(profissionalRequest.Id, profissionalRequest.Nome, profissionalRequest.Cpf, profissionalRequest.Email, profissionalRequest.Telefone, _especialidadeConverter.RequestListToEntityList(profissionalRequest.Especialidades), null);
    }

    public ICollection<ProfissionalResponse> EntityListToResponseList(IEnumerable<Profissional> profissionais)
    {
        return (profissionais == null)
            ? new List<ProfissionalResponse>()
            : profissionais.Select(a => EntityToResponse(a)).ToList();
    }

    public ICollection<Profissional> RequestListToEntityList(IEnumerable<ProfissionalRequest> profissionalRequests)
    {
        if (profissionalRequests == null)
        {
            return new List<Profissional>();
        }

        return profissionalRequests.Select(a => RequestToEntity(a)).ToList();
    }
}