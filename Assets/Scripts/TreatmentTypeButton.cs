using UnityEngine;
using UnityEngine.SceneManagement;

public class TreatmentTypeButton : MonoBehaviour
{
    public string typeNaam; // Zet dit in de Inspector voor elke knop

    public void OpenLevelSelectScene()
    {
        TreatmentTypeHolder.SelectedType = typeNaam;
        SceneManager.LoadScene("LevelSelectScene");
    }
}
