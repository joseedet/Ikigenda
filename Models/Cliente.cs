using System;
using System.Collections.Generic;

namespace Ikigenda.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public int TipoDocumentoId { get; set; }

    public string Documento { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Poblacion { get; set; }

    public string? CodPostal { get; set; }

    public string? Provincia { get; set; }

    public string? Correo { get; set; }

    public string? Observaciones { get; set; }

    public string Telefono { get; set; } = null!;

    public virtual ICollection<Historial> Historials { get; set; } = new List<Historial>();

    public virtual TipoDocumento TipoDocumento { get; set; } = null!;
}
