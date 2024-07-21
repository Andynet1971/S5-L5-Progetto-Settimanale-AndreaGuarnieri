using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models;
using System.Security.Claims;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Controllers
{
    public class AccountController : Controller
    {
        // Servizio per la gestione degli utenti
        private readonly IUtente _utenteService;

        // Costruttore che inizializza il servizio tramite dependency injection
        public AccountController(IUtente utenteService)
        {
            _utenteService = utenteService;
        }

        // Azione per visualizzare la pagina di login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Azione HTTP POST per gestire l'autenticazione dell'utente
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Controlla se il modello è valido
            if (ModelState.IsValid)
            {
                // Autentica l'utente
                var user = _utenteService.Authenticate(model.Username, model.Password);
                if (user != null)
                {
                    // Crea i claims per l'utente autenticato
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe // Imposta il cookie persistente se richiesto
                    };

                    // Esegui il login dell'utente
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Reindirizza alla pagina principale dopo il login
                    return RedirectToAction("Index", "Home");
                }

                // Se l'autenticazione fallisce, aggiungi un messaggio di errore
                ModelState.AddModelError("", "Username o password non validi");
                return RedirectToAction("AccessDenied");
            }

            // Se il modello non è valido, ritorna la vista di login con i dati immessi
            return View(model);
        }

        // Azione HTTP POST per gestire il logout dell'utente
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Esegui il logout dell'utente
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Reindirizza alla pagina principale dopo il logout
            return RedirectToAction("Index", "Home");
        }

        // Azione per visualizzare la pagina di accesso negato
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
