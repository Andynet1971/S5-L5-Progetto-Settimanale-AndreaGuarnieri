using Microsoft.AspNetCore.Mvc;
using S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Controllers
{
    public class TipoViolazioneController : Controller
    {
        // Servizio per gestire le operazioni sui tipi di violazione
        private readonly ITipoViolazione _tipoViolazioneService;

        // Costruttore che inizializza il servizio tramite dependency injection
        public TipoViolazioneController(ITipoViolazione tipoViolazioneService)
        {
            _tipoViolazioneService = tipoViolazioneService;
        }

        // Azione per visualizzare la lista di tutti i tipi di violazione
        public IActionResult Index()
        {
            // Recupera tutti i tipi di violazione e li passa alla vista
            return View(_tipoViolazioneService.GetAll());
        }

        // Azione per visualizzare il form di creazione di un nuovo tipo di violazione
        public IActionResult Create()
        {
            return View();
        }

        // Azione HTTP POST per creare un nuovo tipo di violazione
        [HttpPost]
        public IActionResult Create(TipoViolazione tipoViolazione)
        {
            if (ModelState.IsValid)
            {
                // Aggiunge il nuovo tipo di violazione al database
                _tipoViolazioneService.Add(tipoViolazione);
                // Reindirizza alla lista dei tipi di violazione
                return RedirectToAction(nameof(Index));
            }
            // Se il modello non è valido, restituisce il modello alla vista con i possibili errori di validazione
            return View(tipoViolazione);
        }
    }
}
