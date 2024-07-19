namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public class VerbaliPerTrasgressoreViewModel
    {
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public int TotaleVerbali { get; set; }
    }

    public class PuntiDecurtatiPerTrasgressoreViewModel
    {
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public int TotalePuntiDecurtati { get; set; }
    }

    public class ViolazioniGraviViewModel
    {
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public DateTime DataViolazione { get; set; }
        public int DecurtamentoPunti { get; set; }
        public decimal Importo { get; set; }
    }
}
