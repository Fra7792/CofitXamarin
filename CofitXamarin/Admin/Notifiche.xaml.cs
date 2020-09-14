using CofitXamarin.Model;
using Plugin.CloudFirestore;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Notifiche : ContentPage
    {
        public Notifiche()
        {
            InitializeComponent();
                        
        }

        protected override void OnAppearing()
        {
            PopulateNotificheList();
        }

        public async void PopulateNotificheList()
        {
            NotificheList.ItemsSource = null;

            //recupero le notifiche dal database di firebase in ordine di colore. Quelle non lette verranno visualizzate prima.

            var group = await CrossCloudFirestore.Current
                                        .Instance.GetCollection("Notifiche").OrderBy("ColoreTesto").GetDocumentsAsync();
            
            
            var notifiche = group.ToObjects<StrutturaNotifica>().ToList().OrderByDescending(x => x.ColoreTesto); 
            
            NotificheList.ItemsSource = notifiche;            

        }

        private async void NotificheList_ItemTapped(object sender, ItemTappedEventArgs e) 
            
            /*Quando premo su una notifica passo alla page con i file caricati dal cliente
              e contemporaneamente modifico il colore della notifica                                                                                          
            */
        {
            StrutturaNotifica details = e.Item as StrutturaNotifica;
            if (details != null)
            {
                string id = details.Id;
                var database = CrossCloudFirestore.Current.Instance.GetCollection("Notifiche").GetDocument(id);
                await database.UpdateDataAsync(new { ColoreTesto = "Black" });
               await Navigation.PushAsync(new DownloadDocAdmin(details.Id));
            }
        }
    
    }
}