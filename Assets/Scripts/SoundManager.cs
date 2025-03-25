
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;
    public AudioClip uiClick;
    public AudioClip uiError;
    public AudioClip uiSelect;
    public AudioClip uiWhoosh;
    public AudioClip rewardSticker;
    public AudioClip rewardComplete;
    public AudioClip avatarMove;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void PlayUIClick() => audioSource.PlayOneShot(uiClick);
    public void PlayUIError() => audioSource.PlayOneShot(uiError);
    public void PlayUISelect() => audioSource.PlayOneShot(uiSelect);
    public void PlayUIWhoosh() => audioSource.PlayOneShot(uiWhoosh);
    public void PlayRewardSticker() => audioSource.PlayOneShot(rewardSticker);
    public void PlayRewardComplete() => audioSource.PlayOneShot(rewardComplete);
    public void PlayAvatarMove() => audioSource.PlayOneShot(avatarMove);
}
