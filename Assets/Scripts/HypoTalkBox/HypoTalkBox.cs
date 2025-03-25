using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HypoTalkBox : MonoBehaviour
{
    public HypoTalkAnimation hypoTalkAnimation;
    public AudioClip audioClip;
    public TMP_Text text_box;

    public Toggle VolumeSwitch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TriggerAnimation(audioClip, "Dit is een voorbeeld om te demonstreren hoe deze prefab werkt");
    }

    public void TriggerAnimation(AudioClip audioClip, string speech)
    {
        hypoTalkAnimation.StartAnimation(audioClip);
        StartCoroutine(TypeText(speech, audioClip.length));
    
        
    }

    public void SwitchVolume(bool value) 
    {
        if (VolumeSwitch.isOn) hypoTalkAnimation.audioSource.volume = 0;
        else hypoTalkAnimation.audioSource.volume = 1;
    }

    private IEnumerator TypeText(string speech, float audioDuration)
    {
        text_box.text = ""; // Clear existing text
        int charIndex = 0;
        int totalCharCount = speech.Length;

        // Adjust typing speed to be faster than the audio duration per character
        float typingSpeed = (audioDuration / totalCharCount) * 0.95f; // Adjust 0.95f for faster typing speed

        while (charIndex < totalCharCount)
        {
            text_box.text += speech[charIndex];
            charIndex++;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

}
