using CofitXamarin.Model;
using Plugin.CloudFirestore;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisualizzaTasse : ContentPage
    {
        public VisualizzaTasse(string id)
        {
            InitializeComponent();

            uid.Text = id;
        }

        protected override void OnAppearing()
        {
            PopulateTasseList();
        }

        public async void PopulateTasseList()
        {

            string id = uid.Text;
            TasseList.ItemsSource = null;

            var group = await CrossCloudFirestore.Current
                                        .Instance.GetCollectionGroup(id).GetDocumentsAsync();

            var tasse = group.ToObjects<StrutturaTassa>();

            TasseList.ItemsSource = tasse;


        }



        private async void btnUpdate_Clicked(object sender, EventArgs e)
        {
            string id = uid.Text;
            var menu = sender as MenuItem;
            StrutturaTassa details = menu.CommandParameter as StrutturaTassa;
            string nomeDocumento = details.Tassa;


            var databaseTasse = CrossCloudFirestore.Current.Instance.GetCollection(id).GetDocument(nomeDocumento);
                       
                await databaseTasse.UpdateDataAsync(new { Stato = "Pagato" });
                await databaseTasse.UpdateDataAsync(new { ColoreTesto = "Green" });                        
             
            PopulateTasseList();
                                                   
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            string id = uid.Text;
            var menu = sender as MenuItem;
            StrutturaTassa details = menu.CommandParameter as StrutturaTassa;
            string nomeDocumento = details.Tassa;

            await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection(id)
                         .GetDocument(nomeDocumento)
                         .DeleteDocumentAsync();

            PopulateTasseList();
        }

        async void DisplayData(object Sender, EventArgs e)
        {
          
            string id = uid.Text;
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
            if (result < 0 && stato.Text.Equals("Emesso"))
            {
                await databaseTasse.UpdateDataAsync(new { Stato = "Scaduto" });
                await databaseTasse.UpdateDataAsync(new { ColoreTesto = "Red" });
                PopulateTasseList();
            }
        }
    }
}