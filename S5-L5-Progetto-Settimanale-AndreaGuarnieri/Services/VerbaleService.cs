using System.Data.SqlClient;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public class VerbaleService : IVerbale
    {
        private readonly string _connectionString;

        // Costruttore che inizializza la stringa di connessione tramite dependency injection
        public VerbaleService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Metodo per ottenere tutti i verbali
        public IEnumerable<Verbale> GetAll()
        {
            var verbali = new List<Verbale>();

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM VERBALE";
                var cmd = new SqlCommand(query, conn);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var verbale = new Verbale
                        {
                            Idverbale = reader.GetInt32(5),
                            DataViolazione = reader.GetDateTime(0),
                            IndirizzoViolazione = reader.GetString(1),
                            Nominativo_Agente = reader.GetString(2),
                            DataTrascrizioneVerbale = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                            Idanagrafica = reader.GetInt32(4)
                        };
                        verbali.Add(verbale);
                    }
                }
            }

            return verbali;
        }

        // Metodo per ottenere i dettagli dei verbali
        public IEnumerable<VerbaleDetailViewModel> GetVerbaliWithDetails()
        {
            var query = @"
                SELECT v.Idverbale, v.DataViolazione, v.IndirizzoViolazione, a.Cognome, a.Nome, tv.Descrizione
                FROM VERBALE v
                JOIN ANAGRAFICA a ON v.Idanagrafica = a.Idanagrafica
                JOIN VERBALE_VIOLAZIONI vv ON v.Idverbale = vv.Idverbale
                JOIN TIPO_VIOLAZIONE tv ON vv.Idviolazione = tv.Idviolazione";

            var verbali = new List<VerbaleDetailViewModel>();

            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(query, conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var verbale = new VerbaleDetailViewModel
                        {
                            Idverbale = reader.GetInt32(0),
                            DataViolazione = reader.GetDateTime(1),
                            IndirizzoViolazione = reader.GetString(2),
                            Cognome = reader.GetString(3),
                            Nome = reader.GetString(4),
                            DescrizioneViolazione = reader.GetString(5)
                        };
                        verbali.Add(verbale);
                    }
                }
            }

            return verbali;
        }

        // Metodo per ottenere un verbale specifico per ID
        public Verbale GetById(int id)
        {
            Verbale verbale = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM VERBALE WHERE Idverbale = @Id";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        verbale = new Verbale
                        {
                            Idverbale = reader.GetInt32(5),
                            DataViolazione = reader.GetDateTime(0),
                            IndirizzoViolazione = reader.GetString(1),
                            Nominativo_Agente = reader.GetString(2),
                            DataTrascrizioneVerbale = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                            Idanagrafica = reader.GetInt32(4)
                        };
                    }
                }
            }

            return verbale;
        }

        // Metodo per aggiungere un nuovo verbale
        public void Add(Verbale verbale)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO VERBALE (DataViolazione, IndirizzoViolazione, Nominativo_Agente, DataTrascrizioneVerbale, Idanagrafica) VALUES (@DataViolazione, @IndirizzoViolazione, @Nominativo_Agente, @DataTrascrizioneVerbale, @Idanagrafica); SELECT CAST(scope_identity() AS int)";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                    cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione);
                    cmd.Parameters.AddWithValue("@Nominativo_Agente", verbale.Nominativo_Agente);
                    cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Idanagrafica", verbale.Idanagrafica);
                    conn.Open();
                    verbale.Idverbale = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Metodo per aggiornare un verbale esistente
        public void Update(Verbale verbale)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE VERBALE SET DataViolazione = @DataViolazione, IndirizzoViolazione = @IndirizzoViolazione, Nominativo_Agente = @Nominativo_Agente, DataTrascrizioneVerbale = @DataTrascrizioneVerbale, Idanagrafica = @Idanagrafica WHERE Idverbale = @Idverbale";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                    cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione);
                    cmd.Parameters.AddWithValue("@Nominativo_Agente", verbale.Nominativo_Agente);
                    cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Idanagrafica", verbale.Idanagrafica);
                    cmd.Parameters.AddWithValue("@Idverbale", verbale.Idverbale);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Metodo per aggiungere una violazione ad un verbale esistente
        public void AddVerbaleViolazioni(VerbaleViolazioni verbaleViolazioni)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO VERBALE_VIOLAZIONI (Idverbale, Idviolazione) VALUES (@Idverbale, @Idviolazione)";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Idverbale", verbaleViolazioni.Idverbale);
                    cmd.Parameters.AddWithValue("@Idviolazione", verbaleViolazioni.Idviolazione);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Metodo per ottenere il totale dei verbali per trasgressore
        public IEnumerable<VerbaliPerTrasgressoreViewModel> GetVerbaliPerTrasgressore()
        {
            var result = new List<VerbaliPerTrasgressoreViewModel>();

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT a.Cognome, a.Nome, COUNT(v.Idverbale) AS TotaleVerbali
                    FROM ANAGRAFICA a
                    JOIN VERBALE v ON a.Idanagrafica = v.Idanagrafica
                    GROUP BY a.Cognome, a.Nome";
                var cmd = new SqlCommand(query, conn);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new VerbaliPerTrasgressoreViewModel
                        {
                            Cognome = reader.GetString(0),
                            Nome = reader.GetString(1),
                            TotaleVerbali = reader.GetInt32(2)
                        });
                    }
                }
            }

            return result;
        }

        // Metodo per ottenere il totale dei punti decurtati per trasgressore
        public IEnumerable<PuntiDecurtatiPerTrasgressoreViewModel> GetPuntiDecurtatiPerTrasgressore()
        {
            var result = new List<PuntiDecurtatiPerTrasgressoreViewModel>();

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT a.Cognome, a.Nome, SUM(tv.DecurtamentoPunti) AS TotalePuntiDecurtati
                    FROM ANAGRAFICA a
                    JOIN VERBALE v ON a.Idanagrafica = v.Idanagrafica
                    JOIN VERBALE_VIOLAZIONI vv ON v.Idverbale = vv.Idverbale
                    JOIN TIPO_VIOLAZIONE tv ON vv.Idviolazione = tv.Idviolazione
                    GROUP BY a.Cognome, a.Nome";
                var cmd = new SqlCommand(query, conn);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new PuntiDecurtatiPerTrasgressoreViewModel
                        {
                            Cognome = reader.GetString(0),
                            Nome = reader.GetString(1),
                            TotalePuntiDecurtati = reader.GetInt32(2)
                        });
                    }
                }
            }

            return result;
        }

        // Metodo per ottenere le violazioni che superano 10 punti di decurtamento
        public IEnumerable<ViolazioniGraviViewModel> GetViolazioniSuperano10Punti()
        {
            var result = new List<ViolazioniGraviViewModel>();

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT a.Cognome, a.Nome, v.DataViolazione, tv.DecurtamentoPunti, tv.Importo
                    FROM ANAGRAFICA a
                    JOIN VERBALE v ON a.Idanagrafica = v.Idanagrafica
                    JOIN VERBALE_VIOLAZIONI vv ON v.Idverbale = vv.Idverbale
                    JOIN TIPO_VIOLAZIONE tv ON vv.Idviolazione = tv.Idviolazione
                    WHERE tv.DecurtamentoPunti > 10";
                var cmd = new SqlCommand(query, conn);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new ViolazioniGraviViewModel
                        {
                            Cognome = reader.GetString(0),
                            Nome = reader.GetString(1),
                            DataViolazione = reader.GetDateTime(2),
                            DecurtamentoPunti = reader.GetInt32(3),
                            Importo = reader.GetDecimal(4)
                        });
                    }
                }
            }

            return result;
        }

        // Metodo per ottenere le violazioni con importo maggiore di 400 euro
        public IEnumerable<ViolazioniGraviViewModel> GetViolazioniImportoMaggiore400()
        {
            var result = new List<ViolazioniGraviViewModel>();

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT a.Cognome, a.Nome, v.DataViolazione, tv.DecurtamentoPunti, tv.Importo
                    FROM ANAGRAFICA a
                    JOIN VERBALE v ON a.Idanagrafica = v.Idanagrafica
                    JOIN VERBALE_VIOLAZIONI vv ON v.Idverbale = vv.Idverbale
                    JOIN TIPO_VIOLAZIONE tv ON vv.Idviolazione = tv.Idviolazione
                    WHERE tv.Importo > 400";
                var cmd = new SqlCommand(query, conn);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new ViolazioniGraviViewModel
                        {
                            Cognome = reader.GetString(0),
                            Nome = reader.GetString(1),
                            DataViolazione = reader.GetDateTime(2),
                            DecurtamentoPunti = reader.GetInt32(3),
                            Importo = reader.GetDecimal(4)
                        });
                    }
                }
            }

            return result;
        }
    }
}
