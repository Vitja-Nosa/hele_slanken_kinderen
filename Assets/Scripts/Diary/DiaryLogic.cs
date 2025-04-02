using UnityEngine;
using UnityEngine.UIElements;

public class DiaryLogic : MonoBehaviour
{
    public GameObject Diary;
    public GameObject DiaryCreator;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    public void OpenDiaryCreator() { DiaryCreator.SetActive(true); }
    public void OpenDiary() { Diary.SetActive(true); playerMovement.isInMenu = true; }
    public void CloseDiary() { Diary.SetActive(false); DiaryCreator.SetActive(false); playerMovement.isInMenu = false; }
}
