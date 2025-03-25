using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
using System;

public class RegisterManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField repeatPasswordInput;
    public Button registerButton;

    private void Start()
    {
        registerButton.onClick.AddListener(OnRegisterClicked);
    }

    private async void OnRegisterClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string repeatPassword = repeatPasswordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(repeatPassword))
        {
            Debug.LogWarning("Vul alle velden in.");
            return;
        }

        if (password != repeatPassword)
        {
            Debug.LogWarning("Wachtwoorden komen niet overeen.");
            return;
        }

        var registerData = new RegisterData
        {
            Email = email,
            Password = password,
            ConfirmPassword = repeatPassword
        };

        string json = JsonUtility.ToJson(registerData);

        var response = await WebClient.instance.SendPostRequest("/auth/register", json);

        if (response is WebRequestData<string> success)
        {
            Debug.Log("Registratie gelukt: " + success.Data);
            // Hier kun je doorgaan naar de login scene bijvoorbeeld
            // SceneManager.LoadScene("LoginScene");
        }
        else if (response is WebRequestError error)
        {
            Debug.LogError("Fout bij registreren: " + error.ErrorMessage);
        }
    }

    [Serializable]
    public class RegisterData
    {
        public string Email;
        public string Password;
        public string ConfirmPassword;
    }
}
