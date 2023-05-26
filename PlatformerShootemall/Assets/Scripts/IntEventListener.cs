using UnityEngine;
using UnityEngine.Events;

public class IntEventListener : MonoBehaviour
{
    public UnityEvent<int> OnEvent;
    [SerializeField] private IntEventSO _event;

    private void Start() => _event.Event += InvokeEvent;
    private void OnDestroy() => _event.Event -= InvokeEvent;

    private void InvokeEvent(int value)
    {
        OnEvent?.Invoke(value);
    }
}