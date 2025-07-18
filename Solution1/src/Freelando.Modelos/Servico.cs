namespace Freelando.Modelo;
public class Servico
{
    public Servico() { }
    public Servico(Guid id, string? titulo, string? descricao, StatusServico status, Contrato contrato, Projeto projeto)
    {
        Id = id;
        Titulo = titulo;
        Descricao = descricao;
        Status = status;
        Contrato = contrato;
        Projeto = projeto;
    }

    public Guid Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public StatusServico Status { get; set; }
    public Contrato Contrato { get; set; }
    public Projeto Projeto { get; set; }
    public Guid ProjetoId { get; set; }

}
