using CofitXamarin.Helper;
using CofitXamarin.Interfaces;
using CofitXamarin.Model;
using Plugin.CloudFirestore;
using Plugin.FilePicker;
using System;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin.Admin
{


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InsNovitaPage : ContentPage
    {

        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public InsNovitaPage()
        {
            InitializeComponent();
        }

        private async void btnExplorerFile_Clicked(object sender, EventArgs e)
        {
            FirebaseAuthentification metodo;
            metodo = DependencyService.Get<FirebaseAuthentification>();

            //recupero l'uid dell'admin
            string uid = metodo.RecoveryId();
            //recupero l'indirizzo email
            string email = metodo.RecoveryEmail();
            try
            {
                var fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                {
                    return; 
                }
                imgChoosed.Source = ImageSource.FromStream(() => //inserisco l'immagine nell'imageView
                {
                    var imageStram = fileData.GetStream();

                    return imageStram;

                });

                //carico la novità nello storage e nel cloud

                StrutturaUpload fileNovita = new StrutturaUpload();                
                progressBar.IsRunning = true;
                DependencyService.Get<IMessage>().LongAlert("Caricamento in corso");
                string nomeFile = fileData.FileName;                                        
                await firebaseHelper.UploadNovita(fileData.GetStream(), nomeFile);
                string url = await firebaseHelper.UploadNovita(fileData.GetStream(), nomeFile);
                fileNovita.NomeFile = nomeFile;
                fileNovita.Uri = url;
                await CrossCloudFirestore.Current
                .Instance
                .GetCollection("Novità").GetDocument(nomeFile).SetDataAsync(fileNovita);
                string message = "Caricamento effettuato";
                DependencyService.Get<IMessage>().ShortAlert(message);                
                progressBar.IsRunning = false;
            }
            catch (Exception ex2)
            {

            }
        }
    }


}