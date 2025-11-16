using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeCar : MonoBehaviour
{
    [SerializeField] private MissionDisplay missionDisplay;

    public void EscapeFactory()
    {
        if (!missionDisplay.IsFinalMission())
            return;

        missionDisplay.CompleteMission(4);
        Invoke("EndGame", 2);
        // 게임 종료 처리
    }

    private void EndGame()
    {
        SceneManager.LoadScene("End");
    }
}
