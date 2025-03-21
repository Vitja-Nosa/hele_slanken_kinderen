using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class AppointmentsApiClient : ApiClient
{
    public override string Route => "/api/appointments";

    public async Awaitable<IWebRequestReponse> ReadAllAppointments()
    {
        IWebRequestReponse webRequestResponse = await WebClient.instance.SendGetRequest(Route);
        return ParseResponse<List<Appointment>>(webRequestResponse, true);
    }

    public async Awaitable<IWebRequestReponse> CreateAppointment(Appointment appointment)
    {
        string data = JsonUtility.ToJson(appointment);

        IWebRequestReponse webRequestResponse = await WebClient.instance.SendPostRequest(Route, data);
        return ParseResponse<Appointment>(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> UpdateAppointment(Appointment appointment)
    {
        string data = JsonUtility.ToJson(appointment);

        IWebRequestReponse webRequestResponse = await WebClient.instance.SendPutRequest(Route, data);
        return ParseResponse<Appointment>(webRequestResponse);
    }
    public async Awaitable<IWebRequestReponse> DeleteAppointment(Appointment appointment)
    {
        return await WebClient.instance.SendDeleteRequest(Route + $"/{appointment.id}");
    }
}