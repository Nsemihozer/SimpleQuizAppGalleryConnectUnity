using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenuManager : MonoBehaviour
{
    public Text verifyCodeText, emailText, passwordText;
    public Text  emailSignInText, passwordSignInText;
    
    
    public void ActivatePanel(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
    
    public void DeActivatePanel(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void GetVerifyCode()
    {
        AuthManager._instance.RequestVerifyCode(emailText.text);
    }
    
    public void Register()
    {
        AuthManager._instance.RegisterUser(emailText.text,verifyCodeText.text,passwordText.text);
    }
    
    public void SignIn()
    {
        AuthManager._instance.SignInEmail(emailSignInText.text,passwordSignInText.text);
    }
    public void SignInAnon()
    {
        AuthManager._instance.SignAnon();
    }

    public void SignOut()
    {
        AuthManager._instance.Logout();
    }
    
}
