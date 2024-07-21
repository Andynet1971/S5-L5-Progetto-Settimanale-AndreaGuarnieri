using System;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public interface IUtente
    {
        Utente Authenticate(string username, string password);
        void CreateUser(string username, string password, string role);
    }
}
