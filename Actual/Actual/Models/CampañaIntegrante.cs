using System;
using System.Collections.Generic;

namespace Actual.Models;

public partial class CampañaIntegrante
{
    public int IdCampaña { get; set; }

    public int IdIntegrante { get; set; }

    public virtual Campaña IdCampañaNavigation { get; set; } = null!;

    public virtual Integrante IdIntegranteNavigation { get; set; } = null!;
}
