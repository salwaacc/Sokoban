using UnityEngine;

public class DoorController : MonoBehaviour
{

    [Header("Door Sprites")]
    [SerializeField] private SpriteRenderer doorRenderer;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;

    [Header("Door Collision")]
    [SerializeField] private Collider2D blockingCollider;

    public void SetOpen(bool isOpen)
    {
        doorRenderer.sprite = isOpen ? openSprite : closedSprite;

        blockingCollider.enabled = !isOpen;
    }
}


