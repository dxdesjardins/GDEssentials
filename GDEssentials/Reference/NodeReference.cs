using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class NodeReference : Resource
{
    private Node instantiateReference;
    private System.Action<Node> dispatchEvent = delegate { };
    private Node instance;

    public Node Instance {
        get {
            if (instance == null && instantiateReference != null)
                instance = instantiateReference;
            return instance;
        }
        set {
            instance = value;
            if (instance != null) {
                if (dispatchEvent != null) 
                    dispatchEvent.Invoke(instance);
            }
            else {
                if (dispatchEvent != null)
                    foreach (System.Delegate d in dispatchEvent.GetInvocationList())
                        dispatchEvent -= (System.Action<Node>)d;
            }
        }
    }

    public T GetInstance<T>() {
        if (instance is T _instance)
            return _instance;
        return default;
    }

    public void AddListener(System.Action<Node> listener) {
        dispatchEvent += listener;
        if (instance != null)
            listener.Invoke(instance);
    }

    public void RemoveListener(System.Action<Node> listener) {
        dispatchEvent -= listener;
    }
}
