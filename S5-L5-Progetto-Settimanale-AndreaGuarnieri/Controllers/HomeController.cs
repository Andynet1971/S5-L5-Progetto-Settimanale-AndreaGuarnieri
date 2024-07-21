using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models;
using System.Diagnostics;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Controllers
{
    [Authorize] // Richiede che l'utente sia autenticato per accedere a tutte le azioni di questo controller
    public class HomeController : Controller
    {
        // Servizio per gestire le operazioni sui verbali
        private readonly IVerbale _verbaleService;

        // Logger per registrare informazioni e tracciare gli eventi del controller
        private readonly ILogger<HomeController> _logger;

        // Costruttore che inizializza i servizi tramite dependency injection
        public HomeController(IVerbale verbaleService, ILogger<HomeController> logger)
        {
            _verbaleService = verbaleService;
            _logger = logger;
        }

        // Azione per visualizzare la pagina iniziale con i dettagli dei verbali
        public IActionResult Index()
        {
            // Recupera tutti i verbali con i dettagli e li passa alla vista
            IEnumerable<VerbaleDetailViewModel> verbali = _verbaleService.GetVerbaliWithDetails();
            return View(verbali);
        }

        // Azione per visualizzare la pagina di privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // Azione per visualizzare la pagina delle visualizzazioni
        public IActionResult Visualizzazioni()
        {
            return View();
        }

        // Azione per gestire gli errori e visualizzare una pagina di errore
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
