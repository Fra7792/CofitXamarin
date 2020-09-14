using CofitXamarin.Model;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CofitXamarin.Helper
{
    class FirebaseHelper
    {
        FirebaseClient firebase = new FirebaseClient("https://cofitxamarin.firebaseio.com/");  //mi collego al databade di firebase
        FirebaseStorage firebaseStorage = new FirebaseStorage("cofitxamarin.appspot.com");  //mi collego allo storage di firebase


        public async Task AddDocument(string nomeFile, string uri, string uid, string percorso) //metodo asincrono per salvare l'url del documento nel database di firebase
        {
         
            await firebase
              .Child(percorso).Child(uid)
              .PostAsync(new StrutturaUpload() { NomeFile = nomeFile, Uri = uri });
        }
       

        public async Task<string> UploadFile(Stream fileStream, string fileName, string uid, string percorso) //metodo per caricare il file nello storage
        {
                      
            var storageImage = await firebaseStorage
                .Child(percorso) 
                .Child(uid)
                .Child(fileName)
                .PutAsync(fileStream);
            string imgurl = storageImage; //mi ritorna il percorso di firebase in cui viene caricato il file
            return imgurl;
        }

        public async Task<string> UploadFoto(Stream fileStream, string uid) //metodo per salavare l'immagine del profilo
        {

            var storageImage = await firebaseStorage
                 .Child("Profilo")
                .Child(uid)                
                .PutAsync(fileStream);
            string imgurl = storageImage;
            return imgurl;
        }

        public async Task<string> UploadNovita(Stream fileStream, string nomeFile)
        {

            var storageImage = await firebaseStorage
                 .Child("Novità").Child(nomeFile)               
                .PutAsync(fileStream);
            string imgurl = storageImage;
            return imgurl;
        }

        public async Task<List<StrutturaUpload>> GetAllUpload(string uid, string percorso) //metodo per visualizzare tutti i file caricati nello storage, recuperati dal database realtime di firebase
        {
        
            return (await firebase.Child(percorso).Child(uid).OnceAsync<StrutturaUpload>()).Select(item => new StrutturaUpload
            {
                NomeFile = item.Object.NomeFile,
                Uri = item.Object.Uri
            }).ToList();
        }
       
        public async Task DeleteFile(string fileName, string uid, string percorso) //cancella un file dallo storage di firebase
        {        
            await firebaseStorage
                 .Child(percorso)
                 .Child(uid)
                 .Child(fileName)
                 .DeleteAsync();
        } 

        public async Task DeleteUpload(string filename, string uid, string percorso) //cancella le informazioni di un file dal database
        {          
            var toDeleteDocument = (await firebase
              .Child(percorso).Child(uid)
              .OnceAsync<StrutturaUpload>()).Where(a => a.Object.NomeFile == filename).FirstOrDefault();
            await firebase.Child(percorso).Child(uid).Child(toDeleteDocument.Key).DeleteAsync();

        }

        public async Task<string> GetFile(string fileName, string percorso, string uid)
        {
            return await firebaseStorage
                .Child(percorso)
                .Child(uid)
                .Child(fileName)
                .GetDownloadUrlAsync();
        }
       

    }

}