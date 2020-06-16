// Include Facebook namespace
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using Photon.Realtime;

using Photon.Pun;

// [..]

public class FacebookScript : MonoBehaviourPunCallbacks {
  
  public GameObject DialogLoggedIn;
  public GameObject DialogLoggedOut;
  public GameObject DialogUsername;
  public GameObject DialogProfilePic;

private void Awake()
{
    if (!FB.IsInitialized)
    {
        // Initialize the Facebook SDK
        Debug.Log("Initialising Facebook");
        FB.Init(InitCallback);
    }
    else
    {
        FacebookLogin();
        Debug.Log("Facebook Initialised already");
    }
    SetMenu(FB.IsLoggedIn);
}

private void InitCallback()
{
    if (FB.IsInitialized)
    {
        FacebookLogin();
    }
    else
    {
        Debug.Log("Failed to initialize the Facebook SDK");
    }
}

public void FacebookLogin()
{
    if (FB.IsLoggedIn)
    {
        OnFacebookLoggedIn();
        Debug.Log("Facebook is Logged In");
    }
    else
    {
        Debug.Log("Logging into Facebook");
        var perms = new List<string>(){"public_profile", "email"};
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }
}

 public void FacebookShare()
    {
        Debug.Log("Sharing!!!");
        FB.ShareLink(new System.Uri("https://droidbuilders.uk"), "Check it out!",
            "39.1% Racing",
            new System.Uri("https://droidbuilders.uk/wp-content/uploads/2020/06/New-DBUK2020.png"));
    }
    
private void AuthCallback(ILoginResult result)
{
    if (FB.IsLoggedIn)
    {
        OnFacebookLoggedIn();
    }
    else
    {
        Debug.LogErrorFormat("Error in Facebook login {0}", result.Error);
    }
    
    SetMenu(FB.IsLoggedIn);
}

private void OnFacebookLoggedIn()
{
    // AccessToken class will have session details
    string aToken = AccessToken.CurrentAccessToken.TokenString;
    string facebookId = AccessToken.CurrentAccessToken.UserId;
    PhotonNetwork.AuthValues = new AuthenticationValues();
    PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Facebook;
    PhotonNetwork.AuthValues.UserId = facebookId; // alternatively set by server
    PhotonNetwork.AuthValues.AddAuthParameter("token", aToken);
    PhotonNetwork.ConnectUsingSettings();
}

private void SetMenu(bool isLoggedIn) {
    if(isLoggedIn) 
    {
        DialogLoggedIn.SetActive(true);
        DialogLoggedOut.SetActive(false);
        
        FB.API ("/me?fields=first_name,last_name,email", HttpMethod.GET, DisplayUsername);
        FB.API ("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);        
        
    } else {
        DialogLoggedIn.SetActive(false);
        DialogLoggedOut.SetActive(true);
    }
}

void DisplayUsername(IResult result)
{

    Text UserName = DialogUsername.GetComponent<Text> ();

    if (result.Error == null) {

        UserName.text = result.ResultDictionary ["first_name"] + " " + result.ResultDictionary ["last_name"];

    } else {
        Debug.Log (result.Error);
    }

}

void DisplayProfilePic(IGraphResult result)
{

    if (result.Texture != null) {

        Image ProfilePic = DialogProfilePic.GetComponent<Image> ();

        ProfilePic.sprite = Sprite.Create (result.Texture, new Rect (0, 0, 128, 128), new Vector2 ());

    }

}
}