using System;
using System.Collections.Generic;

namespace Actual.Models;

public partial class Campaña
{
    public int Id { get; set; }

    public string NombreCampaña { get; set; } = null!;

    public string DescripcionGeneral { get; set; } = null!;

    public string Objetivos { get; set; } = null!;

    public string Vision { get; set; } = null!;

    public string Riesgos { get; set; } = null!;

    public string Enlace { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaCierre { get; set; }

    public byte? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdCategoria { get; set; }

    public int? IdCategoriaAsesoramiento { get; set; }

    public string? Integrantes { get; set; }

    public virtual CategoriaAsesoramiento? IdCategoriaAsesoramientoNavigation { get; set; }

    public virtual Categorium? IdCategoriaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Imagen> Imagens { get; set; } = new List<Imagen>();
}
