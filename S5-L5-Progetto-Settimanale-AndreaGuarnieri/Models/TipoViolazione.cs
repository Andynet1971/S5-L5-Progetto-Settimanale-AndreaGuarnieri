using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public class TipoViolazione
    {
        public int Idviolazione { get; set; }

        [Required(ErrorMessage = "La descrizione è obbligatoria")]
        [StringLength(100, ErrorMessage = "La descrizione non può superare i 100 caratteri")]
        public string Descrizione { get; set; }

        [Required(ErrorMessage = "L'importo è obbligatorio")]
        [Range(0.01, 10000, ErrorMessage = "L'importo deve essere compreso tra 0.01 e 10000")]
        public decimal Importo { get; set; }

        [Required(ErrorMessage = "Il decurtamento punti è obbligatorio")]
        [Range(0, 20, ErrorMessage = "Il decurtamento punti deve essere compreso tra 0 e 20")]
        public int DecurtamentoPunti { get; set; }
    }
}
