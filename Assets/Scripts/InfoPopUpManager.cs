using UnityEngine;

public class InfoPopUpManager : MonoBehaviour
{
    public void InfoPopUp()
    {
        this.gameObject.SetActive(true);
    }

    public void ClosePopUp()
    {
        this.gameObject.SetActive(false);
    }
}
