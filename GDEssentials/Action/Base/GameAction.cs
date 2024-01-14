using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

public abstract partial class GameAction : Resource
{
    public virtual bool Invoke(Node node) { return true; }
}
