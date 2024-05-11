using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class InvokeIntEventAction : GameAction
{
    [Export] IntEvent intEvent;
    [Export] int param;

    public override bool Invoke(Node node) {
        intEvent.Invoke(param);
        return true;
    }
}
