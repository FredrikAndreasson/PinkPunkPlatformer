using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public Vector2 speed = new Vector2(1.0f, 0.0f);
    void Start()
    {

    }

    void Update()
    {
        //look for wall collision
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, speed, 0.5f, ~enemyLayerMask);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Wall"))
            {
                speed = -speed;
            }
        }

        //change direction of sprite based on movement
        if (speed.x > 0 && transform.localScale.x > 0) { transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y); }
        else if (speed.x < 0 && transform.localScale.x < 0) { transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y); }

        //move
        transform.position += Vector3.right * speed.x * Time.deltaTime;
    }
}
