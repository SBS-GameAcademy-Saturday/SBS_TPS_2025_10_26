using UnityEngine;

public class GeneratorComputer : MonoBehaviour
{
    [Header("Generators")]
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private MainComputer mainComputer;
    [SerializeField] private MissionDisplay missionDisplay;

    public bool isOff = false;

    public void ToggleComputer()
    {
        if (isOff)
            return;
        if (!mainComputer.isOff)
            return;

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Stop();
            audioSources[i].mute = true;
        }
        missionDisplay.CompleteMission(3);
    }
}
