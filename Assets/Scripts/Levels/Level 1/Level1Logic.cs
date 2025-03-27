using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level1Logic : MonoBehaviour
{
    // Objects in scene
    public Button StartVideoButton;
    public VideoPlayer videoPlayer;
    public VideoClip[] Videos;
    public HypoTalkBox hypoTalkBox;

    // audio files for hypo
    public AudioClip[] audioClips;

    // ModelAPI client
    public ChildApiClient childApiClient;
    public DiaryEntryApiClient diaryEntryApiClient;
    public ChildLevelCompletionApiClient childLevelCompletionApiClient;

    private bool IsLoggedIn;
    private Child? child = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;

        StartLevelLoggedIn();

        //IsLoggedIn = LevelSetup.LoggedIn;

        //if (IsLoggedIn)
        //{
        //    child = await LevelSetup.GetChildInfo(childApiClient);
        //    if (child != null)
        //        StartLevelLoggedIn();
        //}

        //StartLevelLoggedOut();
    }

    private async void StartLevelLoggedIn()
    {
        await StartOfLevel();
    }

    private async void StartLevelLoggedOut()
    {
        AskingAge();
        await StartOfLevel();
    }

    private void AskingAge()
    {
        throw new NotImplementedException();
    }

    private async Awaitable StartOfLevel()
    {
        StartVideoButton.interactable = false;
        hypoTalkBox.TriggerAnimation(audioClips[0], "Welkom bij het eerste level. Je gaat een uitleg krijgen over hoe het inchecken er aan toe gaat in het ziekenhuis. Druk op de play knop als je er klaar voor bent.");
        await LevelSetup.WaitForLengthOfAudio(audioClips[0]);
        StartVideoButton.interactable = true;
    }

    private async Awaitable EndOfLevel()
    {
        hypoTalkBox.TriggerAnimation(audioClips[1], "Hopelijk is nu duidelijk hoe inchecken zal gaan. Wat voor sticker zou je het inchecken bij de kinderarts willen geven?");
        await LevelSetup.WaitForLengthOfAudio(audioClips[1]);
    }

    private async void OnVideoFinished(VideoPlayer vp)
    {
        await EndOfLevel();
        int stickerId = await AskForSticker();
        await LevelSetup.PostLevelCompletion(1, childLevelCompletionApiClient);
        await LevelSetup.PostDiaryEntry(new DiaryEntry
        {
            childId = "",
            id = "",
            content = "level 'Inchecken Bij De Kinderarts' voltooid",
            stickerId = stickerId,
            date = DateTime.Now.ToString()
        },
        diaryEntryApiClient);
    }

    private async Task<int> AskForSticker()
    {
        throw new NotImplementedException();
    }
}
