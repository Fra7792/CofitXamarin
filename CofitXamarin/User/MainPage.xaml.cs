using CofitXamarin.Interfaces;
using CofitXamarin.Model;
using CofitXamarin.User;
using Firebase.Storage;
using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.OpenWhatsApp;

namespace CofitXamarin
{
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {

        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = new MenuVM();

            

            Detail = new NavigationPage(new HomePage());

            string email;

            FirebaseAuthentification metodo;
            metodo = DependencyService.Get<FirebaseAuthentification>();
            email = metodo.RecoveryEmail();

            txtEmail.Text = email;
            recuperaFoto();

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
                            Detail = new NavigationPage(new HomePage());
                            IsPresented = false;
                        }
                        break;

                    case "Profilo":
                        {
                            Detail = new NavigationPage(new Profilo());
                            IsPresented = false;
                        }
                        break;

                    case "Novità":
                        {
                            Detail = new NavigationPage(new NovitàPage());
                            IsPresented = false;
                        }
                        break;

                    case "Inserisci documenti":
                        {
                            Detail = new NavigationPage(new UploadDocumentsPage());
                            IsPresented = false;
                        }
                        break;

                    case "Scarica documenti":
                        {
                            Detail = new NavigationPage(new DownloadDoc());
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

   

        public async void recuperaFoto()
        {
            FirebaseAuthentification metodo;
            metodo = DependencyService.Get<FirebaseAuthentification>();
            string id = metodo.RecoveryId();
            FirebaseStorage firebaseStorage = new FirebaseStorage("cofitxamarin.appspot.com");
            try
            {
                string url = await firebaseStorage
                .Child("Profilo")
                .Child(id + "." + "jpg")
                .GetDownloadUrlAsync();
                imgProfilo.Source = ImageSource.FromUri(new Uri(url));

            }
            catch (Exception e)
            {

            }
        }

    }
      
}
