using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;

public class GooglePlayGamesExampleScript : MonoBehaviour
{
    public string Token;
    public string Error;
    public TextMeshProUGUI logText;
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        SignIn();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }
    public void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            // Perfectly login success

            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();
            string ImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();

            logText.text = "Success \n" + name;
            PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
            {
                Debug.Log("Authorization code: " + code);
                Token = code;
                logText.text = $"token : {Token.Length}";
                // This token serves as an example to be used for SignInWithGooglePlayGames
            });
        }
        else
        {
            logText.text = "Sign in Failed!";
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
            // Login failed
        }
    }
}