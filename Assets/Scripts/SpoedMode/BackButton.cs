using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void GoBackToTreatmentType()
    {
        SceneManager.LoadScene("TreatmentTypeScene");
    }
}

