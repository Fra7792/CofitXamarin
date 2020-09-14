using CofitXamarin.Interfaces;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Debiti : ContentPage
    {
        public Debiti()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            PopulateDebitiList();
        }
        public void PopulateDebitiList()
        {
            DebitiList.ItemsSource = null;
            DebitiList.ItemsSource = DependencyService.Get<ISQLiteDebiti>().GetDebiti();
            if ((DebitiList.ItemsSource as List<StrutturaConto>).Count == 0)
            {
                DebitiList.IsVisible = false;
                empty.IsVisible = true;
            }
        }

        private void AddDebito(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddDebiti(null));            
        }

        private void EditDebito(object sender, ItemTappedEventArgs e)
        {
            StrutturaConto details = e.Item as StrutturaConto;
            if (details != null)
            {
                Navigation.PushAsync(new AddDebiti(details));
            }
        }

        private async void DeleteDebito(object sender, EventArgs e)
        {
            bool res = await DisplayAlert("Message", "Sei sicuro di voler cancellare questo credito?", "Sì", "No");
            if (res)
            {
                var menu = sender as MenuItem;
                StrutturaConto details = menu.CommandParameter as StrutturaConto;
                DependencyService.Get<ISQLiteDebiti>().DeleteDebito(details.Id);
                PopulateDebitiList();
            }
        }
    }
}