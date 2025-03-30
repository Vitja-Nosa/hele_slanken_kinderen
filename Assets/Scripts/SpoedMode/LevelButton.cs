using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
