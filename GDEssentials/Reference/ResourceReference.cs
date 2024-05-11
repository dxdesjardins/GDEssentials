using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

public abstract partial class ResourceReference<T> : ResourceSingleton<T> where T : ResourceReference<T>
{
    [Export] public Resource instance;

    public new static Resource Instance => ResourceSingleton<T>.Instance.instance;
}