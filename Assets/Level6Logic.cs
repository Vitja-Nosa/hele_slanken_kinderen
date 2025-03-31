using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Level6Logic : LevelLogic
{
    // Objects in scene
    public Button StartVideoButton;
    public VideoClip Video;
    public HypoTalkBox hypoTalkBox;

    // audio file for hypo
    public AudioClip[] audioClips;

    private Child? child = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        // To detect when video that is played ended
        base.videoPlayer.loopPointReached += OnVideoFinished;
        base.videoPlayer.clip = Video;

        if (LevelSetup.LoggedIn)
            child = await base.GetChildInfo();

        await StartOfLevel();
    }
    private async Task StartOfLevel()
    {
        StartVideoButton.interactable = false;

        // Message 0
        hypoTalkBox.TriggerAnimation(audioClips[0], "In het vorige level hebben we samen gekeken naar de bloedtest die gedaan gaat worden.");
        await base.WaitForLengthOfAudio(audioClips[0]);
        await Task.Delay(300);

        // Message 1
        hypoTalkBox.TriggerAnimation(audioClips[1], "Ik ga nu vertellen over hoe deze test duidelijk kan maken of je wel of geen diabetes hebt, en dat je je geen zorgen hoeft te maken.");
        await base.WaitForLengthOfAudio(audioClips[1]);
        await Task.Delay(300);

        // Message 2
        hypoTalkBox.TriggerAnimation(audioClips[2], "Als uit de vorige test blijkt dat je bloedsuiker te hoog is, en je ook andere symptomen hebt die bij diabetes type-1 horen.");
        await base.WaitForLengthOfAudio(audioClips[2]);
        await Task.Delay(300);

        // Message 3
        hypoTalkBox.TriggerAnimation(audioClips[3], "Dan kan de diagnose voor Diabetes type-1 worden vastgesteld.");
        await base.WaitForLengthOfAudio(audioClips[3]);

        // Message 4
        hypoTalkBox.TriggerAnimation(audioClips[4], "In de volgende video wordt uitgelegd wat nou precies een diagnose is. Druk maar op de play knop als je er klaar voor bent!");
        await base.WaitForLengthOfAudio(audioClips[4]);

        StartVideoButton.interactable = true;
    }

    private async Awaitable EndOfLevel()
    {
        // Message 5
        await Task.Delay(300);
        hypoTalkBox.TriggerAnimation(audioClips[5], "Een diagnose zoals in de video is uitgelegd kan ook bij jouw worden gedaan voor diabetes type-1");
        await base.WaitForLengthOfAudio(audioClips[5]);
        await Task.Delay(300);

        // Message 6
        hypoTalkBox.TriggerAnimation(audioClips[6], "Hier hoef je je geen zorgen over te maken, want in de volgende levels zal ik alles stap voor stap met je uitleggen!");
        await base.WaitForLengthOfAudio(audioClips[6]);
        await Task.Delay(300);

        // Message 7
        hypoTalkBox.TriggerAnimation(audioClips[7], "Wat voor sticker vindt jij passen bij een diagnose stellen voor diabetes type-1?");
        await base.WaitForLengthOfAudio(audioClips[7]);
        await Task.Delay(300);

        if (!LevelSetup.LoggedIn) await base.stickerPopup.AskingSticker();
    }

    private async void OnVideoFinished(VideoPlayer vp)
    {
        await EndOfLevel();

        // If user is logged in create a LevelCompletion and diary entry for this level
        if (LevelSetup.LoggedIn)
        {
            await base.EnterLevelCompletion(6, "Vandaag heb ik geleerd wat een diagnose is en dat die voor diabetes type-1 kan worden gedaan!");
        }

        // method to leave the level
        base.LeaveLevel();
    }

}