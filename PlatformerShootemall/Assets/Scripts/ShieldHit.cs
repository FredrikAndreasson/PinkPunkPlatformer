using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        switch (collision.gameObject.tag)
        {
            case "Enemy":
                collision.gameObject.transform.position = gameObject.transform.position + 2 * (gameObject.transform.position - transform.position);
                break;
            case "Wall":
                break;
            default:
                break;
        }
    }
}
