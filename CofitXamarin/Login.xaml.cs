using CofitXamarin.Admin;
using CofitXamarin.Interfaces;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;



namespace CofitXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            string mail = txtEmail.Text;
            string pass = txtPass.Text;
            
            var fbLogin = DependencyService.Get<FirebaseAuthentification>();
            await fbLogin.LoginWithEP(mail, pass);
            var user = fbLogin.UserValido(mail, pass);
            
            if (user) //se l'user esiste effettua il login
            {
                if (mail.Equals("francesco0792@gmail.com"))
                {
                    await Navigation.PushAsync(new NavigationAdmin());
                    Navigation.RemovePage(this);
                }
                else await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(this);
            }
            else return;
                      
            
        }

        private async void btnRegister_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Register());
            Navigation.RemovePage(this);            
        }

        private void btnResetPassoword_Clicked(object sender, EventArgs e) //resetta la password dell'utente
        {                        
                string email = txtEmail.Text;                        
                var metodo = DependencyService.Get<FirebaseAuthentification>();
                metodo.ResetPassword(email);           

        }

        
    }
}