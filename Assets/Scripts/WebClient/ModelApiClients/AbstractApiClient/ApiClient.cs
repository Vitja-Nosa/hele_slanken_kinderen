using UnityEngine;
using System.Collections.Generic;

public abstract class ApiClient : MonoBehaviour
{
    public abstract string Route { get; }

    protected IWebRequestReponse ParseResponse<T>(IWebRequestReponse webRequestResponse, bool isArray = false)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log($"Response data raw: {data.Data}");

                if (isArray)
                {
                    List<T> parsedList = JsonHelper.ParseJsonArray<T>(data.Data);
                    return new WebRequestData<List<T>>(parsedList);
                }
                else
                {
                    T parsedObject = JsonUtility.FromJson<T>(data.Data);
                    return new WebRequestData<T>(parsedObject);
                }

            default:
                return webRequestResponse;
        }
    }
}
