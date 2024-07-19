namespace S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models
{
    public class VerbaleViolazioni
    {
        public int Idverbale { get; set; }
        public Verbale Verbale { get; set; }
        public int Idviolazione { get; set; }
        public TipoViolazione TipoViolazione { get; set; }
    }
}
