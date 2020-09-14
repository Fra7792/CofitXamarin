using SQLite;

namespace CofitXamarin
{
    public class StrutturaConto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descrizione { get; set; }
        public string Importo { get; set; }
        public string Scadenza { get; set; }
        public string Pagato { get; set; }
    }
}