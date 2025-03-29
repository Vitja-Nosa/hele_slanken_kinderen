using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level3Logic : LevelLogic
{
    // Objects in scene
    public Button StartVideoButton;
    public VideoClip Video;
    public HypoTalkBox hypoTalkBox;

    // audio file for hypo
    public AudioClip[] audioClips;

    private bool IsLoggedIn;
    private Child? child = null;

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
        hypoTalkBox.TriggerAnimation(audioClips[0], "Stel je voor dat je naar de dokter gaat omdat je je niet zo lekker voelt. De dokter wil jou graag helpen, maar hij of zij moet eerst goed begrijpen wat er aan de hand is.");
        await base.WaitForLengthOfAudio(audioClips[0]);
        await Task.Delay(1000);

        // Message 1
        hypoTalkBox.TriggerAnimation(audioClips[1], "Daarom gaat de dokter je een paar vragen stellen, zoals:\r\nHoe voel je je? Heb je goed gegeten? Heb je veel dorst? Moet je vaak naar de wc?");
        await base.WaitForLengthOfAudio(audioClips[1]);
        await Task.Delay(200);

        // Message 2
        hypoTalkBox.TriggerAnimation(audioClips[2], "Dit noemen we een anamnese! Het is eigenlijk gewoon een gesprekje waarin jij en je papa of mama vertellen hoe het met je gaat. Zo kan de dokter bedenken welk onderzoek of welke behandeling je nodig hebt om je weer beter te voelen! ");
        await base.WaitForLengthOfAudio(audioClips[2]);
        await Task.Delay(200);

        // Message 3
        hypoTalkBox.TriggerAnimation(audioClips[3], "Stel je voor dat je lichaam een beetje als een praatjesmaker is. Het kan niet echt praten zoals wij, maar het heeft wel manieren om je te laten weten dat er iets niet helemaal goed gaat. Die manieren noemen we symptomen.");
        await base.WaitForLengthOfAudio(audioClips[3]);
        await Task.Delay(200);

        // Message 4
        hypoTalkBox.TriggerAnimation(audioClips[4], "Symptomen zijn eigenlijk kleine hints van je lichaam. Ze geven een seintje dat het misschien hulp nodig heeft. Net zoals een auto lampjes laat knipperen als er iets mis is, geeft jouw lichaam ook signalen af.");
        await base.WaitForLengthOfAudio(audioClips[4]);
        await Task.Delay(200);

        // Message 5
        hypoTalkBox.TriggerAnimation(audioClips[5], "Nu gaat de meneer in de video je precies vertellen welke symptomen er kunnen zijn en hoe je er diabetes mee kan herkennen. Druk maar op de play knop als je er klaar voor bent!");
        await base.WaitForLengthOfAudio(audioClips[5]);
        await Task.Delay(200);

        StartVideoButton.interactable = true;
    }

    private async Awaitable EndOfLevel()
    {
        hypoTalkBox.TriggerAnimation(audioClips[6], "Je hebt geleerd wat een amnese is en wat symptonen zijn. Wat voor sticker zou je willen geven aan wat je hebt geleerd?");
        await base.WaitForLengthOfAudio(audioClips[6]);
        if (!LevelSetup.LoggedIn) await base.stickerPopup.AskingSticker();
    }

    private async void OnVideoFinished(VideoPlayer vp)
    {
        await EndOfLevel();

        // If user is logged in create a LevelCompletion and diary entry for this level
        if (IsLoggedIn)
        {
            await base.EnterLevelCompletion(3, "Vandaag heb ik geleerd wat een anamnese en symptonen!");
        }

        // method to leave the level
        base.LeaveLevel();
    }
}
