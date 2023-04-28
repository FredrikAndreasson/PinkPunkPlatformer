using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointTraverser : MonoBehaviour
{
    public GameObject[] waypoints;
    private int currentTarget = 0;

    public bool reverse; //if true, it reverse the platform backwards. Otherwise it loops from the start
    int direction = 1;

    public bool traverse = true; 

    public float speed = 4f;

    private void Update()
    {
        if (traverse == false)
            return;

        if (Vector2.Distance(waypoints[currentTarget].transform.position, transform.position) < 0.1f)
        {
            if (!reverse)
            {
                currentTarget++;
                if (currentTarget >= waypoints.Length)
                {
                    currentTarget = 0;
                }
            }
            else
            {
                currentTarget = currentTarget + direction;
                if (currentTarget >= waypoints.Length - 1)
                    direction = -1;
                if (currentTarget <= 0)
                    direction = 1;
            }

        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentTarget].transform.position, Time.deltaTime * speed);
    }
}
