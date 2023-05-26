using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform player;
  

    // focus camera on player's position
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}

