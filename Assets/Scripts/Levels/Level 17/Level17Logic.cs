using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level17Logic : LevelLogic
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
            "Je mag naar huis! Maar je bent niet alleen: je ouders helpen je, en jij leert steeds meer zelf doen. Je meet je bloedsuiker en spuit insuline zoals je hebt geleerd. Klik maar op de startknop om de video te bekijken!");
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
            "Goed bezig! Nu weet je wat er thuis allemaal gebeurt. Kies een sticker die daarbij past.");
        await WaitForLengthOfAudio(outroClip);

        if (!IsLoggedIn)
        {
            await stickerPopup.AskingSticker();
        }
        else
        {
            await EnterLevelCompletion(17,
                "Ik weet nu hoe ik thuis goed omga met mijn diabetes.");
        }

        LeaveLevel();
    }
}
