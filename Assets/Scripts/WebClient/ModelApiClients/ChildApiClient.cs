using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ChildApiClient : ApiClient
{
    public override string Route => "/api/child";

    public async Awaitable<IWebRequestReponse> GetChild()
    {
        IWebRequestReponse webRequestResponse = await WebClient.instance.SendGetRequest(Route);
        return ParseResponse<Child>(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreateChild(Child child)
    {
        string data = JsonUtility.ToJson(child);

        IWebRequestReponse webRequestResponse = await WebClient.instance.SendPostRequest(Route, data);
        return ParseResponse<Child>(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> UpdateChild(Child child)
    {
        string data = JsonUtility.ToJson(child);

        IWebRequestReponse webRequestResponse = await WebClient.instance.SendPutRequest(Route, data);
        return ParseResponse<Child>(webRequestResponse);
    }
}