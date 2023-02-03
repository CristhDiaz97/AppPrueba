using System;
using System.Collections.Generic;

namespace WebEmpleo.Models;

public partial class Empresa
{
    public int IdEmpresa { get; set; }

    public string? IdUsuarios { get; set; }

    public string Ciudad { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public int? Industria { get; set; }

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public int CantidadEmpleados { get; set; }

    public int CantidadVacantes { get; set; }

    public bool Estado { get; set; }

    public DateTime? FchCreate { get; set; }

    public DateTime? FchUpdate { get; set; }

    public virtual AspNetUser? IdUsuariosNavigation { get; set; }

    public virtual Industrium? IndustriaNavigation { get; set; }
}
