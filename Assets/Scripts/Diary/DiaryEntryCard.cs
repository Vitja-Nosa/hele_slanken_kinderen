using System;
using TMPro;
using UnityEngine;

public class DiaryEntryCard : MonoBehaviour
{
    public TMP_Text Content;
    public TMP_Text Time;
    public StickerRenderer sticker;

    public void LoadDiaryEntry(DiaryEntry diaryEntry)
    {
        Content.text = diaryEntry.content;

        // Parse date string into DateTime and extract the time in 24-hour format
        if (DateTime.TryParse(diaryEntry.date, out DateTime parsedDate))
        {
            Time.text = parsedDate.ToString("HH:mm"); // 24-hour format (e.g., 14:45)
        }
        else
        {
            Debug.LogError("Invalid date format: " + diaryEntry.date);
            Time.text = "Invalid Date";
        }

        Debug.Log(diaryEntry.stickerId);

        if (diaryEntry != null && diaryEntry.stickerId > 0)
            sticker.SetSticker((int)diaryEntry.stickerId);
        else sticker.SetEmpty();
    }
}
