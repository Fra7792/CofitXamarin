using Plugin.CloudFirestore;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaClienti : ContentPage
    {
        public ListaClienti()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            PopulateUsersList();
        }

        public async void PopulateUsersList() // popolo la listview con tutti i clienti registrati su firebase
        {
            UsersList.ItemsSource = null;

            var group = await CrossCloudFirestore.Current
                                        .Instance.GetCollectionGroup("Users").GetDocumentsAsync();

            var users = group.ToObjects<StrutturaUser>().OrderBy(x => x.Denominazione);

            UsersList.ItemsSource = users;
                                   
        }

        public async void PopulateSearchUser() //popolo la listview con i soli clienti che presentano nel nome ciò che è presente nella SearchView
        {
            UsersList.ItemsSource = null;
            var query = await CrossCloudFirestore.Current
                .Instance.GetCollection("Users").WhereGreaterThanOrEqualsTo("Search", search.Text).GetDocumentsAsync();
            var users = query.ToObjects<StrutturaUser>().OrderBy(x => x.Denominazione);
            UsersList.ItemsSource = users;
            
        }

        private void UsersList_ItemTapped(object sender, ItemTappedEventArgs e) 
        {
            StrutturaUser details = e.Item as StrutturaUser;
            if (details != null)
            {
                Navigation.PushAsync(new MenuCliente(details)); //passo i dati del cliente nella page menucliente
            }
        }

        private void search_SearchButtonPressed(object sender, EventArgs e)
        {
            PopulateSearchUser();
        }
    }
}