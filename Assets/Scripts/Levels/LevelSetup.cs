using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class LevelSetup
{
    public static bool LoggedIn = false;

    public static async Awaitable<Child?> GetChildInfo(ChildApiClient childApiClient)
    {
        IWebRequestReponse response = await childApiClient.GetChild();

        switch (response)
        {
            case WebRequestData<Child> data:
                return data.Data;
            case WebRequestError error:
                Debug.Log("WebRequest Error:" + error.ErrorMessage);
                return null;
            default:
                Debug.Log("Can not parse web response to Child object");
                return null;
        }
    }

    public static async Awaitable PostDiaryEntry(DiaryEntry diaryEntry, DiaryEntryApiClient diaryEntryApiClient)
    {
        try { await diaryEntryApiClient.CreateDiaryEntry(diaryEntry); }
        catch(Exception ex) { throw new Exception("Error while creating diary entry:" + ex.Message); }
    }

    public static async Awaitable PostLevelCompletion(int levelId, ChildLevelCompletionApiClient childLevelCompletionApiClient)
    {
        try { await childLevelCompletionApiClient.RecordLevelCompletion(levelId); }
        catch (Exception ex) { throw new Exception("Error while creating level completion:" + ex.Message); }
    }

    public static async Task WaitForLengthOfAudio(AudioClip clip)
    {
        float waitTime = clip.length * 1000;
        await Task.Delay((int)waitTime);
    }
}
