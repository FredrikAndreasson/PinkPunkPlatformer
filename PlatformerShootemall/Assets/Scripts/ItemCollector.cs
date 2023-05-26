using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemCollector : MonoBehaviour
{
    public int collectedFruit = 0;
    [SerializeField] private AudioClip collectSFX;

    [SerializeField] private IntEventSO _ScoreUpdatedEvent;
    [SerializeField] private IntEventSO _ScoreEvent;

    private void Start()
    {
        _ScoreUpdatedEvent.Event += UpdateScore;
    }

    //if colliding with a fruit, trigger animation and sound, then destroy it and update score with fruit's value
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
                _ScoreEvent.Invoke(collision.gameObject.GetComponent<FruitValue>().value);
                Debug.Log("Fruits collected: " + collectedFruit);
            }
            
            
        }
    }

    private void UpdateScore(int score)
    {
        collectedFruit = score;
    }
}
