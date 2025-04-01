using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GegevensManager : MonoBehaviour
{
    public TMP_InputField childFirstName;
    public TMP_InputField childLastName;
    public TMP_InputField childBirthDate;
    public TMP_InputField doctorName;
    public TMP_Dropdown treatmentPlan;

    public TMP_InputField parentFirstName;
    public TMP_InputField parentLastName;
    public TMP_InputField parentBirthDate;
    public TMP_Dropdown relationDropdown;

    public Button createAccountButton;
    public TMP_Text feedbackText;
    public ChildApiClient childApiClient;
    public GuardianApiClient guardianApiClient;

    public void Start()
    {
        createAccountButton.onClick.AddListener(OnCreateAccountClicked);
    }

    private async void OnCreateAccountClicked()
    {
        // Validatie
        if (string.IsNullOrEmpty(childFirstName.text) ||
            string.IsNullOrEmpty(childLastName.text) ||
            string.IsNullOrEmpty(childBirthDate.text) ||
            string.IsNullOrEmpty(doctorName.text) ||
            string.IsNullOrEmpty(parentFirstName.text) ||
            string.IsNullOrEmpty(parentLastName.text) ||
            string.IsNullOrEmpty(parentBirthDate.text) ||
            relationDropdown.value == 0)
        {
            feedbackText.text = "Vul alle velden in.";
            return;
        }


        Child child = new Child
        {
            userId = "willekeurig",
            firstName = childFirstName.text,
            lastName = childLastName.text,
            dateOfBirth = DateTime.Parse(childBirthDate.text),
            doctorName = doctorName.text,
            creationDate = DateTime.Now,
            treatmentPath = treatmentPlan.options[treatmentPlan.value].text[0].ToString()
        };

        
        Guardian guardian = new Guardian
        {
            firstName = parentFirstName.text,
            lastName = parentLastName.text,
            type = relationDropdown.options[relationDropdown.value].text
        };

        
        IWebRequestReponse childResult = await childApiClient.CreateChild(child);
        IWebRequestReponse guardianResult = await guardianApiClient.CreateGuardian(guardian);

        if (childResult is WebRequestData<Child> && guardianResult is WebRequestData<Guardian>)
        {
            feedbackText.text = "Account succesvol aangemaakt!";
            SceneManager.LoadScene("OverworldScene");
        }
        else
        {
            Debug.Log("Er is iets fout gegaan bij het aanmaken van het account.");
        }
    }
}
