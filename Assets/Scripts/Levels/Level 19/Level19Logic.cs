using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Level19Logic : LevelLogic
{
    public HypoTalkBox hypoTalkBox;

    public GameObject child6;
    public GameObject child8;
    public GameObject child10;
    public GameObject child12;

    public Button buttonFinish;

    public AudioClip introClip;
    public AudioClip[] childClips; // 0 = 6 jaar, 1 = 8 jaar, 2 = 10 jaar, 3 = 12 jaar
    public string[] childTexts;    // Teksten per kind
    public AudioClip outroClip;

    private int currentIndex = 0;
    private GameObject[] children;
    private bool IsLoggedIn;
    private Child? child;

    async void Start()
    {
        IsLoggedIn = LevelSetup.LoggedIn;
        if (IsLoggedIn)
            child = await GetChildInfo();

        // Alle kinderen uit
        child6.SetActive(false);
        child8.SetActive(false);
        child10.SetActive(false);
        child12.SetActive(false);
        buttonFinish.gameObject.SetActive(false);

        children = new GameObject[] { child6, child8, child10, child12 };

        // Start met intro
        hypoTalkBox.TriggerAnimation(introClip,
            "Als je groeit, verandert je lichaam én je leven. Daarom verandert je behandeling soms mee. Klik op de kinderen om te zien wat er in elke fase gebeurt!");
        await WaitForLengthOfAudio(introClip);

        ShowChild(currentIndex);
    }

    private void ShowChild(int index)
    {
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetActive(i == index);
        }

        // Voeg click event toe aan actieve kind
        Button childButton = children[index].GetComponent<Button>();
        childButton.onClick.RemoveAllListeners();
        childButton.onClick.AddListener(() => OnChildClicked(index));
    }

    private async void OnChildClicked(int index)
    {
        hypoTalkBox.TriggerAnimation(childClips[index], childTexts[index]);
        await WaitForLengthOfAudio(childClips[index]);

        children[index].SetActive(false);
        currentIndex++;

        if (currentIndex < children.Length)
        {
            ShowChild(currentIndex);
        }
        else
        {
            buttonFinish.gameObject.SetActive(true);
            buttonFinish.onClick.RemoveAllListeners();
            buttonFinish.onClick.AddListener(OnFinishClicked);
        }
    }

    private async void OnFinishClicked()
    {
        buttonFinish.interactable = false;

        hypoTalkBox.TriggerAnimation(outroClip,
            "Top gedaan! Jij weet nu dat je behandeling mee verandert met jou! Welke sticker hoort bij dit level?");
        await WaitForLengthOfAudio(outroClip);

        if (!IsLoggedIn)
        {
            await stickerPopup.AskingSticker();
        }
        else
        {
            await EnterLevelCompletion(19,
                "Ik weet nu dat mijn behandeling verandert als ik ouder word. De dokter helpt mee.");
        }

        LeaveLevel();
    }
}
