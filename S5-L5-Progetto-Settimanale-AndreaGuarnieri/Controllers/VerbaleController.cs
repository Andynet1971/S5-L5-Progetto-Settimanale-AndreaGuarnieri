using Microsoft.AspNetCore.Mvc;
using S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Controllers
{
    public class VerbaleController : Controller
    {
        private readonly IVerbale _verbaleService;
        private readonly IAnagrafica _anagraficaService;
        private readonly ITipoViolazione _tipoViolazioneService;
        private readonly ILogger<VerbaleController> _logger;

        public VerbaleController(IVerbale verbaleService, IAnagrafica anagraficaService, ITipoViolazione tipoViolazioneService, ILogger<VerbaleController> logger)
        {
            _verbaleService = verbaleService;
            _anagraficaService = anagraficaService;
            _tipoViolazioneService = tipoViolazioneService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var verbali = _verbaleService.GetAll().ToList();
            foreach (var verbale in verbali)
            {
                verbale.Anagrafica = _anagraficaService.GetById(verbale.Idanagrafica);
            }
            return View(verbali);
        }

        public IActionResult Create()
        {
            ViewBag.TipoViolazioni = _tipoViolazioneService.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(VerbaleCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var anagrafica = new Anagrafica
                    {
                        Cognome = model.Cognome,
                        Nome = model.Nome,
                        Indirizzo = model.Indirizzo,
                        Città = model.Città,
                        CAP = model.CAP,
                        Cod_Fisc = model.Cod_Fisc
                    };

                    _anagraficaService.Add(anagrafica);

                    var verbale = new Verbale
                    {
                        DataViolazione = model.DataViolazione,
                        IndirizzoViolazione = model.IndirizzoViolazione,
                        Nominativo_Agente = model.Nominativo_Agente,
                        DataTrascrizioneVerbale = model.DataTrascrizioneVerbale,
                        Idanagrafica = anagrafica.Idanagrafica
                    };

                    _verbaleService.Add(verbale);

                    foreach (var idviolazione in model.Idviolazioni)
                    {
                        var verbaleViolazioni = new VerbaleViolazioni
                        {
                            Idverbale = verbale.Idverbale,
                            Idviolazione = idviolazione
                        };
                        _verbaleService.AddVerbaleViolazioni(verbaleViolazioni);
                    }

                    _logger.LogInformation("Verbale creato con successo: {@Verbale}", verbale);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante la creazione del verbale");
                    ModelState.AddModelError("", "Si è verificato un errore durante la creazione del verbale. Riprova.");
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

            ViewBag.TipoViolazioni = _tipoViolazioneService.GetAll();
            return View(model);
        }

        public IActionResult VerbaliPerTrasgressore()
        {
            var verbaliPerTrasgressore = _verbaleService.GetVerbaliPerTrasgressore();
            return View(verbaliPerTrasgressore);
        }

        public IActionResult PuntiDecurtatiPerTrasgressore()
        {
            var puntiPerTrasgressore = _verbaleService.GetPuntiDecurtatiPerTrasgressore();
            return View(puntiPerTrasgressore);
        }

        public IActionResult ViolazioniSuperano10Punti()
        {
            var violazioni = _verbaleService.GetViolazioniSuperano10Punti();
            return View(violazioni);
        }

        public IActionResult ViolazioniImportoMaggiore400()
        {
            var violazioni = _verbaleService.GetViolazioniImportoMaggiore400();
            return View(violazioni);
        }
    }
}
