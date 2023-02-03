using System;
using System.Collections.Generic;

namespace WebEmpleo.Models;

public partial class NivelesEducativo
{
    public int IdNivelEducativo { get; set; }

    public string Descripcion { get; set; } = null!;

    public bool Estado { get; set; }

    public DateTime? FchCreate { get; set; }

    public DateTime? FchUpdate { get; set; }

    public virtual ICollection<Person> People { get; } = new List<Person>();
}
