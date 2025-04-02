using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level16Logic : LevelLogic
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
            "Je leert samen met je ouders wat diabetes is, hoe je bloedsuiker te meten, en wat je moet doen bij een hypo of hyper. Dat is belangrijk om goed voor jezelf te kunnen zorgen. Klik op de startknop om hier een korte video over te bekijken!");
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
            "Knap dat je dat allemaal leert! Kies maar een sticker die erbij past.");
        await WaitForLengthOfAudio(outroClip);

        if (!IsLoggedIn)
        {
            await stickerPopup.AskingSticker();
        }
        else
        {
            await EnterLevelCompletion(16,
                "Ik heb geleerd wat een hypo en hyper zijn, en wat ik dan moet doen.");
        }

        LeaveLevel();
    }
}
