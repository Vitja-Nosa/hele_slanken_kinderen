using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Level13Logic : LevelLogic
{
    public HypoTalkBox hypoTalkBox;

    public Button buttonStart;
    public Button buttonNext;
    public Button buttonBack;
    public Button buttonFinish;

    public Image stripDisplay;
    public List<Sprite> stripImages; // 6 afbeeldingen
    public AudioClip[] stripClips;   // 6 audiofragmenten
    public string[] stripTexts;      // 6 tekstregels

    public AudioClip introClip;
    public AudioClip outroClip;

    private int currentIndex = 0;
    private bool IsLoggedIn;
    private Child? child;

    async void Start()
    {
        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await GetChildInfo();

        stripDisplay.gameObject.SetActive(false);
        buttonNext.interactable = false;
        buttonBack.interactable = false;
        buttonFinish.gameObject.SetActive(false);
        buttonStart.gameObject.SetActive(false);

        buttonStart.onClick.AddListener(OnStartClicked);
        buttonNext.onClick.AddListener(OnNextClicked);
        buttonBack.onClick.AddListener(OnBackClicked);
        buttonFinish.onClick.AddListener(OnFinishClicked);

        hypoTalkBox.TriggerAnimation(introClip,
            "Je wordt even opgenomen in het ziekenhuis. Dat is om te zorgen dat je bloedsuiker weer goed wordt. Ik laat je zien wat er allemaal gebeurt. Druk op start als je klaar bent!");
        await WaitForLengthOfAudio(introClip);

        buttonStart.gameObject.SetActive(true);
    }

    private async void OnStartClicked()
    {
        buttonStart.gameObject.SetActive(false);
        stripDisplay.gameObject.SetActive(true);
        currentIndex = 0;
        await ShowCurrentStrip();
    }

    private async void OnNextClicked()
    {
        if (currentIndex < stripImages.Count - 1)
            currentIndex++;

        await ShowCurrentStrip();
    }

    private async void OnBackClicked()
    {
        if (currentIndex > 0)
            currentIndex--;

        await ShowCurrentStrip();
    }

    private async Task ShowCurrentStrip()
    {
        stripDisplay.sprite = stripImages[currentIndex];
        buttonNext.interactable = false;
        buttonBack.interactable = false;
        buttonFinish.gameObject.SetActive(false);

        hypoTalkBox.TriggerAnimation(stripClips[currentIndex], stripTexts[currentIndex]);
        await WaitForLengthOfAudio(stripClips[currentIndex]);

        buttonNext.interactable = currentIndex < stripImages.Count - 1;
        buttonBack.interactable = currentIndex > 0;
        buttonFinish.gameObject.SetActive(currentIndex == stripImages.Count - 1);
    }

    private async void OnFinishClicked()
    {
        buttonNext.interactable = false;
        buttonBack.interactable = false;
        buttonFinish.interactable = false;

        hypoTalkBox.TriggerAnimation(outroClip,
            "Goed gedaan! Je weet nu wat er gebeurt bij een opname in het ziekenhuis. Welke sticker vind jij dat erbij past?");
        await WaitForLengthOfAudio(outroClip);

        if (!IsLoggedIn)
        {
            await stickerPopup.AskingSticker();
        }
        else
        {
            await EnterLevelCompletion(13,
                "Ik weet nu wat er gebeurt als ik wordt opgenomen in het ziekenhuis.");
        }

        LeaveLevel();
    }
}
