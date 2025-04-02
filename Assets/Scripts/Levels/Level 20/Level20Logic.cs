using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Level20Logic : LevelLogic
{
    public HypoTalkBox hypoTalkBox;

    public Button buttonSad;
    public Button buttonLittleSad;
    public Button buttonNeutral;
    public Button buttonHappy;
    public Button buttonSuperHappy;
    public Button buttonFinish;

    public AudioClip introClip;
    public AudioClip emotionClip;
    public AudioClip outroClip;
    public AudioClip supportClip;

    private bool IsLoggedIn;
    private Child? child;

    async void Start()
    {
        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await GetChildInfo();

        HideEmotionButtons();
        buttonFinish.gameObject.SetActive(false);

        hypoTalkBox.TriggerAnimation(introClip,
            "Je bent al heel ver gekomen. Ik ben trots op je! Weet je... het is oké om je soms een beetje gek te voelen. Hoe gaat het met je? Klik op de emoji die op dit moment het beste bij jouw gevoel past!");
        await WaitForLengthOfAudio(introClip);

        ShowEmotionButtons();
    }

    private void ShowEmotionButtons()
    {
        buttonSad.gameObject.SetActive(true);
        buttonLittleSad.gameObject.SetActive(true);
        buttonNeutral.gameObject.SetActive(true);
        buttonHappy.gameObject.SetActive(true);
        buttonSuperHappy.gameObject.SetActive(true);

        buttonSad.onClick.AddListener(OnEmotionClicked);
        buttonLittleSad.onClick.AddListener(OnEmotionClicked);
        buttonNeutral.onClick.AddListener(OnEmotionClicked);
        buttonHappy.onClick.AddListener(OnEmotionClicked);
        buttonSuperHappy.onClick.AddListener(OnEmotionClicked);
    }

    private void HideEmotionButtons()
    {
        buttonSad.gameObject.SetActive(false);
        buttonLittleSad.gameObject.SetActive(false);
        buttonNeutral.gameObject.SetActive(false);
        buttonHappy.gameObject.SetActive(false);
        buttonSuperHappy.gameObject.SetActive(false);
    }

    private async void OnEmotionClicked()
    {
        HideEmotionButtons();

        hypoTalkBox.TriggerAnimation(emotionClip, "Fijn dat je dat deelt. Het is goed om te praten over je gevoel. Het is niet altijd makkelijk om diabetes te hebben. Soms wil je gewoon even geen gedoe. Dat mag. Je hoeft niet altijd sterk te zijn. Als je je verdrietig voelt, of boos, of in de war — dan is dat helemaal oké.");
        await WaitForLengthOfAudio(emotionClip);

        hypoTalkBox.TriggerAnimation(supportClip, "Iedereen heeft zulke momenten. Wat belangrijk is, is dat je erover praat. Je ouders, de dokter, of iemand op school kan je helpen. Je bent niet alleen. En weet je? Je doet het al supergoed. Echt waar. Je mag trots zijn op jezelf.");
        await WaitForLengthOfAudio(supportClip);

        buttonFinish.gameObject.SetActive(true);
        buttonFinish.onClick.RemoveAllListeners();
        buttonFinish.onClick.AddListener(OnFinishClicked);
    }

    private async void OnFinishClicked()
    {
        buttonFinish.gameObject.SetActive(false);

        hypoTalkBox.TriggerAnimation(outroClip,
            "Goed gedaan! Onthoud dat je altijd mag zeggen hoe je je voelt. Je staat er niet alleen voor. Welke sticker vind je dat bij dit level past?");
        await WaitForLengthOfAudio(outroClip);

        if (!IsLoggedIn)
        {
            await stickerPopup.AskingSticker();
        }
        else
        {
            await EnterLevelCompletion(20,
                "Ik weet nu dat ik altijd mag zeggen hoe ik me voel. Dat is belangrijk.");
        }

        LeaveLevel();
    }
}
