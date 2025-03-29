using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public TMP_Text feedbackText;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
    }

    private async void OnLoginClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Vul zowel e-mail als wachtwoord in.";
            return;
        }

        var loginData = new LoginRequest
        {
            Email = email,
            Password = password
        };

        string json = JsonUtility.ToJson(loginData);
        var response = await WebClient.instance.SendPostRequest("/auth/login", json);

        if (response is WebRequestData<string> success)
        {
            Debug.Log("Login gelukt!");
            WebClient.instance.SetToken(success.Data);

            // oudergegevens ophalen
            var guardianResponse = await new GuardianApiClient().GetGuardian();
            if (guardianResponse is WebRequestData<Guardian> guardianData)
            {
                SessionManager.instance.guardian = guardianData.Data;

                // kindgegevens ophalen
                var childResponse = await new ChildApiClient().GetChild();
                if (childResponse is WebRequestData<Child> childData)
                {
                    SessionManager.instance.child = childData.Data;

                    //Beide bestaan → ga naar Overworld
                    SceneManager.LoadScene("OverworldScene");
                    return;
                }
            }

            //Niet alles ingevuld → stuur naar Gegevens invulscherm
            SceneManager.LoadScene("GegevensScene");
        }
        else if (response is WebRequestError error)
        {
            Debug.LogError("Login fout: " + error.ErrorMessage);
            feedbackText.text = "Fout bij inloggen: " + error.ErrorMessage;
        }

        LevelSetup.LoggedIn = true;

        LevelSetup.LoggedIn = false;
    }


    [Serializable]
    public class LoginRequest
    {
        public string Email;
        public string Password;
    }
}
