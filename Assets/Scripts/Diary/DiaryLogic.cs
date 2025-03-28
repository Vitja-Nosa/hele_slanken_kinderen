using UnityEngine;
using UnityEngine.UIElements;

public class DiaryLogic : MonoBehaviour
{
    public GameObject Diary;
    public GameObject DiaryCreator;


    public void OpenDiaryCreator() { DiaryCreator.SetActive(true); }
    public void OpenDiary() { Diary.SetActive(true); }
    public void CloseDiary() { Diary.SetActive(false); DiaryCreator.SetActive(false); }
}
