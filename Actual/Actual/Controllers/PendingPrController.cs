using Actual.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Actual.Controllers
{
    public class PendingPrcontroller : Controller
    {
        private readonly CrowdFundingContext _context;

        public PendingPrcontroller(CrowdFundingContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string buscarCampaña, string categoriaCampaña)
        {
            IQueryable<Campaña> CrowdfundingContext = _context.Campañas
                .Include(m => m.IdCategoriaNavigation)
                .Where(m => m.Estado == 1)
                .Include(m => m.IdCategoriaAsesoramientoNavigation)
                .Include(m => m.Imagens)
                .Include(m => m.IdUsuarioNavigation)
                .Where(x => x.IdUsuarioNavigation.Id != UserID.GetIdUser(HttpContext));

            if (!string.IsNullOrEmpty(buscarCampaña))
            {
                CrowdfundingContext = CrowdfundingContext.Where(m => m.NombreCampaña.Contains(buscarCampaña));
            }

            if (!string.IsNullOrEmpty(categoriaCampaña))
            {
                CrowdfundingContext = CrowdfundingContext.Where(m => m.IdCategoriaNavigation.NombreCategoria == categoriaCampaña);
            }
            return View(await CrowdfundingContext.ToListAsync());
        }

        public async Task<IActionResult> IndexF(string categoria)
        {
            IQueryable<Campaña> CrwodfundingContext = _context.Campañas;

            if (!string.IsNullOrEmpty(categoria))
            {
                CrwodfundingContext = CrwodfundingContext.Include(m => m.IdCategoriaNavigation)
                                                         .Where(m => m.IdCategoriaNavigation.NombreCategoria.Equals(categoria));
            }
            else
            {
                CrwodfundingContext = CrwodfundingContext.Include(m => m.IdCategoriaNavigation);
            }

            CrwodfundingContext = CrwodfundingContext.Include(m => m.IdCategoriaAsesoramientoNavigation)
                                                        .Include(m => m.Imagens)
                                                        .Include(m => m.IdUsuarioNavigation);

            return View(await CrwodfundingContext.ToListAsync());

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Campañas == null)
            {
                return NotFound();
            }

            var campaña = await _context.Campañas
                          .Include(m => m.IdCategoriaNavigation)
                          .Include(m => m.IdCategoriaAsesoramientoNavigation)
                          .Include(m => m.Imagens)
                          .Include(m => m.IdUsuarioNavigation)
                          .FirstOrDefaultAsync(m => m.Id == id);
            if (campaña == null)
            {
                return NotFound();
            }

            return View(campaña);

        }
    }
}
