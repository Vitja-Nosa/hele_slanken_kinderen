using UnityEngine;

public class MotivationScript : LevelLogic
{
    public HypoTalkBox hypoTalkBox;
    public AudioClip[] audioClips;
    string[] bericht = {
    "Hoera! Je hebt dit level gehaald! Jij wordt steeds slimmer over diabetes. Op naar het volgende avontuur!",
    "Superknap! Je hebt weer iets nieuws geleerd. Blijf zo doorgaan, je bent een echte diabetes-held!",
    "Wow, wat goed! Jij weet steeds beter hoe je voor jezelf moet zorgen. Klaar voor de volgende uitdaging?",
    "Lekker bezig! Je leert steeds meer over diabetes. Blijf spelen en ontdekken!",
    "Je vliegt erdoorheen! Super gedaan, wil je zien wat het volgende level voor je in petto heeft?",
    "Feestje! Je hebt een nieuw level uitgespeeld. Jij bent een echte doorzetter!",
    "Heldhaftig! Je leert steeds beter hoe je goed voor jezelf zorgt. Dat is super belangrijk!",
    "Slimme zet! Je hebt weer een stukje van de diabetes-puzzel opgelost. Wat zal je hierna ontdekken?",
    "Top gedaan! Jij bent een kanjer. Klaar voor een nieuwe uitdaging?",
    "Toppers zoals jij geven niet op! Elke keer leer je iets nieuws. Jij wordt een echte diabetes-expert!"
    };

    void Start()
    {
        StartMotivation();
    }

    private async void StartMotivation() 
    {
        await StartAudio();
    }

    private async Awaitable StartAudio() 
    {
        int randomIndex = UnityEngine.Random.Range(0, 10);

        hypoTalkBox.TriggerAnimation(audioClips[randomIndex], bericht[randomIndex]);
        await base.WaitForLengthOfAudio(audioClips[randomIndex]);
    }
}
