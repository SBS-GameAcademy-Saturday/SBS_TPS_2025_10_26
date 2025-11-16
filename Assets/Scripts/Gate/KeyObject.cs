using UnityEngine;

public class KeyObject : MonoBehaviour
{
    [SerializeField] private KeyList keyList;

    public void FoundKey()
    {
        keyList.hasKey = true;
        gameObject.SetActive(false);
    }
}
