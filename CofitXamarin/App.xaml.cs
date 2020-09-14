using CofitXamarin.Admin;
using CofitXamarin.Interfaces;
using Xamarin.Forms;

namespace CofitXamarin
{
    public partial class App : Application
    {

        FirebaseAuthentification auth;

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            DependencyService.Get<ISQLite>().GetConnectionWithCreateDatabase();

            DependencyService.Get<ISQLiteDebiti>().GetConnectionWithCreateDatabase();

            auth = DependencyService.Get<FirebaseAuthentification>();
            string email = auth.RecoveryEmail();

            if(auth.IsSignIn())
            {
                if (email.Equals("francesco0792@gmail.com"))
                {
                    MainPage = new NavigationPage(new NavigationAdmin());
                }
                else
                {
                    MainPage = new NavigationPage(new MainPage());
                }                                               
            }
            else
            {
                MainPage = new NavigationPage (new Register());
            }

            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
