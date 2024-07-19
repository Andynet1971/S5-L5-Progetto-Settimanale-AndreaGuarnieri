using System.ComponentModel.DataAnnotations;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public class Anagrafica
    {
        public int Idanagrafica { get; set; }

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

        public ICollection<Verbale> Verbali { get; set; }
    }
}
