using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level2Logic : LevelLogic
{
    public Button StartVideoButton;
    public VideoClip VideoClip; // 1 video voor alle leeftijden
    public HypoTalkBox hypoTalkBox;
    public AudioClip[] audioClips;

    private bool IsLoggedIn;
    private Child? child = null;

    async void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        IsLoggedIn = LevelSetup.LoggedIn;

        if (IsLoggedIn)
            child = await GetChildInfo();

        videoPlayer.clip = VideoClip;
        await StartOfLevel();
    }

    private async Task StartOfLevel()
    {
        StartVideoButton.interactable = false;
        hypoTalkBox.TriggerAnimation(audioClips[0],
            "Voordat je naar de dokter gaat, moeten jij en je ouders even wat papierwerk doen. Zo weet het ziekenhuis wie je bent en hoe je verzekerd bent. Je gaat nu uitleg krijgen over hoe dit van gang gaat! Druk op de play knop als je er klaar voor bent.");
        await WaitForLengthOfAudio(audioClips[0]);
        StartVideoButton.interactable = true;
    }

    private async Task EndOfLevel()
    {
        hypoTalkBox.TriggerAnimation(audioClips[1],
            "Goed gedaan! Nu weet je wat je moet gaan doen bij de balie. Wat voor sticker past bij dit stukje van jouw ziekenhuisavontuur?");
        await WaitForLengthOfAudio(audioClips[1]);
        if (!IsLoggedIn)
            await stickerPopup.AskingSticker();
    }

    private async void OnVideoFinished(VideoPlayer vp)
    {
        await EndOfLevel();

        if (IsLoggedIn)
        {
            await EnterLevelCompletion(2,
                "Vandaag heb ik gezien hoe we bij de balie onze gegevens en verzekering regelen voordat we naar de dokter gaan.");
        }

        LeaveLevel();
    }
}
