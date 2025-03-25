using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HypoTalkAnimation : MonoBehaviour
{
    public Sprite[] HypoTalkFrames;
    public float frameRate = 0.1f; // Adjust animation speed
    public Image imageComponent;
    public AudioSource audioSource;
    private Coroutine animationCoroutine;

    public void StartAnimation(AudioClip audioClip)
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();

            if (animationCoroutine == null && HypoTalkFrames.Length > 0)
            {
                animationCoroutine = StartCoroutine(Animate());
            }
            StartCoroutine(StopAnimationWhenAudioEnds());
        }
        else Debug.Log("No audio clip detected");
    }

    private IEnumerator StopAnimationWhenAudioEnds()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        StopAnimation();
    }

    public void StopAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        if (HypoTalkFrames.Length > 0)
        {
            imageComponent.sprite = HypoTalkFrames[0]; // Reset to first frame
        }
    }

    private IEnumerator Animate()
    {
        int frameIndex = 0;
        while (true)
        {
            imageComponent.sprite = HypoTalkFrames[frameIndex];
            frameIndex = (frameIndex + 1) % HypoTalkFrames.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }
}
