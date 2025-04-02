using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level10Logic : LevelLogic
{
    public Button StartVideoButton;
    public VideoClip video; // [0] = voor kinderen < 7 jaar, [1] = voor kinderen 7 jaar en ouder
    public HypoTalkBox hypoTalkBox;
    public AudioClip[] audioClips; // [0] = intro Iedereen [1 = outro voor iedereen

    private bool IsLoggedIn;
    private Child? child = null;
    private int age = -1;

    async void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.clip = video;

        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await GetChildInfo();

        await StartOfLevel();
    }


    private async Task StartOfLevel()
    {
        StartVideoButton.interactable = false;
        hypoTalkBox.TriggerAnimation(audioClips[0], "Nu ga je naar huis met een behandelplan. Je ouders krijgen uitleg over wat je elke dag moet doen om je bloedsuiker goed te houden. Klik op de start knop om de video te bekijken.");
        await WaitForLengthOfAudio(audioClips[0]);

        StartVideoButton.interactable = true;
    }

    private async Task EndOfLevel()
    {
        hypoTalkBox.TriggerAnimation(audioClips[1], "Goed gedaan! Je weet nu wat je thuis moet doen om je diabetes onder controle te houden. Welke sticker vind je dat bij dit level past?");
        await WaitForLengthOfAudio(audioClips[1]);

        if (!IsLoggedIn)
            await stickerPopup.AskingSticker();
    }

    private async void OnVideoFinished(VideoPlayer vp)
    {
        await EndOfLevel();

        if (IsLoggedIn)
        {
            await EnterLevelCompletion(10,
                "Vandaag ging ik naar huis met een behandelplan. Ik weet nu wat ik dagelijks moet doen om mijn diabetes goed te beheren.");
        }
        LeaveLevel();
    }
}
