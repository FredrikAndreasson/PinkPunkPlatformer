using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    public Transform objectToFollow;

    private void Update()
    {
        if (objectToFollow != null)
        {
            Vector3 targetPosition = objectToFollow.position + new Vector3(0,0, -5);
            transform.position = targetPosition;
        }
    }
}
