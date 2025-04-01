using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadLevel()
    {
        LevelSetup.LoggedIn = false;
        Debug.Log(LevelSetup.LoggedIn);
        SceneManager.LoadScene(sceneToLoad);
    }
}
