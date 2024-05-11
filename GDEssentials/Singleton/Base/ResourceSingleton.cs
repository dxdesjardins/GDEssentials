using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

public abstract partial class ResourceSingleton<T> : Resource where T : ResourceSingleton<T>
{
    private static T instance;

    public static T Instance {
        get {
            instance ??= GetInstance();
            return instance;
        }
        set { instance = value; }
    }

    public ResourceSingleton() {
        if (instance != null && !Engine.IsEditorHint()) {
            GD.PrintErr("Class " + typeof(T).Name + " exists multiple times in violation of singleton pattern. Destroying copy.");
            this.Free();
        }
    }

    protected static T GetInstance() {
        string resourcePath = "res://Resource/Reference/ResourceSingleton/" + typeof(T).Name + ".tres";
        resourcePath = ResourceLoader.Exists(resourcePath) ? resourcePath : "res://Resource/Reference/ResourceReference/" + typeof(T).Name + ".tres";
        T _instance = GD.Load(resourcePath) as T;
        if (_instance == null)
            GD.PrintErr("Warning: ", typeof(T).Name, " failed to load. Is the resource in the singleton folder?");
        return _instance;
    }
}
