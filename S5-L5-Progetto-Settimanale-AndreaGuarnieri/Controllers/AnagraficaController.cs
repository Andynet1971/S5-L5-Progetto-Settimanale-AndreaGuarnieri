using Microsoft.AspNetCore.Mvc;
using S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Controllers
{
    public class AnagraficaController : Controller
    {
        private readonly IAnagrafica _anagraficaService;

        public AnagraficaController(IAnagrafica anagraficaService)
        {
            _anagraficaService = anagraficaService;
        }

        public IActionResult Index()
        {
            return View(_anagraficaService.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Anagrafica anagrafica)
        {
            if (ModelState.IsValid)
            {
                _anagraficaService.Add(anagrafica);
                return RedirectToAction(nameof(Index));
            }
            return View(anagrafica);
        }
    }
}
