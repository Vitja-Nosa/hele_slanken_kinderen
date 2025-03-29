using UnityEngine;

public class LevelSelectLoader : MonoBehaviour
{
    public GameObject[] treatmentPrefabs; // alle prefabs in de Inspector zetten
    public Transform contentHolder; // leeg GameObject in scene

    private void Start()
    {
        string gekozenType = TreatmentTypeHolder.SelectedType;

        foreach (GameObject prefab in treatmentPrefabs)
        {
            if (prefab.name == gekozenType)
            {
                Instantiate(prefab, contentHolder);
                return;
            }
        }

        Debug.LogWarning("Geen prefab gevonden voor: " + gekozenType);
    }

}

