using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Lambchomp.Essentials;

[GlobalClass]
[Tool]
public partial class NodeReference : Resource
{
    [Export] protected PackedScene packedScene;
    protected System.Action<Node> dispatchEvent;
    protected Node instance;

    public Node Instance {
        get {
            if (instance == null && packedScene != null)
                instance = packedScene.Instantiate();
            else if (instance == null && packedScene == null && !Engine.IsEditorHint()) {
                StackTrace stackTrace = new StackTrace();
                StackFrame callerFrame = stackTrace.GetFrame(2);
                MethodBase callerMethod = callerFrame.GetMethod();
                GD.Print("NodeReference " + this.ResourceName + "from " + callerMethod.DeclaringType.Name + " has no Instance or PackedScene");
            }
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

    public void AddListener(System.Action<Node> listener) {
        dispatchEvent += listener;
        if (instance != null)
            listener.Invoke(instance);
    }

    public void RemoveListener(System.Action<Node> listener) {
        dispatchEvent -= listener;
    }
}
