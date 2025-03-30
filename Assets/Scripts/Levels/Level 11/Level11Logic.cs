using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Level11Logic : LevelLogic
{
    public Button buttonWhyReturn;
    public Button buttonWhatDoctorDoes;
    public Button buttonHowDoctorKnows;
    public Button buttonFinish;

    public HypoTalkBox hypoTalkBox;
    public AudioClip[] audioClips; // [0] = knop 1, [1] = knop 2, [2] = knop 3
    public AudioClip introClip;
    public AudioClip outroClip;

    private bool IsLoggedIn;
    private Child? child;

    async void Start()
    {
        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await GetChildInfo();

        DisableAllButtons();

        // Start met intro
        hypoTalkBox.TriggerAnimation(introClip,
            "Je krijgt een vervolgafspraak bij de kinderarts of diabetesverpleegkundige. Zo kunnen zij zien hoe het gaat met prikken, eten en je bloedsuiker. Klik één voor één op de knoppen 1 tot en met 3.");
        await WaitForLengthOfAudio(introClip);

        EnableAllButtons();

        // Voeg click-events toe
        buttonWhyReturn.onClick.AddListener(OnWhyReturnClicked);
        buttonWhatDoctorDoes.onClick.AddListener(OnWhatDoctorDoesClicked);
        buttonHowDoctorKnows.onClick.AddListener(OnHowDoctorKnowsClicked);
        buttonFinish.onClick.AddListener(OnFinishClicked);
    }

    private void DisableAllButtons()
    {
        buttonWhyReturn.interactable = false;
        buttonWhatDoctorDoes.interactable = false;
        buttonHowDoctorKnows.interactable = false;
        buttonFinish.interactable = false;
    }

    private void EnableAllButtons()
    {
        buttonWhyReturn.interactable = true;
        buttonWhatDoctorDoes.interactable = true;
        buttonHowDoctorKnows.interactable = true;
        buttonFinish.interactable = true;
    }

    private void OnWhyReturnClicked()
    {
        hypoTalkBox.TriggerAnimation(audioClips[0],
            "Je moet soms terug naar het ziekenhuis, maar hoezo? Je komt af en toe terug naar het ziekenhuis zodat de dokter kan kijken of alles goed gaat. Zo blijf jij je goed voelen!");
    }

    private void OnWhatDoctorDoesClicked()
    {
        hypoTalkBox.TriggerAnimation(audioClips[1],
            "De dokter gaat allemaal dingen controleren, maar wat precies? De dokter kijkt naar je bloedsuiker, hoe het gaat met spuiten en eten. Je kunt vragen stellen en samen kijken jullie hoe het gaat.");
    }

    private void OnHowDoctorKnowsClicked()
    {
        hypoTalkBox.TriggerAnimation(audioClips[2],
            "De dokter gaat er achter komen of het goed met je gaat, maar hoe weet hij dit? Je vertelt hoe jij je voelt en wat je gemeten hebt. De dokter kijkt naar je dagboekje en praat met jou en je ouders. Daardoor weet de dokter dat het goed met je gaat! Klik op de groene knop als je dit begrijpt!");
    }

    private async void OnFinishClicked()
    {
        await EndOfLevel();
    }

    private async Task EndOfLevel()
    {
        DisableAllButtons();

        hypoTalkBox.TriggerAnimation(outroClip,
            "Goed gedaan! Je weet nu dat je af en toe terugkomt naar het ziekenhuis om alles goed te houden.");
        await WaitForLengthOfAudio(outroClip);

        if (!IsLoggedIn)
        {
            await stickerPopup.AskingSticker();
        }
        else
        {
            await EnterLevelCompletion(11,
                "Ik weet nu dat ik soms terug moet naar de dokter om te kijken hoe het gaat. Dat voelt fijn en veilig.");
        }

        LeaveLevel();
    }
}
