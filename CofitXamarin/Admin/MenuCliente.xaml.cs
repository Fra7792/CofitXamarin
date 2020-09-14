using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuCliente : ContentPage
    {

        

        public MenuCliente(StrutturaUser details)
        {
            InitializeComponent();

            if (details != null)
            {
                
                //recupero i dati del cliente

                uid.Text = details.Id;
                
                titolo.Title = details.Denominazione;


            }
        }

        private async void btn_addTasse_Clicked(object sender, EventArgs e)
        {
            string id = uid.Text;
            
            await Navigation.PushAsync(new AddTasse(id));
        }

        private async void btnProfilo_Clicked(object sender, EventArgs e)
        {
            string id = uid.Text;

            await Navigation.PushAsync(new VisualizzaProfiloCliente(id));
        }

        private async void btnVisualizzaTasse_Clicked(object sender, EventArgs e)
        {
            string id = uid.Text;

            await Navigation.PushAsync(new VisualizzaTasse(id));
        }

        private async void btn_UploadDoc_Clicked(object sender, EventArgs e)
        {
            string id = uid.Text;

            await Navigation.PushAsync(new UpdateDocumentAdmin(id));
        }

        private async void btnDownloadDoc_Clicked(object sender, EventArgs e)
        {
            string id = uid.Text;

            await Navigation.PushAsync(new DownloadDocAdmin(id));
        }
    }
}