using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Level21Logic : LevelLogic
{
    public HypoTalkBox hypoTalkBox;

    public Button buttonStart;
    public Button buttonNext;
    public Button buttonBack;
    public Button buttonFinish;

    public Image stripDisplay;
    public List<Sprite> stripImages;

    public AudioClip introClip;
    public AudioClip[] pageClips; // 0,1,2
    public string[] stripTexts;

    public AudioClip outroClip;

    private int currentIndex = 0;
    private bool IsLoggedIn;
    private Child? child;

    async void Start()
    {
        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await GetChildInfo();

        buttonNext.interactable = false;
        buttonBack.interactable = false;
        buttonFinish.gameObject.SetActive(false);
        stripDisplay.gameObject.SetActive(false);

        buttonStart.onClick.AddListener(OnStartClicked);
        buttonNext.onClick.AddListener(OnNextClicked);
        buttonBack.onClick.AddListener(OnBackClicked);
        buttonFinish.onClick.AddListener(OnFinishClicked);

        hypoTalkBox.TriggerAnimation(introClip,
            "Op school is het soms net even anders als je diabetes hebt. Ik laat je zien waar je dan op moet letten. Klik op de start knop om de strip te bekijken!");
        await WaitForLengthOfAudio(introClip);

        buttonStart.gameObject.SetActive(true);
    }

    private async void OnStartClicked()
    {
        buttonStart.gameObject.SetActive(false);
        stripDisplay.gameObject.SetActive(true);
        currentIndex = 0;
        ShowCurrentStrip();
        await PlayHypoForCurrentStrip();
        UpdateButtons();
    }

    private async void OnNextClicked()
    {
        if (currentIndex < stripImages.Count - 1)
            currentIndex++;

        ShowCurrentStrip();
        await PlayHypoForCurrentStrip();
        UpdateButtons();
    }

    private async void OnBackClicked()
    {
        if (currentIndex > 0)
            currentIndex--;

        ShowCurrentStrip();
        await PlayHypoForCurrentStrip();
        UpdateButtons();
    }

    private void ShowCurrentStrip()
    {
        stripDisplay.sprite = stripImages[currentIndex];
    }

    private void UpdateButtons()
    {
        buttonBack.interactable = currentIndex > 0;
        buttonNext.interactable = currentIndex < stripImages.Count - 1;
        buttonFinish.gameObject.SetActive(currentIndex == stripImages.Count - 1);
    }

    private async Task PlayHypoForCurrentStrip()
    {
        // Buttons tijdelijk uitschakelen
        buttonNext.interactable = false;
        buttonBack.interactable = false;
        buttonFinish.gameObject.SetActive(false);

        if (currentIndex >= 0 && currentIndex < pageClips.Length)
        {
            hypoTalkBox.TriggerAnimation(pageClips[currentIndex], stripTexts[currentIndex]);
            await WaitForLengthOfAudio(pageClips[currentIndex]);
        }

        // Buttons weer activeren op basis van index
        UpdateButtons();
    }

    private async void OnFinishClicked()
    {
        buttonNext.interactable = false;
        buttonBack.interactable = false;
        buttonFinish.interactable = false;

        hypoTalkBox.TriggerAnimation(outroClip,
            "Goed gedaan! Nu weet je hoe jij ook op school goed kunt omgaan met diabetes. Jij bent een echte topper! Welke sticker past bij deze strip?");
        await WaitForLengthOfAudio(outroClip);

        stripDisplay.gameObject.SetActive(false);
        buttonNext.gameObject.SetActive(false);
        buttonBack.gameObject.SetActive(false);
        buttonFinish.gameObject.SetActive(false);

        if (!IsLoggedIn)
        {
            await stickerPopup.AskingSticker();
        }
        else
        {
            await EnterLevelCompletion(21,
                "Ik heb geleerd hoe ik op school goed kan omgaan met mijn diabetes.");
        }

        LeaveLevel();
    }
}
