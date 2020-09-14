using CofitXamarin.Interfaces;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDebiti : ContentPage
    {

        StrutturaConto debitoDetails;
        public AddDebiti(StrutturaConto details)
        {
            InitializeComponent();

            if (details != null)
            {
                debitoDetails = details;
                PopulateDetails(debitoDetails);
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
            this.Title = "Modifica debito";
        }

        private void SaveDebito(object sender, EventArgs e)
        {
            if (saveBtn.Text == "Aggiungi")
            {
                StrutturaConto debito = new StrutturaConto();
                var scelta = spinner.SelectedItem;
                debito.Tipo = (string)scelta;
                debito.Descrizione = descrizione.Text;
                debito.Importo = importo.Text;
                debito.Scadenza = data_scadenza.Date.ToString(); ;
                if (da_pagare.IsChecked == true)
                {
                    debito.Pagato = "Sì";
                }

                else if (da_pagare.IsChecked == false)
                {
                    debito.Pagato = "No";
                }
                

                bool res = DependencyService.Get<ISQLiteDebiti>().SaveDebito(debito);
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
                debitoDetails.Tipo = (string)scelta;
                debitoDetails.Descrizione = descrizione.Text;
                debitoDetails.Importo = importo.Text;
                debitoDetails.Scadenza = data_scadenza.Date.ToString();
                if (da_pagare.IsChecked == true)
                {
                    debitoDetails.Pagato = "Sì";
                }
                else if (da_pagare.IsChecked == false)
                {
                    debitoDetails.Pagato = "No";
                }
                bool res = DependencyService.Get<ISQLiteDebiti>().UpdateDebito(debitoDetails);
                if (res)
                {
                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Message", "Modifica non riuscita", "Ok");
                }
            }
        }


    }
}