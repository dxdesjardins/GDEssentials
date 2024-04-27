using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

public abstract partial class StaticReference<T> : NodeReference where T : StaticReference<T>
{
    private static NodeReference referenceInstance;

    public static Node Instance_ { 
        get { return referenceInstance.Instance; } 
        set { referenceInstance.Instance = value; }
    }

    public static void AddListener_(System.Action<Node> listener) {
        if (referenceInstance == null)
            GD.PrintErr("StaticReference " + typeof(T).Name + " has no instance. Failed to Add Listener.");
        referenceInstance.AddListener(listener);
    }

    public static void RemoveListener_(System.Action<Node> listener) {
        if (referenceInstance == null)
            GD.PrintErr("StaticReference " + typeof(T).Name + " has no instance. Failed to Remove Listener.");
        referenceInstance.RemoveListener(listener);
    }

    public StaticReference() {
        if (referenceInstance == null)
            referenceInstance = this;
        else {
            GD.PrintErr("StaticReference " + typeof(T).Name + " exists multiple times. Destroying copy.");
            this.Free();
        }
    }
}
