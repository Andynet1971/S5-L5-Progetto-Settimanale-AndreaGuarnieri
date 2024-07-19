using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public class VerbaleService : IVerbale
    {
        private readonly string _connectionString;
        private readonly ILogger<VerbaleService> _logger;

        public VerbaleService(IConfiguration configuration, ILogger<VerbaleService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

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
                    _logger.LogInformation("Verbale aggiunto al database: {@Verbale}", verbale);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'inserimento del verbale: {@Verbale}", verbale);
                throw;
            }
        }

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
                    _logger.LogInformation("Verbale aggiornato nel database: {@Verbale}", verbale);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'aggiornamento del verbale: {@Verbale}", verbale);
                throw;
            }
        }

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
                    _logger.LogInformation("Violazione aggiunta al verbale nel database: {@VerbaleViolazioni}", verbaleViolazioni);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'inserimento della violazione del verbale: {@VerbaleViolazioni}", verbaleViolazioni);
                throw;
            }
        }

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
