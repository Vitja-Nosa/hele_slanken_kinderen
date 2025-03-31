using UnityEngine;

public class LevelCompletedPopUp : MonoBehaviour
{
    public GameObject PopUp;
    private AudioSource audioSource;
    public AudioClip completedSound;


    private void Start()
    {
        if (StatusManager.Instance.displayCompleted)
            PopUp.SetActive(true);
    }
    private void OnEnable()
    {
        audioSource.clip = completedSound;
        audioSource.Play();
    }

    public void ClosePopUp()
    {
        PopUp.SetActive(false);
    }
}
