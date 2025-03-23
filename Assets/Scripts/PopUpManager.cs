using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    public GameObject popUpPanel;
    public TextMeshProUGUI dokterNaamTekst;
    public TextMeshProUGUI afspraakDatumTekst;
    public TextMeshProUGUI afspraakNaamTekst;
    public TextMeshProUGUI afspraakBeschrijvingTekst;

    public void ToonAfspraak(string dokter, string datum, string naam, string beschrijving)
    {
        popUpPanel.SetActive(true);
        afspraakNaamTekst.text = $"{naam}";
        dokterNaamTekst.text = $"Dokter: {dokter}";
        afspraakDatumTekst.text = $"Datum: {datum}";
        afspraakBeschrijvingTekst.text = $"{beschrijving}";
    }

    public void SluitPopUp()
    {
        popUpPanel.SetActive(false);
    }
}
