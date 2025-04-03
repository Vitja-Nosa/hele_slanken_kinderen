using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppointmentCreator : MonoBehaviour
{
    public AppointmentsApiClient appointmentsApiClient;

    public TMP_InputField AppointmentName;
    public TMP_InputField DoctorName;
    public TMP_InputField Day;
    public TMP_InputField Time;

    public TMP_Text feedbackText;
    public GameObject FeedbackTextContainer;

    void Start()
    {
        gameObject.SetActive(false); // Making sure object is not shown on start
        FeedbackTextContainer.SetActive(false); // Hides the feedback text container
    }

    public void CloseCreator()
    {
        gameObject.SetActive(false);
        FeedbackTextContainer.SetActive(false);
    }
    
    public async void Create()
    {
        DateTime dateInput;
        
        // Date Validation
        try { dateInput = DateTime.Parse(Day.text + " " + Time.text + ":00"); }
        catch (FormatException)
        {
            FeedbackTextContainer.SetActive(true);
            feedbackText.text = "Datum invalid. Gebruik:\nDag: DD-MM-JJJJ\nTijd: HH:mm";
            return;
        }

        // Creating Appointment object
        Appointment newEntry = new()
        {
            id = "serverGenerated",
            childId = "serverGenerated",
            doctorName = DoctorName.text,
            date = dateInput.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            appointmentName = AppointmentName.text,
            description = " "
        };

        // Creating Appointment
        IWebRequestReponse result = await appointmentsApiClient.CreateAppointment(newEntry);
        
        // Check if Appointment is made succesfully
        if (result is WebRequestError)
        {
            FeedbackTextContainer.SetActive(true);
            feedbackText.text = "Afspraak aanmaken fout gegaan";
            return;
        }

        SceneManager.LoadScene("AgendaScene");
    }
}
