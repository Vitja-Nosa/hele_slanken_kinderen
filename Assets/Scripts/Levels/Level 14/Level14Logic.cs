using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level14Logic : LevelLogic
{
    public HypoTalkBox hypoTalkBox;
    public Button playButton;

    public AudioClip introClip;
    public AudioClip outroClip;

    private bool IsLoggedIn;
    private Child? child;

    async void Start()
    {
        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await GetChildInfo();

        videoPlayer.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);

        hypoTalkBox.TriggerAnimation(introClip,
            "Soms ben je zo ziek dat je snel hulp nodig hebt. Dan krijg je in het ziekenhuis een infuus met insuline en vocht. Dat helpt om weer beter te worden. Klik op de startknop om hier een video over te bekijken!");
        await WaitForLengthOfAudio(introClip);

        playButton.gameObject.SetActive(true);
        playButton.onClick.AddListener(OnPlayClicked);
    }

    private void OnPlayClicked()
    {
        playButton.gameObject.SetActive(false);
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private async void OnVideoFinished(VideoPlayer vp)
    {
        videoPlayer.gameObject.SetActive(false);

        hypoTalkBox.TriggerAnimation(outroClip,
            "Dat was spannend hè? Gelukkig helpt het snel. Welke sticker past bij dit deel van je avontuur?");
        await WaitForLengthOfAudio(outroClip);

        if (!IsLoggedIn)
        {
            await stickerPopup.AskingSticker();
        }
        else
        {
            await EnterLevelCompletion(14,
                "Ik heb geleerd dat ik een infuus krijg als ik heel ziek ben.");
        }

        LeaveLevel();
    }
}
