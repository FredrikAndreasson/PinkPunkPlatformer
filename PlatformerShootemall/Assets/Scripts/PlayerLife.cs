using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;

    public int health = 5;
    public int maxHealth = 5;

    [SerializeField] private StatsController lifeController;
    [SerializeField] private AudioSource deathSFX;
    [SerializeField] private AudioSource getHitSFX;
    [SerializeField] private Transform shield;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
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
                DealDamage(collision.gameObject);
                break;
            case "Trap":
                DealDamage(collision.gameObject);
                break;
            case "Level edge":
                Die();
                break;
            default:
                break;
        }
    }
    private void DealDamage(GameObject gameObject)
    {
        //update hitpoints
        //todo: update with relative damage from gameobject (remove hardcoding)
        Debug.Log("Hit by " + gameObject.name);
        health -= 1;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            GetHit(gameObject);
        }
    }

    public void GetHit(GameObject gameObject)
    {
        //trigger animation
        animator.SetTrigger("hit");
        //get knocked back
        transform.position = transform.position + (transform.position - gameObject.transform.position);
        getHitSFX.Play();

    }


    public void Die()
    {
        //trigger death animation
        animator.SetTrigger("death");
        deathSFX.Play();
        Destroy(shield.gameObject);
        //disable movement
        body.bodyType = RigidbodyType2D.Static;
    }
    //restart level
     private void Restart()
     {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
     }
}
