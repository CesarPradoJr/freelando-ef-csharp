﻿using Freelando.Api.Requests;
using Freelando.Api.Responses;
using Freelando.Modelo;

namespace Freelando.Api.Converters;

public class ContratoConverter
{
    private ServicoConverter? _servicoConvert;
    private ProfissionalConverter? _profissionalConvert;
    public ContratoResponse EntityToResponse(Contrato? contrato)
    {
        _servicoConvert = new ServicoConverter();
        _profissionalConvert = new ProfissionalConverter();

        if (contrato == null)
        {
            return new ContratoResponse(Guid.Empty, 0.0, null, Guid.Empty, Guid.Empty);
        }

        return new ContratoResponse(contrato.Id, contrato.Valor, contrato.Vigencia, contrato.ServicoId, contrato.ProfissionalId);
    }

    public Contrato RequestToEntity(ContratoRequest? contratoRequest)
    {
        _servicoConvert = new ServicoConverter();
        if (contratoRequest == null)
        {
            return new Contrato(Guid.Empty, 0.0, null, null, null);
        }

        return new Contrato(contratoRequest.Id, contratoRequest.Valor, contratoRequest.Vigencia, _servicoConvert.RequestToEntity(contratoRequest.Servico), _profissionalConvert.RequestToEntity(contratoRequest.Profissional));
    }

    public ICollection<ContratoResponse> EntityListToResponseList(IEnumerable<Contrato> contratos)
    {
        return (contratos == null)
            ? new List<ContratoResponse>()
            : contratos.Select(a => EntityToResponse(a)).ToList();
    }

    public ICollection<Contrato> RequestListToEntityList(IEnumerable<ContratoRequest> contratosRequests)
    {
        if (contratosRequests == null)
        {
            return new List<Contrato>();
        }

        return contratosRequests.Select(a => RequestToEntity(a)).ToList();
    }
}