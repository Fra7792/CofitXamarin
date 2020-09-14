using System;
using System.Threading.Tasks;

namespace CofitXamarin.Interfaces
{

    // Interfaccia per l'autentifiazione di firebase
    public interface FirebaseAuthentification
    {
        Task<String> LoginWithEP(string E, string P);  
       
        Task<String> RegisterWithEP(string E, string P);

        bool SignOut();
        bool IsSignIn();

        string RecoveryEmail(); 

        string RecoveryId();

        void ResetPassword(string E);

        bool UserValido(string E, string P);
    }
}
