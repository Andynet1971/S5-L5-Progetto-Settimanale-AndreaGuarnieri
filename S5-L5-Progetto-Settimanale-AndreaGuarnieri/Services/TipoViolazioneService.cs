using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public class TipoViolazioneService : ITipoViolazione
    {
        private readonly string _connectionString;
        private readonly ILogger<TipoViolazioneService> _logger;

        public TipoViolazioneService(IConfiguration configuration, ILogger<TipoViolazioneService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public IEnumerable<TipoViolazione> GetAll()
        {
            var tipoViolazioni = new List<TipoViolazione>();

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM TIPO_VIOLAZIONE";
                var cmd = new SqlCommand(query, conn);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tipoViolazione = new TipoViolazione
                        {
                            Idviolazione = reader.GetInt32(3),
                            Descrizione = reader.GetString(0),
                            Importo = reader.GetDecimal(1),
                            DecurtamentoPunti = reader.GetInt32(2)
                        };
                        tipoViolazioni.Add(tipoViolazione);
                    }
                }
            }

            return tipoViolazioni;
        }

        public TipoViolazione GetById(int id)
        {
            TipoViolazione tipoViolazione = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM TIPO_VIOLAZIONE WHERE Idviolazione = @Id";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tipoViolazione = new TipoViolazione
                        {
                            Idviolazione = reader.GetInt32(3),
                            Descrizione = reader.GetString(0),
                            Importo = reader.GetDecimal(1),
                            DecurtamentoPunti = reader.GetInt32(2)
                        };
                    }
                }
            }

            return tipoViolazione;
        }

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
                    _logger.LogInformation("Violazione aggiunta al database: {@TipoViolazione}", tipoViolazione);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'inserimento della violazione: {@TipoViolazione}", tipoViolazione);
                throw;
            }
        }

        public void Update(TipoViolazione tipoViolazione)
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
                _logger.LogInformation("Violazione aggiornata nel database: {@TipoViolazione}", tipoViolazione);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM TIPO_VIOLAZIONE WHERE Idviolazione = @Id";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                _logger.LogInformation("Violazione eliminata dal database: {Id}", id);
            }
        }
    }
}
