using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TMP_SpriteDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public List<Sprite> Stickers; // Lijst met stickers (sprites)
    public Image dropdownButtonImage; // De Image van de dropdown-knop
    public Sprite emptySprite; // Optioneel: een "leeg" plaatje als standaard
    public TMP_InputField Content;
    public DiaryEntryApiClient diaryEntryApiClient;
    public DiaryEntriesLoader diaryEntriesLoader;



    void Start()
    {
        UpdateDropdownOptions();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        // Standaard "LEEG" selecteren en afbeelding weghalen
        dropdown.value = 0;
        dropdownButtonImage.sprite = emptySprite;
    }

    void UpdateDropdownOptions()
    {
        dropdown.options.Clear(); // Oude opties verwijderen

        // Eerste optie: "LEEG"
        TMP_Dropdown.OptionData emptyOption = new TMP_Dropdown.OptionData();
        emptyOption.text = "LEEG"; // Tekst tonen in dropdown
        emptyOption.image = emptySprite; // Geen afbeelding of een lege afbeelding
        dropdown.options.Add(emptyOption);

        // Toevoegen van stickers als opties
        foreach (var sticker in Stickers)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.image = sticker;
            dropdown.options.Add(option);
        }

        // Extra lege optie aan het einde
        TMP_Dropdown.OptionData lastEmptyOption = new TMP_Dropdown.OptionData();
        lastEmptyOption.text = "";
        lastEmptyOption.image = emptySprite;
        dropdown.options.Add(lastEmptyOption);

        dropdown.RefreshShownValue(); // Dropdown vernieuwen
    }

    void OnDropdownValueChanged(int index)
    {
        if (index == 0) // Als eerste "LEEG" is geselecteerd
        {
            dropdownButtonImage.sprite = emptySprite; // Geen afbeelding tonen
        }
        else if (index == dropdown.options.Count - 1) // Als laatste "LEEG" is geselecteerd
        {
            dropdownButtonImage.sprite = Stickers.Count > 0 ? Stickers[Stickers.Count - 1] : emptySprite;
        }
        else if (index - 1 < Stickers.Count) // Corrigeer index omdat "LEEG" een extra optie is
        {
            dropdownButtonImage.sprite = Stickers[index - 1]; // Toon gekozen sticker
        }
    }

    public void CloseCreator()
    {
        gameObject.SetActive(false);
    }

    public void OpenCreator()
    {
        gameObject.SetActive(true);
    }

    public async void Create()
    {
        DiaryEntry newEntry = new()
        {
            id = "",
            childId = "",
            content = Content.text,
            stickerId = dropdown.value,
            date = DateTime.Now.ToString()
        };

        await diaryEntryApiClient.CreateDiaryEntry(newEntry);
        await diaryEntriesLoader.RequestDiaryEntries();

        CloseCreator();
    }
}
