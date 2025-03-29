using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;

    public Child child;
    public Guardian guardian;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // voorkom dubbele sessies
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // blijft bestaan tussen scenes
    }

   
    // Geeft true als zowel kind- als oudergegevens bestaan
    public bool HasFilledInData()
    {
        return child != null && guardian != null;
    }

 
    // Leeg de sessie (bij uitloggen)
    public void ClearSession()
    {
        child = null;
        guardian = null;
    }
}
