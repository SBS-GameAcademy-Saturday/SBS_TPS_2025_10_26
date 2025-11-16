using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndCanvas : MonoBehaviour
{
    [SerializeField] private Button replayButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        replayButton.onClick.AddListener(OnRePlayButton);
        exitButton.onClick.AddListener(OnExitButton);
    }

    private void OnRePlayButton()
    {

        SceneManager.LoadScene("Main");
    }

    private void OnExitButton()
    {
        // 게임 종료 Application.Quit()
        Application.Quit();
    }
}
