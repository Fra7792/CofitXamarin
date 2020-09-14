using System;
using System.Collections.Generic;
using System.Text;

namespace CofitXamarin.Interfaces
{
   public interface IDownloader    //interfaccia per il download dei file
    {
        void DownloadFile(string url, string folder);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;
    }
}
