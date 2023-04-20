using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;
    [SerializeField] private int playerMaxHealth = 5;
    private int currentHealth;
    [SerializeField] private AudioSource deathSFX;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        currentHealth = playerMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }
    //If hitting a trap
    private void OnCollisionEnter2D(Collision2D collision)
    {

        switch (collision.gameObject.tag)
        {
            case "Enemy":
                GetHitBy(collision.gameObject);
                break;
            case "Death Trap":
                Die();
                break;
            default:
                break;
        }
    }
    private void GetHitBy(GameObject gameObject)
    {
        animator.SetTrigger("hit");
        transform.position = transform.position + (transform.position - gameObject.transform.position);
    }

    private void Die()
    {
        //trigger death animation
        animator.SetTrigger("death");
        //deathSFX.Play();
        //disable movement
        //body.bodyType = RigidbodyType2D.Static;
    }
    //restart level
    /* private void Restart()
     {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
     }*/
}
