using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

public abstract partial class ParamEvent<T> : GameEvent
{
    [Export] private bool dispatchLastStateOnAdd = false;
    private List<ParamEventListener<T>> eventListeners = new();
    private List<Action<T>> scriptEventListeners = new();
    protected T lastParameter;
    public T LastParameter { get { return (lastParameter != null) ? lastParameter : invokingParam; } }
    private bool hasParameter;
    public bool HasParameter { get { return hasParameter || isInvoking; } }
    private bool isInvoking;
    public bool IsInvoking { get { return isInvoking; } }
    private T invokingParam;
    public T InvokingParam { get { return invokingParam; } }


    public void Invoke(T param) {
        isInvoking = true;
        invokingParam = param;
        for (int i = scriptEventListeners.Count - 1; i >= 0; i--)
            scriptEventListeners[i].Invoke(param);
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].Dispatch(param);
        lastParameter = param;
        hasParameter = true;
        isInvoking = false;
    }

    public override void AddListener(Action listener) {
        Action<T> paramAction = (_) => { listener.Invoke(); };
        AddListener(paramAction);
    }

    public void AddListener(Action<T> listener) {
        scriptEventListeners.Add(listener);
        if (dispatchLastStateOnAdd && hasParameter)
            listener.Invoke(lastParameter);
    }

    public void RemoveListener(Action<T> listener) {
        scriptEventListeners.Remove(listener);
    }

    public void AddListener(ParamEventListener<T> listener) {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
        if (dispatchLastStateOnAdd && hasParameter)
            listener.Dispatch(lastParameter);
    }

    public void RemoveListener(ParamEventListener<T> listener) {
            eventListeners.Remove(listener);
    }
}
