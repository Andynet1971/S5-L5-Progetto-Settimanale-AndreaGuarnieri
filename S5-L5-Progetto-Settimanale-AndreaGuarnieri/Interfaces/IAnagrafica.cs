
namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public interface IAnagrafica
    {
        IEnumerable<Anagrafica> GetAll();
        Anagrafica GetById(int id);
        void Add(Anagrafica anagrafica);
        void Update(Anagrafica anagrafica);
        void Delete(int id);
    }
}
