using System.Data.SqlClient;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public class TipoViolazioneService : ITipoViolazione
    {
        private readonly string _connectionString;

        // Costruttore che inizializza la stringa di connessione tramite dependency injection
        public TipoViolazioneService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Metodo per ottenere tutte le violazioni
        public IEnumerable<TipoViolazione> GetAll()
        {
            var tipoViolazioni = new List<TipoViolazione>();

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM TIPO_VIOLAZIONE";
                var cmd = new SqlCommand(query, conn);
                conn.Open();

                // Esegue la query e legge i risultati
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tipoViolazione = new TipoViolazione
                        {
                            Idviolazione = reader.GetInt32(reader.GetOrdinal("Idviolazione")),
                            Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                            Importo = reader.GetDecimal(reader.GetOrdinal("Importo")),
                            DecurtamentoPunti = reader.GetInt32(reader.GetOrdinal("DecurtamentoPunti"))
                        };
                        tipoViolazioni.Add(tipoViolazione);
                    }
                }
            }

            return tipoViolazioni;
        }

        // Metodo per ottenere una violazione specifica per ID
        public TipoViolazione GetById(int id)
        {
            TipoViolazione tipoViolazione = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM TIPO_VIOLAZIONE WHERE Idviolazione = @Id";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                // Esegue la query e legge il risultato
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tipoViolazione = new TipoViolazione
                        {
                            Idviolazione = reader.GetInt32(reader.GetOrdinal("Idviolazione")),
                            Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                            Importo = reader.GetDecimal(reader.GetOrdinal("Importo")),
                            DecurtamentoPunti = reader.GetInt32(reader.GetOrdinal("DecurtamentoPunti"))
                        };
                    }
                }
            }

            return tipoViolazione;
        }

        // Metodo per aggiungere una nuova violazione
        public void Add(TipoViolazione tipoViolazione)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO TIPO_VIOLAZIONE (Descrizione, Importo, DecurtamentoPunti) VALUES (@Descrizione, @Importo, @DecurtamentoPunti)";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Descrizione", tipoViolazione.Descrizione);
                    cmd.Parameters.AddWithValue("@Importo", tipoViolazione.Importo);
                    cmd.Parameters.AddWithValue("@DecurtamentoPunti", tipoViolazione.DecurtamentoPunti);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Metodo per aggiornare una violazione esistente
        public void Update(TipoViolazione tipoViolazione)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE TIPO_VIOLAZIONE SET Descrizione = @Descrizione, Importo = @Importo, DecurtamentoPunti = @DecurtamentoPunti WHERE Idviolazione = @Id";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", tipoViolazione.Idviolazione);
                    cmd.Parameters.AddWithValue("@Descrizione", tipoViolazione.Descrizione);
                    cmd.Parameters.AddWithValue("@Importo", tipoViolazione.Importo);
                    cmd.Parameters.AddWithValue("@DecurtamentoPunti", tipoViolazione.DecurtamentoPunti);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Metodo per eliminare una violazione per ID
        public void Delete(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Elimina i record correlati da VERBALE_VIOLAZIONI
                    string deleteVerbaleViolazioniQuery = "DELETE FROM VERBALE_VIOLAZIONI WHERE Idviolazione = @Id";
                    using (var deleteVerbaleViolazioniCmd = new SqlCommand(deleteVerbaleViolazioniQuery, conn))
                    {
                        deleteVerbaleViolazioniCmd.Parameters.AddWithValue("@Id", id);
                        deleteVerbaleViolazioniCmd.ExecuteNonQuery();
                    }

                    // Elimina il record da TIPO_VIOLAZIONE
                    string deleteTipoViolazioneQuery = "DELETE FROM TIPO_VIOLAZIONE WHERE Idviolazione = @Id";
                    using (var deleteTipoViolazioneCmd = new SqlCommand(deleteTipoViolazioneQuery, conn))
                    {
                        deleteTipoViolazioneCmd.Parameters.AddWithValue("@Id", id);
                        deleteTipoViolazioneCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
