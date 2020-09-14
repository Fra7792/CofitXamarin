using CofitXamarin.Helper;
using CofitXamarin.Interfaces;
using Firebase.Storage;
using Plugin.CloudFirestore;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profilo : ContentPage
        
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        MediaFile file;
        public Profilo()
        {
            InitializeComponent();

            try
            {
                recuperaFoto();

            }catch(Exception e)
            {

            }
            
            recoveryData();
                                 
        }

       

        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Anagrafica());
            Navigation.RemovePage(this);
        }
        public async void recoveryData() //recupero i dati dal cloud e li visualizzo nell'interfaccia utente
        {
           
            string id;
            FirebaseAuthentification metodo;
            metodo = DependencyService.Get<FirebaseAuthentification>();
            id = metodo.RecoveryId();
            string indEmail = metodo.RecoveryEmail();

            try {
                StrutturaUser user = new StrutturaUser();

                var document = await CrossCloudFirestore.Current
                                            .Instance
                                            .GetCollection("Users")
                                            .GetDocument(id)
                                            .GetDocumentAsync();

                var utente = document.ToObject<StrutturaUser>();

                denominazione.Text = utente.Denominazione;
                email.Text = indEmail;
                string citta = utente.Citta;
                string indirizzo = utente.Indirizzo;
                indirizzoCompleto.Text = indirizzo + ", " + citta;
                numTelefono.Text = utente.NumTelefono;
                numCellulare.Text = utente.NumCellulare;
                iva.Text = utente.Iva;
                cf.Text = utente.CodiceFiscale;
                tipoCliente.Text = utente.TipoCliente;
                contabilita.Text = utente.TipoContabilita;

            } catch(NullReferenceException e)
            {
                DependencyService.Get<IMessage>().ShortAlert("Inserisci i tuoi dati per completare la registrazione");
            }

        }

        private async void btnEditFoto_Clicked(object sender, EventArgs e) //bottone per cambiare l'immagine del profilo
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });
                if (file == null)
                    return;
                imgProfilo.Source = ImageSource.FromStream(() =>
                {
                    var imageStram = file.GetStream();

                    return imageStram;
                });
            }
            catch (Exception ex)
            {

            }

            string nomeFile = Path.GetFileName(file.Path);  //salvo l'immagine nello storage di firebase e l'url viene salvato tra i dati dell'utente.
            FirebaseAuthentification metodo;
            metodo = DependencyService.Get<FirebaseAuthentification>();
            string uid = metodo.RecoveryId();
            string estensione = Path.GetExtension(file.Path);            
            await firebaseHelper.UploadFoto(file.GetStream(), uid + "." + "jpg");
            string url = await firebaseHelper.UploadFile(file.GetStream(), nomeFile, uid, "Profilo");
            await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("Users")
                         .GetDocument(uid)
                         .UpdateDataAsync(new { Url = url });


        }
        public async void recuperaFoto() //recupero la foto del profilo
        {            
            FirebaseAuthentification metodo;
            metodo = DependencyService.Get<FirebaseAuthentification>();
            string id = metodo.RecoveryId();
            FirebaseStorage firebaseStorage = new FirebaseStorage("cofitxamarin.appspot.com");
            try {
                string url = await firebaseStorage
                .Child("Profilo")
                .Child(id + "." + "jpg")
                .GetDownloadUrlAsync();
                imgProfilo.Source = ImageSource.FromUri(new Uri(url)); //visualizzo la foto utilizzando l'url

            } catch(Exception e)
            {

            }
        }

    
    }
   
}