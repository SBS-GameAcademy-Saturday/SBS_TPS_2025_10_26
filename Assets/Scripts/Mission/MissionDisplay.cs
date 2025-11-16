using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MissionDisplay : MonoBehaviour
{
    [Header("Mission Text")]
    [SerializeField] private TextMeshProUGUI mission_1;
    [SerializeField] private TextMeshProUGUI mission_2;
    [SerializeField] private TextMeshProUGUI mission_3;
    [SerializeField] private TextMeshProUGUI mission_4;

    private bool mission_1_cleared = false;
    private bool mission_2_cleared = false;
    private bool mission_3_cleared = false;
    private bool mission_4_cleared = false;
    
    public void OnMissionAction(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    public void CompleteMission(int missionIndex)
    {
        switch(missionIndex)
        {
            case 1:
                mission_1_cleared = true;
                mission_1.color = Color.green;
                break;
            case 2:
                mission_2_cleared = true;
                mission_2.color = Color.green;
                break;
            case 3:
                mission_3_cleared = true;
                mission_3.color = Color.green;
                break;
            case 4:
                mission_4_cleared = true;
                mission_4.color = Color.green;
                break;
        }
    }

    public bool IsFinalMission()
    {
        return mission_1_cleared && mission_2_cleared && mission_3_cleared;
    }
}
