using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level22Logic : LevelLogic
{
    public HypoTalkBox hypoTalkBox;

    public Button buttonStart;
    public Button buttonNext;
    public Button buttonBack;
    public Button buttonFinish;

    public Image stripDisplay;
    public List<Sprite> stripImages;
    public AudioClip introClip;
    public AudioClip[] stripClips;
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

        buttonStart.gameObject.SetActive(false);
        buttonNext.interactable = false;
        buttonBack.interactable = false;
        buttonFinish.gameObject.SetActive(false);
        stripDisplay.gameObject.SetActive(false);

        buttonStart.onClick.AddListener(OnStartClicked);
        buttonNext.onClick.AddListener(OnNextClicked);
        buttonBack.onClick.AddListener(OnBackClicked);
        buttonFinish.onClick.AddListener(OnFinishClicked);

        hypoTalkBox.TriggerAnimation(introClip,
            "Steeds meer dingen kun je zelf doen. Dat heet zelfmanagement. Kijk maar even mee!");
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
        buttonNext.interactable = false;
        buttonBack.interactable = false;
        buttonFinish.gameObject.SetActive(false);

        hypoTalkBox.TriggerAnimation(stripClips[currentIndex], stripTexts[currentIndex]);
        await WaitForLengthOfAudio(stripClips[currentIndex]);

        UpdateButtons();
    }

    private async void OnFinishClicked()
    {
        buttonNext.interactable = false;
        buttonBack.interactable = false;
        buttonFinish.interactable = false;

        hypoTalkBox.TriggerAnimation(outroClip,
            "Goed gedaan! Jij weet nu wat zelfmanagement is en wat je zelf al kunt doen. Welke sticker past bij dit level?");
        await WaitForLengthOfAudio(outroClip);

        if (!IsLoggedIn)
        {
            await stickerPopup.AskingSticker();
        }
        else
        {
            await EnterLevelCompletion(22,
                "Ik heb geleerd wat zelfmanagement is en wat ik zelf al kan doen.");
        }

        LeaveLevel();
    }
}
