using CofitXamarin.Interfaces;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Crediti : ContentPage
    {
        public Crediti()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            PopulateCreditiList();
        }
        public void PopulateCreditiList()
        {
            CreditiList.ItemsSource = null;
            CreditiList.ItemsSource = DependencyService.Get<ISQLite>().GetCrediti();
            if((CreditiList.ItemsSource as List<StrutturaConto>).Count == 0)
            {
                CreditiList.IsVisible = false;
                empty.IsVisible = true;
            }
        }

        private void AddCredito(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddCrediti(null));            
        }

        private void EditCredito(object sender, ItemTappedEventArgs e)
        {
            StrutturaConto details = e.Item as StrutturaConto;
            if (details != null)
            {
                Navigation.PushAsync(new AddCrediti(details));
            }
        }

        private async void DeleteCredito(object sender, EventArgs e)
        {
            bool res = await DisplayAlert("Message", "Sei sicuro di voler cancellare questo credito?", "Sì", "No");
            if (res)
            {
                var menu = sender as MenuItem;
                StrutturaConto details = menu.CommandParameter as StrutturaConto;
                DependencyService.Get<ISQLite>().DeleteCredito(details.Id);
                PopulateCreditiList();
            }
        }
    }
}