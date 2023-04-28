using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

    public static T instance => s_instance;

    protected static T s_instance;

    protected virtual void OnDestroy()
    {
        if(s_instance == this)
        {
            s_instance = null;
        }
    }

    private void Awake()
    {
        if(s_instance == null)
        {
            s_instance = this as T;
        }
        else
        {
            Debug.LogError($"Instance of {typeof(T)} already exists!");
        }
    }

    private void OnValidate()
    {
        gameObject.name = typeof(T).Name;
    }
}
