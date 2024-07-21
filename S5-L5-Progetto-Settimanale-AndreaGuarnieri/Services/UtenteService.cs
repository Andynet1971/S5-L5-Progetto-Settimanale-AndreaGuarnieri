using System.Data.SqlClient;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public class UtenteService : IUtente
    {
        private readonly string _connectionString;

        // Costruttore che inizializza la stringa di connessione tramite dependency injection
        public UtenteService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Metodo per autenticare un utente
        public Utente Authenticate(string username, string password)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Utenti WHERE Username = @Username";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();

                // Esegue la query e legge i risultati
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var user = new Utente
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Password = reader.GetString(2),
                            Role = reader.GetString(3)
                        };

                        // Verifica la password
                        if (password == user.Password)
                        {
                            return user;
                        }
                    }
                }
            }

            return null;
        }

        // Metodo per creare un nuovo utente
        public void CreateUser(string username, string password, string role)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Utenti (Username, Password, Role) VALUES (@Username, @Password, @Role)";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Role", role);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
