using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
public class authenticationManager : MonoBehaviour
{
    //firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser User;
    public TMP_Text username;
    public TMP_Text email;

    private string usernameText;
    private string emailText;


    //Login Variables
    [Header("Login")]
    public TMP_InputField emailLogin;
    public TMP_InputField passwordLogin;
    public TMP_Text errorMessageLogin;


    

    //Register Variables
    [Header("Register")]
    public TMP_InputField emailRegister;
    public TMP_InputField usernameRegister;

    public TMP_InputField passwordRegister;
    public TMP_InputField verifyPasswordRegister;
    public TMP_Text errorMessageRegister;


    //Panel Variables
    [Header("Panels/Buttons")]
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject backgroundPanel;
    public GameObject btnMenu;
    public GameObject menuPanel;
    public GameObject forgetPasswordPanel;
    
    [Header("Password Change Dialogs")]
    public TMP_Text DialogBoxChangePassword;
    public TMP_Text DialogBoxForgetPassword;
    public TMP_InputField userFP_EMAIL;


 

    private void Awake ()
    {
        //Check that all of the necessary dependencies for Firebase are Present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(Task =>
        {
            dependencyStatus = Task.Result;
            if(dependencyStatus == DependencyStatus.Available)
            {
                //if they are availible Intialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all firebase dependencies");
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting Up Firebase Auth");
        //Set the authentication Instance object
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

   
    
    private bool SignedIn;
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if(auth.CurrentUser != User)
        {
            SignedIn = User != auth.CurrentUser && auth.CurrentUser != null;
            if(!SignedIn)
            { 
                Debug.Log("Signed Out " + User.Email);
                goToLogin();
            }
            User = auth.CurrentUser;
            if(SignedIn)
            {
                Debug.Log("Signed In " + User.Email);
                enterMainScene();
            }
        }
    }


    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void loginButton ()
    {
        //call the login coroutine passing the email & password
        StartCoroutine(Login(emailLogin.text, passwordLogin.text));
    }

    public void registerButton()
    {
        //call the register coroutine passing the email & password
        StartCoroutine(Register(emailRegister.text, passwordRegister.text, usernameRegister.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        //call the Firebase Auth Sign in funtion passing the email & password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if(LoginTask.Exception != null)
        {
            //if there are errors then
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed";
            switch(errorCode)
            {
                case AuthError.MissingEmail:
                message = "Missing Email";
                break;

                case AuthError.MissingPassword:
                message = "Missing Password";
                break;

                case AuthError.InvalidEmail:
                message = "Invalid Email";
                break;

                case AuthError.UserNotFound:
                message = "User not found";
                break;
            }
        errorMessageLogin.text = message;
        }
        else
        {
                //User is logged in
                //Now get the result
                User = LoginTask.Result;
                Debug.LogFormat("User Signed in Successfully: {0}, ({1})", User.DisplayName, User.Email);
                errorMessageLogin.text = "";
                enterMainScene();
                
        }
    }
    

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //if username field is blank then show a warning
            errorMessageRegister.text = "Missing Username";
        }
        else if(passwordRegister.text != verifyPasswordRegister.text)
        {
            errorMessageRegister.text = "Password do not match with confirm password";
        }
        else
        {
            //Call the firebase auth sign function passing the email & password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //wait until task completes

            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if(RegisterTask.Exception != null)
            {
                //if there are errors
                Debug.LogWarning(message: $"failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed";

                switch(errorCode)
                {
                    case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;

                    case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;

                    case AuthError.WeakPassword:
                    message = "Weak Password";
                    break;

                    case AuthError.EmailAlreadyInUse:
                    message = "Email Already in Use";
                    break;
                }
                errorMessageRegister.text = message;
            }
            else
            {
                //User has now been Created
                //Now get the result
                User = RegisterTask.Result;

                if(User != null)
                {
                    //Create a user Profile and set the Username
                    UserProfile profile = new UserProfile{DisplayName = _username};

                    //Call the firebase auth update user profile function passing the profile with username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if(ProfileTask.Exception != null)
                    {
                        //if there are errors handle them
                        Debug.LogWarning(message: $"failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        errorMessageRegister.text = "Username Set Failed";
                    }
                    else
                    {
                        //User is now set
                        goToLogin();
                    }
                }
            }
        }
    }

    

    public void enterMainScene()
    {
        backgroundPanel.SetActive(false);
        btnMenu.SetActive(true);
        usernameText = User.DisplayName;
        username.text = "Welcome " + usernameText;
        emailText = User.Email;
        email.text = emailText;
    }


    public void goToLogin()
    {
        backgroundPanel.SetActive(true);
        registerPanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
        loginPanel.SetActive(true);
        btnMenu.SetActive(false);
        clearScreen();
    }

    public void goToRegister()
    {
        forgetPasswordPanel.SetActive(false);
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        clearScreen();
    }

    public void signOut()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignOut();
        menuPanel.SetActive(false);
        goToLogin();
        clearScreen();
    }

    public void clearScreen()
    {
        emailLogin.text = null;
        passwordLogin.text =  null;
        emailRegister.text = null;
        passwordRegister.text = null;
        usernameRegister.text = null;
        verifyPasswordRegister.text = null;
        userFP_EMAIL.text = null;
    }
private string changepassMAIL;
private TMP_Text DialogboxMSG;
    private IEnumerator changePassword(string mail, TMP_Text DialogBox)
    {
        string message;
        DialogboxMSG = DialogBox;
        changepassMAIL = mail;
        var changePASS = auth.SendPasswordResetEmailAsync(changepassMAIL);
        yield return new WaitUntil(predicate: () => changePASS.IsCompleted);

        if(changePASS.Exception != null)
        {
            //if there are errors then
            Debug.LogWarning(message: $"Failed to register task with {changePASS.Exception}");
            FirebaseException firebaseEx = changePASS.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            message = "Process Failed";

            switch(errorCode)
            {
                case AuthError.InvalidEmail:
                message = "Invalid Email";
                break;

                case AuthError.MissingEmail:
                message = "Missing Email";
                break;
            }

            DialogboxMSG.text = message;
         }
         else
         {
             message = "Password change link has been sent to: " +  changepassMAIL;
             Debug.LogFormat(message);
             DialogboxMSG.text = message;
         }
    }

    public void btnChangePassword()
    {
        //call the changePassword coroutine passing the email & Dialogbox
        StartCoroutine(changePassword(getUserEmail(), DialogBoxChangePassword));
    }

    public void btnForgetPassword()
    {
        string _mail;
        _mail = userFP_EMAIL.text;
        //call the changePassword coroutine passing the email & Dialogbox
        StartCoroutine(changePassword(_mail, DialogBoxForgetPassword));
    }

    public void goToForgetPassword()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        forgetPasswordPanel.SetActive(true);
        clearScreen();
    }


    private string getUserEmail()
    {
        string mail;
        mail = User.Email;
        return mail;
    }
}


