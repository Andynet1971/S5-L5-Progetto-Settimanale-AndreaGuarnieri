using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models;
using System;
using System.Linq;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Controllers
{
    public class ViolazioneController : Controller
    {
        private readonly ITipoViolazione _tipoViolazioneService;
        private readonly ILogger<ViolazioneController> _logger;

        public ViolazioneController(ITipoViolazione tipoViolazioneService, ILogger<ViolazioneController> logger)
        {
            _tipoViolazioneService = tipoViolazioneService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_tipoViolazioneService.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TipoViolazione tipoViolazione)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _tipoViolazioneService.Add(tipoViolazione);
                    _logger.LogInformation("Violazione aggiunta con successo: {@TipoViolazione}", tipoViolazione);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante l'aggiunta della violazione");
                    ModelState.AddModelError("", "Si è verificato un errore durante l'aggiunta della violazione. Riprova.");
                }
            }
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        _logger.LogWarning("Errore nel campo {Key}: {Error}", key, error.ErrorMessage);
                    }
                }
            }
            return View(tipoViolazione);
        }
    }
}
