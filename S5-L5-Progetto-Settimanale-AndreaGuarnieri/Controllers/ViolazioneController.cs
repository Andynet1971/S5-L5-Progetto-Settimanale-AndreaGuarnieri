using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Controllers
{
    // Limita l'accesso ai soli utenti con il ruolo "Admin".
    [Authorize(Roles = "Admin")]
    public class ViolazioneController : Controller
    {
        private readonly ITipoViolazione _tipoViolazioneService;

        // Inietta il servizio ITipoViolazione tramite Dependency Injection.
        public ViolazioneController(ITipoViolazione tipoViolazioneService)
        {
            _tipoViolazioneService = tipoViolazioneService;
        }

        // Restituisce la vista Index con l'elenco di tutte le violazioni.
        public IActionResult Index()
        {
            return View(_tipoViolazioneService.GetAll());
        }

        // Restituisce la vista per creare una nuova violazione.
        public IActionResult Create()
        {
            return View();
        }

        // Gestisce l'invio del modulo per creare una nuova violazione.
        [HttpPost]
        public IActionResult Create(TipoViolazione tipoViolazione)
        {
            // Verifica se il modello è valido.
            if (ModelState.IsValid)
            {
                try
                {
                    _tipoViolazioneService.Add(tipoViolazione);
                    // Reindirizza all'azione Index dopo l'aggiunta.
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Si è verificato un errore durante l'aggiunta della violazione. Riprova.");
                }
            }
            return View(tipoViolazione);
        }

        // Restituisce la vista per modificare una violazione esistente.
        public IActionResult Edit(int id)
        {
            var tipoViolazione = _tipoViolazioneService.GetById(id);
            if (tipoViolazione == null)
            {
                return NotFound();
            }
            return View(tipoViolazione);
        }

        // Gestisce l'invio del modulo per modificare una violazione esistente.
        [HttpPost]
        public IActionResult Edit(TipoViolazione tipoViolazione)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _tipoViolazioneService.Update(tipoViolazione);
                    // Reindirizza all'azione Index dopo l'aggiornamento.
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Si è verificato un errore durante l'aggiornamento della violazione. Riprova.");
                }
            }
            return View(tipoViolazione);
        }

        // Gestisce la cancellazione di una violazione esistente.
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _tipoViolazioneService.Delete(id);
                // Reindirizza all'azione Index dopo la cancellazione.
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
