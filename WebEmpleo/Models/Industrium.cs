using System;
using System.Collections.Generic;

namespace WebEmpleo.Models;

public partial class Industrium
{
    public int IdIndustria { get; set; }

    public string Descripcion { get; set; } = null!;

    public bool? Estado { get; set; }

    public DateTime? FchCreacion { get; set; }

    public virtual ICollection<Empresa> Empresas { get; } = new List<Empresa>();
}
