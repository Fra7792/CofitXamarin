using CofitXamarin.Helper;
using CofitXamarin.Interfaces;
using CofitXamarin.Model;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


// è uguale alla page dell'Admin, cambia solo il percorso del file da scaricare


namespace CofitXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DownloadDoc : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public IDownloadFile File;
        bool isDownloading = true;

        public DownloadDoc()
        {
            InitializeComponent();
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

                while(isDownloading)
                {
                    isDownloading = IsDownloading(file);
                }
            });
            
            if(!isDownloading)
            {
                await DisplayAlert("FIle Status", "File Downloaded", "OK");
              
            }
        }

        public bool IsDownloading(IDownloadFile File)
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

        protected async override void OnAppearing()
        {
            FirebaseAuthentification metodo = DependencyService.Get<FirebaseAuthentification>();
            string id = metodo.RecoveryId();
            base.OnAppearing();
            var allUpload = await firebaseHelper.GetAllUpload(id, "UploadCofit");
            DocList.ItemsSource = allUpload;

        }

       

        private async void btnDownload_Clicked(object sender, EventArgs e)
        {

            FirebaseAuthentification metodo;
            metodo = DependencyService.Get<FirebaseAuthentification>();
            string uid = metodo.RecoveryId();
            var menu = sender as MenuItem;
            StrutturaUpload details = menu.CommandParameter as StrutturaUpload;
            string nomeDocumento = details.NomeFile;

            string path = await firebaseHelper.GetFile(nomeDocumento, "UploadCofit", uid);
            if (path != null)
            {
                DependencyService.Get<IMessage>().LongAlert("Download in corso");
                var Url = path;
                DownloadFile(Url);
                
                
            }
        }

 
    }
}