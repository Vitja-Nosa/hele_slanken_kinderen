using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class DiaryEntryApiClient : ApiClient
{
    public override string Route => "/api/diary";

    public async Awaitable<IWebRequestReponse> GetDiaryEntries()
    {
        IWebRequestReponse webRequestResponse = await WebClient.instance.SendGetRequest(Route);
        return ParseResponse<DiaryEntry>(webRequestResponse, true);
    }

    public async Awaitable<IWebRequestReponse> CreateDiaryEntry(DiaryEntry diaryEntry)
    {
        string data = JsonUtility.ToJson(diaryEntry);

        IWebRequestReponse webRequestResponse = await WebClient.instance.SendPostRequest(Route, data);
        return ParseResponse<DiaryEntry>(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> UpdateDiaryEntry(DiaryEntry diaryEntry)
    {
        string data = JsonUtility.ToJson(diaryEntry);

        IWebRequestReponse webRequestResponse = await WebClient.instance.SendPutRequest(Route + $"/{diaryEntry.id}", data);
        return ParseResponse<DiaryEntry>(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeleteDiaryEntry(string diaryEntryId)
    {
        return await WebClient.instance.SendDeleteRequest(Route + $"/{diaryEntryId}");
    }
}