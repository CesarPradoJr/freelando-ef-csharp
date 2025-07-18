using Freelando.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelando.Modelos;
public class ProfissionalEspecialidade
{
    public Guid ProfissionalId { get; set; }
    public Profissional Profissionais { get; set; }
    public Guid EspecialidadeId { get; set; }
    public Especialidade Especialidades { get; set; }

}
