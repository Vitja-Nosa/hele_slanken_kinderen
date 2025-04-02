using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Level24Logic : LevelLogic
{
    public HypoTalkBox hypoTalkBox;

    public AudioClip introClip;
    public AudioClip outroClip;

    public AudioClip dietistClip;
    public AudioClip diabetescentrumClip;
    public AudioClip vervolgtrajectClip;

    public Button buttonDietist;
    public Button buttonDiabetescentrum;
    public Button buttonVervolgtraject;
    public Button buttonFinish;

    private bool clickedDietist = false;
    private bool clickedDiabetescentrum = false;
    private bool clickedVervolgtraject = false;

    private bool IsLoggedIn;
    private Child? child;

    async void Start()
    {
        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await GetChildInfo();

        buttonFinish.gameObject.SetActive(false);
        SetButtonsActive(false);

        hypoTalkBox.TriggerAnimation(introClip,
            "Je bent nu bijna aan het einde van jouw ziekenhuisavontuur. Wat heb je al veel geleerd! Maar dit is nog maar het begin. Er zijn mensen die je blijven helpen. Klik 1 voor 1 op de plaatjes om er informatie over te horen!");
        await WaitForLengthOfAudio(introClip);

        SetButtonsActive(true);

        buttonDietist.onClick.AddListener(() => OnClickInfo(buttonDietist, dietistClip, "Je weet nu dat eten heel belangrijk is. Een diëtist kan je helpen bij gezonde voeding, speciaal afgestemd op jouw lichaam.", () => clickedDietist = true));
        buttonDiabetescentrum.onClick.AddListener(() => OnClickInfo(buttonDiabetescentrum, diabetescentrumClip, "Misschien moet je ooit nog langs een diabetescentrum. Hier werken experts die je verder begeleiden en alles weten over diabetes.", () => clickedDiabetescentrum = true));
        buttonVervolgtraject.onClick.AddListener(() => OnClickInfo(buttonVervolgtraject, vervolgtrajectClip, "Soms krijg je extra afspraken of lessen om je te helpen. Dat noemen we een vervolgtraject.", () => clickedVervolgtraject = true));
        buttonFinish.onClick.AddListener(OnFinishClicked);
    }

    private async void OnClickInfo(Button button, AudioClip clip, string text, System.Action onClicked)
    {
        SetButtonsActive(false); // Alle knoppen tijdelijk uitschakelen
        onClicked.Invoke();      // Markeer deze als geklikt

        hypoTalkBox.TriggerAnimation(clip, text);
        await WaitForLengthOfAudio(clip);

        button.gameObject.SetActive(false); // Verberg deze knop

        CheckIfFinished();
        SetRemainingButtonsActive(); // Activeer alleen overgebleven knoppen
    }

    private void CheckIfFinished()
    {
        if (clickedDietist && clickedDiabetescentrum && clickedVervolgtraject)
        {
            buttonFinish.gameObject.SetActive(true);
        }
    }

    private async void OnFinishClicked()
    {
        buttonFinish.interactable = false;

        hypoTalkBox.TriggerAnimation(outroClip,
            "Jij hebt laten zien hoe sterk en slim je bent. Vergeet niet: je doet dit samen met je ouders, je dokters en iedereen om je heen. Wat een reis… Welke sticker hoort bij dit laatste level?");
        await WaitForLengthOfAudio(outroClip);

        if (!IsLoggedIn)
        {
            await stickerPopup.AskingSticker();
        }
        else
        {
            await EnterLevelCompletion(24,
                "Ik weet nu welke hulp ik nog krijg na het ziekenhuis.");
        }

        LeaveLevel();
    }

    private void SetButtonsActive(bool state)
    {
        buttonDietist.interactable = state && !clickedDietist;
        buttonDiabetescentrum.interactable = state && !clickedDiabetescentrum;
        buttonVervolgtraject.interactable = state && !clickedVervolgtraject;
    }

    private void SetRemainingButtonsActive()
    {
        if (!clickedDietist) buttonDietist.interactable = true;
        if (!clickedDiabetescentrum) buttonDiabetescentrum.interactable = true;
        if (!clickedVervolgtraject) buttonVervolgtraject.interactable = true;
    }
}
