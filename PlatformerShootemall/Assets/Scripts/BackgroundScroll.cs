using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float scrollOffset = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3((player.position.x)/scrollOffset, (player.position.y)/scrollOffset, transform.position.z);
    }
}
