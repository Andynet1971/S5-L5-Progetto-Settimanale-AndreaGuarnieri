using System.Collections.Generic;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public interface ITipoViolazione
    {
        IEnumerable<TipoViolazione> GetAll();
        TipoViolazione GetById(int id);
        void Add(TipoViolazione tipoViolazione);
        void Update(TipoViolazione tipoViolazione);
        void Delete(int id);
    }
}
