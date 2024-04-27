using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

public abstract partial class ResourceSingleton<T> : Resource where T : ResourceSingleton<T>
{
    private static T instance;

    public static T Instance {
        get {
            if (instance == null)
                CreateInstance();
            return instance;
        }
        set { instance = value; }
    }

    public ResourceSingleton() {
        if (instance == default)
            instance = this as T;
        else {
            GD.PrintErr("Class " + typeof(T).Name + " exists multiple times in violation of singleton pattern. Destroying copy.");
            this.Free();
        }
    }

    protected static T CreateInstance() {
        if (instance != null)
            GD.PrintErr("Warning: Instancing missing ResourceSingleton: ", typeof(T).Name, ".");    
        T _instance = (T)Activator.CreateInstance(typeof(T));
        _instance.ResourceName = typeof(T).Name;
        instance = _instance;
        return instance;
    }
}

