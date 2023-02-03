using System;
using System.Collections.Generic;

namespace WebEmpleo.Models;

public partial class Experiencia
{
    public int IdExperienciaLaboral { get; set; }

    public int IdPersona { get; set; }

    public DateTime FchInicio { get; set; }

    public DateTime FchFin { get; set; }

    public string Descripcion { get; set; } = null!;

    public bool Estado { get; set; }

    public DateTime FchCreate { get; set; }

    public DateTime FchUpdate { get; set; }

    public virtual Person IdPersonaNavigation { get; set; } = null!;
}
