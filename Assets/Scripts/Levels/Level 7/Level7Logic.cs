using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level7Logic : LevelLogic
{
    public Button StartVideoButton;
    public VideoClip video; // [0] = onder 7 jaar, [1] = 7 jaar en ouder
    public HypoTalkBox hypoTalkBox;
    public AgePopup agePopup;
    public AudioClip[] audioClips; // [0] = intro, [1] = outro

    private bool IsLoggedIn;
    private Child? child = null;

    async void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        IsLoggedIn = LevelSetup.LoggedIn;

        if (IsLoggedIn)
            child = await GetChildInfo();

        videoPlayer.clip = video;
        await StartOfLevel();
    }

    private async Task StartOfLevel()
    {
        StartVideoButton.interactable = false;
        hypoTalkBox.TriggerAnimation(audioClips[0],
            "Je hebt gehoord dat je diabetes hebt. Dat is best even schrikken. Gelukkig zijn er goede manieren om met diabetes om te gaan. We leggen je rustig uit wat het betekent en wat er gaat gebeuren. Klik op de play knop om de video te bekijken.");
        await WaitForLengthOfAudio(audioClips[0]);

        StartVideoButton.interactable = true;
    }

    private async Task EndOfLevel()
    {
        hypoTalkBox.TriggerAnimation(audioClips[1],
            "Goed gedaan! Je weet nu wat diabetes is en wat er gaat veranderen. Je bent niet alleen – je ouders en de dokters helpen je. Welke sticker vind jij dat past bij dit level?");
        await WaitForLengthOfAudio(audioClips[1]);
        if (!IsLoggedIn)
            await stickerPopup.AskingSticker();
    }

    private async void OnVideoFinished(VideoPlayer vp)
    {
        await EndOfLevel();

        if (IsLoggedIn)
        {
            await EnterLevelCompletion(7,
                "Ik heb gehoord dat ik diabetes heb. Dat vond ik best spannend, maar ik heb ook geleerd dat ik goed geholpen word. Ik weet nu wat het betekent en wat we gaan doen.");
        }

        LeaveLevel();
    }
}
