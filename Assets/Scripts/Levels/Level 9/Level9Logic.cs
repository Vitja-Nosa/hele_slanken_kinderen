using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level9Logic : LevelLogic
{
    public Button StartVideoButton;
    public VideoClip Video; // [0] = onder 7 jaar, [1] = 7 jaar en ouder
    public HypoTalkBox hypoTalkBox;
    public AgePopup agePopup;
    public AudioClip[] audioClips; // [0] = intro jong, [1] = intro oud, [2] = outro

    private bool IsLoggedIn;
    private Child? child = null;
    private int age = -1;

    async void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.clip = Video;
        IsLoggedIn = LevelSetup.LoggedIn;

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
                "Soms voel je je een beetje raar. Je bent dan moe of duizelig, of je krijgt trek. Dat kan komen door je bloedsuiker. Je leert nu hoe dat voelt en wat je dan moet doen. Klik op de start knop om de video te bekijken.");
            await WaitForLengthOfAudio(audioClips[0]);
        }
        else
        {
            hypoTalkBox.TriggerAnimation(audioClips[1],
                "Als je diabetes hebt, is het belangrijk dat je goed weet hoe je lichaam voelt. Bij een hypo of hyper voel je je anders. Je leert nu hoe je dat herkent en wat je dan moet doen met eten of drinken. Klik op de start knop om de video te bekijken.");
            await WaitForLengthOfAudio(audioClips[1]);
        }

        StartVideoButton.interactable = true;
    }

    private async Task EndOfLevel()
    {
        hypoTalkBox.TriggerAnimation(audioClips[2],
            "Goed gedaan! Je weet nu hoe je een hypo of hyper kunt herkennen en wat je dan moet doen. Dat is superbelangrijk!");
        await WaitForLengthOfAudio(audioClips[2]);

        if (!IsLoggedIn)
            await stickerPopup.AskingSticker();
    }

    private async void OnVideoFinished(VideoPlayer vp)
    {
        await EndOfLevel();

        if (IsLoggedIn)
        {
            await EnterLevelCompletion(9,
                "Vandaag heb ik geleerd hoe ik een hypo of hyper kan herkennen. En wat ik dan moet doen met eten of drinken. Ik begrijp mijn lichaam nu beter.");
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
