using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOverworld : MonoBehaviour
{
    public void ToOverworld()
    {
        // Load the overworld scene
        SceneManager.LoadScene("OverworldScene");
    }
}
