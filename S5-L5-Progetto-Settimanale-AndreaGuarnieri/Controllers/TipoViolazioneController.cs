using Microsoft.AspNetCore.Mvc;
using S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Controllers
{
    public class TipoViolazioneController : Controller
    {
        private readonly ITipoViolazione _tipoViolazioneService;

        public TipoViolazioneController(ITipoViolazione tipoViolazioneService)
        {
            _tipoViolazioneService = tipoViolazioneService;
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
                _tipoViolazioneService.Add(tipoViolazione);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoViolazione);
        }
    }
}
