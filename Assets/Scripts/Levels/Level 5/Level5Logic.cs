using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Level5Logic : LevelLogic
{
    // Objects in scene
    public Button StartVideoButton;
    public VideoClip Video;
    public HypoTalkBox hypoTalkBox;

    // audio file for hypo
    public AudioClip[] audioClips;

    private bool IsLoggedIn;
    private Child? child = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        // To detect when video that is played ended
        base.videoPlayer.loopPointReached += OnVideoFinished;
        base.videoPlayer.clip = Video;

        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await base.GetChildInfo();

        await StartOfLevel();
    }
    private async Task StartOfLevel()
    {
        StartVideoButton.interactable = false;

        // Message 0
        hypoTalkBox.TriggerAnimation(audioClips[0], "Bij het onderzoek van de doktor naar diabetes type 1 hoort ook een bloedtest. Dit is niks om bang van te zijn.");
        await base.WaitForLengthOfAudio(audioClips[0]);
        await Task.Delay(300);

        // Message 1
        hypoTalkBox.TriggerAnimation(audioClips[1], "Om uit te leggen hoe deze test gaat heb ik een filmpje klaar gezet voor jouw. Druk maar op de play knop als je er klaar voor bent!");
        await base.WaitForLengthOfAudio(audioClips[1]);
        await Task.Delay(300);

        StartVideoButton.interactable = true;
    }

    private async Awaitable EndOfLevel()
    {
        // Message 2
        hypoTalkBox.TriggerAnimation(audioClips[2], "In het volgende level ga ik ook nog uitleggen wat er met deze bloedtest wordt gedaan!");
        await base.WaitForLengthOfAudio(audioClips[2]);
        await Task.Delay(300);

        // Message 3
        hypoTalkBox.TriggerAnimation(audioClips[3], "Wat voor sticker wil jij deze bloedtest geven?");
        await base.WaitForLengthOfAudio(audioClips[3]);

        if (!LevelSetup.LoggedIn) await base.stickerPopup.AskingSticker();
    }

    private async void OnVideoFinished(VideoPlayer vp)
    {
        await EndOfLevel();

        // If user is logged in create a LevelCompletion and diary entry for this level
        if (IsLoggedIn)
        {
            await base.EnterLevelCompletion(5, "Vandaag heb ik bekeken hoe 'Een bloedglucose meting bij de huisarts' er aan toe gaat!");
        }

        // method to leave the level
        base.LeaveLevel();
    }

}
