using UnityEngine;

public class SawScript : MonoBehaviour
{
    public float speed = 2f;
    void Update()
    {
        transform.Rotate(0, 0, -360 * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.instance.TakeDamage();
        }
    }
}
