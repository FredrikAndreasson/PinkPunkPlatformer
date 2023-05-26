using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHit : MonoBehaviour
{
    //destroy bullets that hit shield
    private void OnCollisionEnter2D(Collision2D collision)
    {

        switch (collision.gameObject.tag)
        {
            case "Bullet":
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }
}
