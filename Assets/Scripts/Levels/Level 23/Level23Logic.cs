using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Level23Logic : LevelLogic
{
    public HypoTalkBox hypoTalkBox;

    public AudioClip introClip;
    public AudioClip outroClip;
    public AudioClip correctClip;
    public AudioClip wrongClip;

    public AudioClip fruitClip;
    public AudioClip candyClip;
    public AudioClip sportsClip;
    public AudioClip tvClip;
    public AudioClip sodaClip;
    public AudioClip sleepClip;

    public Button fruitButton;
    public Button candyButton;
    public Button sportsButton;
    public Button tvButton;
    public Button sodaButton;
    public Button sleepButton;
    public Button finishButton;

    private int correctSelections = 0;
    private bool IsLoggedIn;
    private Child? child;

    async void Start()
    {
        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await GetChildInfo();

        finishButton.gameObject.SetActive(false);
        SetAllButtonsInteractable(false);

        hypoTalkBox.TriggerAnimation(introClip,
            "Gezond leven helpt om je lekker te voelen, ook met diabetes! Kies maar eens wat jij denkt dat goed voor je is. Als je alles wat gezond is hebt aangeklikt klik dan op de groene knop!");
        await WaitForLengthOfAudio(introClip);

        SetAllButtonsInteractable(true);

        fruitButton.onClick.AddListener(() =>
            OnChoice(true, fruitButton, correctClip, "Goed gedaan! Dit is een gezonde keuze.",
                     fruitClip, "Fruit eten is supergezond!"));

        candyButton.onClick.AddListener(() =>
            OnChoice(false, candyButton, wrongClip, "Oei! Dat is niet echt gezond. Let goed op!",
                     candyClip, "Snoep is lekker, maar niet te veel! Dat is ongezond."));

        sportsButton.onClick.AddListener(() =>
            OnChoice(true, sportsButton, correctClip, "Goed gedaan! Dit is een gezonde keuze.",
                     sportsClip, "Buiten spelen is goed voor je bloedsuiker!"));

        tvButton.onClick.AddListener(() =>
            OnChoice(false, tvButton, wrongClip, "Oei! Dat is niet echt gezond. Let goed op!",
                     tvClip, "Te lang tv kijken is niet zo gezond."));

        sodaButton.onClick.AddListener(() =>
            OnChoice(false, sodaButton, wrongClip, "Oei! Dat is niet echt gezond. Let goed op!",
                     sodaClip, "Frisdrank bevat veel suiker. Beter iets anders!"));

        sleepButton.onClick.AddListener(() =>
            OnChoice(true, sleepButton, correctClip, "Goed gedaan! Dit is een gezonde keuze.",
                     sleepClip, "Goed slapen is heel belangrijk voor je gezondheid!"));

        finishButton.onClick.AddListener(OnFinishClicked);
    }

    private async void OnChoice(bool isCorrect, Button button,
        AudioClip feedbackClip, string feedbackText,
        AudioClip explainClip, string explainText)
    {
        SetAllButtonsInteractable(false);

        button.gameObject.SetActive(false); // Verberg knop

        // Stap 1: Correct of fout reactie
        hypoTalkBox.TriggerAnimation(feedbackClip, feedbackText);
        await WaitForLengthOfAudio(feedbackClip);

        await Task.Delay(500); // Wacht 1 seconde

        // Stap 2: Uitleg
        hypoTalkBox.TriggerAnimation(explainClip, explainText);
        await WaitForLengthOfAudio(explainClip);

        if (isCorrect)
        {
            correctSelections++;
        }

        if (correctSelections >= 3)
        {
            SetAllButtonsInteractable(false);
            finishButton.gameObject.SetActive(true);
        }
        else
        {
            SetAllButtonsInteractable(true);
        }
    }

    private async void OnFinishClicked()
    {
        SetAllButtonsInteractable(false);
        finishButton.interactable = false;

        hypoTalkBox.TriggerAnimation(outroClip,
            "Goed bezig! Jij weet nu wat belangrijk is om gezond te blijven met diabetes. Welke sticker past bij dit level?");
        await WaitForLengthOfAudio(outroClip);

        if (!IsLoggedIn)
        {
            await stickerPopup.AskingSticker();
        }
        else
        {
            await EnterLevelCompletion(23,
                "Ik weet nu wat gezonde keuzes zijn bij diabetes.");
        }

        LeaveLevel();
    }

    private void SetAllButtonsInteractable(bool state)
    {
        fruitButton.interactable = state;
        candyButton.interactable = state;
        sportsButton.interactable = state;
        tvButton.interactable = state;
        sodaButton.interactable = state;
        sleepButton.interactable = state;
    }
}
