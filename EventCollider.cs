using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCollider : MonoBehaviour
{
    public enum ColliderType { Collision, Trigger };
    public enum EventType { Enter, Exit };

    public UnityEvent OnEnterEvent;
    public ColliderType TypeOfCollider;
    public EventType TypeOfEvent;
    public List<GameObject> EventProvoker;

    private void OnTriggerEnter(Collider other)
    {
        if (TypeOfCollider == ColliderType.Collision)
            return;

        if (TypeOfEvent == EventType.Exit)
            return;

        if (EventProvoker.Contains(other.gameObject))
            OnEnterEvent?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (TypeOfCollider == ColliderType.Trigger)
            return;

        if (TypeOfEvent == EventType.Exit)
            return;

        if (EventProvoker.Contains(collision.gameObject))
            OnEnterEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (TypeOfCollider == ColliderType.Collision)
            return;

        if (TypeOfEvent == EventType.Enter)
            return;

        if (EventProvoker.Contains(other.gameObject))
            OnEnterEvent?.Invoke();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (TypeOfCollider == ColliderType.Trigger)
            return;

        if (TypeOfEvent == EventType.Enter)
            return;

        if (EventProvoker.Contains(collision.gameObject))
            OnEnterEvent?.Invoke();
    }
}
