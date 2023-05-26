using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GhostBehaviour;

public class PlayerLife : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;
    private bool dead = false;

    public int health = 5;
    public int maxHealth = 5;

    [SerializeField] private StatsController lifeController;
    private AudioSource audioSource;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip getHitSFX;
    [SerializeField] private Transform shield;
    [SerializeField] private IntEventSO _HealthUpdatedEvent;
    [SerializeField] private IntEventSO _DamageEvent;
  
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        _HealthUpdatedEvent.Event += UpdateHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        switch (collision.gameObject.tag)
        {
            case "Enemy":
                Vector2 hitDirection = (collision.transform.position - transform.position).normalized;

                if (hitDirection.y > 0)
                {
                    _DamageEvent.Invoke(1);
                    GetHit(collision.gameObject);
                }
                break;
            case "Bullet":
                _DamageEvent.Invoke(1);
                GetHit(collision.gameObject);
                break;
            case "Trap":
                _DamageEvent.Invoke(1);
                GetHit(collision.gameObject);
                break;
            case "Level edge":
                Die();
                break;
            default:
                break;
        }
    }
    //deal damage or kill player if not enough health left
    public void UpdateHealth(int health)
    {
        this.health = health;
    }
    private void Update()
    {
        if (health <= 0 && !dead)
        {
            dead = true;
            Die();
        }
    }

    public void GetHit(GameObject gameObject)
    {
        //trigger animation
        animator.SetTrigger("hit");
        //get knocked back
        transform.position = transform.position + (transform.position - gameObject.transform.position)/2;
        audioSource.PlayOneShot(getHitSFX);

    }

    //trigger death animation, sound and restart level
    public void Die()
    {
        //trigger death animation
        animator.SetTrigger("death");
        audioSource.PlayOneShot(deathSFX);
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
