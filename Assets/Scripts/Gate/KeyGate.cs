using UnityEngine;

public class KeyGate : MonoBehaviour
{
    [SerializeField] private KeyList keyList;

    [SerializeField] private Animator animator;
    [SerializeField] private MissionDisplay missionDisplay;

    private bool pauseInteraction = false;

    public void ToggleGate()
    {
        if (!keyList.hasKey)
            return;

        if (pauseInteraction)
            return;

        bool isOpened = animator.GetBool("open");
        animator.SetBool("open", !isOpened);
        pauseInteraction = true;
        Invoke("ResumeInteraction", 4);
        missionDisplay.CompleteMission(1);
    }

    private void ResumeInteraction()
    {
        pauseInteraction = false;
    }

}
