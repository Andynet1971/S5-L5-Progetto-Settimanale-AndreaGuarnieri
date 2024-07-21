using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Controllers
{
    [Authorize] // Richiede l'autenticazione per accedere a questo controller
    public class VerbaleController : Controller
    {
        private readonly IVerbale _verbaleService;
        private readonly IAnagrafica _anagraficaService;
        private readonly ITipoViolazione _tipoViolazioneService;

        // Inizializza i servizi tramite dependency injection
        public VerbaleController(IVerbale verbaleService, IAnagrafica anagraficaService, ITipoViolazione tipoViolazioneService)
        {
            _verbaleService = verbaleService;
            _anagraficaService = anagraficaService;
            _tipoViolazioneService = tipoViolazioneService;
        }

        [Authorize(Roles = "Admin")] // Richiede il ruolo di Admin per accedere a questo metodo
        public IActionResult Index()
        {
            // Recupera tutti i verbali e aggiunge le informazioni anagrafiche ad ogni verbale
            var verbali = _verbaleService.GetAll().ToList();
            foreach (var verbale in verbali)
            {
                verbale.Anagrafica = _anagraficaService.GetById(verbale.Idanagrafica);
            }
            return View(verbali);
        }

        [Authorize(Roles = "Admin")] // Richiede il ruolo di Admin per accedere a questo metodo
        public IActionResult Create()
        {
            // Recupera tutte le possibili violazioni e le passa alla vista tramite ViewBag
            ViewBag.TipoViolazioni = _tipoViolazioneService.GetAll();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Richiede il ruolo di Admin per accedere a questo metodo
        public IActionResult Create(VerbaleCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Crea un nuovo record anagrafico
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

                    // Crea un nuovo verbale associato al record anagrafico creato
                    var verbale = new Verbale
                    {
                        DataViolazione = model.DataViolazione,
                        IndirizzoViolazione = model.IndirizzoViolazione,
                        Nominativo_Agente = model.Nominativo_Agente,
                        DataTrascrizioneVerbale = model.DataTrascrizioneVerbale,
                        Idanagrafica = anagrafica.Idanagrafica
                    };

                    _verbaleService.Add(verbale);

                    // Aggiunge le violazioni associate al verbale creato
                    foreach (var idviolazione in model.Idviolazioni)
                    {
                        var verbaleViolazioni = new VerbaleViolazioni
                        {
                            Idverbale = verbale.Idverbale,
                            Idviolazione = idviolazione
                        };
                        _verbaleService.AddVerbaleViolazioni(verbaleViolazioni);
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Si è verificato un errore durante la creazione del verbale. Riprova.");
                }
            }

            // Se il modello non è valido, restituisce il modello alla vista con i possibili errori di validazione
            ViewBag.TipoViolazioni = _tipoViolazioneService.GetAll();
            return View(model);
        }

        public IActionResult VerbaliPerTrasgressore()
        {
            // Recupera e visualizza i verbali per trasgressore
            var verbaliPerTrasgressore = _verbaleService.GetVerbaliPerTrasgressore();
            return View(verbaliPerTrasgressore);
        }

        public IActionResult PuntiDecurtatiPerTrasgressore()
        {
            // Recupera e visualizza i punti decurtati per trasgressore
            var puntiPerTrasgressore = _verbaleService.GetPuntiDecurtatiPerTrasgressore();
            return View(puntiPerTrasgressore);
        }

        public IActionResult ViolazioniSuperano10Punti()
        {
            // Recupera e visualizza le violazioni che superano i 10 punti di decurtamento
            var violazioni = _verbaleService.GetViolazioniSuperano10Punti();
            return View(violazioni);
        }

        public IActionResult ViolazioniImportoMaggiore400()
        {
            // Recupera e visualizza le violazioni con un importo maggiore di 400 euro
            var violazioni = _verbaleService.GetViolazioniImportoMaggiore400();
            return View(violazioni);
        }
    }
}
