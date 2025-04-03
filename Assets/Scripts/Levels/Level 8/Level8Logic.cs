using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level8Logic : LevelLogic
{
    public Button StartVideoButton;
    public VideoClip Video;
    public HypoTalkBox hypoTalkBox;
    public AgePopup agePopup;
    public AudioClip[] audioClips; // [0] = intro jong, [1] = intro oud, [2] = outro

    private bool IsLoggedIn;
    private Child? child = null;
    private int age = -1;

    async void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;

        IsLoggedIn = LevelSetup.LoggedIn;
        videoPlayer.clip = Video;
        if (IsLoggedIn)
            child = await GetChildInfo();

        if (!IsLoggedIn || child == null)
            await StartLevelLoggedOut();
        else
            await StartLevelLoggedIn();
    }

    private async Task StartLevelLoggedIn()
    {
        var birthDate = DateTime.Parse(child!.dateOfBirth);
        age = CalculateAge(birthDate);
        await StartOfLevel();
    }

    private async Task StartLevelLoggedOut()
    {
        age = await agePopup.AskingAge();
        await StartOfLevel();
    }

    private async Task StartOfLevel()
    {
        StartVideoButton.interactable = false;

        if (age < 7)
        {
            hypoTalkBox.TriggerAnimation(audioClips[0],
                "Je gaat nu leren hoe je met prikken, meten en eten goed met diabetes omgaat! Klik op de start knop om de video te bekijken!");
            await WaitForLengthOfAudio(audioClips[0]);
        }
        else
        {
            hypoTalkBox.TriggerAnimation(audioClips[1],
                "Nu we weten dat je diabetes hebt, is het tijd om te beginnen met de behandeling. Je leert hoe je suiker meet, hoe je insuline geeft met een pen of pomp, en waarom eten belangrijk is. Klik op de start knop om de video te bekijken!");
            await WaitForLengthOfAudio(audioClips[1]);
        }

        StartVideoButton.interactable = true;
    }

    private async Task EndOfLevel()
    {
        hypoTalkBox.TriggerAnimation(audioClips[2],
            "Goed gedaan! Je weet nu hoe je bloedsuiker meet en insuline geeft. Je bent al begonnen met je behandeling. Wat voor sticker past daarbij?");
        await WaitForLengthOfAudio(audioClips[2]);

        if (!IsLoggedIn)
            await stickerPopup.AskingSticker();
    }

    private async void OnVideoFinished(VideoPlayer vp)
    {
        await EndOfLevel();

        if (IsLoggedIn)
        {
            await EnterLevelCompletion(8,
                "Vandaag heb ik geleerd hoe ik mijn bloedsuiker meet, insuline gebruik en wat belangrijk is om te eten. Mijn behandeling is nu echt begonnen!");
        }

        LeaveLevel();
    }

    private int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        int calculatedAge = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-calculatedAge)) calculatedAge--;
        return calculatedAge;
    }
}
