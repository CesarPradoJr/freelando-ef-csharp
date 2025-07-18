using Freelando.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelando.Modelo;
public class Projeto
{
    public Projeto() { }
    public Projeto(Guid id, string? titulo, string descricao, StatusProjeto status, Cliente? cliente, ICollection<Especialidade> especialidades, ICollection<Servico> servico) 
    {
        Id = id;
        Cliente = cliente;
        Titulo = titulo;
        Descricao = descricao;
        Status = status;
        Especialidades = especialidades;
        Servico = servico;
    }
    public Guid Id { get; set; }
    public string? Titulo { get; set; }
    public  string? Descricao { get; set; }
    public StatusProjeto Status { get; set; }
    public Cliente? Cliente { get; set; }
    public ICollection<Especialidade> Especialidades { get; set; }
    public ICollection<ProjetoEspecialidade> ProjetosEspecialidade { get; } = [];
    public ICollection<Servico> Servico { get; set; }
}
