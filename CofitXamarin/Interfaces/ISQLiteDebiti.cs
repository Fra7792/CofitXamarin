using SQLite;
using System.Collections.Generic;


namespace CofitXamarin.Interfaces
{
    public interface ISQLiteDebiti //interfaccia per CRUD database dei debiti
    {
        SQLiteConnection GetConnectionWithCreateDatabase();

        bool SaveDebito(StrutturaConto conto);

        List<StrutturaConto> GetDebiti();

        bool UpdateDebito(StrutturaConto conto);
        void DeleteDebito(int Id);
    }
}
