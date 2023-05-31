using System;
using UnityEngine;

[CreateAssetMenu]
public class IntEventSO : ScriptableObject
{
    public event Action<int> Event;

    public void Invoke(int value) => Event?.Invoke(value);
}