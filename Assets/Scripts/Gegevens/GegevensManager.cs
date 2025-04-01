using System.Collections.Generic;
using System.Threading.Tasks;
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

    private Dictionary<int, string> treatmentPlanValues = new Dictionary<int, string>();

    public void Start()
    {
        // Populate treatmentPlan dropdown with options displaying different text
        treatmentPlan.options.Clear();
        treatmentPlan.options.Add(new TMP_Dropdown.OptionData("-"));
        treatmentPlan.options.Add(new TMP_Dropdown.OptionData("Zonder ziekenhuisopname"));
        treatmentPlan.options.Add(new TMP_Dropdown.OptionData("Met ziekenhuisopname"));
        treatmentPlan.RefreshShownValue();

        // Map the displayed text to the corresponding values
        treatmentPlanValues.Add(0, null);
        treatmentPlanValues.Add(1, "A");
        treatmentPlanValues.Add(2, "B");
    }

    public async void OnCreateAccountClicked()
    {
        Debug.Log("OnCreateAccountClicked");

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
            Debug.Log("Vul alle velden in.");
            feedbackText.text = "Vul alle velden in";
            return;
        }

        DateTime brithDateChild = DateTime.Now;
        DateTime birthDateGuardian = DateTime.Now;

        // check if dates are valid
        try // check child
        {
            brithDateChild = DateTime.Parse(childBirthDate.text);
        }
        catch
        {
            feedbackText.text = "Geboortedatum is verkeerd formaat";
            return;
        }
        try // check guardian
        {
            birthDateGuardian = DateTime.Parse(childBirthDate.text);
        }
        catch
        {
            feedbackText.text = "Geboortedatum is verkeerd formaat";
            return;
        }

        // Retrieve the value from the dictionary based on the selected index
        string selectedTreatmentPlan = treatmentPlanValues[treatmentPlan.value];

        Child child = new Child
        {
            userId = "willekeurig",
            firstName = childFirstName.text,
            lastName = childLastName.text,
            dateOfBirth = DateTime.Parse(childBirthDate.text).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            doctorName = doctorName.text,
            creationDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            treatmentPath = selectedTreatmentPlan
        };

        Guardian guardian = new Guardian
        {
            userId = "willekeurig",
            childId = "willekeurig",
            id = "willekeurig",
            phone = "06382998",
            email = RegisterEmail.email,
            firstName = parentFirstName.text,
            lastName = parentLastName.text,
            type = relationDropdown.options[relationDropdown.value].text
        };

        IWebRequestReponse childResult = await childApiClient.CreateChild(child);
        IWebRequestReponse guardianResult = await guardianApiClient.CreateGuardian(guardian);

        if (childResult is WebRequestData<Child> && guardianResult is WebRequestData<Guardian>)
        {
            feedbackText.text = "Account succesvol aangemaakt!";
            SceneManager.LoadScene("IntroductieScene");
        }
        else
        {
            feedbackText.text = "Er is iets fout gegaan bij het aanmaken van het account.";
            Debug.Log("Er is iets fout gegaan bij het aanmaken van het account.");
        }
    }
}
