using System;
using System.Collections.Generic;

namespace Ikigenda.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? NombreUsuario { get; set; }

    public string Contrasenya { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public sbyte Activo { get; set; }

    public int RolId { get; set; }

    public virtual Rol Rol { get; set; } = null!;
}
