using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // rotate sprite relative to mouse cursor
    void Update()
    {
        if (MainMenu.isPaused) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);

        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
