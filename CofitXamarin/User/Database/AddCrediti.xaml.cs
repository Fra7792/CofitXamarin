using CofitXamarin.Interfaces;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCrediti : ContentPage
    {

        StrutturaConto creditoDetails;
        public AddCrediti(StrutturaConto details)
        {
            InitializeComponent();

            if (details != null)
            {
                creditoDetails = details;
                PopulateDetails(creditoDetails);
            }
        }

        private void PopulateDetails(StrutturaConto details)
        {
            var scelta = details.Tipo;
            spinner.SelectedItem = scelta;
            descrizione.Text = details.Descrizione;
            importo.Text = details.Importo;
            DateTime data = DateTime.Parse(details.Scadenza);
            data_scadenza.Date = data;
            string pagato = details.Pagato.Trim();
            if (pagato.Equals("Sì"))
            {
                da_pagare.IsChecked = true;                
            }
            else if (pagato.Equals("No"))
                {
                    da_pagare.IsChecked = false;                   
            }
            saveBtn.Text = "Modifica";
            this.Title = "Modifica credito";
        }

        private void SaveCredito(object sender, EventArgs e)
        {
            if (saveBtn.Text == "Aggiungi")
            {
                StrutturaConto credito = new StrutturaConto();
                var scelta = spinner.SelectedItem;
                credito.Tipo = (string) scelta;
                credito.Descrizione = descrizione.Text;
                credito.Importo = importo.Text;
                credito.Scadenza = data_scadenza.Date.ToString(); ;
                if (da_pagare.IsChecked == true)
                {
                    credito.Pagato = "Sì";                    
                }

               else if (da_pagare.IsChecked == false)
                {
                    credito.Pagato = "No";                    
                }
                

                bool res = DependencyService.Get<ISQLite>().SaveCredito(credito);
                if (res)
                {
                    Navigation.PushAsync(new MainPage());
                    
                }
                else
                {
                    DisplayAlert("Message", "Inserimento fallito", "Ok");
                }
            }
            else
            {                
                var scelta = spinner.SelectedItem;
                creditoDetails.Tipo = (string) scelta;
                creditoDetails.Descrizione = descrizione.Text;
                creditoDetails.Importo = importo.Text;
                creditoDetails.Scadenza = data_scadenza.Date.ToString();             
                if (da_pagare.IsChecked == true)
                {
                    creditoDetails.Pagato = "Sì";                    
                }
                else if (da_pagare.IsChecked == false)
                {
                    creditoDetails.Pagato = "No";                    
                }
                bool res = DependencyService.Get<ISQLite>().UpdateCredito(creditoDetails);
                if (res)
                {
                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Message", "Data Failed To Update", "Ok");
                }
            }
        }

     
    }
}