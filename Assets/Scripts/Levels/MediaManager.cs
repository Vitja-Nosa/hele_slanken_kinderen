using UnityEngine;
using UnityEngine.Video;

public class MediaController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public void PlayMedia()
    {
        if (!videoPlayer.isPlaying)
            videoPlayer.Play();
    }

    public void PauseMedia()
    {
        if (videoPlayer.isPlaying)
            videoPlayer.Pause();
    }
}
