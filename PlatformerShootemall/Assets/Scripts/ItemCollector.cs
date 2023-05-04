using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemCollector : MonoBehaviour
{
    public int collectedFruit = 0;
    [SerializeField] private AudioClip collectSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //When boxCollider is triggered, do this
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Fruit")) 
        {
            if (collision.gameObject.GetComponent<BoxCollider2D>().enabled)
            {
                collision.gameObject.GetComponent<Animator>().SetTrigger("collected");
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<AudioSource>().PlayOneShot(collectSFX);
                Destroy(collision.gameObject, 0.3f);
                collectedFruit += collision.gameObject.GetComponent<FruitValue>().value;
                Debug.Log("Fruits collected: " + collectedFruit);
            }
            
            
        }
    }
}
