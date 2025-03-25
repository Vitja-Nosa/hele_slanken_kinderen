using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Globalization;
using System.Linq;
using UnityEngine.UI;

public class DiaryEntriesLoader : MonoBehaviour
{
    public TMP_Text DateDisplay;
    public DiaryEntryApiClient diaryEntryApiClient;
    public DiaryEntryCard[] cards;
    private List<List<DiaryEntry>> sortedDiaryEntries = new();
    private int currentPageIndex = 0;

    public Button PageLeftButton;
    public Button PageRightButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        sortedDiaryEntries = await RequestDiaryEntries();
        if (sortedDiaryEntries.Count > 0)
        {
            LoadPage(sortedDiaryEntries.Count - 1); // Start at the last page
        }
        UpdatePageButtons();
    }

    public void PageRight()
    {
        if (currentPageIndex < sortedDiaryEntries.Count - 1)
        {
            LoadPage(currentPageIndex + 1);
        }
        UpdatePageButtons();
    }

    public void PageLeft()
    {
        if (currentPageIndex > 0)
        {
            LoadPage(currentPageIndex - 1);
        }
        UpdatePageButtons();
    }

    // Request and sort all diary entries
    public async Awaitable<List<List<DiaryEntry>>> RequestDiaryEntries()
    {
        // Create WebRequest
        IWebRequestReponse response = await diaryEntryApiClient.GetDiaryEntries();
        List<DiaryEntry> entries = new();

        // Check if response is valid 
        switch (response)
        {
            case WebRequestData<List<DiaryEntry>> webResponse:
                entries = webResponse.Data;
                break;
            case WebRequestError error:
                Debug.Log(error.ErrorMessage);
                break;
            default:
                Debug.Log("Could not parse the web response");
                break;
        }

        // Sort the response
        List<List<DiaryEntry>> sortedEntries = entries
            .GroupBy(e => DateTime.Parse(e.date, CultureInfo.InvariantCulture).Date) // Group by date
            .OrderBy(g => g.Key) // Sort by date ascending
            .Select(g => g.OrderBy(e => DateTime.Parse(e.date, CultureInfo.InvariantCulture)).ToList()) // Sort entries in each group by full datetime
            .ToList();

        return sortedEntries;
    }

    private void LoadPage(int pageIndex)
    {
        if (sortedDiaryEntries.Count == 0 || pageIndex < 0 || pageIndex >= sortedDiaryEntries.Count)
            return;

        currentPageIndex = pageIndex;
        List<DiaryEntry> entries = sortedDiaryEntries[pageIndex];

        // Use the separate method to format the date
        DateDisplay.text = FormatDateInDutch(entries[0].date);

        // Fill cards with entries
        for (int i = 0; i < cards.Length; i++)
        {
            if (i < entries.Count)
            {
                Debug.Log(entries[i].stickerId);
                cards[i].LoadDiaryEntry(entries[i]);
                cards[i].gameObject.SetActive(true);
            }
            else
            {
                cards[i].gameObject.SetActive(false);
            }
        }

        UpdatePageButtons();
    }

    private void UpdatePageButtons()
    {
        PageLeftButton.gameObject.SetActive(CanGoLeft());
        PageRightButton.gameObject.SetActive(CanGoRight());
    }

    public bool CanGoLeft() => currentPageIndex > 0;
    public bool CanGoRight() => currentPageIndex < sortedDiaryEntries.Count - 1;

    private string FormatDateInDutch(string dateString)
    {
        DateTime date = DateTime.Parse(dateString, CultureInfo.InvariantCulture);
        CultureInfo dutchCulture = new CultureInfo("nl-NL");
        string formattedDate = date.ToString("dddd dd MMMM yyyy", dutchCulture);

        return char.ToUpper(formattedDate[0]) + formattedDate.Substring(1); // Capitalize first letter
    }
}
