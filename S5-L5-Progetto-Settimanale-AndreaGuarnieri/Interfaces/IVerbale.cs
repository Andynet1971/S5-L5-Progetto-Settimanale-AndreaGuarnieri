
namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public interface IVerbale
    {
        IEnumerable<Verbale> GetAll();
        Verbale GetById(int id);
        void Add(Verbale verbale);
        void Update(Verbale verbale);
        void AddVerbaleViolazioni(VerbaleViolazioni verbaleViolazioni);
        IEnumerable<VerbaliPerTrasgressoreViewModel> GetVerbaliPerTrasgressore();
        IEnumerable<PuntiDecurtatiPerTrasgressoreViewModel> GetPuntiDecurtatiPerTrasgressore();
        IEnumerable<ViolazioniGraviViewModel> GetViolazioniSuperano10Punti();
        IEnumerable<ViolazioniGraviViewModel> GetViolazioniImportoMaggiore400();
    }
}
