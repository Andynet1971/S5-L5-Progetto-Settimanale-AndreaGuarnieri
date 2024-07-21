using Microsoft.AspNetCore.Mvc;
using S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Controllers
{
    public class AnagraficaController : Controller
    {
        // Servizio per gestire le operazioni sull'anagrafica
        private readonly IAnagrafica _anagraficaService;

        // Costruttore che inizializza il servizio tramite dependency injection
        public AnagraficaController(IAnagrafica anagraficaService)
        {
            _anagraficaService = anagraficaService;
        }

        // Azione per visualizzare la lista di tutte le anagrafiche
        public IActionResult Index()
        {
            // Recupera tutte le anagrafiche e le passa alla vista
            return View(_anagraficaService.GetAll());
        }

        // Azione per visualizzare la pagina di creazione di una nuova anagrafica
        public IActionResult Create()
        {
            return View();
        }

        // Azione HTTP POST per creare una nuova anagrafica
        [HttpPost]
        public IActionResult Create(Anagrafica anagrafica)
        {
            // Controlla se il modello è valido
            if (ModelState.IsValid)
            {
                // Aggiunge la nuova anagrafica
                _anagraficaService.Add(anagrafica);
                // Reindirizza all'azione Index per visualizzare la lista aggiornata
                return RedirectToAction(nameof(Index));
            }
            // Se il modello non è valido, ritorna la vista di creazione con i dati immessi
            return View(anagrafica);
        }
    }
}
