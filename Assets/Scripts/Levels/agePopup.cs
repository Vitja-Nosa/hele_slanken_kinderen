using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgePopup : MonoBehaviour
{
    public GameObject PopUp;
    public TMP_InputField ageInputField; // Assign in Inspector
    public Button submitButton; // Assign in Inspector

    private int enteredAge;
    private bool ageSubmitted = false;

    public void Awake()
    {
        submitButton.onClick.AddListener(OnSubmitAge);
        PopUp.SetActive(false); // Hide popup initially
    }

    private void OnSubmitAge()
    {
        if (int.TryParse(ageInputField.text, out enteredAge) && enteredAge > 0)
        {
            ageSubmitted = true;
            PopUp.SetActive(false); // Hide popup after valid input
        }
    }

    public async Task<int> AskingAge()
    {
        PopUp.SetActive(true); // Show popup
        ageSubmitted = false;
        enteredAge = 0;

        while (!ageSubmitted)
        {
            await Task.Yield(); // Wait until user submits valid input
        }

        return enteredAge;
    }
}
