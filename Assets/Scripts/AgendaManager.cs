using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AgendaManager : MonoBehaviour
{
    public Transform afsprakenContainer; // Dit is "Content"
    public GameObject afspraakPrefab; // Dit is de prefab van een afspraak
    public RectTransform contentRect; // Dit is de RectTransform van "Content"

    void Start()
    {
        GenerateAfspraken();
        AdjustContentHeight(); // Fix scrolling probleem
    }

    public PopUpManager popUpManager; // Sleep deze in Unity in de Inspector!

    void GenerateAfspraken()
    {
        string[,] afspraken = {
        {"Dr. Jansen", "15 maart", "Controle", "Jaarlijkse controle bij de kinderarts"},
        {"Dr. De Vries", "22 maart", "Bloedonderzoek", "Routine bloedglucosemeting"},
        {"Dr. Van Dijk", "3 april", "Consult", "Bespreking van medicatieaanpassingen"},
        {"Dr. Bakker", "10 april", "Voedingsadvies", "Advies over gezonde voeding en diabetes"},
        {"Dr. Vermeer", "18 april", "Controle", "Endocrinologische controle"},
    };

        for (int i = 0; i < afspraken.GetLength(0); i++)
        {
            GameObject nieuweAfspraak = Instantiate(afspraakPrefab, afsprakenContainer);
            TextMeshProUGUI afspraakText = nieuweAfspraak.transform.Find("Button/AfspraakTekst").GetComponent<TextMeshProUGUI>();
            afspraakText.text = $"{i + 1}. {afspraken[i, 2]} - {afspraken[i, 1]}";

            Button button = nieuweAfspraak.GetComponentInChildren<Button>();
            int index = i;
            button.onClick.AddListener(() => popUpManager.ToonAfspraak(
                afspraken[index, 0], afspraken[index, 1], afspraken[index, 2], afspraken[index, 3]
            ));
        }
    }


    void AdjustContentHeight()
    {
        // Tel het aantal afspraken
        int aantal = afsprakenContainer.childCount;

        // Hoogte per prefab-item (stel zelf in, bv. 100 of 120 afhankelijk van jouw design)
        float itemHeight = 120f;
        float spacing = 10f; // komt overeen met je Vertical Layout Group spacing

        // Bereken nieuwe hoogte
        float nieuweHoogte = (aantal * itemHeight) + ((aantal - 1) * spacing);

        // Zet de hoogte van de Content container
        RectTransform contentRT = afsprakenContainer.GetComponent<RectTransform>();
        contentRT.sizeDelta = new Vector2(contentRT.sizeDelta.x, nieuweHoogte);
    }

}
