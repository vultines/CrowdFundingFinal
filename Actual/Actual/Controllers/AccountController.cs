using Actual.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Actual.Controllers
{
    public class AccountController : Controller
    {
        private readonly CrowdFundingContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(CrowdFundingContext appDbContext, IWebHostEnvironment webHostEnvironmen)
        {
            _webHostEnvironment = webHostEnvironmen;
            _context = appDbContext;
            
        }


        //Encriptar la contraseña 
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }


        
        //Registrar Usuario
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("CorreoElectronico,Contraseña,Nombre,Apellido,Celular")] Usuario usuario, string ConfirmPass)
        {
            if (ModelState.IsValid)
            {
                // Verificar que la contraseña y la confirmación coincidan
                if (usuario.Contraseña != ConfirmPass)
                {
                    ModelState.AddModelError("", "La contraseña y la confirmación no coinciden");
                    return View(usuario);
                }
                // Verificamos si ya existe un usuario con el mismo correo electrónico
                var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.CorreoElectronico == usuario.CorreoElectronico);
                if (usuarioExistente != null)
                {
                    // Ya existe un usuario con el mismo correo electrónico, agregamos un mensaje de error al modelo
                    ModelState.AddModelError("CorreoElectronico", "Ya existe un usuario con este correo electrónico");
                }
                else
                {
                    usuario.Ruta = System.IO.File.ReadAllBytes(Path.Combine(_webHostEnvironment.WebRootPath, "img", "avatar", "avatar-1.png"));
                    // No existe un usuario con el mismo correo electrónico, podemos crear el nuevo usuario
                    usuario.Contraseña = HashPassword(usuario.Contraseña);
                    _context.Usuarios.Add(usuario);

                    await _context.SaveChangesAsync();
                    //HttpContext.Session.Set("UserImage", usuario.Ruta);
                    return RedirectToAction("Create", "Account");
                }
            }
            return View(usuario);
        }

        //Loguearse
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Loguear(Usuario usuario)
        {
            //var usuarioValido = _context.Usuarios.FirstOrDefault(u => u.CorreoElectronico == usuario.CorreoElectronico && u.Contraseña == usuario.Contraseña);
            var contraseñaEncriptada = HashPassword(usuario.Contraseña);
            var usuarioValido = _context.Usuarios.FirstOrDefault(u => u.CorreoElectronico == usuario.CorreoElectronico && u.Contraseña == contraseñaEncriptada);
            if (usuarioValido != null)
            {
                // El usuario es válido, redirigimos a la página principal del sitio
                //UserID.IdUser = usuarioValido.Id;
                // Verificar si el usuario tiene una imagen de perfil
                if (usuarioValido.Ruta != null)
                {
                    // Guardar la imagen del usuario en la sesión
                    HttpContext.Session.Set("UserImage", usuarioValido.Ruta);
                }

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.UserData,usuarioValido.Id.ToString()),
                        new Claim("UserID", usuarioValido.Id.ToString()),
                        new Claim(ClaimTypes.Name, usuarioValido.Nombre),
                        new Claim(ClaimTypes.Email, usuarioValido.CorreoElectronico),
                        new Claim(ClaimTypes.Role, usuarioValido.Rol)
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // La cookie persistirá incluso después de cerrar el navegador
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(15) // Tiempo de expiración de la cookie
                };
                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                //string userName = claimsIdentity.FindFirstValue(ClaimTypes.Name);
                //UserID.IdUser = Convert.ToInt32(HttpContext.User.FindFirstValue("UserID"));

              
                return RedirectToAction("Index", "Home");
            }
            else
            {

                // El usuario no es válido, mostramos un mensaje de error en la vista
                ModelState.AddModelError("", "El nombre de usuario o la contraseña son incorrectos");
                ViewData["Message"] = "El nombre de usuario o la contraseña son incorrectos";
            }

            ViewData["Message"] = "Credenciales incorrectas";


            //// Si llegamos aquí, es porque hubo un error en la validación, volvemos a mostrar la vista de Login
            return View("Create", usuario);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            // Obtener el ID del usuario actualmente autenticado
            //int userId = UserID.IdUser;
            int userId = UserID.GetIdUser(HttpContext);

            //int userId = Convert.ToInt32(HttpContext.User.FindFirstValue("UserID"));
            // Buscar al usuario en la base de datos

            Usuario usuario = await _context.Usuarios.FindAsync(userId);
            // Mostrar la vista de edición con los datos del usuario
            return View(usuario);
        }

        [HttpGet]
        public async Task<IActionResult> VerPerfil(int id)
        {
            // Obtener el ID del usuario actualmente autenticado
            //int userId = UserID.IdUser;
            // Buscar al usuario en la base de datos
            Usuario usuario = await _context.Usuarios.FindAsync(id);
            // Mostrar la vista de edición con los datos del usuario
            return View(usuario);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Usuario usuario, IFormFile Ruta)
        {
            if (!ModelState.IsValid)
            {
                //int userId = UserID.IdUser;
                int userId = Convert.ToInt32(HttpContext.User.FindFirstValue("UserID"));
                // Obtener el usuario de la base de datos
                var usuarioDb = _context.Usuarios.Find(userId);
                if (usuarioDb != null)
                {
                    if (Ruta != null && Ruta.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await Ruta.CopyToAsync(memoryStream);
                            usuario.Ruta = memoryStream.ToArray();
                        }
                        //// Actualizar solo los campos específicos 
                        usuarioDb.Ruta = usuario.Ruta; // Agregar esta línea

                        //// Guardar los cambios en la base de datos

                        //// Almacenar la nueva imagen en la sesión
                        HttpContext.Session.Set("UserImage", usuario.Ruta);
                    }
                    usuarioDb.Nombre = usuario.Nombre;
                    usuarioDb.Apellido = usuario.Apellido;
                    usuarioDb.Celular = usuario.Celular;
                    usuarioDb.FechaActualizacion = DateTime.Now;
                    _context.SaveChanges();
                }
                // Redirigir al usuario a la página de inicio
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("Edit", "Account", new { id = usuario.Id });
            }
            // Si el modelo no es válido, volver a mostrar la vista de edición
            return View(usuario);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                // Buscar al usuario en la base de datos por el correo electrónico
                var usuario = _context.Usuarios.FirstOrDefault(u => u.CorreoElectronico == model.Email);
                if (usuario != null)
                {
                    // El usuario existe, generar un token y enviarlo por correo electrónico
                    var token = GenerarToken();
                    usuario.Token = token;
                    usuario.TokenExpiracion = DateTime.Now.AddHours(1);
                    _context.SaveChanges();



                    // Enviar el token por correo electrónico
                    var email = new Email();
                    email.EnviarToken(token, usuario.CorreoElectronico);



                    // Mostrar un mensaje de éxito
                    ViewBag.Message = "Se ha enviado un enlace de restablecimiento a su correo electrónico";
                }
                else
                {

                    // Asignar un mensaje de error a ViewBag.Message si es necesario
                    ViewBag.Message = "Ocurrió un error al procesar la solicitud";

                    // El usuario no existe, mostrar un mensaje de error
                    ModelState.AddModelError("", "No se encontró un usuario con ese correo electrónico");
                }
            }
            // Si llegamos aquí, es porque hubo un error en la validación o el usuario no existe, volvemos a mostrar la vista de olvido de contraseña
            return View(model);
        }

        public string GenerarToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var data = new byte[4];
                rng.GetBytes(data);
                var token = BitConverter.ToString(data).Replace("-", "");
                return token;
            }
        }


        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                // Buscar al usuario en la base de datos por el token
                var usuario = _context.Usuarios.FirstOrDefault(u => u.Token == model.Token && u.TokenExpiracion > DateTime.Now);
                if (usuario != null)
                {
                    // El token es válido, permitir al usuario restablecer su contraseña
                    //usuario.Contraseña = model.NewPassword;
                    usuario.Contraseña = HashPassword(model.NewPassword);
                    usuario.Token = null;
                    usuario.TokenExpiracion = null;
                    _context.SaveChanges();
                    // Redirigir al usuario a la página de inicio de sesión
                    return RedirectToAction("Create", "Account");
                }
                else
                {
                    // El token no es válido o ha expirado, mostrar un mensaje de error
                    ModelState.AddModelError("", "El token no es válido o ha expirado");
                }
            }
            // Si llegamos aquí, es porque hubo un error en la validación, volvemos a mostrar la vista de restablecimiento de contraseña
            return View(model);
        }


        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(Actual.Models.ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Obtener el ID del usuario actualmente autenticado
                int userId = UserID.GetIdUser(HttpContext);
                // Buscar al usuario en la base de datos
                Usuario usuario = _context.Usuarios.Find(userId);
                // Encriptamos la contraseña actual ingresada por el usuario antes de compararla con la base de datos
                var oldPasswordHash = HashPassword(model.OldPassword);
                // Verificar que la contraseña actual ingresada por el usuario sea correcta
                if (usuario.Contraseña == oldPasswordHash)
                {
                    // Actualizar la contraseña del usuario en la base de datos
                    // Encriptamos la nueva contraseña antes de guardarla en la base de datos
                    usuario.Contraseña = HashPassword(model.NewPassword);
                    usuario.FechaActualizacion = DateTime.Now;
                    _context.SaveChanges();
                    // Enviar un correo electrónico al usuario notificándole que su contraseña ha sido cambiada
                    //Email email = new Email();
                    //email.EnviarToken("change", "", usuario.CorreoElectronico);
                    // Redirigir al usuario a la página de inicio
                    return RedirectToAction("Create", "Home");
                }
                else
                {
                    // Si la contraseña actual ingresada por el usuario no es correcta, mostrar un mensaje de error
                    ModelState.AddModelError("", "La contraseña actual es incorrecta");
                }
            }

            // Si el modelo no es válido o si la contraseña actual ingresada por el usuario no es correcta, volver a mostrar la vista de cambio de contraseña
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Remove("UserImage");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Create", "Account");
        }
    }


   
}
