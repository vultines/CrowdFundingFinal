using Actual.Models;
using Actual.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Actual.Controllers
{
    public class ReporteController : Controller
    {
        private readonly CrowdFundingContext _context;

        public ReporteController(CrowdFundingContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(DateTime? fechaInicio, DateTime? fechaFin)
        {
            ReporteViewModels reporte = new ReporteViewModels();

            // Filtrar por fechas si se proporcionan
            var campañas = _context.Campañas.AsQueryable();
            if (fechaInicio.HasValue)
            {
                campañas = campañas.Where(l => l.FechaInicio >= fechaInicio.Value);
            }
            if (fechaFin.HasValue)
            {
                campañas = campañas.Where(l => l.FechaCierre <= fechaFin.Value);
            }

            reporte.Campaña_Count = await campañas.Where(l => l.Estado == 1).CountAsync();
            reporte.Usuarios_Count = await _context.Usuarios.CountAsync();

            return View(reporte);
        }

    }
}
