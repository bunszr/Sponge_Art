using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;

// Author: Ryan Hipple , https://youtu.be/raQ3iHhE_Kk?t=1677 
[CreateAssetMenu(fileName = "GameEvent", menuName = "SEZER/GameEvent")]
public class GameEvent : ScriptableObject
{
    private readonly List<GameEventListener> eventListeners = new List<GameEventListener>();
    private Action actionList;

    public void Raise()
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised();
        actionList?.Invoke();
    }

    public void RaiseWithDelay(float delay)
    {
        GameManager.Ins.StartCoroutine(MethodCO(delay));
    }

    IEnumerator MethodCO(float delay)
    {
        yield return new WaitForSeconds(delay);
        Raise();
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void RegisterListener(Action action)
    {
        actionList += action;
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }

    public void UnregisterListener(Action action)
    {
        actionList -= action;
    }
}