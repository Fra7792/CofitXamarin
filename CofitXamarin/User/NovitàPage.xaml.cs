using CofitXamarin.Model;
using Plugin.CloudFirestore;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin.User
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NovitàPage : ContentPage
    {
        public NovitàPage()
        {
            InitializeComponent();
            
        }
               
        protected override void OnAppearing()
        {
            PopulateNovitaList();
        }

        public async void PopulateNovitaList()
        {
            NovitaList.ItemsSource = null;

            var group = await CrossCloudFirestore.Current
                                        .Instance.GetCollectionGroup("Novità").GetDocumentsAsync();

            var novita = group.ToObjects<StrutturaUpload>();

            NovitaList.ItemsSource = novita;

        }
    }
}