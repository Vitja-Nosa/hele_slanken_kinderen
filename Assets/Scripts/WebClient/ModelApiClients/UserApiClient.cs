using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class UserApiClient : ApiClient
{
    public override string Route => "/api/account/";

    public async Awaitable<IWebRequestReponse> Register(User user)
    {
        string data = JsonUtility.ToJson(user);

        Debug.Log(data);
        return await WebClient.instance.SendPostRequest(Route + "register", data);
    }

    public async Awaitable<IWebRequestReponse> Login(User user)
    {
        string data = JsonUtility.ToJson(user);

        IWebRequestReponse response = await WebClient.instance.SendPostRequest(Route + "login", data);
        return ProcessLoginResponse(response);

    }

    private IWebRequestReponse ProcessLoginResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                string token = JsonHelper.ExtractToken(data.Data);
                WebClient.instance.SetToken(token);
                return new WebRequestData<string>("Succes");
            default:
                return webRequestResponse;
        }
    }
}