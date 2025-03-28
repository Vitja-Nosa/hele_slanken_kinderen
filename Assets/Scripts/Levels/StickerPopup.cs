using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StickerPopup : MonoBehaviour
{
    public GameObject PopUp;

    private bool stickerSubmitted = false;
    private int selectedStickerIndex = 0;

    public void Start()
    {
        PopUp.SetActive(false); // Hide popup initially
    }

    public void SubmitSticker(int selectedSticker)
    {
        selectedStickerIndex = selectedSticker;
        PopUp.SetActive(false); // Hide popup
        stickerSubmitted = true;
    }

    public async Task<int> AskingSticker()
    {
        PopUp.SetActive(true); // Show popup
        stickerSubmitted = false;
        selectedStickerIndex = 0;

        while (!stickerSubmitted)
        {
            await Task.Yield(); // Wait until user submits a selection
        }

        return selectedStickerIndex;
    }
}
