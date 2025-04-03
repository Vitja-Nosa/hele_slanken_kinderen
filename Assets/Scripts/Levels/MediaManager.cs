using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MediaController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button PauseButton;
    public Button PlayButton;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;

        PauseButton.gameObject.SetActive(false);
        PlayButton.gameObject.SetActive(false);
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        PauseButton.gameObject.SetActive(false);
        PlayButton.gameObject.SetActive(false);
    }

    public void PlayMedia()
    {
        if (!videoPlayer.isPlaying)
            videoPlayer.Play();

        PauseButton.gameObject.SetActive(true);
        PlayButton.gameObject.SetActive(false);
    }

    public void PauseMedia()
    {
        if (videoPlayer.isPlaying)
            videoPlayer.Pause();

        PauseButton.gameObject.SetActive(false);
        PlayButton.gameObject.SetActive(true);
    }
}
