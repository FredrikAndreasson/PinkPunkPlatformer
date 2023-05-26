using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float scrollOffset = 2.0f;

    // move background slightly when player moves
    void Update()
    {
        transform.position = new Vector3((player.position.x) / scrollOffset, (player.position.y) / scrollOffset, transform.position.z);
    }
}
