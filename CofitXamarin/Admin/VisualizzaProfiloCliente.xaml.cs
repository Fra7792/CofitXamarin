using Plugin.CloudFirestore;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisualizzaProfiloCliente : ContentPage
    {
        public VisualizzaProfiloCliente(string id)
        {
            InitializeComponent();
           
            uid.Text = id;

            recoveryData(id);
        }

        //recupero tutti i dati del cliente dal database utilizzando il suo UID e li visualizzo nell'interfaccia utente

        public async void recoveryData(string id)
        {
           
            try
            {
                StrutturaUser user = new StrutturaUser();

                var document = await CrossCloudFirestore.Current
                                            .Instance
                                            .GetCollection("Users")
                                            .GetDocument(id)
                                            .GetDocumentAsync();

                var utente = document.ToObject<StrutturaUser>();

                denominazione.Text = utente.Denominazione;

                string citta = utente.Citta;
                string indirizzo = utente.Indirizzo;
                indirizzoCompleto.Text = indirizzo + ", " + citta;
                email.Text = utente.Email;
                numTelefono.Text = utente.NumTelefono;
                numCellulare.Text = utente.NumCellulare;
                iva.Text = utente.Iva;
                cf.Text = utente.CodiceFiscale;
                tipoCliente.Text = utente.TipoCliente;
                contabilita.Text = utente.TipoContabilita;
                imgProfilo.Source = ImageSource.FromUri(new Uri(utente.Url));

            }
            catch (Exception e)
            {

            }

        }

        private void btnCell_Clicked(object sender, EventArgs e)
        {
            string number = numCellulare.Text;
            PhoneDialer.Open(number);
        }

        private void btnEmail_Clicked(object sender, EventArgs e)
        {
            string indEmail = email.Text;
            Device.OpenUri(new Uri("mailto:" + indEmail));
        }
    }


}


