using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GuardianApiClient : ApiClient
{
    public override string Route => "/api/guardians";

    public async Awaitable<IWebRequestReponse> GetGuardian()
    {
        IWebRequestReponse webRequestResponse = await WebClient.instance.SendGetRequest(Route);
        return ParseResponse<Guardian>(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreateGuardian(Guardian guardian)
    {
        string data = JsonUtility.ToJson(guardian);
        IWebRequestReponse webRequestResponse = await WebClient.instance.SendPostRequest(Route, data);
        return ParseResponse<Guardian>(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> UpdateGuardian(Guardian guardian)
    {
        string data = JsonUtility.ToJson(guardian);
        IWebRequestReponse webRequestResponse = await WebClient.instance.SendPutRequest(Route, data);
        return ParseResponse<Guardian>(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeleteGuardian(Guardian guardian)
    {
        return await WebClient.instance.SendDeleteRequest(Route + $"/{guardian.id}");
    }
}
