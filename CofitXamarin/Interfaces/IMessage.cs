using System;
using System.Collections.Generic;
using System.Text;

namespace CofitXamarin.Interfaces
{
    public interface IMessage  //interfaccia per visualizzare un Toast
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
