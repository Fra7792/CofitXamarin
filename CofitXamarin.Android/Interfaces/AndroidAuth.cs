using Android.Content;
using Android.Content.Res;
using Android.Widget;
using CofitXamarin.Droid.Interfaces;
using CofitXamarin.Interfaces;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


//Attraverso l'assembly collego l'interfaccia ad android.

[assembly: Dependency(typeof(AndroidAuth))]

namespace CofitXamarin.Droid.Interfaces
{
    public class AndroidAuth : FirebaseAuthentification
    {
        public async Task<string> LoginWithEP(string E, string P)
        {
            try
            {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(E, P);                
                var token = await user.User.GetIdTokenAsync(false);                 
                return token.Token;                               
            }
            catch (FirebaseAuthInvalidUserException notFound)
            {
                return notFound.Message;
            }
            catch (Exception err)
            {
                return err.Message;
            }            
            
        }

        public bool UserValido(string E, string P)
        {
           
            var utente = FirebaseAuth.Instance.CurrentUser;
            if (utente != null)
            {
                Toast.MakeText(Android.App.Application.Context, "Accesso eseguito", ToastLength.Short).Show();
                return true;
            }
            else
            {
                Toast.MakeText(Android.App.Application.Context, "Email e password non corrispondono", ToastLength.Short).Show();
                return false;
            }
                
                
        }

        public async Task<string> RegisterWithEP(string E, string P)
        {
            try
            {
                var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(E, P);
                var token = await user.User.GetIdTokenAsync(false);
                return token.Token;
            }
            catch (FirebaseAuthInvalidUserException notFound)
            {
                return notFound.Message;
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        public bool IsSignIn()
        {
            var user = Firebase.Auth.FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public bool SignOut()
        {
            try
            {
                Firebase.Auth.FirebaseAuth.Instance.SignOut();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string RecoveryEmail()
        {
            try
            {
                String E;
                E = FirebaseAuth.Instance.CurrentUser.Email;

                return E;

            } catch(NullReferenceException e)
            {

            }

            return "";
        }

        public string RecoveryId()
        {
            try
            {
                string id;
                id = FirebaseAuth.Instance.CurrentUser.Uid;
                return id;
            } catch(NullReferenceException e)
            {

            }

            return "";
        }
        
        public void ResetPassword(string E)
        {
            try
            {
                FirebaseAuth.Instance.SendPasswordResetEmail(E);
                Toast.MakeText(Android.App.Application.Context, "E' stata inviata un'e-mail al tuo indirizzo ", ToastLength.Long).Show();
            } catch(Exception e)
            {
                Toast.MakeText(Android.App.Application.Context, "Inserisci un indirizzo email valido! ", ToastLength.Short).Show();
            }
            
        }
            
        
    }
}