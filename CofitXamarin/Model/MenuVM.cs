using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CofitXamarin.Model
{
    public class MenuVM
    {
        public ObservableCollection<ListGroupM> MenuList { get; set; }

        public MenuVM()
        {
            this.MenuList = new ObservableCollection<ListGroupM>();

            var group1 = new List<StrutturaMenu>
            {
                new StrutturaMenu("Home", "home.png"),
                new StrutturaMenu("Profilo", "profilo_outline.png"),
                new StrutturaMenu("Novità", "star.png"),
                new StrutturaMenu("Esci", "logout.png")
            };

            MenuList.Add(new ListGroupM("Menù", group1));

            var group2 = new List<StrutturaMenu>
            {
                new StrutturaMenu("Inserisci documenti", "upload.png"),
                new StrutturaMenu("Scarica documenti", "document.png")
            };

            MenuList.Add(new ListGroupM("Documenti", group2));

            var group3 = new List<StrutturaMenu>
            {
                new StrutturaMenu("Whatsapp", "whatsapp.png"),
                new StrutturaMenu("Via Gabriele d'Annunzio 24, Pescara", "position1.png"),
                new StrutturaMenu("085377395", "icontelephone.png"),
                new StrutturaMenu("info@cofit.consulting.com", "email2.png"),
                new StrutturaMenu("Facebook", "iconfacebook.png"),
                new StrutturaMenu("www.cofitConsulting.com", "iconweb.png")
            };

            MenuList.Add(new ListGroupM("Assistenza", group3));

            var group4 = new List<StrutturaMenu>
            {
                new StrutturaMenu("Agenzia delle entrate", ""),
                new StrutturaMenu("Agenzia entrate riscossione", ""),
                new StrutturaMenu("INPS", ""),
                new StrutturaMenu("INAIL", ""),
                new StrutturaMenu("Camera di commercio Pescara", "")                
            };

            MenuList.Add(new ListGroupM("Link utili", group4));
        }
    }
}
