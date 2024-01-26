using System;
using System.Collections.Generic;

namespace Ikigenda.Models;

public partial class Historial
{
    public int HistorialId { get; set; }

    public int PlaninId { get; set; }

    public int ClienteId { get; set; }

    public int TipoServicioId { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Observaciones { get; set; } = null!;

    public DateTime DateLocal { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual Planin Planin { get; set; } = null!;

    public virtual TipoServicio TipoServicio { get; set; } = null!;
}
