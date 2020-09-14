using CofitXamarin.Droid;
using CofitXamarin.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_Android))]
namespace CofitXamarin.Droid
{
    class SQLite_Android : ISQLite
    {
        SQLiteConnection con;
        public SQLiteConnection GetConnectionWithCreateDatabase()
        {
            string fileName = "creditiDatabase.db3";
            string documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine(documentPath, fileName);
            con = new SQLiteConnection(path);
            con.CreateTable<StrutturaConto>();
            return con;
        }
        public bool SaveCredito(StrutturaConto credito)
        {
            bool res = false;
            try
            {
                con.Insert(credito);
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public List<StrutturaConto> GetCrediti()
        {
            string sql = "SELECT * FROM StrutturaConto";
            List<StrutturaConto> crediti = con.Query<StrutturaConto>(sql);
            return crediti;
        }
        public bool UpdateCredito(StrutturaConto credito)
        {
            bool res = false;
            try
            {
                string sql = $"UPDATE StrutturaConto SET Tipo='{credito.Tipo}',Descrizione='{credito.Descrizione}',Importo='{credito.Importo}'," +
                                $"Scadenza='{credito.Scadenza}',Pagato='{credito.Pagato}' WHERE Id={credito.Id}";
                con.Execute(sql);
                res = true;
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public void DeleteCredito(int Id)
        {
            string sql = $"DELETE FROM StrutturaConto WHERE Id={Id}";
            con.Execute(sql);
        }
    }
}