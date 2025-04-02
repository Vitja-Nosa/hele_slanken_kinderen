using UnityEngine;
using UnityEngine.SceneManagement;

public class PageLoader : MonoBehaviour
{
    public void LoadAgenda()
    {
        SceneManager.LoadScene("AgendaScene");
    }
    public void LoadLogin()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void LoadOverworld()
    {
        SceneManager.LoadScene("OverworldScene");
    }
    public void LoadInstructions()
    {
        SceneManager.LoadScene("IntroductieScene");
    }
}
