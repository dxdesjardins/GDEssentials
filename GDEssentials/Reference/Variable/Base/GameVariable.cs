using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

public abstract partial class GameVariable<T> : Resource
{
    protected T variable;
    private T runtimeValue;
    private bool initialized;

    public T Value {
        get	{
            if (!initialized) {
                runtimeValue = variable;
                initialized = true;
            }
            return runtimeValue;
        }
        set {
            initialized = true;
            runtimeValue = value;
        }
    }

    public void SetRuntimeValue (T value) {
        Value = value;
    }
}
