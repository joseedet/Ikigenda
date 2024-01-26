using System;
using System.Collections.Generic;

namespace Ikigenda.Models;

public partial class Planin
{
    public int PlaninId { get; set; }

    public string Notas { get; set; } = null!;

    public sbyte Disponible { get; set; }

    public DateTime FechaLocal { get; set; }

    public virtual ICollection<Historial> Historials { get; set; } = new List<Historial>();
}
