using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ChildLevelCompletionApiClient : ApiClient
{
    public override string Route => "/api/child/levels";

    public async Awaitable<IWebRequestReponse> GetAllLevelCompletions()
    {
        IWebRequestReponse webRequestResponse = await WebClient.instance.SendGetRequest(Route);
        return ParseResponse<ChildLevelCompletion>(webRequestResponse, true);
    }

    public async Awaitable<IWebRequestReponse> GetLevelCompletion(int levelId)
    {
        IWebRequestReponse webRequestResponse = await WebClient.instance.SendGetRequest(Route + $"/{levelId}");
        return ParseResponse<ChildLevelCompletion>(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> RecordLevelCompletion(int levelId, int stickerId = 0)
    {
        return await WebClient.instance.SendPostRequest(Route + $"/{levelId}" + $"/{stickerId}");
    }

    public async Awaitable<IWebRequestReponse> DeleteLevelCompletion(int levelId)
    {
        return await WebClient.instance.SendDeleteRequest(Route + $"/{levelId}");
    }
}   