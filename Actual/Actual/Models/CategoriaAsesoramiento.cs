using System;
using System.Collections.Generic;

namespace Actual.Models;

public partial class CategoriaAsesoramiento
{
    public int Id { get; set; }

    public string NombreAsesoramiento { get; set; } = null!;

    public byte? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual ICollection<Campaña> Campañas { get; set; } = new List<Campaña>();
}
