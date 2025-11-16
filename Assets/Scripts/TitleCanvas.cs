using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        playButton.onClick.AddListener(OnPlayButton);
        exitButton.onClick.AddListener(OnExitButton);
    }

    private void OnPlayButton()
    {

        SceneManager.LoadScene("Main");
    }

    private void OnExitButton()
    {
        // 게임 종료 Application.Quit()
        Application.Quit();
    }
}
