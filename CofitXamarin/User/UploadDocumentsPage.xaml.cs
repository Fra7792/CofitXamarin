using CofitXamarin.Helper;
using CofitXamarin.Interfaces;
using CofitXamarin.Model;
using Plugin.CloudFirestore;
using Plugin.FilePicker;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin.User
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadDocumentsPage : ContentPage
    {

        FirebaseHelper firebaseHelper = new FirebaseHelper();
        MediaFile file;
        FirebaseAuthentification metodo = DependencyService.Get<FirebaseAuthentification>();

        public UploadDocumentsPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {            
            string uid = metodo.RecoveryId();
            base.OnAppearing();
            var allUpload = await firebaseHelper.GetAllUpload(uid, "Upload");
            lstFiles.ItemsSource = allUpload;

        }
       
        private async void btnCaptureCamera_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if(!CrossMedia.Current.IsTakePhotoSupported && !CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Messaggio", "Foto Capture and Pick Not supported", "Ok");
                return;
            }
            else
            {
                file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Images",
                    Name = DateTime.Now + "_test.jpg"
                });

                if(file == null)
                {
                    return;
                }
                imgChoosed.Source = ImageSource.FromStream(() =>
                {
                    var imageStram = file.GetStream();

                    return imageStram;
                });
            }
        }

        private async void btnExplorerFile_Clicked(object sender, EventArgs e)
        {
            if (pickerUpload.SelectedIndex == -1)
            {
                DependencyService.Get<IMessage>().ShortAlert("Seleziona il tipo di documento da inviare");
                return;
            }

            string uid = metodo.RecoveryId();
            string email = metodo.RecoveryEmail();
            try
            {
                var fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                {
                    return; 
                }
                imgChoosed.Source = ImageSource.FromStream(() =>
                {
                    var imageStram = fileData.GetStream();
                    
                    return imageStram;                   

                });

                progressBar.IsRunning = true;
                DependencyService.Get<IMessage>().LongAlert("Caricamento in corso");                
                string estensione = Path.GetExtension(fileData.FileName);                
                string nomeFileCompleto = nomeFile() + estensione;
                await firebaseHelper.UploadFile(fileData.GetStream(), nomeFileCompleto, uid, "Upload");
                string imgurl = await firebaseHelper.UploadFile(fileData.GetStream(), nomeFileCompleto, uid, "Upload");
                await firebaseHelper.AddDocument(nomeFileCompleto, imgurl, uid, "Upload");
                invioNotifica(uid, email);                          
                string message = "Caricamento effettuato";               
                DependencyService.Get<IMessage>().ShortAlert(message);
                OnAppearing();
                progressBar.IsRunning = false;
            } catch(Exception ex2)
            {

            }
        }

        private async void invioNotifica(string uid, string email)
        {
            StrutturaNotifica notifica = new StrutturaNotifica();
            notifica.Id = uid;
            notifica.Data = dataInserimento();
            notifica.Email = email;
            notifica.ColoreTesto = "Red";
            await CrossCloudFirestore.Current
                .Instance
                .GetCollection("Notifiche").GetDocument(uid).SetDataAsync(notifica);
        }
      
        private async void BtnUpload_Clicked(object sender, EventArgs e)
        {
            if(imgChoosed.Source == null )
            {
                DependencyService.Get<IMessage>().ShortAlert("Scatta prima la foto");
                return;
            }

            if(pickerUpload.SelectedIndex == -1)
            {
                DependencyService.Get<IMessage>().ShortAlert("Seleziona il tipo di documento da inviare");
                return;
            }
            progressBar.IsRunning = true;
            DependencyService.Get<IMessage>().LongAlert("Caricamento in corso");            
            
            string uid = metodo.RecoveryId();
            string email = metodo.RecoveryEmail();                  
            string extension = Path.GetExtension(file.Path);
            string nomeFileCompleto = nomeFile() + extension;            
            await firebaseHelper.UploadFile(file.GetStream(), nomeFileCompleto, uid, "Upload");
            string imgurl = await firebaseHelper.UploadFile(file.GetStream(), nomeFileCompleto, uid, "Upload");                                   
            await firebaseHelper.AddDocument(nomeFileCompleto, imgurl, uid, "Upload");
            invioNotifica(uid, email);            
            OnAppearing();            
            DependencyService.Get<IMessage>().ShortAlert("Caricamento effettuato");
            OnAppearing();
            progressBar.IsRunning = false;

        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            FirebaseAuthentification metodo;
            metodo = DependencyService.Get<FirebaseAuthentification>();
            string uid = metodo.RecoveryId();

            var menu = sender as MenuItem;
            StrutturaUpload details = menu.CommandParameter as StrutturaUpload;
            string nomeDocumento = details.NomeFile;            
            await firebaseHelper.DeleteFile(nomeDocumento, uid, "Upload");
            await firebaseHelper.DeleteUpload(nomeDocumento, uid, "Upload");
            OnAppearing();

        }

        private string dataInserimento() //recupero la data e l'ora in cui inserisco il file
        {           
            DatePicker datePicker = new DatePicker();
            TimePicker timePicker = new TimePicker();
            timePicker.Format = "HH, mm, ss ";
            timePicker.Time = DateTime.Now.TimeOfDay;
            string ora = timePicker.Time.ToString();
            string data = datePicker.Date.ToString();
            string dataOra;
            return dataOra = data + " " + ora;
        }

       private string nomeFile()
        {
            var scelta = pickerUpload.SelectedItem;
            string nome = (string)scelta;
            string dataFoto = dataInserimento();
            string nomeFile = nome + " " + dataFoto;
            return nomeFile;
        }
    }
   
}