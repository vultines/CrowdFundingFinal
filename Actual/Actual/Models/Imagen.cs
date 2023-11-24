using System;
using System.Collections.Generic;

namespace Actual.Models;

public partial class Imagen
{
    public int Id { get; set; }

    public byte[] Ruta { get; set; } = null!;

    public int? IdCampaña { get; set; }

    public byte? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual Campaña? IdCampañaNavigation { get; set; }
}
