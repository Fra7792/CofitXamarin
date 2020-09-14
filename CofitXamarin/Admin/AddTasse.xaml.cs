using CofitXamarin.Interfaces;
using CofitXamarin.Model;
using Plugin.CloudFirestore;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTasse : ContentPage
    {
        public AddTasse(string id)
        {
            InitializeComponent();

            addYears();

            uid.Text = id;  
        }
        private void addYears()
        {
            int minDate = 2017;
            DatePicker datePicker = new DatePicker();
            int maxYear = datePicker.Date.Year;
            for(int i=maxYear; i>=minDate; i--)
            {
                string year = i.ToString();
                pickerAnno.Items.Add(year);
            }
            
        }

        private async void btnAddTassa_Clicked(object sender, EventArgs e)
        {

            //se il tipo di tassa non è scelta non fa procedere
            if(pickerTassa.SelectedIndex == -1 || pickerAnno.SelectedIndex == -1 || pickerPeriodo.SelectedIndex == -1 || pickerStato.SelectedIndex == -1 || importo.Text == null)
            {
                DependencyService.Get<IMessage>().ShortAlert("Devi completare tutti i campi");
                return;
            }

            var sceltaTassa = pickerTassa.SelectedItem;
            string tassa = (string)sceltaTassa;
            var sceltaAnno = pickerAnno.SelectedItem;
            string anno = (string)sceltaAnno;
            var sceltaPeriodo = (string) pickerPeriodo.SelectedItem;
            string periodo = (string) sceltaPeriodo;
            string annoScadenza = dataScadenza.Date.ToString();            
            string id = uid.Text;
            var sceltaStato = pickerStato.SelectedItem;
            string stato = (string)sceltaStato;
            StrutturaTassa strutturaTassa = new StrutturaTassa();   //crea un oggetto di tipo tassa  

             //setta importo, scadenza, stato e periodo con i dati presi dalle view
            strutturaTassa.Importo = importo.Text;  
            strutturaTassa.Scadenza = annoScadenza;
            strutturaTassa.Stato = stato;
            strutturaTassa.Periodo = periodo + " " + anno;

            // in relazione allo stato della tassa setta il colore del testo
            if(stato.Equals("Emesso")) 
            {
                strutturaTassa.ColoreTesto = "Black";  
            }
            else if(stato.Equals("Scaduto"))
            {
                strutturaTassa.ColoreTesto = "Red";
            }
            else if(stato.Equals("Pagato"))
            {
                strutturaTassa.ColoreTesto = "Green";
            }

            /* il nome della tassa è formato dal tipo di tassa e dal periodo di riferimento (anno compreso)
             * successivamente verranno salvare le informazioni nel cloud di firebase 
             */

            string nomeTassa = tassa + " " + periodo + " " + anno; 
            strutturaTassa.Tassa = nomeTassa; //set del nome della tassa

            /*  in firebase verrà creata una collezione con l'uid dell'utente 
             *  e il documento verrà denominato utilizzando il nome completo della tassa
             */
            await CrossCloudFirestore.Current
               .Instance
               .GetCollection(id).GetDocument(nomeTassa).SetDataAsync(strutturaTassa); 
            await DisplayAlert("Successo", "Tassa inserita correttamente", "OK"); //al termine dell'operazione viene visualizzato un DisplayAlert

            await Navigation.PushAsync(new VisualizzaTasse(id));  //una volta inserita la tassa passo alla page contenente tutte le tasse inserite 
            Navigation.RemovePage(this);
        }
    }
   
}