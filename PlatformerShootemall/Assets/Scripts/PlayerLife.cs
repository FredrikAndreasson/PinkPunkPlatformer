using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;

    [SerializeField] private LifeController lifeController;
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
            case "Death Trap":
                Die();
                break;
            default:
                break;
        }
    }
    private void DealDamage(GameObject gameObject)
    {
        
        //update hitpoints
        //todo: update with relative hitpoints from gameobject (remove hardcoding)
        lifeController.DealDamageFrom(gameObject, 1);

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
