using UnityEngine;

public class StairController : MonoBehaviour
{
    
        [Header("Stair Blocker")]
    [SerializeField] private GameObject blocker;

    public void SetActive(bool isActive)
    {
        if (blocker == null) return;

        blocker.SetActive(!isActive);
    }
}
