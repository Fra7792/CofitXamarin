using CofitXamarin.Interfaces;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CofitXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
         
        }

        private async void btnRegister_Clicked(object sender, EventArgs e)
        {
            {
                this.indicator1.IsRunning = true;
                string mail = txtEmail.Text;
                string pass = txtPass.Text;
                string confPass = txtConfPass.Text;

                if(!(pass.Equals(confPass)))
                    {
                       await DisplayAlert("Registrazione Fallita", "" , "Le password sono diverse");
                }

                var fbLogin = DependencyService.Get<FirebaseAuthentification>(); 
                string token = await fbLogin.RegisterWithEP(mail, pass);  //effettua la registrazione su firebase con email e password
                
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(this);

            }
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new Login());
            Navigation.RemovePage(this);
        }

    }
}