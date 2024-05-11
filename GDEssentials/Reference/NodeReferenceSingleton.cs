using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

public abstract partial class NodeReferenceSingleton<T> : NodeReference where T : NodeReferenceSingleton<T>
{
    private static NodeReference referenceInstance;

    public static new Node Instance { 
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

    public NodeReferenceSingleton() {
        if (referenceInstance == null)
            referenceInstance = this;
        else {
            GD.PrintErr("StaticReference " + typeof(T).Name + " exists multiple times. Destroying copy.");
            this.Free();
        }
    }
}
