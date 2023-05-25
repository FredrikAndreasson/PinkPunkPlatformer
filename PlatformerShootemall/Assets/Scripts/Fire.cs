using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Fire : MonoBehaviour
{
    public Sprite off;

    public void TurnOff()
    {
        this.gameObject.GetComponent<Animator>().enabled = false;
        this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = off;
    }

}
