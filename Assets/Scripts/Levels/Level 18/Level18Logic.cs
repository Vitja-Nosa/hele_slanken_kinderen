using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Level18Logic : LevelLogic
{
    public HypoTalkBox hypoTalkBox;

    public Button buttonStart;
    public Button buttonNext;
    public Button buttonBack;
    public Button buttonFinish;

    public Image stripDisplay;
    public List<Sprite> stripImages; // 3 JPGs als sprites

    public AudioClip introClip;
    public AudioClip[] pageClips; // [0], [1], [2] = bij strip 1, 2, 3
    public string[] stripTexts;   // [0], [1], [2] = bij strip 1, 2, 3

    public AudioClip outroClip;

    private int currentIndex = 0;
    private bool IsLoggedIn;
    private Child? child;

    async void Start()
    {
        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await GetChildInfo();

        // Buttons + strip verbergen
        buttonNext.interactable = false;
        buttonBack.interactable = false;
        buttonFinish.gameObject.SetActive(false);
        stripDisplay.gameObject.SetActive(false);

        buttonStart.onClick.AddListener(OnStartClicked);
        buttonNext.onClick.AddListener(OnNextClicked);
        buttonBack.onClick.AddListener(OnBackClicked);
        buttonFinish.onClick.AddListener(OnFinishClicked);

        // Hypo intro
        hypoTalkBox.TriggerAnimation(introClip,
            "Soms kom je terug naar het ziekenhuis, ook als alles goed gaat. Ze kijken of je bloedsuiker goed blijft, en of je ogen en voeten nog helemaal gezond zijn. Klik op de blauwe play-knop om het stripverhaal te bekijken!");
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
        if (currentIndex >= 0 && currentIndex < pageClips.Length)
        {
            hypoTalkBox.TriggerAnimation(pageClips[currentIndex], stripTexts[currentIndex]);
            await WaitForLengthOfAudio(pageClips[currentIndex]);
        }
    }

    private async void OnFinishClicked()
    {
        buttonNext.interactable = false;
        buttonBack.interactable = false;
        buttonFinish.interactable = false;

        hypoTalkBox.TriggerAnimation(outroClip,
            "Goed gedaan! Je weet nu waarom je regelmatig terugkomt en wat de dokter controleert. Zo blijf je gezond. Welke sticker vind je hierbij passen?");
        await WaitForLengthOfAudio(outroClip);

        if (!IsLoggedIn)
        {
            await stickerPopup.AskingSticker();
        }
        else
        {
            await EnterLevelCompletion(18,
                "Vandaag heb ik geleerd dat ik regelmatig terugkom. Ze kijken dan naar mijn bloedsuiker, insuline, maar ook naar mijn ogen en voeten.");
        }

        LeaveLevel();
    }
}
