using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level15Logic : LevelLogic
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
            "Tijdens je opname houden de dokters je goed in de gaten. Ze controleren je bloedsuiker, of er ketonen in je urine zitten en hoe je je voelt. Bekijk de video door op de start knop te klikken!");
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
            "Goed dat ze zo goed voor je zorgen! Kies maar een sticker die bij dit level past.");
        await WaitForLengthOfAudio(outroClip);

        if (!IsLoggedIn)
        {
            await stickerPopup.AskingSticker();
        }
        else
        {
            await EnterLevelCompletion(15,
                "Ik weet dat de dokters mijn bloedsuiker en ketonen goed controleren in het ziekenhuis.");
        }

        LeaveLevel();
    }
}
