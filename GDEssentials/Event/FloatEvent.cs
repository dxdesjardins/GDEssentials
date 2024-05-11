using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
[Tool]
public partial class FloatEvent : ParamEvent<float>
{
    [Export] private float Value {
        get { return lastParameter; }
        set { lastParameter = value; }
    }
}
