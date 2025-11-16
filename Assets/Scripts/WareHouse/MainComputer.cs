using UnityEngine;

public class MainComputer : MonoBehaviour
{
    [Header("Computer On/Off")]
    [SerializeField] private Light computerLight;
    [SerializeField] private MissionDisplay missionDisplay;
    [SerializeField] private Color computerOn = Color.red;
    [SerializeField] private Color computerOff = Color.green;

    public bool isOff = false;

    public void ToggleComputer()
    {
        if (isOff)
            return;

        isOff = true;
        computerLight.color = isOff ? computerOff : computerOn;
        missionDisplay.CompleteMission(2);
    }
}
