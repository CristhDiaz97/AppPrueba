using System;
using System.Collections.Generic;

namespace WebEmpleo.Models;

public partial class Person
{
    public int IdPersona { get; set; }

    public string IdUsuarios { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public int? Edad { get; set; }

    public string Telefono { get; set; } = null!;

    public int IdNivelEducativo { get; set; }

    public string? Notas { get; set; }

    public bool Estado { get; set; }

    public DateTime? FchCreate { get; set; }

    public DateTime? FchUpdate { get; set; }

    public virtual ICollection<Experiencia> Experiencia { get; } = new List<Experiencia>();

    public virtual NivelesEducativo IdNivelEducativoNavigation { get; set; } = null!;

    public virtual AspNetUser IdUsuariosNavigation { get; set; } = null!;

    public virtual ICollection<Postulacion> Postulacions { get; } = new List<Postulacion>();
}
