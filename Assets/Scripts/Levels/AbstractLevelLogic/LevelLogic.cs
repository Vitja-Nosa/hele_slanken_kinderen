using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public abstract class LevelLogic : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public StickerPopup stickerPopup;


    // ModelAPI client
    public ChildApiClient childApiClient;
    public DiaryEntryApiClient diaryEntryApiClient;
    public ChildLevelCompletionApiClient childLevelCompletionApiClient;

    public async Awaitable<Child?> GetChildInfo()
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

    public async Awaitable EnterLevelCompletion(int levelId, string diaryContent)
    {
        await PostLevelCompletion(levelId);
        await PostDiaryEntry(new DiaryEntry
        {
            childId = "",
            id = "",
            content = diaryContent,
            stickerId = await stickerPopup.AskingSticker(),
            date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
        });

        if (StatusManager.Instance != null)
            StatusManager.Instance.CompleteLevel(levelId);
    }
    private async Awaitable PostDiaryEntry(DiaryEntry diaryEntry)
    {
        try { await diaryEntryApiClient.CreateDiaryEntry(diaryEntry); }
        catch (Exception ex) { throw new Exception("Error while creating diary entry:" + ex.Message); }
    }

    private async Awaitable PostLevelCompletion(int levelId)
    {
        try { await childLevelCompletionApiClient.RecordLevelCompletion(levelId); }
        catch (Exception ex) { throw new Exception("Error while creating level completion:" + ex.Message); }
    }

    public async Task WaitForLengthOfAudio(AudioClip clip)
    {
        float waitTime = clip.length * 1000;
        await Task.Delay((int)waitTime);
    }

    protected void LeaveLevel()
    {
        if (LevelSetup.LoggedIn)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("OverworldScene");
        }
        else
            SceneManager.LoadScene("LevelSelectScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = activeScene.GetRootGameObjects();

        foreach (GameObject obj in rootObjects)
        {
            if (obj.name == "LevelCompleted")
            {
                obj.SetActive(true);
                break;
            }
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
