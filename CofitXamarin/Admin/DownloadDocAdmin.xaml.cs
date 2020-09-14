using CofitXamarin.Helper;
using CofitXamarin.Interfaces;
using CofitXamarin.Model;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DownloadDocAdmin : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public IDownloadFile File;
        bool isDownloading = true;
        public DownloadDocAdmin(string id)
        {
            InitializeComponent();
            uid.Text = id;
            CrossDownloadManager.Current.CollectionChanged += (sender, e) =>
           System.Diagnostics.Debug.WriteLine(
               "[DownloadManager] " + e.Action +
               " -> New items: " + (e.NewItems?.Count ?? 0) +
               " at " + e.NewStartingIndex +
               " || Old items: " + (e.OldItems?.Count ?? 0) +
               " at " + e.OldStartingIndex
               );
        }

        public async void DownloadFile(string FileName)
        {
            await Task.Yield();


            
            await Task.Run(() =>
            {
                var downloadManager = CrossDownloadManager.Current; 
                var file = downloadManager.CreateDownloadFile(FileName);
                downloadManager.Start(file, true);

                while (isDownloading)
                {                    
                    isDownloading = IsDownloading(file);

                }
            });

            if (!isDownloading)
            {
                await DisplayAlert("FIle Status", "File Downloaded", "OK");                
            }
        }

        public bool IsDownloading(IDownloadFile File) //stati a cui il download è soggetto
        {
            if (File == null) return false;

            switch (File.Status)
            {
                case DownloadFileStatus.INITIALIZED:
                case DownloadFileStatus.PAUSED:
                case DownloadFileStatus.PENDING:
                case DownloadFileStatus.RUNNING:
                    return true;

                case DownloadFileStatus.COMPLETED:
                case DownloadFileStatus.CANCELED:
                case DownloadFileStatus.FAILED:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AboerDownloading()
        {
            CrossDownloadManager.Current.Abort(File);
        }

        protected async override void OnAppearing()  //recupero tutti i file caricati e li inserisco in una listview
        {
            string id = uid.Text;
            base.OnAppearing();
            var allUpload = await firebaseHelper.GetAllUpload(id, "Upload");
            DocList.ItemsSource = allUpload;

        }
      
        private async void btnDownload_Clicked_1(object sender, EventArgs e)
        {
            string id = uid.Text;
            var menu = sender as MenuItem;
            StrutturaUpload details = menu.CommandParameter as StrutturaUpload;
            string nomeDocumento = details.NomeFile; 

            string path = await firebaseHelper.GetFile(nomeDocumento, "Upload", id); //recupero l'url del file selezionato 
            if (path != null)
            {
                DependencyService.Get<IMessage>().LongAlert("Download in corso"); 
                var Url = path;
                DownloadFile(Url); //procedo al download

            }
        }
    }
}