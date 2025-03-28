using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBackPopup : MonoBehaviour
{
    public void GoBack()
    {
        if (LevelSetup.LoggedIn)
            SceneManager.LoadScene("OverworldScene");
        else
            SceneManager.LoadScene("LevelSelectScene");
    }
}
