using System;
using System.Collections.Generic;

namespace Ikigenda.Models;

/// <summary>
/// Tabla de los tipos de documentos admitidos
/// </summary>
public partial class TipoDocumento
{
    public int TipoDocumentoId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
