using System;
using System.Collections.Generic;

namespace Actual.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string CorreoElectronico { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Rol { get; set; }

    public int Celular { get; set; }

    public byte[]? Ruta { get; set; }

    public byte? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? Token { get; set; }

    public DateTime? TokenExpiracion { get; set; }

    public virtual ICollection<Campaña> Campañas { get; set; } = new List<Campaña>();
}
