
using System.ComponentModel.DataAnnotations;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public class VerbaleCreateViewModel
    {
        [Required(ErrorMessage = "La data di violazione è obbligatoria")]
        public DateTime DataViolazione { get; set; }

        [Required(ErrorMessage = "L'indirizzo della violazione è obbligatorio")]
        [StringLength(100, ErrorMessage = "L'indirizzo della violazione non può superare i 100 caratteri")]
        public string IndirizzoViolazione { get; set; }

        [Required(ErrorMessage = "Il nominativo dell'agente è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il nominativo dell'agente non può superare i 50 caratteri")]
        public string Nominativo_Agente { get; set; }

        public DateTime? DataTrascrizioneVerbale { get; set; }

        // Campi per l'anagrafica
        [Required(ErrorMessage = "Il cognome è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il cognome non può superare i 50 caratteri")]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il nome non può superare i 50 caratteri")]
        public string Nome { get; set; }

        [StringLength(100, ErrorMessage = "L'indirizzo non può superare i 100 caratteri")]
        public string Indirizzo { get; set; }

        [StringLength(50, ErrorMessage = "La città non può superare i 50 caratteri")]
        public string Città { get; set; }

        [StringLength(5, ErrorMessage = "Il CAP non può superare i 5 caratteri")]
        public string CAP { get; set; }

        [Required(ErrorMessage = "Il codice fiscale è obbligatorio")]
        [StringLength(16, ErrorMessage = "Il codice fiscale non può superare i 16 caratteri")]
        public string Cod_Fisc { get; set; }

        [Required(ErrorMessage = "Le violazioni sono obbligatorie")]
        public List<int> Idviolazioni { get; set; } = new List<int>();
    }
}
