using SQLite;
using System.Collections.Generic;


namespace CofitXamarin.Interfaces
{
    public interface ISQLite  //interfaccia per CRUD dei crediti
    {
        SQLiteConnection GetConnectionWithCreateDatabase();

        bool SaveCredito(StrutturaConto conto);

        List<StrutturaConto> GetCrediti();

        bool UpdateCredito(StrutturaConto conto);
        void DeleteCredito(int Id);
    }
}

