using System;
using System.Collections.Generic;
using System.Text;

namespace CofitXamarin.Model
{
    public class StrutturaMenu
    {
        public string OptionMenu { get; set; }

        public string Icon { get; set; }

        public StrutturaMenu(string optionmenu, string icon)
        {
            this.OptionMenu = optionmenu;
            this.Icon = icon;
        }
    }

    public class ListGroupM : List<StrutturaMenu>
    {
        public string GroupName { get; private set; }

        public ListGroupM(string groupname, List<StrutturaMenu> source) : base(source)
        {
            GroupName = groupname;
        }
    }
}
