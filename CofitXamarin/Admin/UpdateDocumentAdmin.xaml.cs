using CofitXamarin.Helper;
using CofitXamarin.Interfaces;
using CofitXamarin.Model;
using Plugin.FilePicker;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateDocumentAdmin : ContentPage
    {

        FirebaseHelper firebaseHelper = new FirebaseHelper();
        MediaFile file;

        public UpdateDocumentAdmin(string id)
        {
            InitializeComponent();
            uid.Text = id;
        }

        protected async override void OnAppearing()
        {
            string id = uid.Text;
            base.OnAppearing();
            var allUpload = await firebaseHelper.GetAllUpload(id, "UploadCofit");
            lstFile.ItemsSource = allUpload;

        }
   
        private async void btnExplorerFile_Clicked(object sender, EventArgs e)
        {
            if (pickerUpload.SelectedIndex == -1 || nome.Text == null)
            {
                DependencyService.Get<IMessage>().ShortAlert("Completa tutti i campi");
                return;
            }
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
                string id = uid.Text;
                string estensione = Path.GetExtension(fileData.FileName);
                string nomeFileCompleto = nomeFile() + estensione;
                await firebaseHelper.UploadFile(fileData.GetStream(), nomeFileCompleto, id, "UploadCofit");
                string imgurl = await firebaseHelper.UploadFile(fileData.GetStream(), nomeFileCompleto, id, "UploadCofit");
                await firebaseHelper.AddDocument(nomeFileCompleto, imgurl, id, "UploadCofit");                                
                DependencyService.Get<IMessage>().ShortAlert("Caricamento effettuato");
                OnAppearing();
                progressBar.IsRunning = false;
            }
            catch (Exception ex2)
            {

            }
        }
        private async void btnCaptureCamera_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported && !CrossMedia.Current.IsPickPhotoSupported)
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

                if (file == null)
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

        private async void BtnUpload_Clicked(object sender, EventArgs e)
        {
            if(imgChoosed.Source == null)
            {
                DependencyService.Get<IMessage>().ShortAlert("Scatta prima la foto");
                return;
            }

            if (pickerUpload.SelectedIndex == -1 || nome.Text == null)
            {
                DependencyService.Get<IMessage>().ShortAlert("Completa tutti i campi");
                return;
            }

            progressBar.IsRunning = true;
            DependencyService.Get<IMessage>().LongAlert("Caricamento in corso");
            string id = uid.Text;
            string extension = Path.GetExtension(file.Path);
            string nomeFileCompleto = nomeFile() + extension;
            await firebaseHelper.UploadFile(file.GetStream(), nomeFileCompleto, id, "UploadCofit");
            string imgurl = await firebaseHelper.UploadFile(file.GetStream(), nomeFileCompleto, id, "UploadCofit");
            await firebaseHelper.AddDocument(nomeFileCompleto, imgurl, id, "UploadCofit");            
            DependencyService.Get<IMessage>().ShortAlert("Caricamento effettuato");
            OnAppearing();
            progressBar.IsRunning = false;

        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            string id = uid.Text;
            var menu = sender as MenuItem;
            StrutturaUpload details = menu.CommandParameter as StrutturaUpload;
            string nomeDocumento = details.NomeFile;
            await firebaseHelper.DeleteFile(nomeDocumento, id, "UploadCofit");
            await firebaseHelper.DeleteUpload(nomeDocumento, id, "UploadCofit");
            OnAppearing();

        }

        private string nomeFile()
        {
            var scelta = pickerUpload.SelectedItem;
            string nomePicker = (string)scelta;            
            string nomeFile = nome.Text;
            string nomeCompleto = "COFIT:" + " " + nomePicker + " " + nomeFile;
            return nomeCompleto;
        }

    }
   
}