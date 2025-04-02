using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Level12Logic : LevelLogic
{
    // Objects in scene
    public HypoTalkBox hypoTalkBox;
    public Button[] QuestionButtons;

    // audio file for hypo
    public AudioClip[] audioClips;
    public AudioClip[] audioAnswers;
    private string[] stringAnwers = new string[]
    {
        "De dokter vindt dat je extra zorg nodig hebt om beter te worden. In het ziekenhuis kunnen ze je goed in de gaten houden.",
        "Als je bloedsuiker te hoog is en je erg ziek of moe bent, kan opname nodig zijn. Dit helpt om je snel beter te maken.",
        "Je krijgt medicijnen en vocht via een infuus om je bloedsuiker te stabiliseren. Dokters en verpleegkundigen zorgen goed voor je.",
        "Als je bloedsuiker weer normaal is en je je beter voelt, mag je naar huis. De dokter geeft je ouders een plan mee om je verder te helpen."
    };
    private bool[] usedButton = new bool[] { false, false, false, false };

    private bool IsLoggedIn;
    private Child? child = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        SetButtonsActive(false);

        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await base.GetChildInfo();

        await StartOfLevel();
    }

    private async Task StartOfLevel()
    {
        // Message 0
        await PlayMessage(0, "We gaan het hebben over wanneer je naar het ziekenhuis moet!");

        // Message 1
        await PlayMessage(1, "Soms, als je bloedsuiker te hoog is of als je je echt heel erg ziek voelt, kan de dokter zeggen dat je naar het ziekenhuis moet.");

        // Message 2
        await PlayMessage(2, "Daar kunnen ze je helpen om beter te worden. We gaan nu ontdekken wat er allemaal kan gebeuren als je in het ziekenhuis moet blijven.");

        // Message 3
        await PlayMessage(3, "Druk op de knoppen om meer te leren over wanneer je misschien opgenomen moet worden in het ziekenhuis!");

        SetButtonsActive(true);
    }

    private async Task EndOfLevel()
    {
        // Message 4
        await PlayMessage(4, "Zo te zien heb je alle mogelijke vragen gesteld.");

        // Message 5
        await PlayMessage(5, "Goed gedaan! Nu weet je meer over waarom het soms nodig is om naar het ziekenhuis te gaan, en hoe ze je daar helpen om beter te worden.");

        // Message 6
        await PlayMessage(6, "Wat voor sticker vind je hierbij passen?");

        // If user is not logged in ask for sticker
        if (!LevelSetup.LoggedIn)
        {
            await base.stickerPopup.AskingSticker();
        }
        else
        {
            await base.EnterLevelCompletion(12, "Vandaag heb ik bekeken wat Een 'Consultatie van specialist' inhoud!");
        }

        // method to leave the level
        base.LeaveLevel();
    }

    // Method to activate QuestionHandler in inspector in unity
    public void OnQuestionClick(int index)
    {
        QuestionHandler(index);
    }

    public async Task QuestionHandler(int index)
    {
        usedButton[index] = true;
        bool allDone = true;

        SetButtonsInteractable(false);

        hypoTalkBox.TriggerAnimation(audioAnswers[index], stringAnwers[index]);
        await base.WaitForLengthOfAudio(audioAnswers[index]);

        for (int i = 0; i < 4; i++)
        {
            if (!usedButton[i])
            {
                QuestionButtons[i].interactable = true;
                allDone = false;
            }
        }

        if (allDone)
        {
            await EndOfLevel();
        }
    }

    private void SetButtonsInteractable(bool interactable)
    {
        foreach (Button button in QuestionButtons)
        {
            button.interactable = interactable;
        }
    }

    private void SetButtonsActive(bool active)
    {
        foreach (Button button in QuestionButtons)
        {
            button.gameObject.SetActive(active);
        }
    }

    private async Task PlayMessage(int clipIndex, string message)
    {
        hypoTalkBox.TriggerAnimation(audioClips[clipIndex], message);
        await base.WaitForLengthOfAudio(audioClips[clipIndex]);
        await Task.Delay(300);
    }
}
