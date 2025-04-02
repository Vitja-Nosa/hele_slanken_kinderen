using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void GoBackToTreatmentType()
    {
        SceneManager.LoadScene("TreatmentTypeScene");
    }

    public void GoBackToLogin()
    {
        SceneManager.LoadScene("LoginScene");
    }
}

