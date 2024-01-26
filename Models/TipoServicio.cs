using System;
using System.Collections.Generic;

namespace Ikigenda.Models;

public partial class TipoServicio
{
    public int TipoServicioId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Historial> Historials { get; set; } = new List<Historial>();
}
