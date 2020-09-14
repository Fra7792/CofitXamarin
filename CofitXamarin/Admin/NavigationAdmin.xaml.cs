using CofitXamarin.Interfaces;
using CofitXamarin.Model;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.OpenWhatsApp;
using Xamarin.Forms.Xaml;

namespace CofitXamarin.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationAdmin : MasterDetailPage
    {
       
        public NavigationAdmin()
        {
            InitializeComponent();

            this.BindingContext = new MenuAdminVM(); //recupero tutte le voci del menù
          
            Detail = new NavigationPage(new AdminPage());
        }

        private void Item_Tapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var item = e.Item as StrutturaMenu;

                switch (item.OptionMenu)
                {
                    case "Home":
                        {
                            Detail = new NavigationPage(new AdminPage());
                            IsPresented = false;
                        }
                        break;

                    case "Inserisci novità":
                        {
                            Detail = new NavigationPage(new InsNovitaPage());
                            IsPresented = false;
                        }
                        break;


                    case "Esci":
                        {
                            FirebaseAuthentification auth;
                            auth = DependencyService.Get<FirebaseAuthentification>();
                            if (auth.SignOut())
                            {
                                Navigation.PushAsync(new Login());
                                Navigation.RemovePage(this);
                            }
                        }
                        break;

                    case "www.cofitConsulting.com":
                        {
                            Uri myUri = new Uri("https://www.cofitconsulting.com/", UriKind.Absolute);
                            Browser.OpenAsync(myUri, BrowserLaunchMode.SystemPreferred);
                        }
                        break;

                    case "Whatsapp":
                        {
                            try
                            {
                                Chat.Open("+393401861219", "");

                            }
                            catch (Exception E)
                            {

                            }

                        }
                        break;

                    case "Facebook":

                        {
                            Device.OpenUri(new Uri("fb://page/219948828196163"));
                        }
                        break;

                    case "Via Gabriele d'Annunzio 24, Pescara":
                        {
                            if (Device.RuntimePlatform == Device.iOS)
                            {
                                // https://developer.apple.com/library/ios/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html
                                Launcher.OpenAsync("http://maps.apple.com/?q=Via+Gabriele+dannunzio+24,+pescara");
                            }
                            else if (Device.RuntimePlatform == Device.Android)
                            {
                                // open the maps app directly
                                Launcher.OpenAsync("geo:0,0?q=Via+Gabriele+d'annunzio+24,+pescara");
                            }
                        }
                        break;


                    case "085377395":
                        {
                            PhoneDialer.Open("085377395");
                        }
                        break;

                    case "info@cofit.consulting.com":
                        {
                            Device.OpenUri(new Uri("mailto:info@cofit.consulting.com"));
                        }
                        break;

                    case "Agenzia delle entrate":
                        {
                            Uri myUri = new Uri("https://www.agenziaentrate.gov.it/portale/home", UriKind.Absolute);
                            Browser.OpenAsync(myUri, BrowserLaunchMode.SystemPreferred);
                        }
                        break;

                    case "Agenzia entrate riscossione":
                        {
                            Uri myUri = new Uri("https://www.agenziaentrateriscossione.gov.it/it/", UriKind.Absolute);
                            Browser.OpenAsync(myUri, BrowserLaunchMode.SystemPreferred);
                        }
                        break;

                    case "INPS":
                        {
                            Uri myUri = new Uri("https://www.inps.it/nuovoportaleinps/default.aspx", UriKind.Absolute);
                            Browser.OpenAsync(myUri, BrowserLaunchMode.SystemPreferred);
                        }
                        break;

                    case "INAIL":
                        {
                            Uri myUri = new Uri("https://www.inail.it/cs/internet/home.html", UriKind.Absolute);
                            Browser.OpenAsync(myUri, BrowserLaunchMode.SystemPreferred);
                        }
                        break;

                    case "Camera di commercio Pescara":
                        {
                            Uri myUri = new Uri("http://www.pe.camcom.it/", UriKind.Absolute);
                            Browser.OpenAsync(myUri, BrowserLaunchMode.SystemPreferred);
                        }
                        break;


                }
            }
            catch (Exception ex)
            {

            }
        }
    }



}
