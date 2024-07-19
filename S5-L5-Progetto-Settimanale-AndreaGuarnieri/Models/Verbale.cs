using System.ComponentModel.DataAnnotations;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public class Verbale
    {
        public int Idverbale { get; set; }

        [Required(ErrorMessage = "La data di violazione è obbligatoria")]
        public DateTime DataViolazione { get; set; }

        [Required(ErrorMessage = "L'indirizzo della violazione è obbligatorio")]
        [StringLength(100, ErrorMessage = "L'indirizzo della violazione non può superare i 100 caratteri")]
        public string IndirizzoViolazione { get; set; }

        [Required(ErrorMessage = "Il nominativo dell'agente è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il nominativo dell'agente non può superare i 50 caratteri")]
        public string Nominativo_Agente { get; set; }

        public DateTime? DataTrascrizioneVerbale { get; set; }

        public int Idanagrafica { get; set; }

        public Anagrafica Anagrafica { get; set; }

        public ICollection<VerbaleViolazioni> VerbaleViolazioni { get; set; }
    }
}
