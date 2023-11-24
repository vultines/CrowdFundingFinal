using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Actual.Models;
using System.Text.RegularExpressions;
using System.Security.Claims;

namespace Actual.Controllers
{
    public class CampañaController : Controller
    {
        private readonly CrowdFundingContext _context;

        public CampañaController(CrowdFundingContext context)
        {
            _context = context;
        }

        // GET: Campaña
        public async Task<IActionResult> Index()
        {
            var crowdFundingContext = _context.Campañas
                                       .Where(c => c.IdUsuario == Convert.ToInt32(HttpContext.User.FindFirstValue("UserID")))
                                       .Where(c=> c.Estado == 1)
                                       .Include(c => c.IdCategoriaAsesoramientoNavigation)
                                       .Include(c => c.IdCategoriaNavigation)
                                       .Include(c => c.Imagens)
                                       .Include(c => c.IdUsuarioNavigation);
            return View(await crowdFundingContext.ToListAsync());
        }

        // GET: Campaña/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundResult();
            }

            //Obtener la campaña y sus imagenes
            var campaña = _context.Campañas
                .Include(c => c.Imagens)
                .Include(c => c.IdCategoriaAsesoramientoNavigation)
                .Include(c => c.IdCategoriaNavigation)
                .SingleOrDefault(c => c.Id == id);

           

            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "NombreCategoria", campaña.IdCategoria);
            ViewData["IdCategoriaAsesoramiento"] = new SelectList(_context.CategoriaAsesoramientos, "Id", "NombreAsesoramiento", campaña.IdCategoriaAsesoramiento);
            return View(campaña);
        }

        // GET: Campaña/Create
        public IActionResult Create()
        {
            ViewData["IdCategoriaAsesoramiento"] = new SelectList(_context.CategoriaAsesoramientos, "Id", "NombreAsesoramiento");
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "NombreCategoria");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Campaña campaña , List<string>ImagePreviews)
        {

            try
            {
                var campañaa = new Campaña();

                campañaa = new Campaña
                {
                    NombreCampaña = campaña.NombreCampaña,
                    DescripcionGeneral = campaña.DescripcionGeneral,
                    Objetivos = campaña.Objetivos,
                    Vision = campaña.Vision,
                    Riesgos = campaña.Riesgos,
                    Enlace = campaña.Enlace,
                    FechaInicio = campaña.FechaInicio,
                    FechaCierre = campaña.FechaCierre,
                    IdCategoria = campaña.IdCategoria,
                    IdCategoriaAsesoramiento = campaña.IdCategoriaAsesoramiento,
                    IdUsuario = UserID.GetIdUser(HttpContext)

                };

                _context.Campañas.Add(campañaa);
               await _context.SaveChangesAsync();

                //Imagenes vista previs base 64

                foreach(var image in ImagePreviews)
                {
                    var base64Data = image.Substring(image.IndexOf(',') + 1);
                    var imageData = Convert.FromBase64String(base64Data);

                    //Agregar la imagen a la campaña
                    var campañaImage = new Imagen
                    {
                        Ruta = imageData,
                        IdCampaña = campañaa.Id
                    };
                    _context.Imagens.Add(campañaImage);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Campaña");


            }catch (Exception ex) 
            {
                return View("ERROR" + ex);
            }


        }

        // GET: Campaña/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Campañas == null)
            {
                return NotFound();
            }

            Imagen ima = new Imagen();
            var campaña = _context.Campañas
                .Include(c => c.Imagens)
                .FirstOrDefault(c => c.Id == id);
            DateTime fechaActual = DateTime.Now;
            campaña.FechaActualizacion = fechaActual;

            if (campaña == null)
            {
                return NotFound();
            }

            ViewData["IdImagen"] = new SelectList(_context.Imagens, "Id", "Ruta", campaña.Imagens);
            ViewData["IdCategoriaAsesoramiento"] = new SelectList(_context.CategoriaAsesoramientos, "Id", "NombreAsesoramiento", campaña.IdCategoriaAsesoramiento);
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "NombreCategoria", campaña.IdCategoria);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", campaña.IdUsuario);
            return View(campaña);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Campaña campaña, List<string> ImagePreviews)
        {

            campaña.IdUsuario = UserID.GetIdUser(HttpContext);
            var campañaEditado = _context.Campañas.Find(id);

            if(_context.Usuarios.Any(u => u.Id == campaña.IdUsuario))
            {
                try
                {
                    foreach(var imagePreview in ImagePreviews)
                    {
                        var base64Data = imagePreview.Substring(imagePreview.IndexOf(',') + 1);
                        var imageData = Convert.FromBase64String(base64Data);

                        var campañaImage = new Imagen
                        {
                            Ruta = imageData,
                            IdCampaña = campaña.Id
                        };
                        _context.Imagens.Add(campañaImage);
                    }

                    campañaEditado.NombreCampaña = campaña.NombreCampaña;
                    campañaEditado.DescripcionGeneral = campaña.DescripcionGeneral;
                    campañaEditado.Objetivos = campaña.Objetivos;
                    campañaEditado.Vision = campaña.Vision;
                    campañaEditado.Riesgos = campaña.Riesgos;
                    campañaEditado.Enlace = campaña.Enlace;
                    campañaEditado.FechaInicio = campaña.FechaInicio;
                    campañaEditado.FechaCierre = campaña.FechaCierre;
                    campañaEditado.IdCategoria = campaña.IdCategoria;
                    campañaEditado.IdCategoriaAsesoramiento = campaña.IdCategoriaAsesoramiento;
                    campañaEditado.IdUsuario = UserID.GetIdUser(HttpContext);

                    campañaEditado.FechaActualizacion = DateTime.Now;

                    _context.Update(campañaEditado);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Campaña");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!CampañaExists(campaña.Id))
                    {
                        return NotFound();
                    }else
                    {
                        throw;
                    }



                }
            }
            else
            {
                ModelState.AddModelError("IdUsuario", "El usuario seleccionado no existe");
                return View(campaña);
            }
        }


        public ActionResult GetImage(int id)
        {
            var image = _context.Imagens.Find(id);
            if (image == null)
            {
                return NotFound();
            }

            return File(image.Ruta, "image/jpeg");
        }




        // GET: Campaña/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var campaña = _context.Campañas
                .Include(c => c.Imagens)
                .FirstOrDefault(c => c.Id == id);

            if (campaña == null)
            {
                return NotFound();
            }

            return View(campaña);
        }

        // POST: Campaña/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
          
            var campaña = await _context.Campañas
                .Include (c => c.Imagens)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campaña == null)
            {
                return NotFound();
            }

            _context.Campañas.Remove(campaña);

            if (campaña.Imagens != null && campaña.Imagens.Count > 0)
            {
                _context.Imagens.RemoveRange(campaña.Imagens);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampañaExists(int id)
        {
          return (_context.Campañas?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //UPDATE Estado

        public async Task<IActionResult> AprobarCampaña(int id)
{
    try
    {
        var campaña = await _context.Campañas.FindAsync(id);

        if (campaña == null)
        {
            return NotFound();
        }

        // Cambiar el estado de la campaña a 2 (o el valor que desees)
        campaña.Estado = 2;

        // Guardar los cambios en la base de datos
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Campaña");
    }
    catch (Exception ex)
    {
        // Manejar cualquier excepción que pueda ocurrir
        return View("ERROR" + ex);
    }
}

    }
}
