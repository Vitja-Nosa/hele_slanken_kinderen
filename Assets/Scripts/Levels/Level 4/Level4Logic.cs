using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level4Logic : LevelLogic
{
    // Objects in scene
    public Sprite[] Frames;
    public HypoTalkBox hypoTalkBox;
    public Image FrameDisplay;

    // audio files for hypo
    public AudioClip[] audioClips;

    private bool IsLoggedIn;
    private Child? child = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    { 
        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await base.GetChildInfo();

        await StartOfLevel();
    }

    private async Task StartOfLevel()
    {

        // message 0
        hypoTalkBox.TriggerAnimation(audioClips[0], "De dokter kijkt niet alleen of je vaak dorst hebt of vaak naar de wc moet, maar ook of je andere klachten hebt.");
        await base.WaitForLengthOfAudio(audioClips[0]);
        await Task.Delay(200);
        FrameDisplay.sprite = null;

        // message 1
        hypoTalkBox.TriggerAnimation(audioClips[1], "Soms kun je bijvoorbeeld misselijk zijn of moeten overgeven.");
        FrameDisplay.sprite = Frames[0];
        await base.WaitForLengthOfAudio(audioClips[1]);
        await Task.Delay(1000);

        // message 2
        hypoTalkBox.TriggerAnimation(audioClips[2], "Je buik kan pijn doen, of je kunt dingen een beetje wazig zien, alsof je ogen niet goed scherpstellen.");
        FrameDisplay.sprite = Frames[1];
        await base.WaitForLengthOfAudio(audioClips[2]);
        await Task.Delay(1000);

        // message 3
        hypoTalkBox.TriggerAnimation(audioClips[3], "Ook kan het zijn dat wondjes langer duren om te genezen of dat je sneller last hebt van infecties op je huid.");
        FrameDisplay.sprite = Frames[2];
        await base.WaitForLengthOfAudio(audioClips[3]);
        await Task.Delay(1000);

        // message 4
        hypoTalkBox.TriggerAnimation(audioClips[4], "Sommige kinderen voelen zich ook sneller boos of verdrietig zonder duidelijke reden.");
        FrameDisplay.sprite = Frames[3];
        await base.WaitForLengthOfAudio(audioClips[4]);
        await Task.Delay(1000);

        // message 5
        hypoTalkBox.TriggerAnimation(audioClips[5], "De dokter controleert al deze dingen om te begrijpen of het misschien door diabetes komt.");
        FrameDisplay.sprite = Frames[4];
        await base.WaitForLengthOfAudio(audioClips[5]);
        await Task.Delay(1000);

        FrameDisplay.gameObject.SetActive(false);
        await EndOfLevel();

        // If user is logged in create a LevelCompletion and diary entry for this level
        if (IsLoggedIn)
        {
            await base.EnterLevelCompletion(1, "Vandaag heb ik bekeken hoe 'Inchecken Bij De Kinderarts' er aan toe gaat!");
        }

        // method to leave the level
        base.LeaveLevel();
    }

    private async Awaitable EndOfLevel()
    {
        hypoTalkBox.TriggerAnimation(audioClips[6], "Je hebt geleerd over wat voor symptomen de dokter nog meer wil controleren bij de controle in het ziekenhuis. Wat voor sticker zou je hierbij willen geven?");
        await base.WaitForLengthOfAudio(audioClips[6]);
        if (!LevelSetup.LoggedIn) await base.stickerPopup.AskingSticker();
    }
}
