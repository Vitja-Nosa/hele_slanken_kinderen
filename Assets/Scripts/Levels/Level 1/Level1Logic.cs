using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level1Logic : LevelLogic
{
    // Objects in scene
    public Button StartVideoButton;
    public VideoClip[] Videos;
    public HypoTalkBox hypoTalkBox;
    public AgePopup agePopup;

    // audio files for hypo
    public AudioClip[] audioClips;

    private bool IsLoggedIn;
    private Child? child = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        // To detect when video that is played ended
        base.videoPlayer.loopPointReached += OnVideoFinished;

        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await base.GetChildInfo();

        if (!IsLoggedIn || child == null)
            StartLevelLoggedOut();
        else
            StartLevelLoggedIn();
    }

    private async void StartLevelLoggedIn()
    {
        //Select right video by childs age
        if (child.dateOfBirth > DateTime.Now.AddYears(-7))
            base.videoPlayer.clip = Videos[0];
        else
            base.videoPlayer.clip = Videos[1];

        //Trigger the start of Level 1
        await StartOfLevel();
    }

    private async void StartLevelLoggedOut()
    {
        // asking age with popup
        int age = await agePopup.AskingAge();

        // Select right video by childs age
        if (age < 7)
            videoPlayer.clip = Videos[0];
        else
            videoPlayer.clip = Videos[1];

        // Trigger the start of Level 1
        await StartOfLevel();
    }

    private async Awaitable StartOfLevel()
    {
        StartVideoButton.interactable = false;
        hypoTalkBox.TriggerAnimation(audioClips[0], "Welkom bij het eerste level. Je gaat een uitleg krijgen over hoe het inchecken er aan toe gaat in het ziekenhuis. Druk op de play knop als je er klaar voor bent.");
        await base.WaitForLengthOfAudio(audioClips[0]);
        StartVideoButton.interactable = true;
    }

    private async Awaitable EndOfLevel()
    {
        hypoTalkBox.TriggerAnimation(audioClips[1], "Hopelijk is nu duidelijk hoe inchecken zal gaan. Wat voor sticker zou je het inchecken bij de kinderarts willen geven?");
        await base.WaitForLengthOfAudio(audioClips[1]);
    }

    private async void OnVideoFinished(VideoPlayer vp)
    {
        await EndOfLevel();

        // If user is logged in create a LevelCompletion and diary entry for this level
        if (IsLoggedIn)
        {
            await base.EnterLevelCompletion(1, "Vandaag heb ik bekeken hoe 'Inchecken Bij De Kinderarts' er aan toe gaat!");
        }

        // method to leave the level
        base.LeaveLevel();
    }
}
