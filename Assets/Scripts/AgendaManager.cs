using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

public class AgendaManager : MonoBehaviour
{
    public Transform afsprakenContainer;
    public GameObject afspraakPrefab;
    public RectTransform contentRect;
    public PopUpManager popUpManager;
    public AppointmentsApiClient appointmentsApiClient;

    private async void Start()
    {
        await LoadAfsprakenFromAPI();
        AdjustContentHeight();
    }

    private async Task LoadAfsprakenFromAPI()
    {
        IWebRequestReponse response = await appointmentsApiClient.ReadAllAppointments();

        switch (response)
        {
            case WebRequestError error:
                Debug.LogError("Fout bij ophalen afspraken: " + error.ErrorMessage);
                break;

            case WebRequestData<List<Appointment>> data:
                foreach (Transform child in afsprakenContainer)
                {
                    Destroy(child.gameObject);
                }

                var afspraken = data.Data;

                for (int i = 0; i < afspraken.Count; i++)
                {
                    Appointment afspraak = afspraken[i];
                    Debug.Log($"Ruwe datum string: {afspraak.date}");

                    DateTime parsedDate;
                    string formattedDate = "Onbekend";

                    // Probeer het ISO-formaat expliciet te parsen
                    if (DateTime.TryParseExact(
                            afspraak.date,
                            "yyyy-MM-ddTHH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None,
                            out parsedDate))
                    {
                        formattedDate = parsedDate.ToString("dd-MM-yyyy");
                    }


                    GameObject nieuweAfspraak = Instantiate(afspraakPrefab, afsprakenContainer);
                    TextMeshProUGUI afspraakText = nieuweAfspraak.transform.Find("Button/AfspraakTekst").GetComponent<TextMeshProUGUI>();
                    afspraakText.text = $"{i + 1}. {afspraak.appointmentName} - {formattedDate}";

                    Button button = nieuweAfspraak.GetComponentInChildren<Button>();
                    int index = i;

                    button.onClick.AddListener(() => popUpManager.ToonAfspraak(
                        afspraak.doctorName,
                        formattedDate,
                        afspraak.appointmentName,
                        string.IsNullOrEmpty(afspraak.description) ? "Geen beschrijving beschikbaar." : afspraak.description
                    ));
                }

                break;

            default:
                Debug.LogError("Onbekend antwoordtype ontvangen: " + response?.GetType().Name);
                break;
        }
    }

    void AdjustContentHeight()
    {
        int aantalAfspraken = afsprakenContainer.childCount;

        if (aantalAfspraken == 0)
        {
            contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 600f);
            return;
        }

        float itemHoogte = ((RectTransform)afsprakenContainer.GetChild(0)).rect.height;
        float spacing = 10f;
        float totaleHoogte = (aantalAfspraken * itemHoogte) + ((aantalAfspraken - 1) * spacing);
        float minimumHoogte = 600f;

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, Mathf.Max(totaleHoogte, minimumHoogte));
    }
}
