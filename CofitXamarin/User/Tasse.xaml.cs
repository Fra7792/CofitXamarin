using CofitXamarin.Interfaces;
using CofitXamarin.Model;
using Plugin.CloudFirestore;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tasse : ContentPage
    {
        public Tasse()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            PopulateTasseList();
        }

        public async void PopulateTasseList()
        {
            FirebaseAuthentification metodo;
            metodo = DependencyService.Get<FirebaseAuthentification>();
            string id = metodo.RecoveryId();

            TasseList.ItemsSource = null;

            var group = await CrossCloudFirestore.Current
                                        .Instance.GetCollectionGroup(id).GetDocumentsAsync();

            var tasse = group.ToObjects<StrutturaTassa>();

            TasseList.ItemsSource = tasse;


        }

        async void DisplayData(object Sender, EventArgs e)
        {
            FirebaseAuthentification metodo;
            metodo = DependencyService.Get<FirebaseAuthentification>();
            string id = metodo.RecoveryId();
            var buttonClickHandler = (ImageButton)Sender;

            StackLayout ParentStackLayout = (StackLayout)buttonClickHandler.Parent;                       
            
            Label data = (Label)ParentStackLayout.Children[2];
            StackLayout stackLayout2 = (StackLayout)ParentStackLayout.Children[3];
            Frame frame = (Frame)stackLayout2.Children[0];
            Label stato = (Label)frame.Children[0];
            Label tassa = (Label)ParentStackLayout.Children[1];
            string nomeDocumento = tassa.Text;
            var databaseTasse = CrossCloudFirestore.Current.Instance.GetCollection(id).GetDocument(nomeDocumento);
            string scadenza = data.Text;            
            var scadDate = DateTime.Parse(scadenza);
            DatePicker dataOggi = new DatePicker();
            DateTime oggi = dataOggi.Date;
            int result = DateTime.Compare(scadDate, oggi);                        
                if(result<0 && !(stato.Text.Equals("Pagato")))
                {
                    await databaseTasse.UpdateDataAsync(new { Stato = "Scaduto" });
                    await databaseTasse.UpdateDataAsync(new { ColoreTesto = "Red" });
                    PopulateTasseList();
            }                                    
        }

       
    }
}