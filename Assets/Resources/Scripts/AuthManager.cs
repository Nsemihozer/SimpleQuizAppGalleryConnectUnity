using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Huawei.Agconnect;
using Huawei.Agconnect.AGCException;
using Huawei.Agconnect.Auth;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static AuthManager _instance;
    void Start()
    {
        if (_instance)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
            try
            {
                Stream inputStream = new MemoryStream(Resources.Load<TextAsset>("agconnect-services").bytes);
                AGConnectInstance.Initialize(new AGConnectOptionsBuilder()
                    .SetInputStream(inputStream).SetPersistPath(new DirectoryInfo(Application.persistentDataPath)));
            }
            catch (System.Exception ex)
            {

                Debug.LogException(ex);
            }
        }
    }

    public async void RequestVerifyCode(string email)
    {
        VerifyCodeSettings settings = new VerifyCodeSettings.Builder()
            .SetAction(VerifyCodeSettings.ActionRegisterLogin)
            .SendInterval(30)
            .SetLang("en-US")
            .Build();
        Task<VerifyCodeResult> verifyCodeResultTask = AGConnectAuth.Instance.RequestVerifyCodeAsync(email, settings);
        try
        {
            await verifyCodeResultTask;
            VerifyCodeResult results = verifyCodeResultTask.Result;
        }
        catch (System.Exception)
        {
            if (verifyCodeResultTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(verifyCodeResultTask.Exception.InnerException.ToString());
        }
    }

    public async void RegisterUser(string email, string verifycode, string password)
    {
        EmailUser emailUser = new EmailUser.Builder()
            .SetEmail(email)
            .SetVerifyCode(verifycode)
            .SetPassword(password)
            .Build();
        Task<ISignInResult> createUserTask = AGConnectAuth.Instance.CreateUserAsync(emailUser);
        try
        {
            await createUserTask;
            var result = createUserTask.Result;
            var user = AGConnectAuth.Instance.GetCurrentUser();
        }
        catch (System.Exception)
        {
            if (createUserTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(createUserTask.Exception.InnerException.ToString());
        }
    }

    public async void SignInEmail(string email, string password)
    {
        AGConnectUser user = AGConnectAuth.Instance.GetCurrentUser();
        if (user == null)
        {
            IAGConnectAuthCredential credential = EmailAuthProvider.CredentialWithPassword(email, password);
            Task<ISignInResult> signInTask = AGConnectAuth.Instance.SignInAsync(credential);
            try
            {
                await signInTask;
                ISignInResult signInResult = signInTask.Result;
                user = AGConnectAuth.Instance.GetCurrentUser();
            }
            catch (System.Exception ex)
            {
                if (signInTask.Exception.InnerException is AGCException exception)
                    Debug.Log(exception.ErrorMessage);
                else
                    Debug.Log(signInTask.Exception.InnerException.ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
