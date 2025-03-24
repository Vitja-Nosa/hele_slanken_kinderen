using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class StickerApiClient : ApiClient
{
    public override string Route => "/api/stickers";

    public async Awaitable<IWebRequestReponse> GetAllStickers()
    {
        IWebRequestReponse webRequestResponse = await WebClient.instance.SendGetRequest(Route);
        return ParseResponse<List<Sticker>>(webRequestResponse, true);
    }

    public async Awaitable<IWebRequestReponse> GetStickerById(int stickerId)
    {
        IWebRequestReponse webRequestResponse = await WebClient.instance.SendGetRequest(Route + $"/{stickerId}");
        return ParseResponse<Sticker>(webRequestResponse);
    }
}