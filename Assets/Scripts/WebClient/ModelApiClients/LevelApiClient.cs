using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class LevelApiClient : ApiClient
{
    public override string Route => "/api/levels";

    public async Awaitable<IWebRequestReponse> GetAllLevels()
    {
        IWebRequestReponse webRequestResponse = await WebClient.instance.SendGetRequest(Route);
        return ParseResponse<List<Level>>(webRequestResponse, true);
    }

    public async Awaitable<IWebRequestReponse> GetLevelById(int levelId)
    {
        IWebRequestReponse webRequestResponse = await WebClient.instance.SendGetRequest(Route + $"/{levelId}");
        return ParseResponse<Level>(webRequestResponse);
    }
}