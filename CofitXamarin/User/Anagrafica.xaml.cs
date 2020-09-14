using CofitXamarin.Interfaces;
using Plugin.CloudFirestore;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Anagrafica : ContentPage
    {
        public Anagrafica()
        {
            InitializeComponent();
                     
            recoveryData();
                                    
        }


        private async void btnInserisci_Clicked(object sender, EventArgs e)  //inserisco tutti i dati nel database
        {
            if(denominazione.Text == null)
            {
                DependencyService.Get<IMessage>().ShortAlert("Inserisci la denominazione");
                return;
            }

            string id;
            FirebaseAuthentification metodo;
            metodo = DependencyService.Get<FirebaseAuthentification>();
            id = metodo.RecoveryId();

            string email;
            email = metodo.RecoveryEmail();

            var scelta = pickerTipoCliente.SelectedItem;
            string tipoCliente = (string)scelta;

            var scelta2 = pickerContabilita.SelectedItem;
            string tipoCont = (string)scelta2;
          
            StrutturaUser user = new StrutturaUser();
            user.Id = id;
            user.Email = email;
            user.Denominazione = denominazione.Text;
            user.Search = denominazione.Text.ToLowerInvariant();
            user.Indirizzo = indirizzo.Text;
            user.Citta = citta.Text;
            user.NumTelefono = numTelefono.Text;
            user.NumCellulare = numCellulare.Text;
            user.Iva = iva.Text.ToUpper();
            user.CodiceFiscale = cf.Text.ToUpper();
            user.TipoContabilita = tipoCont;
            user.TipoCliente = tipoCliente;

            await CrossCloudFirestore.Current
                .Instance
                .GetCollection("Users").GetDocument(id).SetDataAsync(user); 
            await DisplayAlert("Successo", "Anagrafica inserita correttamente", "OK");

            await Navigation.PushAsync(new Profilo());
            Navigation.RemovePage(this);
        }

        public async void recoveryData()
        {
            string id;
            FirebaseAuthentification metodo;
            metodo = DependencyService.Get<FirebaseAuthentification>();
            id = metodo.RecoveryId();

            StrutturaUser user = new StrutturaUser();

            try
            {
                var document = await CrossCloudFirestore.Current
                                        .Instance
                                        .GetCollection("Users")
                                        .GetDocument(id)
                                        .GetDocumentAsync();

                var utente = document.ToObject<StrutturaUser>();

                denominazione.Text = utente.Denominazione;
                citta.Text = utente.Citta;
                indirizzo.Text = utente.Indirizzo;
                numTelefono.Text = utente.NumTelefono;
                numCellulare.Text = utente.NumCellulare;
                iva.Text = utente.Iva.ToUpper();
                cf.Text = utente.CodiceFiscale.ToUpper();
                var scelta1 = utente.TipoCliente;
                var scelta2 = utente.TipoContabilita;
                pickerTipoCliente.SelectedItem = scelta1;
                pickerContabilita.SelectedItem = scelta2;
            } catch(NullReferenceException e)
            {

            }
            

        }
    }
}