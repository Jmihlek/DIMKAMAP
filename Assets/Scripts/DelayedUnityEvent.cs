using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DelayedUnityEvent
{
    [SerializeField]
    private List<DelayedEvent> events = new List<DelayedEvent>();

    public void Invoke()
    {
        foreach (var delayedEvent in events)
        {
            if (delayedEvent.Delay > 0f)
            {
                CoroutineRunner.Instance.StartCoroutine(InvokeWithDelay(delayedEvent.Event, delayedEvent.Delay));
            }
            else
            {
                delayedEvent.Event?.Invoke();
            }
        }
    }

    private IEnumerator InvokeWithDelay(UnityEvent unityEvent, float delay)
    {
        yield return new WaitForSeconds(delay);
        unityEvent?.Invoke();
    }
}

[Serializable]
public class DelayedEvent
{
    public UnityEvent Event;
    public float Delay;
}
