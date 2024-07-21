using System.Data.SqlClient;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public class AnagraficaService : IAnagrafica
    {
        private readonly string _connectionString;

        // Costruttore che inizializza la stringa di connessione e il logger tramite dependency injection
        public AnagraficaService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Metodo per ottenere tutte le anagrafiche
        public IEnumerable<Anagrafica> GetAll()
        {
            var anagrafiche = new List<Anagrafica>();

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM ANAGRAFICA";
                var cmd = new SqlCommand(query, conn);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var anagrafica = new Anagrafica
                        {
                            Idanagrafica = reader.GetInt32(6),
                            Cognome = reader.GetString(0),
                            Nome = reader.GetString(1),
                            Indirizzo = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Città = reader.IsDBNull(3) ? null : reader.GetString(3),
                            CAP = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Cod_Fisc = reader.GetString(5)
                        };
                        anagrafiche.Add(anagrafica);
                    }
                }
            }

            return anagrafiche;
        }

        // Metodo per ottenere un'anagrafica specifica per ID
        public Anagrafica GetById(int id)
        {
            Anagrafica anagrafica = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM ANAGRAFICA WHERE Idanagrafica = @Id";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        anagrafica = new Anagrafica
                        {
                            Idanagrafica = reader.GetInt32(6),
                            Cognome = reader.GetString(0),
                            Nome = reader.GetString(1),
                            Indirizzo = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Città = reader.IsDBNull(3) ? null : reader.GetString(3),
                            CAP = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Cod_Fisc = reader.GetString(5)
                        };
                    }
                }
            }

            return anagrafica;
        }

        // Metodo per aggiungere una nuova anagrafica
        public void Add(Anagrafica anagrafica)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO ANAGRAFICA (Cognome, Nome, Indirizzo, Città, CAP, Cod_Fisc) VALUES (@Cognome, @Nome, @Indirizzo, @Città, @CAP, @Cod_Fisc); SELECT CAST(scope_identity() AS int)";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Cognome", anagrafica.Cognome);
                    cmd.Parameters.AddWithValue("@Nome", anagrafica.Nome);
                    cmd.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Città", anagrafica.Città ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CAP", anagrafica.CAP ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Cod_Fisc", anagrafica.Cod_Fisc);
                    conn.Open();
                    anagrafica.Idanagrafica = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Metodo per aggiornare un'anagrafica esistente
        public void Update(Anagrafica anagrafica)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE ANAGRAFICA SET Cognome = @Cognome, Nome = @Nome, Indirizzo = @Indirizzo, Città = @Città, CAP = @CAP, Cod_Fisc = @Cod_Fisc WHERE Idanagrafica = @Id";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", anagrafica.Idanagrafica);
                cmd.Parameters.AddWithValue("@Cognome", anagrafica.Cognome);
                cmd.Parameters.AddWithValue("@Nome", anagrafica.Nome);
                cmd.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Città", anagrafica.Città ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CAP", anagrafica.CAP ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Cod_Fisc", anagrafica.Cod_Fisc);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Metodo per eliminare un'anagrafica per ID
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM ANAGRAFICA WHERE Idanagrafica = @Id";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
