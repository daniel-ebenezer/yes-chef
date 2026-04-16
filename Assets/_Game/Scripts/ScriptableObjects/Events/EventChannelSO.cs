using UnityEngine;
using UnityEngine.Events;

public abstract class EventChannelSO<T> : ScriptableObject
{
    public UnityEvent<T> OnRaised = new UnityEvent<T>();

    public void Raise(T value)
    {
        OnRaised.Invoke(value);
    }
}