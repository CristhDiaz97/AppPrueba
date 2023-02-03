using System;
using System.Collections.Generic;

namespace WebEmpleo.Models;

public partial class Vacante
{
    public int IdVacante { get; set; }

    public int IdEmpresa { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Localizacion { get; set; } = null!;

    public string Estudios { get; set; } = null!;

    public string Experiencia { get; set; } = null!;

    public string TipoContrato { get; set; } = null!;

    public int CantidadVacantes { get; set; }

    public DateTime? FchVencimiento { get; set; }

    public bool Estado { get; set; }

    public DateTime FchCreate { get; set; }

    public DateTime FchUpdate { get; set; }
}
