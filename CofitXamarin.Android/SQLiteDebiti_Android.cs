using CofitXamarin.Droid;
using CofitXamarin.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDebiti_Android))]
namespace CofitXamarin.Droid
{
    class SQLiteDebiti_Android : ISQLiteDebiti
    {
        SQLiteConnection con;
        public SQLiteConnection GetConnectionWithCreateDatabase()
        {
            string fileName = "debitiDatabase.db3";
            string documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine(documentPath, fileName);
            con = new SQLiteConnection(path);
            con.CreateTable<StrutturaConto>();
            return con;
        }
        public bool SaveDebito(StrutturaConto debito)
        {
            bool res = false;
            try
            {
                con.Insert(debito);
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public List<StrutturaConto> GetDebiti()
        {
            string sql = "SELECT * FROM StrutturaConto";
            List<StrutturaConto> debito = con.Query<StrutturaConto>(sql);
            return debito;
        }
        public bool UpdateDebito(StrutturaConto debito)
        {
            bool res = false;
            try
            {
                string sql = $"UPDATE StrutturaConto SET Tipo='{debito.Tipo}',Descrizione='{debito.Descrizione}',Importo='{debito.Importo}'," +
                                $"Scadenza='{debito.Scadenza}',Pagato='{debito.Pagato}' WHERE Id={debito.Id}";
                con.Execute(sql);
                res = true;
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public void DeleteDebito(int Id)
        {
            string sql = $"DELETE FROM StrutturaConto WHERE Id={Id}";
            con.Execute(sql);
        }
    }
}